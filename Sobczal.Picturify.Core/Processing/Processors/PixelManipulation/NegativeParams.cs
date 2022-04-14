using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.PixelManipulation
{
    public class NegativeParams : ProcessorParams
    {
        public NegativeParams(ChannelSelector channelSelector, IAreaSelector workingArea = null) : base(workingArea, channelSelector)
        {
        }
    }
}