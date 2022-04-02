using System;

namespace Sobczal.Picturify.Core.Utils
{
    public class SquareAreaSelector : IAreaSelector
    {
        public int Left { get; private set; }
        public int Bottom { get; private set; }
        public int Right { get; private set; }
        public int Top { get; private set; }

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

        public void AddBorder(int left, int bottom, int right, int top)
        {
            Left += left;
            Right += left;
            Bottom += bottom;
            Top += bottom;
        }

        public void Crop(int left, int bottom, int right, int top)
        {
            Left -= left;
            Right -= left;
            Bottom -= bottom;
            Top -= bottom;
        }

        public bool Validate(PSize pSize)
        {
            return Left >= 0 && Right < pSize.Width && Bottom >= 0 && Top <= pSize.Height && Left < Right &&
                   Bottom < Top;
        }

        public (int left, int bottom, int right, int top) GetBounds()
        {
            return (Left, Bottom, Right, Top);
        }
    }
}