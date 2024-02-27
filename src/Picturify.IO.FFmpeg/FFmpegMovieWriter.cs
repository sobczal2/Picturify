// # ==============================================================================
// # Solution: Picturify
// # File: FFmpegMovieWriter.cs
// # Author: Łukasz Sobczak
// # Created: 27-02-2024
// # ==============================================================================

using System.Diagnostics;
using System.Diagnostics.Metrics;
using FFMpegCore;
using FFMpegCore.Pipes;
using ImageMagick;
using Picturify.Core;
using Picturify.Core.IO;

namespace Picturify.IO.FFmpeg;

public class FFmpegMovieWriter : IImageWriter, IDisposable
{
    private readonly string _path;
    private readonly Stream _stream;
    private MagickImage? _imageMagickImage;
    private Process _writeProcess;

    public FFmpegMovieWriter(
        string path,
        double outputFramerate,
        ISize size
    )
    {
        _path = path;
        _stream = new MemoryStream();
        
        _writeProcess = new Process();
        _writeProcess.StartInfo = new ProcessStartInfo
        {
            Arguments = $@"-y -r {outputFramerate} -f jpeg_pipe  -s {size.GetIntWidth()}x{size.GetIntHeight()} -i - -shortest {_path}",
            UseShellExecute = false,
            CreateNoWindow = true,
            FileName = "ffmpeg",
            RedirectStandardInput = true,
        };
        
        _writeProcess.Start();
    }

    public bool CanWrite()
    {
        throw new NotImplementedException();
    }

    public void Write(
        IImage image
    )
    {
        if (_imageMagickImage == null)
        {
            _imageMagickImage = new MagickImage(MagickColors.Transparent, image.Size.GetIntWidth(), image.Size.GetIntHeight());
        }
        else if (_imageMagickImage.Width != image.Size.GetIntWidth() || _imageMagickImage.Height != image.Size.GetIntHeight())
        {
            throw new InvalidOperationException("Image size must be the same as the first image size");
        }
        
        using var imageMagickPixels = _imageMagickImage.GetPixels();

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

        _imageMagickImage.Write(_writeProcess.StandardInput.BaseStream, MagickFormat.Jpeg);
    }

    public void Dispose()
    {
        _writeProcess.Close();
        _stream.Dispose();
        _imageMagickImage?.Dispose();
    }
}