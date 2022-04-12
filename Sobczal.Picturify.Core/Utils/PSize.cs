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

        public bool Equals(PSize other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            return obj is PSize other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
            }
        }

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
    }
}