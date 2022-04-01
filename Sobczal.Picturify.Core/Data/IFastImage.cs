using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Sobczal.Picturify.Core.Utils;
using Size = Sobczal.Picturify.Core.Utils.Size;

namespace Sobczal.Picturify.Core.Data
{
    public interface IFastImage
    {
        Size Size { get; }
        void Save(Stream stream, ImageFormat format);
        void Save(string path);
        Task SaveAsync(Stream stream, ImageFormat format, CancellationToken cancellationToken);
        Task SaveAsync(string path, CancellationToken cancellationToken);
        IFastImage ToGrayscale();
        IFastImage ToArgb();
        IFastImage Crop(SquareAreaSelector areaSelector);
        IFastImage GetCopy();
        FastImageB ToByteRepresentation();
        FastImageF ToFloatRepresentation();
    }
}