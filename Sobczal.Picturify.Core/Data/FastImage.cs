using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Data
{


    /// <summary>
    /// <para>
    /// <see cref="FastImage{T}"/> - abstract class representing image in picturify. Currently there are 2 concrete implementations:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="FastImageB"/> - stores image in 4 or 1 channel format - 1 byte per channel
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="FastImageF"/> - stores image in 4 or 1 channel format - 1 float per channel
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// <example>
    /// To create <see cref="FastImage{T}"/> use <see cref="FastImageFactory"/>:
    /// <code>
    /// var fastImage = FastImageFactory.FromFile(@"path", FastImageFactory.Version.Float);
    /// </code>
    /// </example>
    /// </summary>
    public abstract class FastImage<T> : IFastImage
    {
        /// <summary>
        /// Array that hold pixels values. It has always size of <code>[width, height, 4]</code> for 4 channel ARGB image or
        /// <code>[width, height, 1]</code> for 1 channel Grayscale image. When using them in loops you should always
        /// <list type="bullet">
        /// <item>
        /// <description>outer loop: width</description>
        /// </item>
        /// <item>
        /// <description>middle loop: height</description>
        /// </item>
        /// <item>
        /// <description>inner loop: depth</description>
        /// </item>
        /// </list>
        /// <example>
        /// <code>
        /// for(var i = 0; i &lt; width; i++)
        /// {
        /// &#009;for(var j = 0; j &lt; height; j++)
        /// &#009;{
        /// &#009;&#009;for(var k = 0; k &lt; depth; k++)
        /// &#009;&#009;{
        /// &#009;&#009;&#009;Pixels[i,j,k] = ...
        /// &#009;&#009;}
        /// &#009;}
        /// }
        /// </code>
        /// </example>
        /// </summary>
        protected T[,,] Pixels;
        
        /// <summary>
        /// Gets width and height of an image.
        /// </summary>
        public PSize PSize => new PSize(Pixels.GetLength(0), Pixels.GetLength(1));

        /// <summary>
        /// Gets whether image is in Greyscale or not.
        /// </summary>
        public bool Grayscale => Pixels.GetLength(2) == 1;

        #region Constructors

        /// <summary>
        /// Initialize image with no pixel data.
        /// </summary>
        internal FastImage(PSize pSize)
        {
            Pixels = new T[pSize.Width, pSize.Height, 4];
        }

        /// <summary>
        /// Initialize image with pixels.
        /// </summary>
        internal FastImage(T[,,] pixels)
        {
            if (pixels.GetLength(2) != 4 && pixels.GetLength(2) != 1)
                throw new ArgumentException("3rd dimension must be 4(for argb) or 1(for grayscale)", nameof(pixels));
            Pixels = pixels;
        }

        #endregion

        #region Save
        
        /// <summary>
        /// Saves this image to the specified stream in the specified format.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> where the image will be saved.</param>
        /// <param name="format">An <see cref="ImageFormat"/> that specifies the format of the saved image.</param>
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
        
        /// <summary>
        /// Saves this Image to the specified file.
        /// </summary>
        /// <param name="path">A string that contains the name of the file to which to save this <see cref="FastImage{T}"/>.</param>
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

        /// <summary>
        /// Saves this image to the specified stream in the specified format.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> where the image will be saved.</param>
        /// <param name="format">An <see cref="ImageFormat"/> that specifies the format of the saved image.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> allowing to cancel operation.</param>
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

        /// <summary>
        /// Saves this Image to the specified file.
        /// </summary>
        /// <param name="path">A string that contains the name of the file to which to save this <see cref="FastImage{T}"/>.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> allowing to cancel operation.</param>
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

        /// <summary>
        /// Gets <see cref="Bitmap"/> created from data in <see cref="Pixels"/>. Used in save operations.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> allowing to cancel operation.</param>
        /// <returns><see cref="Bitmap"/> containing image from this <see cref="FastImage{T}"/></returns>
        protected abstract Bitmap GetBitmap(CancellationToken cancellationToken);

        /// <summary>
        /// Sets pixels in this image from provided <see cref="Bitmap"/>. Used in resize.
        /// </summary>
        /// <param name="bitmap">Source of image</param>
        /// <param name="cancellationToken">For cancelling operation.</param>
        protected abstract void SetPixelsFromBitmap(Bitmap bitmap, CancellationToken cancellationToken);

        /// <summary>
        /// Gets <see cref="ImageFormat"/> from path (file extension). Available extensions:
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
        /// If file has invalid extension it defaults to png.
        /// </summary>
        /// <param name="path">A string that contains file path.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts image to grayscale.
        /// </summary>
        /// <returns><see cref="FastImage{T}"/> in grayscale.</returns>
        public abstract IFastImage ToGrayscale();

        /// <summary>
        /// Converts image to ARGB.
        /// </summary>
        /// <returns><see cref="FastImage{T}"/> in ARGB.</returns>
        public abstract IFastImage ToArgb();

        #endregion
        
        #region Processing

        /// <summary>
        /// Function to operate on <see cref="Pixels"/> from outside this class.
        /// Processing doesn't happen in place.
        /// </summary>
        /// <param name="processingFunction">Func transforming pixels - input <see cref="Pixels"/>,
        /// output transformed <see cref="Pixels"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> allowing to cancel operation.</param>
        /// <returns>Processed <see cref="FastImage{T}"/></returns>
        public abstract IFastImage Process(Func<T[,,], CancellationToken, T[,,]> processingFunction,
            CancellationToken cancellationToken);

        /// <summary>
        /// Crops image to size specified by <see cref="areaSelector"/>.
        /// </summary>
        /// <param name="areaSelector"><see cref="SquareAreaSelector"/> specifying size of output image.</param>
        /// <returns>Cropped <see cref="FastImage{T}"/></returns>
        public abstract IFastImage Crop(SquareAreaSelector areaSelector);

        /// <summary>
        /// Gets copy of this <see cref="FastImage{T}"/>
        /// </summary>
        /// <returns>Copied <see cref="FastImage{T}"/></returns>
        public abstract IFastImage GetCopy();
        
        /// <summary>
        /// Changes size of this <see cref="FastImage{T}"/> by stretching or squizing image.
        /// (Does not crop sides. See <see cref="Crop"/> for that behaviour.
        /// </summary>
        /// <param name="size">Wanted size</param>
        /// <returns>Resized <see cref="FastImage{T}"/></returns>
        public IFastImage Resize(PSize size)
        {
            //TODO implement without using bitmap
            var sw = new Stopwatch();
            sw.Start();
            var bitmap = GetBitmap(CancellationToken.None);
            var bitmap2 = new Bitmap(bitmap, new Size(size.Width, size.Height));
            SetPixelsFromBitmap(bitmap2, CancellationToken.None);
            sw.Stop();
            PicturifyConfig.LogTime($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}", sw.ElapsedMilliseconds);
            return this;
        }

        /// <summary>
        /// Converts this <see cref="FastImage{T}"/> to <see cref="FastImageB"/>.
        /// If it already is instance of <see cref="FastImageB"/> does nothing.
        /// </summary>
        /// <returns>This <see cref="FastImage{T}"/> if it already is <see cref="FastImageB"/>,
        /// else copy of this image as <see cref="FastImageB"/>.</returns>
        public abstract FastImageB ToByteRepresentation();
        
        /// <summary>
        /// Converts this <see cref="FastImage{T}"/> to <see cref="FastImageF"/>.
        /// If it already is instance of <see cref="FastImageF"/> does nothing.
        /// </summary>
        /// <returns>This <see cref="FastImage{T}"/> if it already is <see cref="FastImageF"/>,
        /// else copy of this image as <see cref="FastImageF"/>.</returns>
        public abstract FastImageF ToFloatRepresentation();

        #endregion
    }
}