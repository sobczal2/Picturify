using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Processing;

namespace Sobczal.Picturify.Core.Data
{
    public static class FastImageExtensions
    {
        public static IFastImage ExecuteProcessor(this IFastImage fastImage, IBaseProcessor processor)
        {
            return ExecuteProcessorAsync(fastImage, processor, CancellationToken.None).Result;
        }
        public static async Task<IFastImage> ExecuteProcessorAsync(this IFastImage fastImage, IBaseProcessor processor, CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();
            sw.Start();
            fastImage = await processor.Before(fastImage, cancellationToken);
            fastImage = await processor.Process(fastImage, cancellationToken);
            fastImage = await processor.After(fastImage, cancellationToken);
            sw.Stop();
            PicturifyConfig.LogTime(processor.GetType().Name, sw.ElapsedMilliseconds);
            return fastImage;
        }
    }
}