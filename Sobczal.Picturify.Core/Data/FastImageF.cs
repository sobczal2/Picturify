using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Data
{
    /// <summary>
    /// Class representing image using 4x float format
    /// </summary>
    /// <example>
    /// To create <see cref="FastImageF"/> use <see cref="FastImageFactory"/>:
    /// <code>
    /// var fastImage = FastImageFactory.FromFile(@"path", FastImageFactory.Version.Float);
    /// </code>
    /// </example>
    public sealed class FastImageF : FastImage<float>
    {
        /// <summary>
        /// Initialize image with no pixel data.
        /// </summary>
        internal FastImageF(PSize pSize) : base(pSize)
        {
        }

        /// <summary>
        /// Initialize image with pixels.
        /// </summary>
        internal FastImageF(float[,,] pixels) : base(pixels)
        {
        }

        /// <summary>
        /// Creates <see cref="FastImageF"/> from <see cref="Image"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to create from.</param>
        internal FastImageF(Image image) : this(new PSize (image.Width, image.Height))
        {
            var bitmap = new Bitmap(image);
            SetPixelsFromBitmap(bitmap, CancellationToken.None);
        }

        /// <summary>
        /// Creates <see cref="FastImageF"/> from file. Supported image types.
        /// <list type="bullet">
        /// <item>jpg</item>
        /// <item>jpeg</item>
        /// <item>png</item>
        /// <item>tiff</item>
        /// <item>bmp</item>
        /// <item>gif</item>
        /// <item>icon</item>
        /// <item>ico</item>
        /// </list>
        /// </summary>
        /// <param name="path">A string to image file.</param>
        internal FastImageF(string path) : this(Image.FromFile(path))
        {
        }

        protected override Bitmap GetBitmap(CancellationToken cancellationToken)
        {
            var widthInBytes = PSize.Width * 4;
            var arr = new byte[widthInBytes * PSize.Height];
            var po = new ParallelOptions();
            po.CancellationToken = cancellationToken;
            var depth = Pixels.GetLength(2);
            Parallel.For(0, PSize.Width, po, i =>
            {
                for (var j = 0; j < PSize.Height; j++)
                {
                    if (depth == 1)
                    {
                        for (var k = 0; k < 3; k++)
                        {
                            arr[j * widthInBytes + i * 4 + k] = (byte) Math.Max(Math.Min(Pixels[i, j, 0] * 255.0f, 255f), 0f);
                        }

                        arr[j * widthInBytes + i * 4 + 3] = 255;
                    }
                    else
                    {
                        for (var k = 0; k < 4; k++)
                        {
                            arr[j * widthInBytes + i * 4 + k] = (byte) Math.Max(Math.Min(Pixels[i, j, 3-k] * 255.0f, 255f), 0f);
                        }
                    }
                }
            });
            var bitmap = new Bitmap(PSize.Width, PSize.Height);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly,
                bitmap.PixelFormat);
            var ptr = bitmapData.Scan0;
            Marshal.Copy(arr, 0, ptr, arr.Length);
            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }

        protected override void SetPixelsFromBitmap(Bitmap bitmap, CancellationToken cancellationToken)
        {
            var width = bitmap.Width;
            var height = bitmap.Height;
            var widthInBytes = width * 4;
            var arr = new byte[widthInBytes * height];
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                bitmap.PixelFormat);
            var ptr = bitmapData.Scan0;
            Marshal.Copy(ptr, arr, 0, arr.Length);
            bitmap.UnlockBits(bitmapData);
            Pixels = new float[width, height, 4];
            Parallel.For(0, width, i =>
            {
                for (var j = 0; j < height; j++)
                {
                    for (var k = 0; k < 4; k++)
                    {
                        Pixels[i, j, k] = arr[j * widthInBytes + i * 4 + 3-k] / 255.0f;
                    }
                }
            });
        }

        public override IFastImage ToGrayscale()
        {
            if (Grayscale)
                return this;
            var arr = new float[PSize.Width, PSize.Height, 1];
            Parallel.For(0, PSize.Width, i =>
            {
                for (var j = 0; j < PSize.Height; j++)
                {
                    arr[i, j, 0] = Pixels[i, j, 1] * 0.3f + Pixels[i, j, 2] * 0.59f + Pixels[i, j, 3] * 0.11f;
                }
            });
            Pixels = arr;
            return this;
        }
        
        public override IFastImage ToArgb()
        {
            if (!Grayscale)
                return this;
            var arr = new float[PSize.Width, PSize.Height, 4];
            Parallel.For(0, PSize.Width, i =>
            {
                for (var j = 0; j < PSize.Height; j++)
                {
                    arr[i, j, 0] = 1.0f;
                    for (var k = 1; k < 4; k++)
                    {
                        arr[i, j, k] = Pixels[i, j, 0];
                    }
                }
            });
            Pixels = arr;
            return this;
        }
        
        public override IFastImage Process(Func<float[,,], CancellationToken, float[,,]> processingFunction,
            CancellationToken cancellationToken)
        {
            Pixels = processingFunction(Pixels, cancellationToken);
            return this;
        }

        public override IFastImage Crop(SquareAreaSelector areaSelector)
        {
            if (areaSelector.LeftInclusive < 0 || areaSelector.RightExclusive >= PSize.Width || areaSelector.BotInclusive < 0 || areaSelector.TopExclusive >= PSize.Height)
                throw new ArgumentException("All values must be in bounds of original image.");
            var depth = Pixels.GetLength(2);
            var arr = new float[areaSelector.Width, areaSelector.Height, depth];
            Parallel.For(0, areaSelector.Width, i =>
            {
                for (var j = 0; j < areaSelector.Height; j++)
                {
                    for (var k = 0; k < depth; k++)
                    {
                        arr[i, j, k] = Pixels[i + areaSelector.LeftInclusive, j + areaSelector.BotInclusive, k];
                    }
                }
            });
            Pixels = arr;
            return this;
        }
        
        public override IFastImage GetCopy()
        {
            var depth = Pixels.GetLength(2);
            var copiedArray = new float[PSize.Width, PSize.Height, depth];
            Array.Copy(Pixels, copiedArray, Pixels.Length);
            return new FastImageF(copiedArray);
        }

        public override FastImageB ToByteRepresentation()
        {
            var sw = new Stopwatch();
            sw.Start();
            var arr = new byte[Pixels.GetLength(0), Pixels.GetLength(1), Pixels.GetLength(2)];
            Parallel.For(0, Pixels.GetLength(0), i =>
            {
                for (var j = 0; j < Pixels.GetLength(1); j++)
                {
                    for (var k = 0; k < Pixels.GetLength(2); k++)
                    {
                        arr[i, j, k] = (byte) (Pixels[i, j, k] * 255.0f);
                    }
                }
            });
            sw.Stop();
            PicturifyConfig.LogTimeDebug($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", sw.ElapsedMilliseconds);
            return new FastImageB(arr);
        }

        public override FastImageF ToFloatRepresentation()
        {
            PicturifyConfig.LogTimeDebug($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", 0);
            return this;
        }
    }
}