using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.PixelManipulation
{
    public class GammaCorrectionParams : ProcessorParams
    {
        public float Gamma { get; set; }
        public GammaCorrectionParams(ChannelSelector channelSelector, float gamma, IAreaSelector workingArea = null) : base(workingArea, channelSelector)
        {
            Gamma = gamma;
        }
    }
}