using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Standard;

namespace Sobczal.Picturify.Core.Processing.Blur
{
    public class MedianProcessor : BaseProcessor<MedianParams, FastImageB>
    {
        private readonly int _tillMedian;
        public MedianProcessor(MedianParams processorParams) : base(processorParams)
        {
            _tillMedian = (ProcessorParams.Range.Width * 2 + 1) * (ProcessorParams.Range.Height * 2 + 1) / 2;
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            var rbp = new RollingBucketProcessor(new RollingBucketParams(ProcessorParams.ChannelSelector,
                ProcessorParams.Range, ProcessCalculateOne, ProcessorParams.EdgeBehaviourType,
                ProcessorParams.WorkingArea));
            fastImage.ExecuteProcessor(rbp);
            return base.Process(fastImage, cancellationToken);
        }

        private byte ProcessCalculateOne(ushort[,] buckets, byte c)
        {
            var j = _tillMedian;
            byte i = 0;
            while (j >= 0)
            {
                j -= buckets[--i, c];
            }
            return i;
        }
    }
}