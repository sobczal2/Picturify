// # ==============================================================================
// # Solution: Picturify
// # File: ImageHelpers.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.Images;

public static class ImageHelpers
{
    internal static IImage CopyImage(
        IImage image,
        ColorSpace targetColorSpace
    )
    {
        IImage newImage = targetColorSpace switch
        {
            ColorSpace.RGB => new RGBImage(image.Size),
            ColorSpace.HSL => new HSLImage(image.Size),
            ColorSpace.HSV => new HSVImage(image.Size),
            _ => throw new ArgumentOutOfRangeException(nameof(image.ColorSpace), image.ColorSpace, null)
        };

        for (var x = 0; x < image.Size.GetIntWidth(); x++)
        for (var y = 0; y < image.Size.GetIntHeight(); y++)
            newImage[x, y] = image[x, y];

        return newImage;
    }
}