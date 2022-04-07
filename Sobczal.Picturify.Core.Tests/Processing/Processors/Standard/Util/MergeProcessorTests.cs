using Sobczal.Picturify.Core.Processing.Standard.Util;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Tests.Processing.Processors.Standard.Util
{
    public class MergeProcessorTests : AbstractProcessorTests<MergeProcessor>
    {
        protected override void PopulateChannelSelectorCheckProcessors()
        {
            ChannelSelectorCheckProcessor = new MergeProcessor(new MergeParams(ChannelSelector.A, null, null, null));

        }

        protected override void PopulateSizeCheckProcessors()
        {
            SizeCheckProcessors.Add(new MergeProcessor(new MergeParams(ChannelSelector.A, null, null, null)));
            SizeCheckProcessors.Add(new MergeProcessor(new MergeParams(ChannelSelector.RGB, null, null, null)));
            SizeCheckProcessors.Add(new MergeProcessor(new MergeParams(ChannelSelector.ARGB, null, null, null)));
        }
    }
}