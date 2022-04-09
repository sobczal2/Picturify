using System.Collections.Generic;
using Sobczal.Picturify.Core.Processing.Standard.Util;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Standard
{
    public class TwoChannelConvolutionParams : ProcessorParams
    {
        public MergeParams.MergingFunc MergingFunc { get; }
        public List<float[,]> ConvolutionMatrixesChannel1 { get; set; }
        public List<float[,]> ConvolutionMatrixesChannel2 { get; set; }
        public EdgeBehaviourSelector.Type EdgeBehaviourType { get; set; }
        public bool UseNonMaximumSuppression { get; set; }

        public TwoChannelConvolutionParams(ChannelSelector channelSelector, List<float[,]> convolutionMatrixesChannel1, List<float[,]> convolutionMatrixesChannel2, MergeParams.MergingFunc mergingFunc, bool useNonMaximumSuppression = false, EdgeBehaviourSelector.Type edgeBehaviourType = EdgeBehaviourSelector.Type.Extend, IAreaSelector workingArea = null) : base(workingArea, channelSelector)
        {
            ConvolutionMatrixesChannel1 = convolutionMatrixesChannel1;
            ConvolutionMatrixesChannel2 = convolutionMatrixesChannel2;
            MergingFunc = mergingFunc;
            UseNonMaximumSuppression = useNonMaximumSuppression;
            EdgeBehaviourType = edgeBehaviourType;
        }
    }
}