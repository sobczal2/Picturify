using System.Collections.Generic;
using Sobczal.Picturify.Core.Data;
using Sobczal.Picturify.Core.Processing;

namespace Sobczal.Picturify.Movie.Transforms
{
    public class MultipleProcessorTransform : IMovieTransform
    {
        private List<IBaseProcessor> _processors;

        public MultipleProcessorTransform(List<IBaseProcessor> processors)
        {
            _processors = processors;
        }


        public IFastImage GetNext(IFastImage fastImage)
        {
            foreach (var processor in _processors)
            {
                fastImage = fastImage.ExecuteProcessor(processor);
            }

            return fastImage;
        }

        public void Reset()
        {
        }
    }
}