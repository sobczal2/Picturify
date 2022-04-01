using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Utils;
using Size = Sobczal.Picturify.Core.Utils.Size;

namespace Sobczal.Picturify.Core.Data
{
    /// <summary>
    /// Class representing image using 4x float format
    /// </summary>
    public class FastImageF : FastImage<float>
    {
        internal FastImageF(Size size) : base(size)
        {
        }

        internal FastImageF(float[,,] pixels) : base(pixels)
        {
        }

        internal FastImageF(Image image) : this(new Size {Width = image.Width, Height = image.Height})
        {
            var widthInBytes = Size.Width * 4;
            var bitmap = new Bitmap(image);
            var arr = new byte[widthInBytes * Size.Height];
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                bitmap.PixelFormat);
            var ptr = bitmapData.Scan0;
            Marshal.Copy(ptr, arr, 0, arr.Length);
            Parallel.For(0, Size.Width, i =>
            {
                for (var j = 0; j < Size.Height; j++)
                {
                    for (var k = 0; k < 4; k++)
                    {
                        _pixels[i, j, k] = arr[j * widthInBytes + i * 4 + 3-k] / 255.0f;
                    }
                }
            });
        }

        internal FastImageF(string path) : this(Image.FromFile(path))
        {
        }

        protected override Bitmap GetBitmap(CancellationToken cancellationToken)
        {
            var widthInBytes = Size.Width * 4;
            var arr = new byte[widthInBytes * Size.Height];
            var po = new ParallelOptions();
            po.CancellationToken = cancellationToken;
            var depth = _pixels.GetLength(2);
            Parallel.For(0, Size.Width, po, i =>
            {
                for (var j = 0; j < Size.Height; j++)
                {
                    if (depth == 1)
                    {
                        for (var k = 0; k < 3; k++)
                        {
                            arr[j * widthInBytes + i * 4 + k] = (byte) (_pixels[i, j, 0] * 255.0f);
                        }

                        arr[j * widthInBytes + i * 4 + 3] = 255;
                    }
                    else
                    {
                        for (var k = 0; k < 4; k++)
                        {
                            arr[j * widthInBytes + i * 4 + k] = (byte) (_pixels[i, j, 3-k] * 255.0f);
                        }
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
        
        public override IFastImage ToGrayscale()
        {
            if (Grayscale)
                return this;
            var arr = new float[Size.Width, Size.Height, 1];
            Parallel.For(0, Size.Width, i =>
            {
                for (var j = 0; j < Size.Height; j++)
                {
                    arr[i, j, 0] = _pixels[i, j, 1] * 0.3f + _pixels[i, j, 2] * 0.59f + _pixels[i, j, 3] * 0.11f;
                }
            });
            _pixels = arr;
            return this;
        }
        
        public override IFastImage ToArgb()
        {
            if (!Grayscale)
                return this;
            var arr = new float[Size.Width, Size.Height, 4];
            Parallel.For(0, Size.Width, i =>
            {
                for (var j = 0; j < Size.Height; j++)
                {
                    arr[i, j, 0] = 1.0f;
                    for (var k = 1; k < 4; k++)
                    {
                        arr[i, j, k] = _pixels[i, j, 0];
                    }
                }
            });
            _pixels = arr;
            return this;
        }
        
        public override IFastImage Process(Func<float[,,], CancellationToken, float[,,]> processingFunction,
            CancellationToken cancellationToken)
        {
            _pixels = processingFunction(_pixels, cancellationToken);
            return this;
        }
        
        public override async Task<IFastImage> ProcessAsync(Func<float[,,], CancellationToken, float[,,]> processingFunction, CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(() =>
            {
                _pixels = processingFunction(_pixels, cancellationToken);
            });
            return this;
        }
        
        public override IFastImage Crop(SquareAreaSelector areaSelector)
        {
            if (!areaSelector.Validate(Size))
                throw new ArgumentException("All values must be in bounds of original image.");
            var depth = _pixels.GetLength(2);
            var arr = new float[areaSelector.Width, areaSelector.Height, depth];
            Parallel.For(0, areaSelector.Width, i =>
            {
                for (var j = 0; j < areaSelector.Height; j++)
                {
                    for (var k = 0; k < depth; k++)
                    {
                        arr[i, j, k] = _pixels[i + areaSelector.Left, j + areaSelector.Bottom, k];
                    }
                }
            });
            _pixels = arr;
            return this;
        }
        
        public override IFastImage GetCopy()
        {
            var depth = _pixels.GetLength(2);
            var copiedArray = new float[Size.Width, Size.Height, depth];
            Array.Copy(_pixels, copiedArray, _pixels.Length);
            return new FastImageF(copiedArray);
        }

        public override FastImageB ToByteRepresentation()
        {
            var sw = new Stopwatch();
            sw.Start();
            var arr = new byte[_pixels.GetLength(0), _pixels.GetLength(1), _pixels.GetLength(2)];
            Parallel.For(0, _pixels.GetLength(0), i =>
            {
                for (var j = 0; j < _pixels.GetLength(1); j++)
                {
                    for (var k = 0; k < _pixels.GetLength(2); k++)
                    {
                        arr[i, j, k] = (byte) (_pixels[i, j, k] * 255.0f);
                    }
                }
            });
            sw.Stop();
            PicturifyConfig.LogDebug($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name} took {sw.ElapsedMilliseconds} ms.");
            return new FastImageB(arr);
        }

        public override FastImageF ToFloatRepresentation()
        {
            return this;
        }
    }
}