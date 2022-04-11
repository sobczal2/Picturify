using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.EdgeDetection
{
    public class NonMaximumGradientSuppressionParams : ProcessorParams
    {
        public FastImageF Gy { get; set; }
        public EdgeBehaviourSelector.Type EdgeBehaviourType { get; set; }
        public NonMaximumGradientSuppressionParams(IAreaSelector workingArea, ChannelSelector channelSelector, FastImageF gy, EdgeBehaviourSelector.Type edgeBehaviourType = EdgeBehaviourSelector.Type.Mirror) : base(workingArea, channelSelector)
        {
            Gy = gy;
            EdgeBehaviourType = edgeBehaviourType;
        }
    }
}