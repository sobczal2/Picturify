// # ==============================================================================
// # Solution: Picturify
// # File: HSLAImage.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

using Picturify.Core.Pixels;

namespace Picturify.Core.Images;

// ReSharper disable once InconsistentNaming
public class HSLAImage : IImage
{
    private readonly HSLAPixel[,] _pixels;

    public HSLAImage(
        ISize size
    )
    {
        Size = size;
        _pixels = new HSLAPixel[size.GetIntWidth(), size.GetIntHeight()];
    }

    public ISize Size { get; }
    public ColorSpace ColorSpace => ColorSpace.HSLA;

    public IPixel this[
        int x,
        int y
    ]
    {
        get => _pixels[x, y];
        set => _pixels[x, y] = (HSLAPixel)value.Clone(ColorSpace);
    }

    public IImage Clone(
        ColorSpace targetColorSpace
    )
    {
        return ImageHelpers.CopyImage(this, targetColorSpace);
    }
}