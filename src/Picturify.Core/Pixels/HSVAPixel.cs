// # ==============================================================================
// # Solution: Picturify
// # File: HSVAPixel.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.Pixels;

// ReSharper disable once InconsistentNaming
public class HSVAPixel : IPixel
{
    private float _hue;
    private float _saturation;
    private float _value;
    private float _alpha;

    public HSVAPixel(
        float hue,
        float saturation,
        float value,
        float alpha
    )
    {
        _hue = hue;
        _saturation = saturation;
        _value = value;
        _alpha = alpha;
    }

    public ColorSpace ColorSpace => ColorSpace.HSVA;

    public float this[
        ColorChannels channels
    ]
    {
        get => channels switch
        {
            ColorChannels.Red => ColorConversions.RedFromHSV(_hue, _saturation, _value),
            ColorChannels.Green => ColorConversions.GreenFromHSV(_hue, _saturation, _value),
            ColorChannels.Blue => ColorConversions.BlueFromHSV(_hue, _saturation, _value),
            ColorChannels.Alpha => _alpha,
            ColorChannels.Hue => _hue,
            ColorChannels.Saturation => _saturation,
            ColorChannels.Value => _value,
            _ => throw new ArgumentOutOfRangeException(nameof(channels), channels, null)
        };
        set => _ = channels switch
        {
            ColorChannels.Red => _hue = value,
            ColorChannels.Green => _saturation = value,
            ColorChannels.Blue => _value = value,
            ColorChannels.Alpha => _alpha = value,
            _ => throw new ArgumentOutOfRangeException(nameof(channels), channels, null)
        };
    }

    public IPixel Clone()
    {
        return new HSVAPixel(
            this[ColorChannels.Hue],
            this[ColorChannels.Saturation],
            this[ColorChannels.Value],
            this[ColorChannels.Alpha]
        );
    }

    public IPixel Clone(
        ColorSpace targetColorSpace
    )
    {
        return PixelHelpers.ToPixel(this, targetColorSpace);
    }
}