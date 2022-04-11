using System;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Exceptions;
using Sobczal.Picturify.Core.Processing.Filters;
using Sobczal.Picturify.Core.Processing.Testing;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.EdgeDetection
{
    public class NonMaximumGradientSuppressionProcessor : BaseProcessor<NonMaximumGradientSuppressionParams, FastImageF>
    {
        private float[,,] _gyPixels;
        private bool[,,] _suppressionMap;
        private BaseFilter _edgeFilter;
        public NonMaximumGradientSuppressionProcessor(NonMaximumGradientSuppressionParams processorParams) : base(processorParams)
        {
        }
        
        public override IFastImage Before(IFastImage fastImage, CancellationToken cancellationToken)
        {
            if (fastImage.PSize.Width != ProcessorParams.Gy.PSize.Width ||
                fastImage.PSize.Height != ProcessorParams.Gy.PSize.Height)
                throw new ParamsArgumentException("different size images not supported.",
                    nameof(ProcessorParams.Gy));
            _edgeFilter = EdgeBehaviourSelector.GetFilter(ProcessorParams.EdgeBehaviourType, new PSize(1, 1));
            AddFilter(_edgeFilter);
            return base.Before(fastImage, cancellationToken);
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            _edgeFilter.Before(ProcessorParams.Gy, new EmptyProcessorParams(), cancellationToken);
            ProcessorParams.Gy.Process(ExtractPixels, cancellationToken);
            ((FastImageF) fastImage).Process(PerformSuppressionOnGx, cancellationToken);
            ProcessorParams.Gy.Process(PerformSuppressionOnGy, cancellationToken);
            _edgeFilter.After(ProcessorParams.Gy, new EmptyProcessorParams(), cancellationToken);
            return base.Process(fastImage, cancellationToken);
        }
        
        private float[,,] ExtractPixels(float[,,] pixels, CancellationToken cancellationToken)
        {
            var po = new ParallelOptions();
            po.CancellationToken = cancellationToken;
            var depth = pixels.GetLength(2);
            _gyPixels = new float[pixels.GetLength(0), pixels.GetLength(1), pixels.GetLength(2)];
            Parallel.For(ProcessorParams.WorkingArea.LeftInclusive, ProcessorParams.WorkingArea.RightExclusive, po, i =>
            {
                for (var j = ProcessorParams.WorkingArea.BotInclusive;
                     j < ProcessorParams.WorkingArea.TopExclusive;
                     j++)
                {
                    for (var k = 0; k < depth; k++)
                    {
                        if (ProcessorParams.ChannelSelector.Used(k) && ProcessorParams.WorkingArea.ShouldEdit(i,j))
                            _gyPixels[i, j, k] = pixels[i, j, k];
                    }
                }
            });
            return pixels;
        }

        private float[,,] PerformSuppressionOnGx(float[,,] pixels, CancellationToken cancellationToken)
        {
            var po = new ParallelOptions();
            po.CancellationToken = cancellationToken;
            var depth = pixels.GetLength(2);
            _suppressionMap = new bool[pixels.GetLength(0), pixels.GetLength(1), pixels.GetLength(2)];
            Parallel.For(ProcessorParams.WorkingArea.LeftInclusive, ProcessorParams.WorkingArea.RightExclusive, po, i =>
            {
                for (var j = ProcessorParams.WorkingArea.BotInclusive;
                     j < ProcessorParams.WorkingArea.TopExclusive;
                     j++)
                {
                    for (var k = 0; k < depth; k++)
                    {
                        if (ProcessorParams.ChannelSelector.Used(k) && ProcessorParams.WorkingArea.ShouldEdit(i, j))
                        {
                            var theta = Math.Atan2(_gyPixels[i, j, k], pixels[i, j, k]);
                            var angle = (int)Math.Round(theta * 4 / Math.PI);

                            var currentPixVal = Math.Abs(_gyPixels[i, j, k]) + Math.Abs(pixels[i, j, k]);
                            switch (angle)
                            {
                                case 0: case 4: case -4:
                                    if (currentPixVal >
                                        Math.Abs(_gyPixels[i - 1, j, k]) + Math.Abs(pixels[i - 1, j, k]) &&
                                        currentPixVal > Math.Abs(_gyPixels[i + 1, j, k]) +
                                        Math.Abs(pixels[i + 1, j, k]))
                                    {
                                        _suppressionMap[i, j, k] = false;
                                    }
                                    else
                                    {
                                        pixels[i, j, k] = 0f;
                                        _suppressionMap[i, j, k] = true;
                                    }
                                    break;
                                case 2: case -2:
                                    if (currentPixVal >
                                        Math.Abs(_gyPixels[i, j - 1, k]) + Math.Abs(pixels[i, j - 1, k]) &&
                                        currentPixVal > Math.Abs(_gyPixels[i, j + 1, k]) +
                                        Math.Abs(pixels[i, j + 1, k]))
                                    {
                                        _suppressionMap[i, j, k] = false;
                                    }
                                    else
                                    {
                                        pixels[i, j, k] = 0f;
                                        _suppressionMap[i, j, k] = true;
                                    }
                                    break;
                                case 1: case -3:
                                    if (currentPixVal >
                                        Math.Abs(_gyPixels[i - 1, j - 1, k]) + Math.Abs(pixels[i - 1, j - 1, k]) &&
                                        currentPixVal > Math.Abs(_gyPixels[i + 1, j + 1, k]) +
                                        Math.Abs(pixels[i + 1, j + 1, k]))
                                    {
                                        _suppressionMap[i, j, k] = false;
                                    }
                                    else
                                    {
                                        pixels[i, j, k] = 0f;
                                        _suppressionMap[i, j, k] = true;
                                    }
                                    break;
                                case 3: case -1:
                                    if (currentPixVal >
                                        Math.Abs(_gyPixels[i - 1, j + 1, k]) + Math.Abs(pixels[i - 1, j + 1, k]) &&
                                        currentPixVal > Math.Abs(_gyPixels[i + 1, j - 1, k]) +
                                        Math.Abs(pixels[i + 1, j - 1, k]))
                                    {
                                        _suppressionMap[i, j, k] = false;
                                    }
                                    else
                                    {
                                        pixels[i, j, k] = 0f;
                                        _suppressionMap[i, j, k] = true;
                                    }
                                    break;
                            }
                        }
                    }
                }
            });
            return pixels;
        }

        private float[,,] PerformSuppressionOnGy(float[,,] pixels, CancellationToken cancellationToken)
        {
            var po = new ParallelOptions();
            po.CancellationToken = cancellationToken;
            var depth = pixels.GetLength(2);
            Parallel.For(ProcessorParams.WorkingArea.LeftInclusive, ProcessorParams.WorkingArea.RightExclusive, po, i =>
            {
                for (var j = ProcessorParams.WorkingArea.BotInclusive;
                     j < ProcessorParams.WorkingArea.TopExclusive;
                     j++)
                {
                    for (var k = 0; k < depth; k++)
                    {
                        if (ProcessorParams.ChannelSelector.Used(k) && ProcessorParams.WorkingArea.ShouldEdit(i, j))
                        {
                            if (_suppressionMap[i, j, k])
                                pixels[i, j, k] = 0f;
                        }
                    }
                }
            });
            return pixels;
        }

        public override IFastImage After(IFastImage fastImage, CancellationToken cancellationToken)
        {
            _gyPixels = null;
            _suppressionMap = null;
            return base.After(fastImage, cancellationToken);
        }
    }
}