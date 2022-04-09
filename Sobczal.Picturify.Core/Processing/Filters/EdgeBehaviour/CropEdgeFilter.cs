using System.Threading;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Processing.Filters.EdgeBehaviour
{
    public class CropEdgeFilter : BaseFilter
    {
        private readonly PSize _range;

        public CropEdgeFilter(PSize range)
        {
            _range = range;
        }
        public override IFastImage Before(IFastImage fastImage, ProcessorParams processorParams, CancellationToken cancellationToken)
        {
            processorParams.WorkingArea?.Resize(-_range.Width, -_range.Height);
            return base.Before(fastImage, processorParams, cancellationToken);
        }

        public override IFastImage After(IFastImage fastImage, ProcessorParams processorParams, CancellationToken cancellationToken)
        {
            fastImage.Crop(new SquareAreaSelector(_range.Width, fastImage.PSize.Width - _range.Width, _range.Height,
                fastImage.PSize.Height - _range.Height));
            return base.After(fastImage, processorParams, cancellationToken);
        }
    }
}