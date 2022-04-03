using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing
{
    public class ProcessorParams
    {
        public IAreaSelector WorkingArea { get; set; }
        public ChannelSelector ChannelSelector { get; set; }

        public ProcessorParams(IAreaSelector workingArea, ChannelSelector channelSelector)
        {
            WorkingArea = workingArea;
            ChannelSelector = channelSelector;
        }
    }
}