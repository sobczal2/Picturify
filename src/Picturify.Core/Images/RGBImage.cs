// # ==============================================================================
// # Solution: Picturify
// # File: RGBImage.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

using Picturify.Core.Pixels;

namespace Picturify.Core.Images;

// ReSharper disable once InconsistentNaming
public class RGBImage : IImage
{
    private readonly RGBPixel[,] _pixels;

    public RGBImage(
        ISize size
    )
    {
        Size = size;
        _pixels = new RGBPixel[size.GetIntWidth(), size.GetIntHeight()];
    }

    public ISize Size { get; }
    public ColorSpace ColorSpace => ColorSpace.RGB;

    public IPixel this[
        int x,
        int y
    ]
    {
        get => _pixels[x, y];
        set => _pixels[x, y] = (RGBPixel)value.Clone(ColorSpace);
    }

    public IImage Clone(
        ColorSpace targetColorSpace
    )
    {
        return ImageHelpers.CopyImage(this, targetColorSpace);
    }
}