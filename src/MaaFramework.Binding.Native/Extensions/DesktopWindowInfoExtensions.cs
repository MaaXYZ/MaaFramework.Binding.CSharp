﻿namespace MaaFramework.Binding;

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
    /// <param name="inputMethod">The input method.</param>
    /// <param name="hWnd">The new handle to a win32 window.</param>
    /// <param name="link">Executes <see cref="MaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <returns>A MaaWin32Controller.</returns>
    /// <exception cref="ArgumentNullException"/>
    public static MaaWin32Controller ToWin32Controller(this DesktopWindowInfo info,
        Win32ScreencapMethod screencapMethod,
        Win32InputMethod inputMethod,
        nint? hWnd = null,
        LinkOption link = LinkOption.Start,
        CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentNullException.ThrowIfNull(info);

        return hWnd.HasValue
            ? new MaaWin32Controller(hWnd.Value, screencapMethod, inputMethod, link, check)
            : new MaaWin32Controller(info, screencapMethod, inputMethod, link, check);
    }
}
