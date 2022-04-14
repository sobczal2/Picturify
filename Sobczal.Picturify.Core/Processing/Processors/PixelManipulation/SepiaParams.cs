using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.PixelManipulation
{
    public class SepiaParams : ProcessorParams
    {
        public SepiaParams(ChannelSelector channelSelector, IAreaSelector workingArea = null) : base(workingArea, channelSelector)
        {
        }
    }
}