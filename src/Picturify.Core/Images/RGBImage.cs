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
    public ISize Size { get; }
    public ColorChannel ColorChannel => ColorChannel.RGB;

    public IPixel this[
        int x,
        int y
    ]
    {
        get => _pixels[x, y];
        set => _pixels[x, y] = (RGBPixel)value.Clone(ColorChannel);
    }

    public IImage Clone(
        ColorChannel targetColorChannel
    )
    {
        return ImageHelpers.CopyImage(this, targetColorChannel);
    }

    internal RGBImage(
        ISize size
    )
    {
        Size = size;
        _pixels = new RGBPixel[size.GetIntWidth(), size.GetIntHeight()];
    }
}