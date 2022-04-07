using System;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing.Standard.Util
{
    public class MergeProcessor : BaseProcessor<MergeParams, FastImageF>
    {
        private float[,,] _tempArr;

        public MergeProcessor(MergeParams processorParams) : base(processorParams)
        {
            if (ProcessorParams.MergingFunction is null)
                ProcessorParams.MergingFunction = (in1, in2, channel) => (in1 + in2) * 0.5f;
        }

        public override IFastImage Before(IFastImage fastImage, CancellationToken cancellationToken)
        {
            base.Before(fastImage, cancellationToken);
            // for testing purposes
            if(ProcessorParams.ToMerge is null) ProcessorParams.ToMerge = FastImageFactory.Empty(fastImage.PSize);
            return fastImage;
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            
            ProcessorParams.ToMerge.ToFloatRepresentation().Process(ExtractPixels, cancellationToken);
            ((FastImageF) fastImage).Process(MergePixels, cancellationToken);
            return base.Process(fastImage, cancellationToken);
        }

        private float[,,] ExtractPixels(float[,,] pixels, CancellationToken cancellationToken)
        {
            var po = new ParallelOptions();
            po.CancellationToken = cancellationToken;
            var depth = pixels.GetLength(2);
            _tempArr = new float[pixels.GetLength(0), pixels.GetLength(1), pixels.GetLength(2)];
            Array.Copy(pixels, _tempArr, pixels.Length);
            Parallel.For(ProcessorParams.WorkingArea.LeftInclusive, ProcessorParams.WorkingArea.RightExclusive, po, i =>
            {
                for (var j = ProcessorParams.WorkingArea.BotInclusive;
                     j < ProcessorParams.WorkingArea.TopExclusive;
                     j++)
                {
                    for (var k = 0; k < depth; k++)
                    {
                        if (ProcessorParams.ChannelSelector.Used(k) && ProcessorParams.WorkingArea.ShouldEdit(i,j))
                            _tempArr[i, j, k] = pixels[i, j, k];
                    }
                }
            });
            return pixels;
        }

        private float[,,] MergePixels(float[,,] pixels, CancellationToken cancellationToken)
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
                        if (ProcessorParams.ChannelSelector.Used(k) && ProcessorParams.WorkingArea.ShouldEdit(i,j))
                            _tempArr[i, j, k] = ProcessorParams.MergingFunction(pixels[i, j, k], _tempArr[i, j, k], k);
                    }
                }
            });
            return _tempArr;
        }

        public override IFastImage After(IFastImage fastImage, CancellationToken cancellationToken)
        {
            //free
            _tempArr = null;
            return base.After(fastImage, cancellationToken);
        }
    }
}