using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Blur
{
    public class MedianParams : ProcessorParams
    {
        public PSize Range { get; set; }
        public EdgeBehaviourSelector.Type EdgeBehaviourType { get; set; }
        public MedianParams(ChannelSelector channelSelector, PSize range, EdgeBehaviourSelector.Type edgeBehaviourType = EdgeBehaviourSelector.Type.Extend, IAreaSelector workingArea = null) : base(workingArea, channelSelector)
        {
            Range = range;
            EdgeBehaviourType = edgeBehaviourType;
        }
    }
}