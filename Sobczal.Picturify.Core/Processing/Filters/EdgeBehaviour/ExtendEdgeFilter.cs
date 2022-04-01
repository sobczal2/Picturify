using System;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Filters;

namespace Sobczal.Picturify.Core.Utils
{
    public class ExtendEdgeFilter : BaseFilter
    {
        private readonly Size _kernelSize;

        public ExtendEdgeFilter(Size kernelSize)
        {
            if (kernelSize.Width % 2 != 1 || kernelSize.Height % 2 != 1)
                throw new ArgumentException("Kernel size must be 2n+1x2n+1.");
            _kernelSize = kernelSize;
        }
        public override FastImage Before(FastImage fastImage)
        {
            return base.Before(fastImage);
        }

        public void BeforeProcessingFunction(float[,,] pixels)
        {
            
        }

        public override FastImage After(FastImage fastImage)
        {
            var areaSelector = new SquareAreaSelector(_kernelSize.Width / 2, _kernelSize.Height / 2,
                fastImage.Size.Width - _kernelSize.Width / 2, fastImage.Size.Height - _kernelSize.Height / 2);
            fastImage.Crop(areaSelector);
            fastImage.AreaSelector = areaSelector;
            return base.After(fastImage);
        }
    }
}