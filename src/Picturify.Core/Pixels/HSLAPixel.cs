// # ==============================================================================
// # Solution: Picturify
// # File: HSLAPixel.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.Pixels;

// ReSharper disable once InconsistentNaming
public class HSLAPixel : IPixel
{
    private float _hue;
    private float _saturation;
    private float _lightness;
    private float _alpha;
    
    public HSLAPixel(
        float hue,
        float saturation,
        float lightness,
        float alpha
    )
    {
        _hue = hue;
        _saturation = saturation;
        _lightness = lightness;
        _alpha = alpha;
    }

    public ColorSpace ColorSpace => ColorSpace.HSLA;

    public float this[
        ColorChannels channels
    ]
    {
        get => channels switch
        {
            ColorChannels.Red => ColorConversions.RedFromHSL(_hue, _saturation, _lightness),
            ColorChannels.Green => ColorConversions.GreenFromHSL(_hue, _saturation, _lightness),
            ColorChannels.Blue => ColorConversions.BlueFromHSL(_hue, _saturation, _lightness),
            ColorChannels.Alpha => _alpha,
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
            ColorChannels.Alpha => _alpha = value,
            _ => throw new ArgumentOutOfRangeException(nameof(channels), channels, null)
        };
    }

    public IPixel Clone()
    {
        return new HSLAPixel(
            this[ColorChannels.Hue],
            this[ColorChannels.Saturation],
            this[ColorChannels.Lightness],
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