// # ==============================================================================
// # Solution: Picturify
// # File: IProcessor.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.Processing;

public interface IProcessor
{
    IEnumerable<ColorSpace> PreferredColorSpaces { get; }
    IEnumerable<ColorSpace> SupportedColorSpaces { get; }

    void Process(
        IImage image
    );
}