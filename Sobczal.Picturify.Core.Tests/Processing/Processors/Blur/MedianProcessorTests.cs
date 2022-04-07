using Sobczal.Picturify.Core.Processing.Blur;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Tests.Processing.Processors.Blur
{
    public class MedianProcessorTests : AbstractProcessorTests<MedianProcessor>
    {
        public MedianProcessorTests()
        {
            UseExactNumersInPixels = false;
        }

        protected override void PopulateWorkingAreaCheckProcessors()
        {
            WorkingAreaCheckProcessor = new MedianProcessor(new MedianParams(ChannelSelector.ARGB, new PSize(5, 5),
                EdgeBehaviourSelector.Type.Constant, null));
        }

        protected override void PopulateChannelSelectorCheckProcessors()
        {
            ChannelSelectorCheckProcessor = new MedianProcessor(new MedianParams(ChannelSelector.ARGB, new PSize(5, 5),
                EdgeBehaviourSelector.Type.Constant, null));
        }

        protected override void PopulateSizeCheckProcessors()
        {
            SizeCheckProcessors.Add(new MedianProcessor(new MedianParams(ChannelSelector.A, new PSize(5, 5),
                EdgeBehaviourSelector.Type.Constant, null)));
            SizeCheckProcessors.Add(new MedianProcessor(new MedianParams(ChannelSelector.RGB, new PSize(1, 5),
                EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(2, 8, 2, 8))));
            SizeCheckProcessors.Add(new MedianProcessor(new MedianParams(ChannelSelector.ARGB, new PSize(5, 1),
                EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(3, 7, 3, 7))));
        }
    }
}