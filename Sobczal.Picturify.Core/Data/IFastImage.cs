using System.Drawing.Imaging;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Data;

/// <summary>
/// Interface representing image container for picturify
/// </summary>
public interface IFastImage
{
    void Save(Stream stream, ImageFormat format);
    void Save(String path);
    Task SaveAsync(Stream stream, ImageFormat format, CancellationToken cancellationToken);
    Task SaveAsync(String path, CancellationToken cancellationToken);
    ImageType GetImageType();
    IFastImage AsARGB();
    IFastImage AsRGB();
    IFastImage AsGreyscale();
    
}