using Sobczal.Picturify.Core.Data.Operators;
using Sobczal.Picturify.Core.Data.Operators.EdgeDetection;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.EdgeDetection
{
    public class DualOperatorParams : ProcessorParams
    {
        public IDualOperatorF ConvolutionOperator { get; set; }
        public OperatorBeforeNormalizationFunc MappingFunc { get; set; }
        public EdgeBehaviourSelector.Type EdgeBehaviourType { get; set; }
        public float LowerBoundForNormalisation { get; set; }
        public float UpperBoundForNormalisation { get; set; }
        public bool UseNonMaximumSuppression { get; set; }

        public DualOperatorParams(ChannelSelector channelSelector, IDualOperatorF convolutionOperator,
            OperatorBeforeNormalizationFunc mappingFunc, float lowerBoundForNormalisation = 0f,
            float upperBoundForNormalisation = 1f,
            bool useNonMaximumSuppression = false,
            EdgeBehaviourSelector.Type edgeBehaviourType = EdgeBehaviourSelector.Type.Extend,
            IAreaSelector workingArea = null) : base(workingArea, channelSelector)

        {
            ConvolutionOperator = convolutionOperator;
            MappingFunc = mappingFunc;
            LowerBoundForNormalisation = lowerBoundForNormalisation;
            UpperBoundForNormalisation = upperBoundForNormalisation;
            UseNonMaximumSuppression = useNonMaximumSuppression;
            EdgeBehaviourType = edgeBehaviourType;
        }
    }
}