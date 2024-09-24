namespace MaaFramework.Binding;

/// <summary>
///     A record providing properties of window information.
/// </summary>
/// <param name="Handle">Gets the handle to a window.</param>
/// <param name="Name">Gets the window name.</param>
/// <param name="ClassName">Gets the window class name.</param>
public record DesktopWindowInfo(
    nint Handle,
    string Name,
    string ClassName
);
