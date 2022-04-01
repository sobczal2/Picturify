using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing
{
    public abstract class BaseProcessor
    {
        public virtual FastImage Before(FastImage fastImage, CancellationToken cancellationToken)
        {
            PicturifyConfig.LogInfo($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} called.");
            return fastImage;
        }

        public virtual async Task<FastImage> BeforeAsync(FastImage fastImage, CancellationToken cancellationToken)
        {
            return await Task.Factory.StartNew(() => Before(fastImage, cancellationToken));
        }

        public abstract FastImage Process(FastImage fastImage, CancellationToken cancellationToken);

        public virtual async Task<FastImage> ProcessAsync(FastImage fastImage, CancellationToken cancellationToken)
        {
            return await Task.Factory.StartNew(() => Process(fastImage, cancellationToken));
        }
        
        public virtual FastImage After(FastImage fastImage, CancellationToken cancellationToken)
        {
            PicturifyConfig.LogInfo($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} called.");
            return fastImage;
        }

        public virtual async Task<FastImage> AfterAsync(FastImage fastImage, CancellationToken cancellationToken)
        {
            return await Task.Factory.StartNew(() => After(fastImage, cancellationToken));
        }
    }
}