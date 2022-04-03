using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing.Filters;

namespace Sobczal.Picturify.Core.Processing
{
    public interface IBaseProcessor
    {
        IFastImage Before(IFastImage fastImage, CancellationToken cancellationToken);
        IFastImage Process(IFastImage fastImage, CancellationToken cancellationToken);
        IFastImage After(IFastImage fastImage, CancellationToken cancellationToken);
        IBaseProcessor AddFilter(BaseFilter filter);
        IBaseProcessor ClearFilters();
    }
}