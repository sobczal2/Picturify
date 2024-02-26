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
    private readonly HSVPixel[,] _pixels;
    public ISize Size { get; }
    public ColorChannel ColorChannel => ColorChannel.HSL;

    public IPixel this[
        int x,
        int y
    ]
    {
        get => _pixels[x, y];
        set => _pixels[x, y] = (HSVPixel)value.Clone(ColorChannel);
    }


    public IImage Clone(
        ColorChannel targetColorChannel
    )
    {
        return ImageHelpers.CopyImage(this, targetColorChannel);
    }

    internal HSLImage(
        ISize size
    )
    {
        Size = size;
        _pixels = new HSVPixel[size.GetIntWidth(), size.GetIntHeight()];
    }
}