namespace MaaFramework.Binding.Infos;

/// <summary>
///     A class providing properties of window information.
/// </summary>
public sealed class WindowInfo
{
    /// <summary>
    ///     Gets the handle to a win32 window.
    /// </summary>
    public required nint Hwnd { get; init; }
}
