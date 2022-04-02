using System;

namespace Sobczal.Picturify.Core.Utils
{
    public class SquareAreaSelector : IAreaSelector
    {
        public int Left { get; }
        public int Bottom { get; }
        public int Right { get; }
        public int Top { get; }

        public int Width => Right - Left + 1;
        public int Height => Top - Bottom + 1;

        public SquareAreaSelector(int left, int bottom, int right, int top)
        {
            if (left >= right || bottom >= top)
                throw new ArgumentException("left must be bigger than right, top must be bigger than bottom");
            Left = left;
            Bottom = bottom;
            Right = right;
            Top = top;
        }
        public bool ShouldEdit(int x, int y)
        {
            return x >= Left && x <= Right && y <= Top && y >= Bottom;
        }

        public bool Validate(PSize pSize)
        {
            return Left >= 0 && Right < pSize.Width && Bottom >= 0 && Top <= pSize.Height && Left < Right &&
                   Bottom < Top;
        }
    }
}