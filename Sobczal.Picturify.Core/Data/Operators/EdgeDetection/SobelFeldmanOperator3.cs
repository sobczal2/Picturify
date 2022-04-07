using System.Collections.Generic;

namespace Sobczal.Picturify.Core.Data.Operators.EdgeDetection
{
    public class SobelFeldmanOperator3 : IDualOperatorF
    {
        public List<float[,]> GetX()
        {
            return new List<float[,]> {new float[,] {{1f, 0f, -1f}}, new float[,] {{3f}, {10f}, {3f}}};
        }

        public List<float[,]> GetY()
        {
            return new List<float[,]> {new float[,] {{1f}, {0f}, {-1f}}, new float[,] {{3f, 10f, 3f}}};
        }
    }
}