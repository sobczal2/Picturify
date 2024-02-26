// # ==============================================================================
// # Solution: Picturify
// # File: PicturifyMathF.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Persistify.Util;

public static class PicturifyMathF
{
    public static float Clamp(
        float value,
        float min,
        float max
    )
    {
        return value < min ? min : value > max ? max : value;
    }
}