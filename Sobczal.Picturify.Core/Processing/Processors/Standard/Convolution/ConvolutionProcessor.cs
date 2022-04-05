using System;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Standard
{
    public class ConvolutionProcessor : BaseProcessor<ConvolutionParams, FastImageF>
    {
        /// <summary>
        /// Traditional convolution. Optimized for small convolution matrixes.
        /// </summary>
        /// <param name="processorParams"></param>
        /// <exception cref="ArgumentException"></exception>
        public ConvolutionProcessor(ConvolutionParams processorParams) : base(processorParams)
        {
            if (ProcessorParams.ConvolutionMatrix.GetLength(0) % 2 != 1 || ProcessorParams.ConvolutionMatrix.GetLength(0) < 1 ||
                ProcessorParams.ConvolutionMatrix.GetLength(0) % 2 != 1 || ProcessorParams.ConvolutionMatrix.GetLength(0) < 1)
                throw new ArgumentException("Matrix must be of size 2*n+1x2*m+1 and both dimensions must be > 0.",
                    nameof(ProcessorParams.ConvolutionMatrix));
            var range = new PSize(processorParams.ConvolutionMatrix.GetLength(0) / 2,
                processorParams.ConvolutionMatrix.GetLength(1) / 2);
            AddFilter(EdgeBehaviourSelector.GetFilter(processorParams.EdgeBehaviourType, range));
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            ((FastImageF)fastImage).Process(ProcessingFunction, cancellationToken);
            return base.Process(fastImage, cancellationToken);
        }

        private float[,,] ProcessingFunction(float[,,] pixels, CancellationToken cancellationToken)
        {
            var po = new ParallelOptions();
            po.CancellationToken = cancellationToken;
            var depth = pixels.GetLength(2);
            var convolutionMatrix = ProcessorParams.ConvolutionMatrix;
            var rangeX = convolutionMatrix.GetLength(0) / 2;
            var rangeY = convolutionMatrix.GetLength(1) / 2;
            var arr = new float[pixels.GetLength(0), pixels.GetLength(1), pixels.GetLength(2)];
            Parallel.For(ProcessorParams.WorkingArea.LeftInclusive, ProcessorParams.WorkingArea.RightExclusive, po, i =>
            {
                for (var j = ProcessorParams.WorkingArea.BotInclusive;
                     j < ProcessorParams.WorkingArea.TopExclusive;
                     j++)
                {
                    for (var k = 0; k < depth; k++)
                    {
                        if (!ProcessorParams.ChannelSelector.Used(k)) continue;
                        var value = 0f;
                        for (var l = -rangeX; l <= rangeX; l++)
                        {
                            for (var m = -rangeY; m <= rangeY; m++)
                            {
                                value += pixels[i + l, j + m, k] * convolutionMatrix[l + rangeX, m + rangeY];
                            }
                        }
                        arr[i, j, k] = value;
                    }
                }
            });
            return arr;
        }
    }
}