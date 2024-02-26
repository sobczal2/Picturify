// # ==============================================================================
// # Solution: Picturify
// # File: Size.cs
// # Author: Łukasz Sobczak
// # Created: 26-02-2024
// # ==============================================================================

namespace Picturify.Core;

public interface ISize
{
    int GetIntWidth();
    int GetIntHeight();
    
    float GetFloatWidth();
    float GetFloatHeight();
}