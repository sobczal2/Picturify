using System;

namespace Sobczal.Picturify.Core.Utils
{
    public class SquareAreaSelector : IAreaSelector
    {
        //first index from left edited

        public int LeftInclusive { get; private set; }
        public int RightExclusive { get; private set; }
        public int BotInclusive { get; private set; }
        public int TopExclusive { get; private set; }

        public int Width => RightExclusive - LeftInclusive;
        public int Height => TopExclusive - BotInclusive;

        public SquareAreaSelector(int leftInclusive, int rightExclusive, int botInclusive, int topExclusive)
        {
            if (leftInclusive < 0 || rightExclusive < 0 || botInclusive < 0 || topExclusive < 0)
                throw new ArgumentException("Invalid arguments");
            LeftInclusive = leftInclusive;
            RightExclusive = rightExclusive;
            BotInclusive = botInclusive;
            TopExclusive = topExclusive;
        }
        public bool ShouldEdit(int x, int y)
        {
            return x >= LeftInclusive && x < RightExclusive && y >= BotInclusive &&
                   y < TopExclusive;
        }

        public void Resize(int left, int right, int bot, int top)
        {
            if (left + LeftInclusive < 0 || right + RightExclusive < 0 || bot + BotInclusive < 0 ||
                top + TopExclusive < 0)
                throw new ArgumentException("Argumants invalid.");
            LeftInclusive += left;
            RightExclusive += right;
            BotInclusive += bot;
            TopExclusive += top;
        }

        public void Resize(int horizontal, int vertical)
        {
            Resize(horizontal, horizontal, vertical, vertical);
        }
        
        public void Validate(PSize pSize)
        {
            LeftInclusive = Math.Max(0, LeftInclusive);
            BotInclusive = Math.Max(0, BotInclusive);
            RightExclusive = Math.Min(pSize.Width, RightExclusive);
            TopExclusive = Math.Min(pSize.Height, TopExclusive);
            if (LeftInclusive > RightExclusive || BotInclusive > TopExclusive)
                throw new ArgumentException("Invalid area selector.");
        }

        public IAreaSelector GetCopy()
        {
            return new SquareAreaSelector(LeftInclusive, RightExclusive, BotInclusive, TopExclusive);
        }

        public SquareAreaSelector AsSquareAreaSelector()
        {
            return this;
        }
    }
}