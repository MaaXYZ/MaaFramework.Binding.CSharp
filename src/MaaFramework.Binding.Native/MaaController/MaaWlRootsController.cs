using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaWlRootsControllerCreate"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaWlRootsController : MaaController
{
#pragma warning disable IDE0032 // 使用自动属性
    private readonly string _debugWlrSocketPath;
    private readonly bool _debugUseWin32VirtualKeyCodes;
#pragma warning restore IDE0032 // 使用自动属性

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ WlrSocketPath = {_debugWlrSocketPath}, UseWin32VirtualKeyCodes = {_debugUseWin32VirtualKeyCodes} }}";

    /// <summary>
    ///     Creates a <see cref="MaaWlRootsController"/> instance.
    /// </summary>
    /// <param name="wlrSocketPath">The wayland socket path (e.g., "/run/user/1000/wayland-0").</param>
    /// <param name="useWin32VirtualKeyCodes">
    ///     <para>If true, key codes passed to <see cref="MaaController.ClickKey(int)"/> / <see cref="MaaController.KeyDown(int)"/> / <see cref="MaaController.KeyUp(int)"/>
    ///     <br/>interpreted as Win32 Virtual-Key codes (VK_*) and translated to Linux evdev codes internally.</para>
    ///     <para>If false, key codes are passed through as raw evdev codes.</para>
    /// </param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaWlRootsControllerCreate"/>.
    ///     <para>This controller is designed for wlroots on Linux.</para>
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="MaaJobStatusException"/>
    public MaaWlRootsController(string wlrSocketPath, bool useWin32VirtualKeyCodes = false, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentException.ThrowIfNullOrEmpty(wlrSocketPath);

        var handle = MaaWlRootsControllerCreate(wlrSocketPath, useWin32VirtualKeyCodes);
        _ = MaaControllerAddSink(handle, MaaEventCallback, (nint)MaaHandleType.Controller);
        SetHandle(handle, needReleased: true);

        _debugWlrSocketPath = wlrSocketPath;
        _debugUseWin32VirtualKeyCodes = useWin32VirtualKeyCodes;

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, wlrSocketPath);
    }
}
