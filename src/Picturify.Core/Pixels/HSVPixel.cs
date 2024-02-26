// # ==============================================================================
// # Solution: Picturify
// # File: HSVPixel.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.Pixels;

public class HSVPixel : IPixel
{
    private float _hue;
    private float _saturation;
    private float _value;

    public float this[
        ColorChannel channel
    ]
    {
        get
        {
            return channel switch
            {
                ColorChannel.Red => ColorConversions.RedFromHSV(_hue, _saturation, _value),
                ColorChannel.Green => ColorConversions.GreenFromHSV(_hue, _saturation, _value),
                ColorChannel.Blue => ColorConversions.BlueFromHSV(_hue, _saturation, _value),
                ColorChannel.Alpha => 1,
                ColorChannel.Hue => _hue,
                ColorChannel.Saturation => _saturation,
                ColorChannel.Lightness => ColorConversions.LightnessFromHSV(_hue, _saturation, _value),
                ColorChannel.Value => _value,
                _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
            };
        }
        set => _ = channel switch
        {
            ColorChannel.Red => _hue = value,
            ColorChannel.Green => _saturation = value,
            ColorChannel.Blue => _value = value,
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };
    }

    public IPixel Clone()
    {
        return new HSVPixel
        {
            [ColorChannel.Hue] = this[ColorChannel.Hue],
            [ColorChannel.Saturation] = this[ColorChannel.Saturation],
            [ColorChannel.Value] = this[ColorChannel.Value]
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