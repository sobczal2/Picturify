// # ==============================================================================
// # Solution: Picturify
// # File: FFmpegMovieReader.cs
// # Author: Łukasz Sobczak
// # Created: 27-02-2024
// # ==============================================================================

using FFMpegCore;
using FFMpegCore.Pipes;
using Picturify.Core;
using Picturify.Core.Images;
using Picturify.Core.IO;
using Picturify.Core.Pixels;
using Picturify.Core.Sizes;

namespace Picturify.IO.FFmpeg;

// ReSharper disable once IdentifierTypo
public class FFmpegMovieReader : IImageReader
{
    private readonly string _path;
    private ISize _size;
    private int _readFrames;
    private double _frameRate;
    private TimeSpan _duration;
    private Queue<IImage> _frames;

    public FFmpegMovieReader(
        string path
    )
    {
        _path = path;
        var info = FFProbe.Analyse(path);
        var videoStream = info.PrimaryVideoStream;

        if (videoStream == null)
        {
            throw new Exception("No video stream found");
        }

        _size = new IntSize(videoStream.Width, videoStream.Height);
        _frameRate = videoStream.FrameRate;
        _duration = info.Duration;
        
        _frames = new Queue<IImage>();

        _readFrames = 0;
    }

    public bool HasNext()
    {
        return _readFrames / _frameRate < _duration.TotalSeconds || _frames.Count > 0;
    }

    public IImage ReadNext()
    {
        if (!HasNext())
        {
            throw new Exception("No more frames to read");
        }
        
        if (_frames.Count == 0)
        {
            ReadFrames();
        }
        
        return _frames.Dequeue();
    }

    private void ReadFrames()
    {
        using var stream = new MemoryStream();

        FFMpegArguments
            .FromFileInput(_path)
            .OutputToPipe(new StreamPipeSink(stream),
                options => options
                    .WithVideoCodec("bmp")
                    .ForceFormat("image2pipe")
                    .WithFrameOutputCount(10)
                    .WithFastStart()
                    .Seek(TimeSpan.FromSeconds(_readFrames / _frameRate))
            )
            .ProcessSynchronously();

        stream.Seek(0, SeekOrigin.Begin);
        using var br = new BinaryReader(stream);
        
        for (var i = 0; i < 10; i++)
        {
            br.ReadBytes(54);

            var image = new RGBAImage(_size);

            for (var h = 0; h < _size.GetIntHeight(); h++)
            {
                for (var w = 0; w < _size.GetIntWidth(); w++)
                {
                    var blue = br.ReadByte();
                    var green = br.ReadByte();
                    var red = br.ReadByte();

                    image[w, h] = new RGBAPixel(
                        red / 255f,
                        green / 255f,
                        blue / 255f,
                        1
                    );
                }
            }

            _readFrames++;
            _frames.Enqueue(image);
        }
    }
}