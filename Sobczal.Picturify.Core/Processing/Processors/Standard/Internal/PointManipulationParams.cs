using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Standard.Util
{
    public class PointManipulationParams : ProcessorParams
    {
        public delegate (float a, float r, float g, float b) PointManipulationFunc(float a, float r, float g, float b, int x, int y, ChannelSelector channelSelector);

        public PointManipulationFunc PointManipulationFunction { get; set; }
        
        public PointManipulationParams(ChannelSelector channelSelector, PointManipulationFunc pointManipulationFunction, IAreaSelector workingArea = null) : base(workingArea, channelSelector)
        {
            PointManipulationFunction = pointManipulationFunction;
        }
    }
}