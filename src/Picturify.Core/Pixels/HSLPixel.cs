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
    private float _saturation;
    private float _lightness;

    public HSLPixel(
        float hue,
        float saturation,
        float lightness
    )
    {
        _hue = hue;
        _saturation = saturation;
        _lightness = lightness;
    }

    public ColorSpace ColorSpace => ColorSpace.HSL;

    public float this[
        ColorChannels channels
    ]
    {
        get => channels switch
        {
            ColorChannels.Red => ColorConversions.RedFromHSL(_hue, _saturation, _lightness),
            ColorChannels.Green => ColorConversions.GreenFromHSL(_hue, _saturation, _lightness),
            ColorChannels.Blue => ColorConversions.BlueFromHSL(_hue, _saturation, _lightness),
            ColorChannels.Alpha => 1,
            ColorChannels.Hue => _hue,
            ColorChannels.Saturation => _saturation,
            ColorChannels.Lightness => _lightness,
            ColorChannels.Value => ColorConversions.ValueFromHSL(_hue, _saturation, _lightness),
            _ => throw new ArgumentOutOfRangeException(nameof(channels), channels, null)
        };
        set => _ = channels switch
        {
            ColorChannels.Red => _hue = value,
            ColorChannels.Green => _saturation = value,
            ColorChannels.Blue => _lightness = value,
            _ => throw new ArgumentOutOfRangeException(nameof(channels), channels, null)
        };
    }

    public IPixel Clone()
    {
        return new HSLPixel(
            this[ColorChannels.Hue],
            this[ColorChannels.Saturation],
            this[ColorChannels.Lightness]
        );
    }

    public IPixel Clone(
        ColorSpace targetColorSpace
    )
    {
        return PixelHelpers.ToPixel(this, targetColorSpace);
    }
}