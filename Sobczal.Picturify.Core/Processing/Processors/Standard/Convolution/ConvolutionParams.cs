using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Standard
{
    public class ConvolutionParams : ProcessorParams
    {
        public EdgeBehaviourSelector.Type EdgeBehaviourType { get; set; }
        public float[,] ConvolutionMatrix { get; set; }
        public ConvolutionParams(ChannelSelector channelSelector, float[,] convolutionMatrix, EdgeBehaviourSelector.Type edgeBehaviourType = EdgeBehaviourSelector.Type.Extend, IAreaSelector workingArea = null) : base(workingArea, channelSelector)
        {
            ConvolutionMatrix = convolutionMatrix;
            EdgeBehaviourType = edgeBehaviourType;
        }
    }
}