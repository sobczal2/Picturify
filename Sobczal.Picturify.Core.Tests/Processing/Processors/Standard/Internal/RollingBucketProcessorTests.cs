using Sobczal.Picturify.Core.Processing.Standard;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Tests.Processing.Processors.Standard.Internal
{
    public class RollingBucketProcessorTests : AbstractProcessorTests<RollingBucketProcessor>
    {
        public RollingBucketProcessorTests()
        {
            UseExactNumersInPixels = false;
        }

        protected override void PopulateWorkingAreaCheckProcessors()
        {
            WorkingAreaCheckProcessor = new RollingBucketProcessor(new RollingBucketParams(ChannelSelector.ARGB,
                new PSize(5, 5), (ushorts, b) => 0, EdgeBehaviourSelector.Type.Constant, null));
        }

        protected override void PopulateChannelSelectorCheckProcessors()
        {
            ChannelSelectorCheckProcessor = new RollingBucketProcessor(new RollingBucketParams(ChannelSelector.ARGB,
                new PSize(5, 5), (ushorts, b) => 0, EdgeBehaviourSelector.Type.Constant, null));
        }

        protected override void PopulateSizeCheckProcessors()
        {
            SizeCheckProcessors.Add(new RollingBucketProcessor(new RollingBucketParams(ChannelSelector.A,
                new PSize(5, 5), (ushorts, b) => 0, EdgeBehaviourSelector.Type.Constant, null)));
            SizeCheckProcessors.Add(new RollingBucketProcessor(new RollingBucketParams(ChannelSelector.RGB,
                new PSize(1, 5), (ushorts, b) => 0, EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(2, 8, 2, 8))));
            SizeCheckProcessors.Add(new RollingBucketProcessor(new RollingBucketParams(ChannelSelector.ARGB,
                new PSize(5, 1), (ushorts, b) => 0, EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(3, 7, 3, 7))));
        }
        }
    }