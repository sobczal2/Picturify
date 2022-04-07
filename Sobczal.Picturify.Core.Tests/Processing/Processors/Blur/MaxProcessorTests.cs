using System.Collections.Generic;
using Sobczal.Picturify.Core.Processing.Blur;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Tests.Processing.Processors.Blur
{
    public class MaxProcessorTests : AbstractProcessorTests<MaxProcessor>
    {
        public MaxProcessorTests()
        {
            UseExactNumersInPixels = false;
        }
        protected override void PopulateChannelSelectorCheckProcessors()
        {
            ChannelSelectorCheckProcessor = new MaxProcessor(new MaxParams(ChannelSelector.ARGB, new PSize(5, 5),
                EdgeBehaviourSelector.Type.Constant, null));
        }

        protected override void PopulateSizeCheckProcessors()
        {
            SizeCheckProcessors.Add(new MaxProcessor(new MaxParams(ChannelSelector.A, new PSize(5, 5),
                EdgeBehaviourSelector.Type.Constant, null)));
            SizeCheckProcessors.Add(new MaxProcessor(new MaxParams(ChannelSelector.RGB, new PSize(1, 5),
                EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(2, 8, 2, 8))));
            SizeCheckProcessors.Add(new MaxProcessor(new MaxParams(ChannelSelector.ARGB, new PSize(5, 1),
                EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(3, 7, 3, 7))));
        }
    }
}