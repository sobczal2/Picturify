using System.Collections.Generic;

namespace Sobczal.Picturify.Core.Data.Operators.EdgeDetection
{
    public class SobelOperator5 : IDualOperatorF
    {
        public List<float[,]> GetX()
        {
            return new List<float[,]> {new float[,] {{-2f, -1f, 0f, 1f, 2f}}, new float[,] {{1f}, {1f}, {2f}, {1f}, {1f}}};
        }

        public List<float[,]> GetY()
        {
            return new List<float[,]> {new float[,] {{-2f}, {-1f}, {0f}, {1f}, {2f}}, new float[,] {{1f, 1f, 2f, 1f, 1f}}};
        }
    }
}