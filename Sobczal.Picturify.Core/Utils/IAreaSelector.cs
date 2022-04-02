namespace Sobczal.Picturify.Core.Utils
{
    public interface IAreaSelector
    {
        bool ShouldEdit(int x, int y);
        void AddBorder(int left, int bottom, int right, int top);
        void Crop(int left, int bottom, int right, int top);
        (int left, int bottom, int right, int top) GetBounds();
    }
}