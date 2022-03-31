using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.InteropServices;
using Sobczal.Picturify.Core.Utils;
using Size = Sobczal.Picturify.Core.Utils.Size;

namespace Sobczal.Picturify.Core.Data;

/// <summary>
/// Class representing image in ARGB format.
/// </summary>
public class FastImageARGB : FastImage<Vector4>
{
    #region Constructors
    
    internal FastImageARGB(Size size) : base(size)
    {
    }

    internal FastImageARGB(Vector4[,] pixels) : base(pixels)
    {
        
    }

    internal FastImageARGB(Image image) : base(new Size{Width = image.Width, Height = image.Height})
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
                Pixels[i, j].W = arr[j * widthInBytes + i * 4 + 3] / 255.0f;
                Pixels[i, j].X = arr[j * widthInBytes + i * 4 + 2] / 255.0f;
                Pixels[i, j].Y = arr[j * widthInBytes + i * 4 + 1] / 255.0f;
                Pixels[i, j].Z = arr[j * widthInBytes + i * 4 + 0] / 255.0f;
            }
        });
    }

    internal FastImageARGB(string path) : this(Image.FromFile(path))
    {
    }
    
    #endregion
    
    #region Utils
    
    public override ImageType GetImageType()
    {
        return ImageType.ARGB;
    }
    
    #endregion

    #region Save
    
    public override void Save(Stream stream, ImageFormat format)
    {
        var bitmap = GetBitmap(CancellationToken.None);
        try
        {
            bitmap.Save(stream, format);
        }
        catch (Exception)
        {
            //TODO log error
        }
    }

    public override void Save(string path)
    {
        var bitmap = GetBitmap(CancellationToken.None);
        var format = GetImageFormat(path);
        try
        {
            bitmap.Save(path, format);
        }
        catch (Exception)
        {
            //TODO log error
        }
    }

    public override async Task SaveAsync(Stream stream, ImageFormat format, CancellationToken cancellationToken)
    {
        await Task.Factory.StartNew(() =>
        {
            var bitmap = GetBitmap(cancellationToken);
            try
            {
                bitmap.Save(stream, format);
            }
            catch (Exception)
            {
                //TODO log error
            }
        });
    }

    public override async Task SaveAsync(string path, CancellationToken cancellationToken)
    {
        await Task.Factory.StartNew(() =>
        {
            var bitmap = GetBitmap(cancellationToken);
            var format = GetImageFormat(path);
            try
            {
                bitmap.Save(path, format);
            }
            catch (Exception)
            {
                //TODO log error
            }
        });
    }

    private Bitmap GetBitmap(CancellationToken cancellationToken)
    {
        var widthInBytes = Size.Width * 4;
        var arr = new byte[widthInBytes * Size.Height];
        var po = new ParallelOptions();
        po.CancellationToken = cancellationToken;
        Parallel.For(0, Size.Height, po, j =>
        {
            for (var i = 0; i < Size.Width; i++)
            {
                arr[j * widthInBytes + i * 4 + 3] = (byte) (Pixels[i, j].W * 255.0f);
                arr[j * widthInBytes + i * 4 + 2] = (byte) (Pixels[i, j].X * 255.0f);
                arr[j * widthInBytes + i * 4 + 1] = (byte) (Pixels[i, j].Y * 255.0f);
                arr[j * widthInBytes + i * 4 + 0] = (byte) (Pixels[i, j].Z * 255.0f);
            }
        });
        var bitmap = new Bitmap(Size.Width, Size.Height);
        var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
        var ptr = bitmapData.Scan0;
        Marshal.Copy(arr, 0, ptr, arr.Length);
        return bitmap;
    }

    private ImageFormat GetImageFormat(string path)
    {
        ImageFormat format;
        switch (Path.GetExtension(path))
        {
            case ".jpg":
                format = ImageFormat.Jpeg;
                break;
            case ".jpeg":
                format = ImageFormat.Jpeg;
                break;
            case ".png":
                format = ImageFormat.Png;
                break;
            case ".tiff":
                format = ImageFormat.Tiff;
                break;
            case ".bmp":
                format = ImageFormat.Bmp;
                break;
            case ".gif":
                format = ImageFormat.Gif;
                break;
            case ".icon":
                format = ImageFormat.Icon;
                break;
            case ".ico":
                format = ImageFormat.Icon;
                break;
            default:
                format = ImageFormat.Png;
                path = path.Replace(Path.GetExtension(path), "png");
                break;
        }

        return format;
    }
    #endregion
    
    #region Transitions
    
    public override IFastImage AsARGB()
    {
        //TODO log
        return this;
    }

    public override IFastImage AsRGB()
    {
        var pixels = new Vector3[Size.Width, Size.Height];
        Parallel.For(0, Size.Height, j =>
        {
            for (var i = 0; i < Size.Width; i++)
            {
                pixels[i, j].X = Pixels[i, j].X;
                pixels[i, j].Y = Pixels[i, j].Y;
                pixels[i, j].Z = Pixels[i, j].Z;
            }
        });
        return new FastImageRGB(pixels);
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