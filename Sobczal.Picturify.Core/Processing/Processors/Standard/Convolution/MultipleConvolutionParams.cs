using System.Collections.Generic;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Standard
{
    public class MultipleConvolutionParams : ProcessorParams
    {
        public EdgeBehaviourSelector.Type EdgeBehaviourType { get; set; }
        public List<float[,]> ConvolutionMatrixes { get; set; }
        
        public MultipleConvolutionParams(ChannelSelector channelSelector, List<float[,]> convolutionMatrixes, EdgeBehaviourSelector.Type edgeBehaviourType = EdgeBehaviourSelector.Type.Extend, IAreaSelector workingArea = null) : base(workingArea, channelSelector)
        {
            ConvolutionMatrixes = convolutionMatrixes;
            EdgeBehaviourType = edgeBehaviourType;
        }
    }
}