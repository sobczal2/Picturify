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
    private float _blue;
    private float _green;
    private float _red;

    public float this[
        ColorChannel channel
    ]
    {
        get
        {
            return channel switch
            {
                ColorChannel.Red => _red,
                ColorChannel.Green => _green,
                ColorChannel.Blue => _blue,
                ColorChannel.Alpha => 1,
                ColorChannel.Hue => ColorConversions.HueFromRGB(_red, _green, _blue),
                ColorChannel.Saturation => ColorConversions.SaturationFromRGB(_red, _green, _blue),
                ColorChannel.Lightness => ColorConversions.LightnessFromRGB(_red, _green, _blue),
                ColorChannel.Value => ColorConversions.ValueFromRGB(_red, _green, _blue),
                _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
            };
        }
        set => _ = channel switch
        {
            ColorChannel.Red => _red = value,
            ColorChannel.Green => _green = value,
            ColorChannel.Blue => _blue = value,
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };
    }

    public IPixel Clone()
    {
        return new RGBPixel
        {
            [ColorChannel.Red] = this[ColorChannel.Red],
            [ColorChannel.Green] = this[ColorChannel.Green],
            [ColorChannel.Blue] = this[ColorChannel.Blue]
        };
    }

    public IPixel Clone(
        ColorChannel targetColorChannel
    )
    {
        return targetColorChannel switch
        {
            ColorChannel.RGB => new RGBPixel
            {
                [ColorChannel.Red] = this[ColorChannel.Red],
                [ColorChannel.Green] = this[ColorChannel.Green],
                [ColorChannel.Blue] = this[ColorChannel.Blue]
            },
            ColorChannel.HSL => new HSLPixel
            {
                [ColorChannel.Hue] = this[ColorChannel.Hue],
                [ColorChannel.Saturation] = this[ColorChannel.Saturation],
                [ColorChannel.Lightness] = this[ColorChannel.Lightness]
            },
            ColorChannel.HSV => new HSVPixel
            {
                [ColorChannel.Hue] = this[ColorChannel.Hue],
                [ColorChannel.Saturation] = this[ColorChannel.Saturation],
                [ColorChannel.Value] = this[ColorChannel.Value]
            },
            _ => throw new ArgumentOutOfRangeException(nameof(targetColorChannel), targetColorChannel, null)
        };
    }
}