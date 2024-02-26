// # ==============================================================================
// # Solution: Picturify
// # File: RGBAImage.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

using Picturify.Core.Pixels;

namespace Picturify.Core.Images;

// ReSharper disable once InconsistentNaming
public class RGBAImage : IImage
{
    private readonly RGBAPixel[,] _pixels;

    public RGBAImage(
        ISize size
    )
    {
        Size = size;
        _pixels = new RGBAPixel[size.GetIntWidth(), size.GetIntHeight()];
    }

    public ISize Size { get; }
    public ColorSpace ColorSpace => ColorSpace.RGBA;

    public IPixel this[
        int x,
        int y
    ]
    {
        get => _pixels[x, y];
        set => _pixels[x, y] = (RGBAPixel)value.Clone(ColorSpace);
    }

    public IImage Clone(
        ColorSpace targetColorSpace
    )
    {
        return ImageHelpers.CopyImage(this, targetColorSpace);
    }
}