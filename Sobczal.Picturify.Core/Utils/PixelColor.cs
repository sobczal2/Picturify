namespace Sobczal.Picturify.Core.Utils
{
    public class PixelColor
    {
        private float[] _valuesF;
        private byte[] _valuesB;

        public PixelColor(float a, float r, float g, float b)
        {
            _valuesF = new float[4];
            _valuesF[0] = a;
            _valuesF[1] = r;
            _valuesF[2] = g;
            _valuesF[3] = b;
            _valuesB = new byte[4];
            _valuesB[0] = (byte) (a * 255f);
            _valuesB[1] = (byte) (r * 255f);
            _valuesB[2] = (byte) (g * 255f);
            _valuesB[3] = (byte) (b * 255f);
        }

        public PixelColor(byte a, byte r, byte g, byte b) : this(a / 255f, r / 255f, g / 255f, b / 255f)
        {
        }

        public float GetChannelF(int channel)
        {
            return _valuesF[channel];
        }
        public byte GetChannelB(int channel)
        {
            return _valuesB[channel];
        }
    }
}