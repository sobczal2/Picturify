// # ==============================================================================
// # Solution: Picturify
// # File: PixelExtensions.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.Pixels;

public static class PixelExtensions
{
    public static IPixel WithColorSpace(
        this IPixel pixel,
        ColorSpace colorSpace
    )
    {
        return pixel.ColorSpace == colorSpace ? pixel : pixel.Clone(colorSpace);
    }
}