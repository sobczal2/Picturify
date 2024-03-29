// # ==============================================================================
// # Solution: Picturify
// # File: RGBPixel.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.Pixels;

// ReSharper disable once InconsistentNaming
public class RGBPixel : IPixel
{
    private float _red;
    private float _green;
    private float _blue;

    public RGBPixel(
        float red,
        float green,
        float blue
    )
    {
        _red = red;
        _green = green;
        _blue = blue;
    }

    public ColorSpace ColorSpace => ColorSpace.RGB;

    public float this[
        ColorChannels channels
    ]
    {
        get
        {
            return channels switch
            {
                ColorChannels.Red => _red,
                ColorChannels.Green => _green,
                ColorChannels.Blue => _blue,
                ColorChannels.Alpha => 1,
                ColorChannels.Hue => ColorConversions.HueFromRGB(_red, _green, _blue),
                ColorChannels.Saturation => ColorConversions.SaturationFromRGB(_red, _green, _blue),
                ColorChannels.Lightness => ColorConversions.LightnessFromRGB(_red, _green, _blue),
                ColorChannels.Value => ColorConversions.ValueFromRGB(_red, _green, _blue),
                _ => throw new ArgumentOutOfRangeException(nameof(channels), channels, null)
            };
        }
        set => _ = channels switch
        {
            ColorChannels.Red => _red = value,
            ColorChannels.Green => _green = value,
            ColorChannels.Blue => _blue = value,
            _ => throw new ArgumentOutOfRangeException(nameof(channels), channels, null)
        };
    }

    public IPixel Clone()
    {
        return new RGBPixel(
            this[ColorChannels.Red],
            this[ColorChannels.Green],
            this[ColorChannels.Blue]
        );
    }

    public IPixel Clone(
        ColorSpace targetColorSpace
    )
    {
        return PixelHelpers.ToPixel(this, targetColorSpace);
    }
}