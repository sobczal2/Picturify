using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Blur
{
    public class GaussianBlurParams : ProcessorParams
    {
        public PSize Range { get; set; }
        public float Sigma { get; set; }
        public EdgeBehaviourSelector.Type EdgeBehaviourType { get; set; }
        public GaussianBlurParams(ChannelSelector channelSelector, PSize range, float sigma, EdgeBehaviourSelector.Type edgeBehaviourType = EdgeBehaviourSelector.Type.Mirror, IAreaSelector workingArea = null) : base(workingArea, channelSelector)
        {
            Range = range;
            Sigma = sigma;
        }
    }
}