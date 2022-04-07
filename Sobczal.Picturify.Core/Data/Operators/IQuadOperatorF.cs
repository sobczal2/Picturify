using System.Collections.Generic;

namespace Sobczal.Picturify.Core.Data.Operators
{
    public interface IQuadOperatorF : IDualOperatorF
    {
        // this diag -> /
        List<float[,]> GetDiag1();
        // this diag -> \
        List<float[,]> GetDiag2();
    }
}