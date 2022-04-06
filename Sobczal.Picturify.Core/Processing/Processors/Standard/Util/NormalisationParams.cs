using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Standard.Util
{
    public class NormalisationParams : ProcessorParams
    {
        public delegate float PreNormalisationFunc(float value);
        public PreNormalisationFunc PreNormalisationFunction { get; set; }
        public float UpperBound { get; set; }
        public float LowerBound { get; set; }

        public NormalisationParams(ChannelSelector channelSelector, PreNormalisationFunc preNormalisationFunction = null, float lowerBound = 0f, float upperBound = 1f,IAreaSelector workingArea = null) : base(workingArea, channelSelector)
        {
            PreNormalisationFunction = preNormalisationFunction;
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }
    }
}