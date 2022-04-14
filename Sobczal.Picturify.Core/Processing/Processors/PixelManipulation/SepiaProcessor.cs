using System;
using System.Threading;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Standard.Util;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.PixelManipulation
{
    public class SepiaProcessor : BaseProcessor<SepiaParams, FastImageF>
    {
        public SepiaProcessor(SepiaParams processorParams) : base(processorParams)
        {
        }

        public override IFastImage Before(IFastImage fastImage, CancellationToken cancellationToken)
        {
            if (fastImage.Grayscale) throw new ArgumentException("Can't be grayscale", nameof(fastImage));
            return base.Before(fastImage, cancellationToken);
        }

        public override IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken)
        {
            var pmp = new PointManipulationProcessor(new PointManipulationParams(ChannelSelector.RGB,
                (a, r, g, b, x, y, selector) =>
                {
                    if (selector.UseRed) r = 0.393f * r + 0.769f * g + 0.189f * b;
                    if (selector.UseGreen) g = 0.349f * r + 0.686f * g + 0.168f * b;
                    if (selector.UseBlue) b = 0.272f * r + 0.534f * g + 0.131f * b;
                    return (a, r, g, b);
                }, ProcessorParams.WorkingArea));
            fastImage.ExecuteProcessor(pmp);
            return base.Process(fastImage, cancellationToken);
        }
    }
}