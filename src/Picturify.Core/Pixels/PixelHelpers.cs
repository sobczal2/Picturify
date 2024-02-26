// # ==============================================================================
// # Solution: Picturify
// # File: PixelHelpers.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.Pixels;

internal static class PixelHelpers
{
    // ReSharper disable once InconsistentNaming
    public static RGBAPixel ToRGBA(
        IPixel pixel
    )
    {
        return new RGBAPixel(
            pixel[ColorChannels.Red],
            pixel[ColorChannels.Green],
            pixel[ColorChannels.Blue],
            pixel[ColorChannels.Alpha]
        );
    }

    // ReSharper disable once InconsistentNaming
    public static RGBPixel ToRGB(
        IPixel pixel
    )
    {
        return new RGBPixel(
            pixel[ColorChannels.Red],
            pixel[ColorChannels.Green],
            pixel[ColorChannels.Blue]
        );
    }

    // ReSharper disable once InconsistentNaming
    public static HSLAPixel ToHSLA(
        IPixel pixel
    )
    {
        return new HSLAPixel(
            pixel[ColorChannels.Hue],
            pixel[ColorChannels.Saturation],
            pixel[ColorChannels.Lightness],
            pixel[ColorChannels.Alpha]
        );
    }

    // ReSharper disable once InconsistentNaming
    public static HSLPixel ToHSL(
        IPixel pixel
    )
    {
        return new HSLPixel(
            pixel[ColorChannels.Hue],
            pixel[ColorChannels.Saturation],
            pixel[ColorChannels.Lightness]
        );
    }

    // ReSharper disable once InconsistentNaming
    public static HSVAPixel ToHSVA(
        IPixel pixel
    )
    {
        return new HSVAPixel(
            pixel[ColorChannels.Hue],
            pixel[ColorChannels.Saturation],
            pixel[ColorChannels.Value],
            pixel[ColorChannels.Alpha]
        );
    }

    // ReSharper disable once InconsistentNaming
    public static HSVPixel ToHSV(
        IPixel pixel
    )
    {
        return new HSVPixel(
            pixel[ColorChannels.Hue],
            pixel[ColorChannels.Saturation],
            pixel[ColorChannels.Value]
        );
    }

    public static IPixel ToPixel(
        IPixel pixel,
        ColorSpace targetColorSpace
    )
    {
        return targetColorSpace switch
        {
            ColorSpace.RGBA => ToRGBA(pixel),
            ColorSpace.RGB => ToRGB(pixel),
            ColorSpace.HSLA => ToHSLA(pixel),
            ColorSpace.HSL => ToHSL(pixel),
            ColorSpace.HSVA => ToHSVA(pixel),
            ColorSpace.HSV => ToHSV(pixel),
            _ => throw new ArgumentOutOfRangeException(nameof(targetColorSpace), targetColorSpace, null)
        };
    }
}