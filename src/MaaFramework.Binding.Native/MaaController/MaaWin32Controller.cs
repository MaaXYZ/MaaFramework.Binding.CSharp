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
    private readonly Win32ScreencapMethod _debugScreencapMethod;
    private readonly Win32InputMethod _debugInputMethod;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ {nameof(_debugInfo.Name)} = {_debugInfo.Name}, {nameof(_debugInfo.ClassName)} = {_debugInfo.ClassName}, ScreencapMethod = {_debugScreencapMethod}, InputMethod = {_debugInputMethod} }}";

    /// <summary>
    ///     Creates a <see cref="MaaWin32Controller"/> instance.
    /// </summary>
    /// <param name="desktopWindow">The desktop window info.</param>
    /// <param name="screencapMethod">The screencap method.</param>
    /// <param name="inputMethod">The input method.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaWin32ControllerCreate"/>.
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="MaaJobStatusException"/>
    public MaaWin32Controller(DesktopWindowInfo desktopWindow, Win32ScreencapMethod screencapMethod, Win32InputMethod inputMethod, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        if (screencapMethod == Win32ScreencapMethod.None) throw new ArgumentException($"Value cannot be {Win32ScreencapMethod.None}.", nameof(screencapMethod));
        if (inputMethod == Win32InputMethod.None) throw new ArgumentException($"Value cannot be {Win32InputMethod.None}.", nameof(inputMethod));

        var handle = MaaWin32ControllerCreate(desktopWindow.Handle, (MaaWin32ScreencapMethod)screencapMethod, (MaaWin32InputMethod)inputMethod, MaaNotificationCallback, nint.Zero);
        SetHandle(handle, needReleased: true);

        _debugInfo = desktopWindow;
        _debugScreencapMethod = screencapMethod;
        _debugInputMethod = inputMethod;

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, desktopWindow, screencapMethod, inputMethod);
    }

    /// <param name="hWnd">The handle to a win32 window.</param>
    /// <param name="screencapMethod">The screencap method.</param>
    /// <param name="inputMethod">The input method.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <inheritdoc cref="Binding.MaaWin32Controller(DesktopWindowInfo, Win32ScreencapMethod, Win32InputMethod, LinkOption, CheckStatusOption)"/>
    public MaaWin32Controller(nint hWnd, Win32ScreencapMethod screencapMethod, Win32InputMethod inputMethod, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
        : this(new DesktopWindowInfo(hWnd, string.Empty, string.Empty),
            screencapMethod, inputMethod, link, check)
    {
    }
}
