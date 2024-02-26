// # ==============================================================================
// # Solution: Picturify
// # File: IPixel.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core;

public interface IPixel
{
    ColorSpace ColorSpace { get; }
    float this[
        ColorChannels channels
    ] { get; set; }

    IPixel Clone();

    IPixel Clone(
        ColorSpace targetColorSpace
    );
}