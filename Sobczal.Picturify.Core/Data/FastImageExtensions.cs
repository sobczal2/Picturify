using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Processing;

namespace Sobczal.Picturify.Core.Data
{
    public static class FastImageExtensions
    {
        public static IFastImage ExecuteProcessor(this IFastImage fastImage, IBaseProcessor processor)
        {
            return fastImage.ExecuteProcessorAsync(processor, CancellationToken.None).Result;
        }
        public static async Task<IFastImage> ExecuteProcessorAsync(this IFastImage fastImage, IBaseProcessor processor, CancellationToken cancellationToken)
        {
            await processor.Before(fastImage, CancellationToken.None);
            await processor.Process(fastImage, CancellationToken.None);
            await processor.After(fastImage, CancellationToken.None);
            return fastImage;
        }
    }
}