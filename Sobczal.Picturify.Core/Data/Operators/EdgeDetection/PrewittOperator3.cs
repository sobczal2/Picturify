using System.Collections.Generic;

namespace Sobczal.Picturify.Core.Data.Operators.EdgeDetection
{
    public class PrewittOperator3 : IQuadOperatorF
    {
        public List<float[,]> GetX()
        {
            return new List<float[,]> {new float[,] {{1f, 0f, -1f}}, new float[,] {{1f}, {1f}, {1f}}};
        }

        public List<float[,]> GetY()
        {
            return new List<float[,]> {new float[,] {{1f}, {0f}, {-1f}}, new float[,] {{1f, 1f, 1f}}};
        }

        public List<float[,]> GetDiag1()
        {
            return new List<float[,]> {new float[,] {{1f, 1f, 0f}, {1f, 0f, -1f}, {0f, -1f, -1f}}};
        }

        public List<float[,]> GetDiag2()
        {
            return new List<float[,]> {new float[,] {{0f, -1f, -1f}, {1f, 0f, -1f}, {1f, 1f, 0f}}};
        }
    }
}