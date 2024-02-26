// # ==============================================================================
// # Solution: Picturify
// # File: ColorMappingProcessor.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

using Picturify.Core.Pixels;

namespace Picturify.Core.Processing;

public class RGBAColorMappingProcessor : IProcessor
{
    private readonly Func<RGBAPixel, RGBAPixel> _mappingFunction;

    public IEnumerable<ColorSpace> SupportedColorSpaces => new[]
        { ColorSpace.RGBA, ColorSpace.RGB, ColorSpace.HSLA, ColorSpace.HSL, ColorSpace.HSVA, ColorSpace.HSV };

    public IEnumerable<ColorSpace> PreferredColorSpaces => new[] { ColorSpace.RGBA };

    public RGBAColorMappingProcessor(
        Func<RGBAPixel, RGBAPixel> mappingFunction
    )
    {
        _mappingFunction = mappingFunction;
    }

    public void Process(
        IImage image
    )
    {
        if (image.ColorSpace != ColorSpace.RGBA)
        {
            Console.WriteLine("Calculating in not optimized color space");
        }

        for (var h = 0; h < image.Size.GetIntHeight(); h++)
        {
            for (var w = 0; w < image.Size.GetIntWidth(); w++)
            {
                image[w, h] = _mappingFunction((RGBAPixel)image[w, h].WithColorSpace(ColorSpace.RGBA));
            }
        }
    }
}