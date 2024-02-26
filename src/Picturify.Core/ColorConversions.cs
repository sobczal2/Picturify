// # ==============================================================================
// # Solution: Picturify
// # File: ColorConversions.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core;

public static class ColorConversions
{
    // ReSharper disable once InconsistentNaming
    public static float HueFromRGB(
        float r,
        float g,
        float b
    )
    {
        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));
        var delta = max - min;

        if (delta == 0)
        {
            return 0;
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (max == r)
        {
            return 60 * (((g - b) / delta) % 6);
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (max == g)
        {
            return 60 * (((b - r) / delta) + 2);
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (max == b)
        {
            return 60 * (((r - g) / delta) + 4);
        }

        throw new Exception("This should never happen");
    }

    // ReSharper disable once InconsistentNaming
    public static float SaturationFromRGB(
        float r,
        float g,
        float b
    )
    {
        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));
        var delta = max - min;

        if (max == 0)
        {
            return 0;
        }

        return delta / max;
    }

    // ReSharper disable once InconsistentNaming
    public static float LightnessFromRGB(
        float r,
        float g,
        float b
    )
    {
        var max = Math.Max(r, Math.Max(g, b));
        var min = Math.Min(r, Math.Min(g, b));

        return (max + min) / 2;
    }

    // ReSharper disable once InconsistentNaming
    public static float ValueFromRGB(
        float r,
        float g,
        float b
    )
    {
        return Math.Max(r, Math.Max(g, b));
    }

    // ReSharper disable once InconsistentNaming
    public static float RedFromHSL(
        float h,
        float s,
        float l
    )
    {
        if (s == 0)
        {
            return l;
        }

        var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
        var p = 2 * l - q;

        return HueToRGB(p, q, h + 1 / 3f);
    }

    // ReSharper disable once InconsistentNaming
    public static float GreenFromHSL(
        float h,
        float s,
        float l
    )
    {
        if (s == 0)
        {
            return l;
        }

        var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
        var p = 2 * l - q;

        return HueToRGB(p, q, h);
    }

    // ReSharper disable once InconsistentNaming
    public static float BlueFromHSL(
        float h,
        float s,
        float l
    )
    {
        if (s == 0)
        {
            return l;
        }

        var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
        var p = 2 * l - q;

        return HueToRGB(p, q, h - 1 / 3f);
    }
    
    // ReSharper disable once InconsistentNaming
    private static float HueToRGB(
        float p,
        float q,
        float t
    )
    {
        if (t < 0)
        {
            t += 1;
        }

        if (t > 1)
        {
            t -= 1;
        }

        if (t < 1 / 6f)
        {
            return p + (q - p) * 6 * t;
        }

        if (t < 1 / 2f)
        {
            return q;
        }

        if (t < 2 / 3f)
        {
            return p + (q - p) * (2 / 3f - t) * 6;
        }

        return p;
    }
    
    // ReSharper disable once InconsistentNaming
    public static float ValueFromHSL(
        float h,
        float s,
        float l
    )
    {
        return l + s * Math.Min(l, 1 - l);
    }
    
    // ReSharper disable once InconsistentNaming
    public static float RedFromHSV(
        float h,
        float s,
        float v
    )
    {
        if (s == 0)
        {
            return v;
        }

        var i = (int) (h * 6);
        var f = h * 6 - i;
        var p = v * (1 - s);
        var q = v * (1 - f * s);
        var t = v * (1 - (1 - f) * s);

        return i switch
        {
            0 => v,
            1 => q,
            2 => p,
            3 => p,
            4 => t,
            5 => v,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    // ReSharper disable once InconsistentNaming
    public static float GreenFromHSV(
        float h,
        float s,
        float v
    )
    {
        if (s == 0)
        {
            return v;
        }

        var i = (int) (h * 6);
        var f = h * 6 - i;
        var p = v * (1 - s);
        var q = v * (1 - f * s);
        var t = v * (1 - (1 - f) * s);

        return i switch
        {
            0 => t,
            1 => v,
            2 => v,
            3 => q,
            4 => p,
            5 => p,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    // ReSharper disable once InconsistentNaming
    public static float BlueFromHSV(
        float h,
        float s,
        float v
    )
    {
        if (s == 0)
        {
            return v;
        }

        var i = (int) (h * 6);
        var f = h * 6 - i;
        var p = v * (1 - s);
        var q = v * (1 - f * s);
        var t = v * (1 - (1 - f) * s);

        return i switch
        {
            0 => p,
            1 => p,
            2 => t,
            3 => v,
            4 => v,
            5 => q,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    // ReSharper disable once InconsistentNaming
    public static float LightnessFromHSV(
        float h,
        float s,
        float v
    )
    {
        return (2 - s) * v / 2;
    }
}