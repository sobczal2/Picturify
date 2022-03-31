using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.InteropServices;
using Sobczal.Picturify.Core.Utils;
using Size = Sobczal.Picturify.Core.Utils.Size;

namespace Sobczal.Picturify.Core.Data;

/// <summary>
/// Class representing image in RGB format
/// </summary>
public class FastImageRGB : FastImage<Vector3>
{
    #region Constructors
    
    internal FastImageRGB(Size size) : base(size)
    {
    }

    internal FastImageRGB(Vector3[,] pixels) : base(pixels)
    {
    }
    
    internal FastImageRGB(Image image) : base(new Size{Width = image.Width, Height = image.Height})
    {
        var widthInBytes = Size.Width * 4;
        var bitmap = new Bitmap(image);
        var arr = new byte[widthInBytes * Size.Height];
        var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
        var ptr = bitmapData.Scan0;
        Marshal.Copy(ptr, arr, 0, arr.Length);
        Parallel.For(0, Size.Height, j =>
        {
            for (var i = 0; i < Size.Width; i++)
            {
                Pixels[i, j].X = arr[j * widthInBytes + i * 4 + 2] / 255.0f;
                Pixels[i, j].Y = arr[j * widthInBytes + i * 4 + 1] / 255.0f;
                Pixels[i, j].Z = arr[j * widthInBytes + i * 4 + 0] / 255.0f;
            }
        });
    }

    internal FastImageRGB(string path) : this(Image.FromFile(path))
    {
    }
    
    #endregion
    
    #region Utils
    
    public override ImageType GetImageType()
    {
        return ImageType.RGB;
    }
    
    #endregion
    
    #region Save
    
    public override void Save(Stream stream, ImageFormat format)
    {
        AsARGB().Save(stream, format);
    }

    public override void Save(string path)
    {
        AsARGB().Save(path);
    }

    public override async Task SaveAsync(Stream stream, ImageFormat format, CancellationToken cancellationToken)
    {
        await AsARGB().SaveAsync(stream, format, cancellationToken);
    }

    public override async Task SaveAsync(string path, CancellationToken cancellationToken)
    {
        await AsARGB().SaveAsync(path, cancellationToken);
    }

    #endregion
    
    #region Transistions
    
    public override IFastImage AsARGB()
    {
        var pixels = new Vector4[Size.Width, Size.Height];
        Parallel.For(0, Size.Height, j =>
        {
            for (var i = 0; i < Size.Width; i++)
            {
                pixels[i, j].W = 1.0f;
                pixels[i, j].X = Pixels[i, j].X;
                pixels[i, j].Y = Pixels[i, j].Y;
                pixels[i, j].Z = Pixels[i, j].Z;
            }
        });
        return new FastImageARGB(pixels);
    }

    public override IFastImage AsRGB()
    {
        //TODO log
        return this;
    }

    public override IFastImage AsGreyscale()
    {
        var pixels = new float[Size.Width, Size.Height];
        Parallel.For(0, Size.Height, j =>
        {
            for (var i = 0; i < Size.Width; i++)
            {
                pixels[i, j] = Pixels[i, j].X * 0.3f + Pixels[i, j].Y * 0.59f + Pixels[i, j].Z * 0.11f;
            }
        });
        return new FastImageGS(pixels);
    }
    
    #endregion
}