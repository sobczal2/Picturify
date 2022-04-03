using System;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Blur
{
    public class MaxParams : ProcessorParams
    {
        public PSize Range { get; set; }
        public EdgeBehaviourSelector.Type EdgeBehaviourType { get; set; }

        public MaxParams(ChannelSelector channelSelector, PSize range, EdgeBehaviourSelector.Type edgeBehaviourType = EdgeBehaviourSelector.Type.Extend, IAreaSelector workingArea = null) : base(workingArea, channelSelector)
        {
            Range = range;
            EdgeBehaviourType = edgeBehaviourType;
        }
    }
}