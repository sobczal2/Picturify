using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing;

namespace Sobczal.Picturify.Movie.Transforms
{
    public class SingleProcessorTransform<T> : IMovieTransform where T : IBaseProcessor
    {
        private readonly T _processor;
        
        public SingleProcessorTransform(T processor)
        {
            _processor = processor;
        }
        public IFastImage GetNext(IFastImage fastImage)
        {
            return fastImage.ExecuteProcessor(_processor);
        }

        public void Reset()
        {
        }
    }
}