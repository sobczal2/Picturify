using System;
using System.Threading;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Exceptions;
using Sobczal.Picturify.Core.Processing.Standard.Util;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.PixelManipulation
{
    public class GammaCorrectionProcessor : BaseProcessor<GammaCorrectionParams, FastImageF>
    {
        public GammaCorrectionProcessor(GammaCorrectionParams processorParams) : base(processorParams)
        {
            if (processorParams.Gamma <= 0)
                throw new ParamsArgumentException(nameof(processorParams.Gamma), "can't be <= 0");
        }
        
        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            var pmp = new PointManipulationProcessor(new PointManipulationParams(ChannelSelector.RGB,
                (a, r, g, b, x, y, selector) =>
                {
                    if (selector.UseAlpha) a = (float) Math.Pow(a, ProcessorParams.Gamma);
                    if (selector.UseRed) r = (float) Math.Pow(r, ProcessorParams.Gamma);
                    if (selector.UseGreen) g = (float) Math.Pow(g, ProcessorParams.Gamma);
                    if (selector.UseBlue) b = (float) Math.Pow(b, ProcessorParams.Gamma);
                    return (a, r, g, b);
                }, ProcessorParams.WorkingArea));
            fastImage.ExecuteProcessor(pmp);
            return base.Process(fastImage, cancellationToken);
        }
    }
}