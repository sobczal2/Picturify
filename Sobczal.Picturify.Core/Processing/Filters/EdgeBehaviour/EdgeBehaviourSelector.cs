using System;
using Sobczal.Picturify.Core.Processing.Filters;
using Sobczal.Picturify.Core.Processing.Filters.EdgeBehaviour;

namespace Sobczal.Picturify.Core.Utils
{
    public class EdgeBehaviourSelector
    {
        public enum Type
        {
            Extend,
            Wrap,
            Mirror,
            Crop,
            Constant,
            AvoidOverlap
        }

        public static BaseFilter GetFilter(Type type, Size kernelSize)
        {
            switch (type)
            {
                case Type.Extend:
                    return new ExtendEdgeFilter(kernelSize);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}