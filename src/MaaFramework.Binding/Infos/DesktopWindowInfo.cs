namespace MaaFramework.Binding;

/// <summary>
///     An abstract record providing properties of window information.
/// </summary>
/// <param name="Handle">Gets the handle to a window.</param>
/// <param name="Name">Gets the window name.</param>
/// <param name="ClassName">Gets the window class name.</param>
public abstract record DesktopWindowInfo(
    nint Handle,
    string Name,
    string ClassName
);
