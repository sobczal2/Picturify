using Sobczal.Picturify.Core.Processing.Standard.Util;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Tests.Processing.Processors.Standard.Util
{
    public class NormalisationProcessorTests : AbstractProcessorTests<NormalisationProcessor>
    {
        protected override void PopulateWorkingAreaCheckProcessors()
        {
            WorkingAreaCheckProcessor = new NormalisationProcessor(new NormalisationParams(ChannelSelector.ARGB));
        }

        protected override void PopulateChannelSelectorCheckProcessors()
        {
            ChannelSelectorCheckProcessor = new NormalisationProcessor(new NormalisationParams(ChannelSelector.ARGB));
        }

        protected override void PopulateSizeCheckProcessors()
        {
            SizeCheckProcessors.Add(new NormalisationProcessor(new NormalisationParams(ChannelSelector.ARGB)));
        }
    }
}