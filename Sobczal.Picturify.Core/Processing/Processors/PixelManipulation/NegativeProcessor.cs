using System.Threading;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Standard.Util;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.PixelManipulation
{
    public class NegativeProcessor : BaseProcessor<NegativeParams, FastImageF>
    {
        public NegativeProcessor(NegativeParams processorParams) : base(processorParams)
        {
        }
        
        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            var pmp = new PointManipulationProcessor(new PointManipulationParams(ChannelSelector.RGB,
                (a, r, g, b, x, y, selector) =>
                {
                    if (selector.UseAlpha) a = 1f - a;
                    if (selector.UseRed) r = 1f - r;
                    if (selector.UseGreen) g = 1f - g;
                    if (selector.UseBlue) b = 1f - b;
                    return (a, r, g, b);
                }, ProcessorParams.WorkingArea));
            fastImage.ExecuteProcessor(pmp);
            return base.Process(fastImage, cancellationToken);
        }
    }
}