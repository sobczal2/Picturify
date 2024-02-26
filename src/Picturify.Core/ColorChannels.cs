// # ==============================================================================
// # Solution: Picturify
// # File: ColorLayer.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core;

[Flags]
public enum ColorChannels
{
    Red = 1,
    Green = 2,
    Blue = 4,
    Alpha = 8,

    // ReSharper disable once InconsistentNaming
    RGB = Red | Green | Blue,

    // ReSharper disable once InconsistentNaming
    RGBA = RGB | Alpha,
    Hue = 16,
    Saturation = 32,
    Lightness = 64,
    Value = 128,

    // ReSharper disable once InconsistentNaming
    HSL = Hue | Saturation | Lightness,
    // ReSharper disable once InconsistentNaming
    HSLA = HSL | Alpha,

    // ReSharper disable once InconsistentNaming
    HSV = Hue | Saturation | Value,
    // ReSharper disable once InconsistentNaming
    HSVA = HSV | Alpha,
    Grayscale = 256
}