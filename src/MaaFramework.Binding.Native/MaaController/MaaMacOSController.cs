using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaMacOSControllerCreate"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaMacOSController : MaaController
{
    private readonly uint _debugWindowId;
    private readonly MacOSScreencapMethod _debugScreencapMethod;
    private readonly MacOSInputMethod _debugInputMethod;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ WindowId = {_debugWindowId}, ScreencapMethod = {_debugScreencapMethod}, InputMethod = {_debugInputMethod} }}";

    /// <summary>
    ///     Creates a <see cref="MaaMacOSController"/> instance.
    /// </summary>
    /// <param name="windowId">The CGWindowID of the target window (0 for desktop).</param>
    /// <param name="screencapMethod">The macOS screencap method.</param>
    /// <param name="inputMethod">The macOS input method.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaMacOSControllerCreate"/>.
    ///     <para>This controller is designed for native macOS applications.</para>
    ///     <para>Requires Screen Recording permission for screencap.</para>
    ///     <para>Input simulation requires Accessibility permission.</para>
    ///     <para>Some features are not supported: start_app, stop_app, scroll.</para>
    ///     <para>Only single touch is supported (contact must be 0).</para>
    /// </remarks>
    /// <exception cref="MaaJobStatusException"/>
    public MaaMacOSController(uint windowId, MacOSScreencapMethod screencapMethod, MacOSInputMethod inputMethod, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        var handle = MaaMacOSControllerCreate(windowId, (MaaMacOSScreencapMethod)screencapMethod, (MaaMacOSInputMethod)inputMethod);
        _ = MaaControllerAddSink(handle, MaaEventCallback, (nint)MaaHandleType.Controller);
        SetHandle(handle, needReleased: true);

        _debugWindowId = windowId;
        _debugScreencapMethod = screencapMethod;
        _debugInputMethod = inputMethod;

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, windowId, screencapMethod, inputMethod);
    }
}
