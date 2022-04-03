using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Processing;

namespace Sobczal.Picturify.Core.Data
{
    /// <summary>
    /// Extensions for <see cref="FastImage{T}"/> to 
    /// </summary>
    public static class FastImageExtensions
    {
        /// <summary>
        /// <see cref="IFastImage"/> extension method for using any <see cref="IBaseProcessor"/> on an image.
        /// </summary>
        /// <param name="fastImage"><see cref="IFastImage"/> to edit using <see cref="processor"/>.</param>
        /// <param name="processor"><see cref="IBaseProcessor"/> processor to use on <see cref="IFastImage"/>.</param>
        /// <returns>Edited <see cref="IFastImage"/>.</returns>
        public static IFastImage ExecuteProcessor(this IFastImage fastImage, IBaseProcessor processor)
        {
            return ExecuteProcessorAsync(fastImage, processor, CancellationToken.None).Result;
        }
        
        /// <summary>
        /// <see cref="IFastImage"/> asynchronous extension method for using any <see cref="IBaseProcessor"/> on an image.
        /// Asynchronous version of <see cref="ExecuteProcessor"/>.
        /// </summary>
        /// <param name="fastImage"><see cref="IFastImage"/> to edit using <see cref="processor"/>.</param>
        /// <param name="processor"><see cref="IBaseProcessor"/> processor to use on <see cref="IFastImage"/>.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> allowing to cancel operation.</param>
        /// <returns><see cref="Task{T}"/> with edited <see cref="IFastImage"/></returns>
        public static async Task<IFastImage> ExecuteProcessorAsync(this IFastImage fastImage, IBaseProcessor processor, CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();
            sw.Start();
            await Task.Factory.StartNew(() =>
            {
                fastImage = processor.Before(fastImage, cancellationToken);
                fastImage = processor.Process(fastImage, cancellationToken);
                fastImage = processor.After(fastImage, cancellationToken);
            });
            sw.Stop();
            PicturifyConfig.LogTime(processor.GetType().Name, sw.ElapsedMilliseconds);
            return fastImage;
        }
    }
}