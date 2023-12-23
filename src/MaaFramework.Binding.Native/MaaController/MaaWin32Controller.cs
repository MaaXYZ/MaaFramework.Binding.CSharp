using static MaaFramework.Binding.Native.Interop.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaWin32ControllerCreate"/>.
/// </summary>
public class MaaWin32Controller : MaaController
{
    /// <inheritdoc cref="MaaWin32Controller(nint, Win32ControllerTypes, MaaCallbackTransparentArg)"/>
    public MaaWin32Controller(nint hWnd, Win32ControllerTypes types)
       : this(hWnd, types, MaaCallbackTransparentArg.Zero)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaWin32Controller"/> instance.
    /// </summary>
    /// <param name="hWnd">The window handle.</param>
    /// <param name="types">The Win32ControllerTypes.</param>
    /// <param name="maaCallbackTransparentArg">The MaaCallbackTransparentArg.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaWin32ControllerCreate"/>.
    /// </remarks>
    public MaaWin32Controller(nint hWnd, Win32ControllerTypes types, MaaCallbackTransparentArg maaCallbackTransparentArg)
    {
        var handle = MaaWin32ControllerCreate(hWnd, (MaaWin32ControllerType)types, MaaApiCallback, maaCallbackTransparentArg);
        SetHandle(handle, needReleased: true);
    }
}
