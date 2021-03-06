using System;
using Sobczal.Picturify.Core.Processing.Filters;
using Sobczal.Picturify.Core.Processing.Filters.EdgeBehaviour;
using Sobczal.Picturify.Core.Processing.Standard;

namespace Sobczal.Picturify.Core.Utils
{
    /// <summary>
    /// Class used to get edge behaviour filter.
    /// <example>
    /// See <see cref="RollingBucketProcessor"/> for intended usage.
    /// </example>
    /// </summary>
    public class EdgeBehaviourSelector
    {
        /// <summary>
        /// Available types of behaviour
        /// If you are curious how each of them work check out
        /// https://en.wikipedia.org/wiki/Kernel_(image_processing)#Edge_Handling
        /// </summary>
        public enum Type
        {
            Extend,
            Wrap,
            Mirror,
            Crop,
            Constant
        }

        /// <summary>
        /// Method used to get instance of <see cref="BaseFilter"/> of desired <see cref="type"/>
        /// and <see cref="range"/>.
        /// </summary>
        /// <param name="type">Desired <see cref="Type"/> of <see cref="BaseFilter"/></param>
        /// <param name="range">Desired range represented by <see cref="PSize"/> width is horizontal range - how much image kernel used
        /// in processing needs from left and right (if image kernel is 5x5, it needs 2px from left and 2px from right so horizontal range is 2). </param>
        /// <param name="defaultValue">Default value for a pixel</param>
        /// <returns><see cref="BaseFilter"/> of adequate type, ready to use.</returns>
        /// <exception cref="NotImplementedException">thrown when <see cref="BaseFilter"/> of
        /// <see cref="Type"/> is not yet implemented.</exception>
        public static BaseFilter GetFilter(Type type, PSize range, PixelColor defaultValue = null)
        {
            switch (type)
            {
                case Type.Extend:
                    return new ExtendEdgeFilter(range);
                case Type.Wrap:
                    return new WrapEdgeFilter(range);
                case Type.Mirror:
                    return new MirrorEdgeFilter(range);
                case Type.Crop:
                    return new CropEdgeFilter(range);
                case Type.Constant:
                    return new ConstantEdgeFilter(range, defaultValue);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}