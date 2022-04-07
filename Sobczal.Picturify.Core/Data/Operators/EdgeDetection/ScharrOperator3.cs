using System.Collections.Generic;

namespace Sobczal.Picturify.Core.Data.Operators.EdgeDetection
{
    public class ScharrOperator3 : IDualOperatorF
    {
        public List<float[,]> GetX()
        {
            return new List<float[,]> {new float[,] {{1f, 0f, -1f}}, new float[,] {{47f}, {162f}, {47f}}};
        }

        public List<float[,]> GetY()
        {
            return new List<float[,]> {new float[,] {{1f}, {0f}, {-1f}}, new float[,] {{47f, 162f, 47f}}};
        }
    }
}