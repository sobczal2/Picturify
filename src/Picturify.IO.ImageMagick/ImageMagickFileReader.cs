// # ==============================================================================
// # Solution: Picturify
// # File: ImageMagickReader.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

using ImageMagick;
using Picturify.Core;
using Picturify.Core.Images;
using Picturify.Core.IO;
using Picturify.Core.Pixels;
using Picturify.Core.Sizes;

namespace Picturify.IO.ImageMagick;

public class ImageMagickFileReader : IImageReader
{
    private readonly string _path;
    private bool _hasNext;

    public ImageMagickFileReader(
        string path
    )
    {
        _path = path;
        _hasNext = true;
    }

    public bool HasNext()
    {
        return _hasNext;
    }

    public IImage ReadNext()
    {
        _hasNext = false;

        using var imageMagickImage = new MagickImage(_path);

        var image = new RGBAImage(
            new IntSize(
                imageMagickImage.Width,
                imageMagickImage.Height
            )
        );

        using var pixels = imageMagickImage.GetPixels();

        for (var h = 0; h < image.Size.GetIntHeight(); h++)
        {
            for (var w = 0; w < image.Size.GetIntWidth(); w++)
            {
                var pixel = pixels[w, h] ?? throw new NullReferenceException();
                image[w, h] = new RGBAPixel(
                    pixel.ToColor()!.R / 255f,
                    pixel.ToColor()!.G / 255f,
                    pixel.ToColor()!.B / 255f,
                    pixel.ToColor()?.A / 255f ?? 1
                );
            }
        }

        return image;
    }
}