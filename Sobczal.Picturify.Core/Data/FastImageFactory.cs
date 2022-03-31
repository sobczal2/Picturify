using System.Drawing;
using Sobczal.Picturify.Core.Utils;
using Size = Sobczal.Picturify.Core.Utils.Size;

namespace Sobczal.Picturify.Core.Data;

public static class FastImageFactory
{
    public static IFastImage Empty(Size size, ImageType type = ImageType.RGB)
    {
        return type switch
        {
            ImageType.GreyScale => new FastImageGS(size),
            ImageType.RGB => new FastImageRGB(size),
            _ => new FastImageARGB(size)
        };
    }
    
    public static IFastImage FromImage(Image image, ImageType type = ImageType.RGB)
    {
        return type switch
        {
            ImageType.GreyScale => new FastImageGS(image),
            ImageType.RGB => new FastImageRGB(image),
            _ => new FastImageARGB(image)
        };
    }

    public static IFastImage FromFile(string path, ImageType type = ImageType.RGB)
    {
        return type switch
        {
            ImageType.GreyScale => new FastImageGS(path),
            ImageType.RGB => new FastImageRGB(path),
            _ => new FastImageARGB(path)
        };
    }
}