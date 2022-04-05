using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Standard.Util
{
    public class MergeParams : ProcessorParams
    {
        public delegate float MergingFunc(float in1, float in2, int channel);
        public MergingFunc MergingFunction { get; set; }
        public IFastImage ToMerge { get; set; }
        public MergeParams(ChannelSelector channelSelector, MergingFunc mergingFunction, IFastImage toMerge, IAreaSelector workingArea = null) : base(workingArea, channelSelector)
        {
            MergingFunction = mergingFunction;
            ToMerge = toMerge;
        }
    }
}