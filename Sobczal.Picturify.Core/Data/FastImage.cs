using System.Drawing;
using System.Drawing.Imaging;
using Sobczal.Picturify.Core.Utils;
using Size = Sobczal.Picturify.Core.Utils.Size;

namespace Sobczal.Picturify.Core.Data;

/// <summary>
/// Abstract class representing image container.
/// </summary>
/// <typeparam name="T">Single pixel representation</typeparam>
public abstract class FastImage<T> : IFastImage
{
    protected T[,] Pixels;
    public Size Size => new Size {Width = Pixels.GetLength(0), Height = Pixels.GetLength(1)};
    
    internal FastImage(Size size)
    {
        Pixels = new T[size.Width, size.Height];
    }

    internal FastImage(T[,] pixels)
    {
        Pixels = pixels;
    }

    #region Utils
    
    public abstract ImageType GetImageType();
    
    #endregion

    #region Save

    public abstract void Save(Stream stream, ImageFormat format);
    public abstract void Save(string path);
    public abstract Task SaveAsync(Stream stream, ImageFormat format, CancellationToken cancellationToken);
    public abstract Task SaveAsync(string path, CancellationToken cancellationToken);
    
    #endregion
    
    #region Transitions
    
    public abstract IFastImage AsARGB();
    public abstract IFastImage AsRGB();
    public abstract IFastImage AsGreyscale();
    
    #endregion
}