namespace MaaFramework.Binding;

/// <summary>
///     A static class providing extension methods for the creation of MaaWin32Controller.
/// </summary>
public static class DesktopWindowInfoExtensions
{
    /// <summary>
    ///     Converts a <see cref="DesktopWindowInfo"/> to a <see cref="MaaWin32Controller"/>.
    /// </summary>
    /// <param name="info">The DesktopWindowInfo.</param>
    /// <param name="screencapMethod">The screencap method.</param>
    /// <param name="mouseMethod">The mouse method.</param>
    /// <param name="keyboardMethod">The keyboard method.</param>
    /// <param name="hWnd">The new handle to a win32 window.</param>
    /// <param name="link">Executes <see cref="MaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <returns>A MaaWin32Controller.</returns>
    /// <exception cref="ArgumentNullException"/>
    public static MaaWin32Controller ToWin32ControllerWith(this DesktopWindowInfo info,
        Win32ScreencapMethod? screencapMethod = null,
        Win32InputMethod? mouseMethod = null,
        Win32InputMethod? keyboardMethod = null,
        nint? hWnd = null,
        LinkOption link = LinkOption.Start,
        CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentNullException.ThrowIfNull(info);

        var handle = hWnd ?? info.Handle;
        return new MaaWin32Controller(
            new DesktopWindowInfo(
                handle,
                handle == info.Handle ? info.Name : string.Empty,
                handle == info.Handle ? info.ClassName : string.Empty,
                screencapMethod ?? info.ScreencapMethod,
                mouseMethod ?? info.MouseMethod,
                keyboardMethod ?? info.KeyboardMethod
            ),
            link,
            check
        );
    }

    /// <inheritdoc cref="ToWin32ControllerWith(DesktopWindowInfo, Win32ScreencapMethod?, Win32InputMethod?, Win32InputMethod?, nint?, LinkOption, CheckStatusOption)"/>
    public static MaaWin32Controller ToWin32Controller(this DesktopWindowInfo info,
        LinkOption link = LinkOption.Start,
        CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentNullException.ThrowIfNull(info);

        return new MaaWin32Controller(info, link, check);
    }
}
