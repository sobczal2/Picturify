using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Utils;
using Size = Sobczal.Picturify.Core.Utils.Size;

namespace Sobczal.Picturify.Core.Data
{


    /// <summary>
    /// Class representing image in ARGB format.
    /// </summary>
    public class FastImage
    {
        private float[,,] _pixels;
        public Size Size => new Size {Width = _pixels.GetLength(0), Height = _pixels.GetLength(1)};
        public bool Grayscale { get; private set; }
        public IAreaSelector AreaSelector { get; set; }

        #region Constructors

        private FastImage(Size size)
        {
            _pixels = new float[size.Width, size.Height, 4];
            Grayscale = false;
            AreaSelector = new SquareAreaSelector(0, 0, Size.Width, Size.Height);
        }

        private FastImage(float[,,] pixels)
        {
            if (pixels.GetLength(2) != 4 && pixels.GetLength(2) != 1)
                throw new ArgumentException("3rd dimension must be 4(for argb) or 1(for grayscale)", nameof(pixels));
            _pixels = pixels;
            Grayscale = pixels.GetLength(2) == 1;
            AreaSelector = new SquareAreaSelector(0, 0, Size.Width, Size.Height);
        }

        private FastImage(Image image) : this(new Size {Width = image.Width, Height = image.Height})
        {
            var widthInBytes = Size.Width * 4;
            var bitmap = new Bitmap(image);
            var arr = new byte[widthInBytes * Size.Height];
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                bitmap.PixelFormat);
            var ptr = bitmapData.Scan0;
            Marshal.Copy(ptr, arr, 0, arr.Length);
            Parallel.For(0, Size.Height, j =>
            {
                for (var i = 0; i < Size.Width; i++)
                {
                    _pixels[i, j, 0] = arr[j * widthInBytes + i * 4 + 3] / 255.0f;
                    _pixels[i, j, 1] = arr[j * widthInBytes + i * 4 + 2] / 255.0f;
                    _pixels[i, j, 2] = arr[j * widthInBytes + i * 4 + 1] / 255.0f;
                    _pixels[i, j, 3] = arr[j * widthInBytes + i * 4 + 0] / 255.0f;
                }
            });
        }

        private FastImage(string path) : this(Image.FromFile(path))
        {
        }

        #endregion
        
        #region Factory

        public static FastImage Empty(Size size)
        {
            var f = new FastImage(size);
            PicturifyConfig.LogInfo($"FastImage.Empty()");
            return f;
        }
        
        public static FastImage FromImage(Image image)
        {
            var f = new FastImage(image);
            PicturifyConfig.LogInfo($"FastImage.Empty()");
            return f;
        }
        
        public static FastImage FromFile(string path)
        {
            var f = new FastImage(path);
            PicturifyConfig.LogInfo($"FastImage.Empty()");
            return f;
        }
        
        #endregion

        #region Save

        public void Save(Stream stream, ImageFormat format)
        {
            PicturifyConfig.LogInfo($"Saving file to stream.");
            var bitmap = GetBitmap(CancellationToken.None);
            try
            {
                bitmap.Save(stream, format);
            }
            catch (Exception e)
            {
                PicturifyConfig.LogError($"Saving failed: {e.Message}");
            }
        }

        public void Save(string path)
        {
            PicturifyConfig.LogInfo($"Saving file {path}.");
            var bitmap = GetBitmap(CancellationToken.None);
            var format = GetImageFormat(path);
            try
            {
                bitmap.Save(path, format);
            }
            catch (Exception e)
            {
                PicturifyConfig.LogError($"Saving failed: {e.Message}");
            }
        }

        public async Task SaveAsync(Stream stream, ImageFormat format, CancellationToken cancellationToken)
        {
            PicturifyConfig.LogInfo($"Saving file stream.");
            await Task.Factory.StartNew(() =>
            {
                var bitmap = GetBitmap(cancellationToken);
                try
                {
                    bitmap.Save(stream, format);
                }
                catch (Exception e)
                {
                    PicturifyConfig.LogError($"Saving failed: {e.Message}");
                }
            });
        }

