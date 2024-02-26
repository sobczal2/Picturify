// # ==============================================================================
// # Solution: Picturify
// # File: IPixel.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core;

public interface IPixel
{
    float this[ColorChannel channel] { get; set; }
    IPixel Clone();
    IPixel Clone(ColorChannel targetColorChannel);
}