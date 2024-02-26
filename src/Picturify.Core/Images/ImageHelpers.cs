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
        ColorChannel targetColorChannel
    )
    {
        IImage newImage = targetColorChannel switch
        {
            ColorChannel.RGB => new RGBImage(image.Size),
            ColorChannel.HSL => new HSLImage(image.Size),
            ColorChannel.HSV => new HSVImage(image.Size),
            _ => throw new ArgumentOutOfRangeException(nameof(image.ColorChannel), image.ColorChannel, null)
        };

        for (var x = 0; x < image.Size.GetIntWidth(); x++)
        for (var y = 0; y < image.Size.GetIntHeight(); y++)
            newImage[x, y] = image[x, y];

        return newImage;
    }
}