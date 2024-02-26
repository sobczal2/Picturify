// # ==============================================================================
// # Solution: Picturify
// # File: HSLPixel.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.Pixels;

// ReSharper disable once InconsistentNaming
public class HSLPixel : IPixel
{
    private float _hue;
    private float _lightness;
    private float _saturation;

    public float this[
        ColorChannel channel
    ]
    {
        get
        {
            return channel switch
            {
                ColorChannel.Red => ColorConversions.RedFromHSL(_hue, _saturation, _lightness),
                ColorChannel.Green => ColorConversions.GreenFromHSL(_hue, _saturation, _lightness),
                ColorChannel.Blue => ColorConversions.BlueFromHSL(_hue, _saturation, _lightness),
                ColorChannel.Alpha => 1,
                ColorChannel.Hue => _hue,
                ColorChannel.Saturation => _saturation,
                ColorChannel.Lightness => _lightness,
                ColorChannel.Value => ColorConversions.ValueFromHSL(_hue, _saturation, _lightness),
                _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
            };
        }
        set => _ = channel switch
        {
            ColorChannel.Red => _hue = value,
            ColorChannel.Green => _saturation = value,
            ColorChannel.Blue => _lightness = value,
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };
    }

    public IPixel Clone()
    {
        return new HSLPixel
        {
            [ColorChannel.Hue] = this[ColorChannel.Hue],
            [ColorChannel.Saturation] = this[ColorChannel.Saturation],
            [ColorChannel.Lightness] = this[ColorChannel.Lightness]
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