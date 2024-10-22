using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaWin32ControllerCreate"/>.
/// </summary>
public class MaaWin32Controller : MaaController
{
    /// <summary>
    ///     Creates a <see cref="MaaWin32Controller"/> instance.
    /// </summary>
    /// <param name="hWnd">The handle to a win32 window.</param>
    /// <param name="screencapMethod">The screencap method.</param>
    /// <param name="inputMethod">The input method.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaWin32ControllerCreate"/>.
    /// </remarks>
    public MaaWin32Controller(nint hWnd, Win32ScreencapMethod screencapMethod, Win32InputMethod inputMethod, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        if (hWnd == nint.Zero) throw new ArgumentException($"Value cannot be {nint.Zero}.", nameof(hWnd));
        if (screencapMethod == Win32ScreencapMethod.None) throw new ArgumentException($"Value cannot be {Win32ScreencapMethod.None}.", nameof(screencapMethod));
        if (inputMethod == Win32InputMethod.None) throw new ArgumentException($"Value cannot be {Win32InputMethod.None}.", nameof(inputMethod));

        var handle = MaaWin32ControllerCreate(hWnd, (MaaWin32ScreencapMethod)screencapMethod, (MaaWin32InputMethod)inputMethod, MaaNotificationCallback, nint.Zero);
        SetHandle(handle, needReleased: true);

        if (link != LinkOption.Start)
            return;

        var status = LinkStart().Wait();
        if (check == CheckStatusOption.ThrowIfNotSucceeded)
            status.ThrowIfNot(MaaJobStatus.Succeeded, MaaJobStatusException.MaaControllerMessage);
    }
}
