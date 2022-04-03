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
    /// Class representing image using 32 BGRA format
    /// </summary>
    /// <example>
    /// To create <see cref="FastImageB"/> use <see cref="FastImageFactory"/>:
    /// <code>
    /// var fastImage = FastImageFactory.FromFile(@"path", FastImageFactory.Version.Byte);
    /// </code>
    /// </example>
    public class FastImageB : FastImage<byte>
    {
        internal FastImageB(PSize pSize) : base(pSize)
        {
        }

        internal FastImageB(byte[,,] pixels) : base(pixels)
        {
        }
        
        /// <summary>
        /// Creates <see cref="FastImageB"/> from <see cref="Image"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to create from.</param>
        internal FastImageB(Image image) : this(new PSize (image.Width, image.Height))
        {
            var widthInBytes = PSize.Width * 4;
            var bitmap = new Bitmap(image);
            var arr = new byte[widthInBytes * PSize.Height];
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                bitmap.PixelFormat);
            var ptr = bitmapData.Scan0;
            Marshal.Copy(ptr, arr, 0, arr.Length);
            Parallel.For(0, PSize.Width, i =>
            {
                for (var j = 0; j < PSize.Height; j++)
                {
                    for (var k = 0; k < 4; k++)
                    {
                        Pixels[i, j, k] = arr[j * widthInBytes + i * 4 + 3-k];
                    }
                }
            });
        }
        
        /// <summary>
        /// Creates <see cref="FastImageB"/> from file. Supported image types.
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
        internal FastImageB(string path) : this(Image.FromFile(path))
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
                            arr[j * widthInBytes + i * 4 + k] = Pixels[i, j, 0];
                        }

                        arr[j * widthInBytes + i * 4 + 3] = 255;
                    }
                    else
                    {
                        for (var k = 0; k < 4; k++)
                        {
                            arr[j * widthInBytes + i * 4 + k] = Pixels[i, j, 3-k];
                        }
                    }
                }
            });
            var bitmap = new Bitmap(PSize.Width, PSize.Height);
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
            var arr = new byte[PSize.Width, PSize.Height, 1];
            Parallel.For(0, PSize.Width, i =>
            {
                for (var j = 0; j < PSize.Height; j++)
                {
                    arr[i, j, 0] = (byte) (Pixels[i, j, 1] * 0.3f + Pixels[i, j, 2] * 0.59f + Pixels[i, j, 3] * 0.11f);
                }
            });
            Pixels = arr;
            return this;
        }
        
        public override IFastImage ToArgb()
        {
            if (!Grayscale)
                return this;
            var arr = new byte[PSize.Width, PSize.Height, 4];
            Parallel.For(0, PSize.Width, i =>
            {
                for (var j = 0; j < PSize.Height; j++)
                {
                    arr[i, j, 0] = 255;
                    for (var k = 1; k < 4; k++)
                    {
                        arr[i, j, k] = Pixels[i, j, 0];
                    }
                }
            });
            Pixels = arr;
            return this;
        }
        
        public override IFastImage Process(Func<byte[,,], CancellationToken, byte[,,]> processingFunction,
            CancellationToken cancellationToken)
        {
            Pixels = processingFunction(Pixels, cancellationToken);
            return this;
        }

        public override IFastImage Crop(SquareAreaSelector areaSelector)
        {
            if (!areaSelector.Validate(PSize))
                throw new ArgumentException("All values must be in bounds of original image.");
            var depth = Pixels.GetLength(2);
            var arr = new byte[areaSelector.Width, areaSelector.Height, depth];
            Parallel.For(0, areaSelector.Width, i =>
            {
                for (var j = 0; j < areaSelector.Height; j++)
                {
                    for (var k = 0; k < depth; k++)
                    {
                        arr[i, j, k] = Pixels[i + areaSelector.Left, j + areaSelector.Bottom, k];
                    }
                }
            });
            Pixels = arr;
            return this;
        }
        
        public override IFastImage GetCopy()
        {
            var depth = Pixels.GetLength(2);
            var copiedArray = new byte[PSize.Width, PSize.Height, depth];
            Array.Copy(Pixels, copiedArray, Pixels.Length);
            return new FastImageB(copiedArray);
        }

        public override FastImageB ToByteRepresentation()
        {
            PicturifyConfig.LogTimeDebug($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", 0);
            return this;
        }

        public override FastImageF ToFloatRepresentation()
        {
            var sw = new Stopwatch();
            sw.Start();
            var arr = new float[Pixels.GetLength(0), Pixels.GetLength(1), Pixels.GetLength(2)];
            Parallel.For(0, Pixels.GetLength(0), i =>
            {
                for (var j = 0; j < Pixels.GetLength(1); j++)
                {
                    for (var k = 0; k < Pixels.GetLength(2); k++)
                    {
                        arr[i, j, k] = Pixels[i, j, k] / 255.0f;
                    }
                }
            });
            sw.Stop();
            PicturifyConfig.LogTimeDebug($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", sw.ElapsedMilliseconds);
            return new FastImageF(arr);
        }
    }
}