        public async Task SaveAsync(string path, CancellationToken cancellationToken)
        {
            PicturifyConfig.LogInfo($"Saving file {path}.");
            await Task.Factory.StartNew(() =>
            {
                var bitmap = GetBitmap(cancellationToken);
                var format = GetImageFormat(path);
                try
                {
                    bitmap.Save(path, format);
                }
                catch (Exception e)
                {
                    PicturifyConfig.LogError($"Saving failed: {e.Message}");
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
                    if (Grayscale)
                    {
                        arr[j * widthInBytes + i * 4 + 3] = (byte) (_pixels[i, j, 0] * 255.0f);
                        arr[j * widthInBytes + i * 4 + 2] = (byte) (_pixels[i, j, 0] * 255.0f);
                        arr[j * widthInBytes + i * 4 + 1] = (byte) (_pixels[i, j, 0] * 255.0f);
                        arr[j * widthInBytes + i * 4 + 0] = (byte) (_pixels[i, j, 0] * 255.0f);
                    }
                    else
                    {
                        arr[j * widthInBytes + i * 4 + 3] = (byte) (_pixels[i, j, 0] * 255.0f);
                        arr[j * widthInBytes + i * 4 + 2] = (byte) (_pixels[i, j, 1] * 255.0f);
                        arr[j * widthInBytes + i * 4 + 1] = (byte) (_pixels[i, j, 2] * 255.0f);
                        arr[j * widthInBytes + i * 4 + 0] = (byte) (_pixels[i, j, 3] * 255.0f);
                    }
                }
            });
            var bitmap = new Bitmap(Size.Width, Size.Height);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly,
                bitmap.PixelFormat);
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
                    break;
            }

            return format;
        }

        #endregion
        
        #region grayscale

        public FastImage ToGrayscale()
        {
            if (Grayscale)
                return this;
            var arr = new float[Size.Width, Size.Height, 1];
            Parallel.For(0, Size.Height, j =>
            {
                for (var i = 0; i < Size.Width; i++)
                {
                    arr[i, j, 0] = _pixels[i, j, 1] * 0.3f + _pixels[i, j, 2] * 0.59f + _pixels[i, j, 3] * 0.11f;
                }
            });
            _pixels = arr;
            Grayscale = true;
            return this;
        }
        
        public FastImage ToArgb()
        {
            if (!Grayscale)
                return this;
            var arr = new float[Size.Width, Size.Height, 4];
            Parallel.For(0, Size.Height, j =>
            {
                for (var i = 0; i < Size.Width; i++)
                {
                    arr[i, j, 0] = 1.0f;
                    arr[i, j, 1] = _pixels[i, j, 0];
                    arr[i, j, 2] = _pixels[i, j, 0];
                    arr[i, j, 3] = _pixels[i, j, 0];
                }
            });
            _pixels = arr;
            Grayscale = false;
            return this;
        }
        
        #endregion
        
        #region Processing
        
        public FastImage Process(Action<float[,,]> processingFunction)
        {
            processingFunction(_pixels);
            return this;
        }
        
        public FastImage ProcessAsync(Action<float[,,]> processingFunction)
        {
            Task.Factory.StartNew(() => processingFunction(_pixels));
            return this;
        }

        public FastImage Crop(SquareAreaSelector areaSelector)
        {
            if (!areaSelector.Validate(Size))
                throw new ArgumentException("All values must be in bounds of original image.");
            if (Grayscale)
            {
                var arr = new float[areaSelector.Width, areaSelector.Height, 1];
                Parallel.For(0, areaSelector.Height, j =>
                {
                    for (var i = 0; i < areaSelector.Width; i++)
                    {
                        arr[i, j, 0] = _pixels[i + areaSelector.Left, j + areaSelector.Bottom, 0];
                    }
                });
                _pixels = arr;
            }
            else
            {
                var arr = new float[areaSelector.Width, areaSelector.Height, 4];
                Parallel.For(0, areaSelector.Height, j =>
                {
                    for (var i = 0; i < areaSelector.Width; i++)
                    {
                        arr[i, j, 0] = _pixels[i + areaSelector.Left, j + areaSelector.Bottom, 0];
                        arr[i, j, 1] = _pixels[i + areaSelector.Left, j + areaSelector.Bottom, 1];
                        arr[i, j, 2] = _pixels[i + areaSelector.Left, j + areaSelector.Bottom, 2];
                        arr[i, j, 3] = _pixels[i + areaSelector.Left, j + areaSelector.Bottom, 3];
                    }
                });
                _pixels = arr;
            }

            return this;
        }

        public FastImage GetCopy()
        {
            if (Grayscale)
            {
                var copiedArray = new float[Size.Width, Size.Height, 1];
                Array.Copy(_pixels, copiedArray, _pixels.Length);
                return new FastImage(copiedArray);
            }
            else
            {
                var copiedArray = new float[Size.Width, Size.Height, 4];
                Array.Copy(_pixels, copiedArray, _pixels.Length);
                return new FastImage(copiedArray);
            }
        }
        
        #endregion
    }
}