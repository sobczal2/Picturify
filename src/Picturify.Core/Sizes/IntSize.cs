// # ==============================================================================
// # Solution: Picturify
// # File: IntSize.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core.Sizes;

public class IntSize : ISize
{
    private readonly int _height;
    private readonly int _width;

    public IntSize(
        int width,
        int height
    )
    {
        _width = width;
        _height = height;
    }

    public int GetIntWidth()
    {
        return _width;
    }

    public int GetIntHeight()
    {
        return _height;
    }

    public float GetFloatWidth()
    {
        return _width;
    }

    public float GetFloatHeight()
    {
        return _height;
    }
}