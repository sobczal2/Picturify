// # ==============================================================================
// # Solution: Picturify
// # File: IImage.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core;

public interface IImage
{
    ISize Size { get; }
    ColorChannel ColorChannel { get; }
    IPixel this[int x, int y] { get; set; }
    
    IImage Clone(ColorChannel targetColorChannel);
}