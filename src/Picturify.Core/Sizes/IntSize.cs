// # ==============================================================================
// # Solution: Picturify
// # File: IntSize.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.Sizes;

public class IntSize : ISize
{
    private readonly int _width;
    private readonly int _height;
    
    public IntSize(int width, int height)
    {
        _width = width;
        _height = height;
    }
    
    public int GetIntWidth() => _width;
    public int GetIntHeight() => _height;
    
    public float GetFloatWidth() => _width;
    public float GetFloatHeight() => _height;
}