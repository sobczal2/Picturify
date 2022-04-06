using System.Collections.Generic;

namespace Sobczal.Picturify.Core.Data.Operators
{
    public interface IDualOperatorF
    {
        List<float[,]> GetX();
        List<float[,]> GetY();
    }
}