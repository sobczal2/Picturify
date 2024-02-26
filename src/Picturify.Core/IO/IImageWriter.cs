// # ==============================================================================
// # Solution: Picturify
// # File: IImageWriter.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.IO;

public interface IImageWriter
{
    public bool CanWrite();
    public void Write(IImage image);
}