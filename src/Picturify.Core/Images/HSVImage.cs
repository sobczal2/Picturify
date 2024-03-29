// # ==============================================================================
// # Solution: Picturify
// # File: HSVImage.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

using Picturify.Core.Pixels;

namespace Picturify.Core.Images;

// ReSharper disable once InconsistentNaming
public class HSVImage : IImage
{
    private readonly HSVPixel[,] _pixels;

    internal HSVImage(
        ISize size
    )
    {
        Size = size;
        _pixels = new HSVPixel[size.GetIntWidth(), size.GetIntHeight()];
    }

    public ISize Size { get; }
    public ColorSpace ColorSpace => ColorSpace.HSV;

    public IPixel this[
        int x,
        int y
    ]
    {
        get => _pixels[x, y];
        set => _pixels[x, y] = (HSVPixel)value.Clone(ColorSpace);
    }


    public IImage Clone(
        ColorSpace targetColorSpace
    )
    {
        return ImageHelpers.CopyImage(this, targetColorSpace);
    }
}