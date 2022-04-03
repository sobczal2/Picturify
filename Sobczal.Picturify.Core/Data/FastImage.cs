using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Data
{


    /// <summary>
    /// Abstract lass representing image format.
    /// </summary>
    public abstract class FastImage<T> : IFastImage
    {
        /// <summary>
        /// When referring to pixels itself it is important to iterate like this [outer loop, middle loop, inner loop], because of performance gains.
        /// </summary>
        protected T[,,] _pixels;
        public PSize PSize => new PSize(_pixels.GetLength(0), _pixels.GetLength(1));

        public bool Grayscale => _pixels.GetLength(2) == 1;

        #region Constructors

        internal FastImage(PSize pSize)
        {
            _pixels = new T[pSize.Width, pSize.Height, 4];
        }

        internal FastImage(T[,,] pixels)
        {
            if (pixels.GetLength(2) != 4 && pixels.GetLength(2) != 1)
                throw new ArgumentException("3rd dimension must be 4(for argb) or 1(for grayscale)", nameof(pixels));
            _pixels = pixels;
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

        protected abstract Bitmap GetBitmap(CancellationToken cancellationToken);

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

        public abstract IFastImage ToGrayscale();

        public abstract IFastImage ToArgb();

        #endregion
        
        #region Processing

        public abstract IFastImage Process(Func<T[,,], CancellationToken, T[,,]> processingFunction,
            CancellationToken cancellationToken);

        public abstract IFastImage Crop(SquareAreaSelector areaSelector);

        public abstract IFastImage GetCopy();
        public abstract FastImageB ToByteRepresentation();
        public abstract FastImageF ToFloatRepresentation();

        #endregion
    }
}