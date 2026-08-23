namespace MaaFramework.Binding;

/// <summary>
///     A record providing properties of window information.
/// </summary>
/// <param name="Handle">Gets the handle to a window.</param>
/// <param name="Name">Gets the window name.</param>
/// <param name="ClassName">Gets the window class name.</param>
/// <param name="ScreencapMethods">The screencap methods.</param>
/// <param name="MouseMethod">The mouse method.</param>
/// <param name="KeyboardMethod">The keyboard method.</param>
public record DesktopWindowInfo(
    nint Handle,
    string Name,
    string ClassName,
    Win32ScreencapMethods ScreencapMethods = Win32ScreencapMethods.Background,
    Win32InputMethod MouseMethod = Win32InputMethod.Seize,
    Win32InputMethod KeyboardMethod = Win32InputMethod.Seize
);
