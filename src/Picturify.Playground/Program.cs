using System.Diagnostics;
using Picturify.Util;
using Picturify.Core;
using Picturify.Core.Pixels;
using Picturify.Core.Processing;
using Picturify.Core.Sizes;
using Picturify.IO.FFmpeg;
using Picturify.IO.ImageMagick;

// var reader = new ImageMagickFileReader("/home/sobczal/Downloads/ducks.png");

var reader = new FFmpegMovieReader("/home/sobczal/Downloads/latawce_cut.mp4");

var writer = new FFmpegMovieWriter("/home/sobczal/Downloads/latawce2.mp4", 30, new IntSize(1920, 1080));

var processor = new RGBAColorMappingProcessor(
    pixel =>
    {
        var red = pixel[ColorChannels.Red];
        var green = pixel[ColorChannels.Green];
        var blue = pixel[ColorChannels.Blue];

        var newRed = red * 0.393f + green * 0.769f + blue * 0.189f;
        var newGreen = red * 0.349f + green * 0.686f + blue * 0.168f;
        var newBlue = red * 0.272f + green * 0.534f + blue * 0.131f;

        newRed = PicturifyMathF.Clamp(newRed, 0, 1);
        newGreen = PicturifyMathF.Clamp(newGreen, 0, 1);
        newBlue = PicturifyMathF.Clamp(newBlue, 0, 1);

        return new RGBAPixel(
            newRed,
            newGreen,
            newBlue,
            pixel[ColorChannels.Alpha]
        );
    }
);

var i = 0;
var sw = new Stopwatch();
while (reader.HasNext())
{
    sw.Restart();
    var image = reader.ReadNext();
    sw.Stop();
    Console.WriteLine($"Read time: {sw.ElapsedMilliseconds}ms");
    // processor.Process(image);
    sw.Restart();
    writer.Write(image);
    sw.Stop();
    Console.WriteLine($"Write time: {sw.ElapsedMilliseconds}ms");
}