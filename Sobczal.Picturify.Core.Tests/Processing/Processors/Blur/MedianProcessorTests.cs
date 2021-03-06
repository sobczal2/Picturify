using Sobczal.Picturify.Core.Processing.Blur;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Tests.Processing.Processors.Blur
{
    public class MedianProcessorTests : AbstractProcessorTests<MedianBlurProcessor>
    {
        public MedianProcessorTests()
        {
            UseExactNumersInPixels = false;
        }

        protected override void PopulateWorkingAreaCheckProcessors()
        {
            WorkingAreaCheckProcessor = new MedianBlurProcessor(new MedianBlurParams(ChannelSelector.ARGB, new PSize(5, 5),
                EdgeBehaviourSelector.Type.Constant, null));
        }

        protected override void PopulateChannelSelectorCheckProcessors()
        {
            ChannelSelectorCheckProcessor = new MedianBlurProcessor(new MedianBlurParams(ChannelSelector.ARGB, new PSize(5, 5),
                EdgeBehaviourSelector.Type.Constant, null));
        }

        protected override void PopulateSizeCheckProcessors()
        {
            SizeCheckProcessors.Add(new MedianBlurProcessor(new MedianBlurParams(ChannelSelector.A, new PSize(5, 5),
                EdgeBehaviourSelector.Type.Constant, null)));
            SizeCheckProcessors.Add(new MedianBlurProcessor(new MedianBlurParams(ChannelSelector.RGB, new PSize(1, 5),
                EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(2, 8, 2, 8))));
            SizeCheckProcessors.Add(new MedianBlurProcessor(new MedianBlurParams(ChannelSelector.ARGB, new PSize(5, 1),
                EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(3, 7, 3, 7))));
        }
    }
}