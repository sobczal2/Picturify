// # ==============================================================================
// # Solution: Picturify
// # File: HSVPixel.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.Pixels;

// ReSharper disable once InconsistentNaming
public class HSVPixel : IPixel
{
    private float _hue;
    private float _saturation;
    private float _value;

    public HSVPixel(
        float hue,
        float saturation,
        float value
    )
    {
        _hue = hue;
        _saturation = saturation;
        _value = value;
    }

    public ColorSpace ColorSpace => ColorSpace.HSV;

    public float this[
        ColorChannels channels
    ]
    {
        get
        {
            return channels switch
            {
                ColorChannels.Red => ColorConversions.RedFromHSV(_hue, _saturation, _value),
                ColorChannels.Green => ColorConversions.GreenFromHSV(_hue, _saturation, _value),
                ColorChannels.Blue => ColorConversions.BlueFromHSV(_hue, _saturation, _value),
                ColorChannels.Alpha => 1,
                ColorChannels.Hue => _hue,
                ColorChannels.Saturation => _saturation,
                ColorChannels.Lightness => ColorConversions.LightnessFromHSV(_hue, _saturation, _value),
                ColorChannels.Value => _value,
                _ => throw new ArgumentOutOfRangeException(nameof(channels), channels, null)
            };
        }
        set => _ = channels switch
        {
            ColorChannels.Red => _hue = value,
            ColorChannels.Green => _saturation = value,
            ColorChannels.Blue => _value = value,
            _ => throw new ArgumentOutOfRangeException(nameof(channels), channels, null)
        };
    }

    public IPixel Clone()
    {
        return new HSVPixel(
            this[ColorChannels.Hue],
            this[ColorChannels.Saturation],
            this[ColorChannels.Value]
        );
    }

    public IPixel Clone(
        ColorSpace targetColorSpace
    )
    {
        return PixelHelpers.ToPixel(this, targetColorSpace);
    }
}