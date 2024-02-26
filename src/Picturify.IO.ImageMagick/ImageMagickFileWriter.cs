// # ==============================================================================
// # Solution: Picturify
// # File: ImageMagickFileWriter.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

using ImageMagick;
using Picturify.Core;
using Picturify.Core.IO;

namespace Picturify.IO.ImageMagick;

public class ImageMagickFileWriter : IImageWriter
{
    private readonly string _path;
    private bool _canWrite;

    public ImageMagickFileWriter(
        string path
    )
    {
        _path = path;
        _canWrite = true;
    }

    public bool CanWrite()
    {
        return _canWrite;
    }

    public void Write(
        IImage image
    )
    {
        _canWrite = false;

        using var imageMagickImage =
            new MagickImage(MagickColors.Transparent, image.Size.GetIntWidth(), image.Size.GetIntHeight());
        using var imageMagickPixels = imageMagickImage.GetPixels();

        for (var h = 0; h < image.Size.GetIntHeight(); h++)
        {
            for (var w = 0; w < image.Size.GetIntWidth(); w++)
            {
                var readPixel = image[w, h];
                var writePixel = imageMagickPixels[w, h] ?? throw new NullReferenceException();
                writePixel[0] = (byte) (readPixel[ColorChannels.Red] * 255);
                writePixel[1] = (byte) (readPixel[ColorChannels.Green] * 255);
                writePixel[2] = (byte) (readPixel[ColorChannels.Blue] * 255);
                writePixel[3] = (byte) (readPixel[ColorChannels.Alpha] * 255);
            }
        }

        imageMagickImage.Write(_path);
    }
}