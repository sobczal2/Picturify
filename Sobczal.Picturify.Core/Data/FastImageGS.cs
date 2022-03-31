using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.InteropServices;
using Sobczal.Picturify.Core.Utils;
using Size = Sobczal.Picturify.Core.Utils.Size;

namespace Sobczal.Picturify.Core.Data;

/// <summary>
/// Class representing image in Greyscale format.
/// </summary>
public class FastImageGS : FastImage<float>
{
    #region Constructors

    internal FastImageGS(Size size) : base(size)
    {
    }
    
    internal FastImageGS(float[,] pixels) : base(pixels)
    {
    }
    
    internal FastImageGS(Image image) : base(new Size{Width = image.Width, Height = image.Height})
    {
        var widthInBytes = Size.Width * 4;
        var bitmap = new Bitmap(image);
        var arr = new byte[widthInBytes * Size.Height];
        var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
        var ptr = bitmapData.Scan0;
        Marshal.Copy(ptr, arr, 0, arr.Length);
        Parallel.For(0, Size.Height, j =>
        {
            for (var i = 0; i < Size.Width; i++)
            {
                Pixels[i, j] = arr[j * widthInBytes + i * 4 + 2] / 255.0f * 0.3f +
                               arr[j * widthInBytes + i * 4 + 1] / 255.0f * 0.59f +
                               arr[j * widthInBytes + i * 4 + 0] / 255.0f * 0.11f;
            }
        });
    }

    internal FastImageGS(string path) : this(Image.FromFile(path))
    {
    }

    #endregion

    #region Utils
    
    public override ImageType GetImageType()
    {
        return ImageType.GreyScale;
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
                pixels[i, j].X = Pixels[i, j];
                pixels[i, j].Y = Pixels[i, j];
                pixels[i, j].Z = Pixels[i, j];
            }
        });
        return new FastImageARGB(pixels);
    }

    public override IFastImage AsRGB()
    {
        var pixels = new Vector3[Size.Width, Size.Height];
        Parallel.For(0, Size.Height, j =>
        {
            for (var i = 0; i < Size.Width; i++)
            {
                pixels[i, j].X = Pixels[i, j];
                pixels[i, j].Y = Pixels[i, j];
                pixels[i, j].Z = Pixels[i, j];
            }
        });
        return new FastImageRGB(pixels);
    }

    public override IFastImage AsGreyscale()
    {
        //TODO log 
        return this;
    }

    #endregion
}