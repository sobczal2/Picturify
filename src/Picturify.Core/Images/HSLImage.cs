// # ==============================================================================
// # Solution: Picturify
// # File: HSLImage.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

using Picturify.Core.Pixels;

namespace Picturify.Core.Images;

// ReSharper disable once InconsistentNaming
public class HSLImage : IImage
{
    private readonly HSLPixel[,] _pixels;

    internal HSLImage(
        ISize size
    )
    {
        Size = size;
        _pixels = new HSLPixel[size.GetIntWidth(), size.GetIntHeight()];
    }

    public ISize Size { get; }
    public ColorSpace ColorSpace => ColorSpace.HSL;

    public IPixel this[
        int x,
        int y
    ]
    {
        get => _pixels[x, y];
        set => _pixels[x, y] = (HSLPixel)value.Clone(ColorSpace);
    }


    public IImage Clone(
        ColorSpace targetColorSpace
    )
    {
        return ImageHelpers.CopyImage(this, targetColorSpace);
    }
}