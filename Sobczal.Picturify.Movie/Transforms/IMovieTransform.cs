using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Movie.Transforms
{
    public interface IMovieTransform
    {
        IFastImage GetNext(IFastImage fastImage);
        void Reset();
    }
}