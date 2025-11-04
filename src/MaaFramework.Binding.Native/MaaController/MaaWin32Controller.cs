using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaWin32ControllerCreate"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaWin32Controller : MaaController
{
    private readonly DesktopWindowInfo _debugInfo;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ {nameof(_debugInfo.Name)} = {_debugInfo.Name}, {nameof(_debugInfo.ClassName)} = {_debugInfo.ClassName}, ScreencapMethod = {_debugInfo.ScreencapMethod}, MouseMethod = {_debugInfo.MouseMethod}, KeyboardMethod = {_debugInfo.KeyboardMethod} }}";

    /// <summary>
    ///     Creates a <see cref="MaaWin32Controller"/> instance.
    /// </summary>
    /// <param name="info">The desktop window info.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaWin32ControllerCreate"/>.
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="MaaJobStatusException"/>
    public MaaWin32Controller(DesktopWindowInfo info, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        if (info.Handle == nint.Zero) throw new ArgumentException("Value cannot be zero.", "info.Handle");

        var handle = MaaWin32ControllerCreate(info.Handle, (MaaWin32ScreencapMethod)info.ScreencapMethod, (MaaWin32InputMethod)info.MouseMethod, (MaaWin32InputMethod)info.KeyboardMethod);
        _ = MaaControllerAddSink(Handle, MaaEventCallback, nint.Zero);
        SetHandle(handle, needReleased: true);

        _debugInfo = info;

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, info);
    }

    /// <param name="hWnd">The handle to a win32 window.</param>
    /// <param name="screencapMethod">The screencap method.</param>
    /// <param name="mouseMethod">The mouse method.</param>
    /// <param name="keyboardMethod">The keyboard method.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <inheritdoc cref="Binding.MaaWin32Controller(DesktopWindowInfo, LinkOption, CheckStatusOption)"/>
    public MaaWin32Controller(nint hWnd, Win32ScreencapMethod screencapMethod, Win32InputMethod mouseMethod, Win32InputMethod keyboardMethod, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
        : this(new DesktopWindowInfo(hWnd, string.Empty, string.Empty, screencapMethod, mouseMethod, keyboardMethod), link, check)
    {
    }
}
