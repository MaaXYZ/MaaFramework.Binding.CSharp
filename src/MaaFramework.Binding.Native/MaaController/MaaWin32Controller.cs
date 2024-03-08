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
    /// <param name="types">The Win32ControllerTypes.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSuccess"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaWin32ControllerCreate"/>.
    /// </remarks>
    public MaaWin32Controller(MaaWin32Hwnd hWnd, Win32ControllerTypes types, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSuccess)
    {
        var handle = MaaWin32ControllerCreate(hWnd, (MaaWin32ControllerType)types, MaaApiCallback, nint.Zero);
        SetHandle(handle, needReleased: true);

        if (link == LinkOption.Start)
        {
            var status = LinkStart().Wait();
            if (check == CheckStatusOption.ThrowIfNotSuccess)
            {
                status.ThrowIfNot(MaaJobStatus.Success, MaaJobStatusException.MaaControllerMessage);
            }
        }
    }
}
