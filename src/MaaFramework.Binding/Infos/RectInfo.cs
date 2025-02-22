namespace MaaFramework.Binding;

/// <summary>
///     A sealed record providing properties of rect information.
/// </summary>
/// <param name="X">Gets the horizontal coordinate.</param>
/// <param name="Y">Gets the vertical coordinate.</param>
/// <param name="Width">Gets the width.</param>
/// <param name="Height">Gets the height.</param>
public sealed record RectInfo(
    int X,
    int Y,
    int Width,
    int Height
);
