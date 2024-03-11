namespace MaaFramework.Binding;

/// <summary>
///     A static class providing extension methods for the creation of MaaWin32Controller.
/// </summary>
public static class WindowInfoExtension
{
    /// <summary>
    ///     Converts a <see cref="WindowInfo"/> to a <see cref="MaaWin32Controller"/>.
    /// </summary>
    /// <param name="info">The WindowInfo.</param>
    /// <param name="types">The Win32ControllerTypes.</param>
    /// <param name="hWnd">The new handle to a win32 window.</param>
    /// <param name="link">Executes <see cref="MaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSuccess"/>; otherwise, not check.</param>
    /// <returns>A MaaWin32Controller.</returns>
    public static MaaWin32Controller ToWin32Controller(this WindowInfo info,
        Win32ControllerTypes types,
        nint? hWnd = null,
        LinkOption link = LinkOption.Start,
        CheckStatusOption check = CheckStatusOption.ThrowIfNotSuccess)
    {
        ArgumentNullException.ThrowIfNull(info);

        return new(hWnd ?? info.Hwnd,
                   types,
                   link,
                   check);
    }
}
