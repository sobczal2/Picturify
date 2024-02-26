// # ==============================================================================
// # Solution: Picturify
// # File: IImageSource.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.IO;

public interface IImageReader
{
    public bool HasNext();
    public IImage ReadNext();
}