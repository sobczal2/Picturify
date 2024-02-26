// # ==============================================================================
// # Solution: Picturify
// # File: RGBAPixel.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.Pixels;

// ReSharper disable once InconsistentNaming
public class RGBAPixel : IPixel
{
    private float _red;
    private float _green;
    private float _blue;
    private float _alpha;
    
    public RGBAPixel(
        float red,
        float green,
        float blue,
        float alpha
    )
    {
        _red = red;
        _green = green;
        _blue = blue;
        _alpha = alpha;
    }

    public ColorSpace ColorSpace => ColorSpace.RGBA;

    public float this[
        ColorChannels channels
    ]
    {
        get => channels switch
        {
            ColorChannels.Red => _red,
            ColorChannels.Green => _green,
            ColorChannels.Blue => _blue,
            ColorChannels.Alpha => _alpha,
            ColorChannels.Hue => ColorConversions.HueFromRGB(_red, _green, _blue),
            ColorChannels.Saturation => ColorConversions.SaturationFromRGB(_red, _green, _blue),
            ColorChannels.Lightness => ColorConversions.LightnessFromRGB(_red, _green, _blue),
            ColorChannels.Value => ColorConversions.ValueFromRGB(_red, _green, _blue),
            _ => throw new ArgumentOutOfRangeException(nameof(channels), channels, null)
        };
        set => _ = channels switch
        {
            ColorChannels.Red => _red = value,
            ColorChannels.Green => _green = value,
            ColorChannels.Blue => _blue = value,
            ColorChannels.Alpha => _alpha = value,
            _ => throw new ArgumentOutOfRangeException(nameof(channels), channels, null)
        };
    }

    public IPixel Clone()
    {
        return new RGBAPixel(
            this[ColorChannels.Red],
            this[ColorChannels.Green],
            this[ColorChannels.Blue],
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