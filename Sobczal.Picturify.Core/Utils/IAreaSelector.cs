namespace Sobczal.Picturify.Core.Utils
{
    public interface IAreaSelector
    {
        bool ShouldEdit(int x, int y);
        void Resize(int left, int right, int bot, int top);
        void Resize(int horizontal, int vertical);
        SquareAreaSelector AsSquareAreaSelector();
        void Validate(PSize pSize);
        int LeftInclusive { get; }
        int RightExclusive { get; }
        int BotInclusive { get; }
        int TopExclusive { get; }
    }
}