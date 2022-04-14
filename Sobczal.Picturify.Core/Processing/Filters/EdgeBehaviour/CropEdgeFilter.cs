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
            processorParams.WorkingArea?.Resize(_range.Width, -_range.Width, _range.Height, -_range.Height);
            return base.Before(fastImage, processorParams, cancellationToken);
        }

        public override IFastImage After(IFastImage fastImage, ProcessorParams processorParams, CancellationToken cancellationToken)
        {
            fastImage.Crop(processorParams.WorkingArea.AsSquareAreaSelector());
            return base.After(fastImage, processorParams, cancellationToken);
        }
    }
}