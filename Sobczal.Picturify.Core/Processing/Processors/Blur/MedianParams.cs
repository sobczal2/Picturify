using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Blur
{
    public class MedianParams : IProcessorParams
    {
        public PSize PSize { get; set; } = new PSize {Width = 7, Height = 7};
        public EdgeBehaviourSelector.Type EdgeBehaviourType { get; set; }
        public ChannelSelector ChannelSelector { get; set; }
    }
}