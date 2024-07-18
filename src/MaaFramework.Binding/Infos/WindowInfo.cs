namespace MaaFramework.Binding;

/// <summary>
///     A class providing properties of window information.
/// </summary>
public sealed class WindowInfo
{
    /// <summary>
    ///     Gets the handle to a win32 window.
    /// </summary>
    public required nint Hwnd { get; init; }

    /// <summary>
    ///     Gets the window name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Gets the window class name.
    /// </summary>
    public required string ClassName { get; init; }
}
