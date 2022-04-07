using Sobczal.Picturify.Core.Processing.Blur;
using Sobczal.Picturify.Core.Processing.Standard.Util;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Tests.Processing.Processors.Standard.Internal
{
    public class PointManipulationProcessorTests : AbstractProcessorTests<PointManipulationProcessor>
    {
        protected override void PopulateChannelSelectorCheckProcessors()
        {
            ChannelSelectorCheckProcessor = new PointManipulationProcessor(new PointManipulationParams(ChannelSelector.ARGB,
                (f, f1, f2, f3, i, i1, selector) =>
                {
                    if (selector.UseAlpha) f = 0;
                    if (selector.UseRed) f1 = 0;
                    if (selector.UseGreen) f2 = 0;
                    if (selector.UseBlue) f3 = 0;
                    return (f, f1, f2, f3);
                }, null));
        }

        protected override void PopulateSizeCheckProcessors()
        {
            SizeCheckProcessors.Add(new PointManipulationProcessor(new PointManipulationParams(ChannelSelector.A,
                (f, f1, f2, f3, i, i1, selector) => (f, f1, f2, f3), null)));
            SizeCheckProcessors.Add(new PointManipulationProcessor(new PointManipulationParams(ChannelSelector.RGB,
                (f, f1, f2, f3, i, i1, selector) => (f, f1, f2, f3), new SquareAreaSelector(2, 8, 2, 8))));
            SizeCheckProcessors.Add(new PointManipulationProcessor(new PointManipulationParams(ChannelSelector.ARGB,
                (f, f1, f2, f3, i, i1, selector) => (f, f1, f2, f3), new SquareAreaSelector(3, 7, 3, 7))));
        }
    }
}