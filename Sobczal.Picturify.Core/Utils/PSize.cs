namespace Sobczal.Picturify.Core.Utils
{
    public struct PSize
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public PSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}