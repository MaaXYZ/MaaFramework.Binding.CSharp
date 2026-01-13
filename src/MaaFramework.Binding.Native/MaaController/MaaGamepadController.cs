using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaGamepadControllerCreate"/>.
/// </summary>
/// <remarks>
///     <para>Requires ViGEm Bus Driver to be installed on the system.</para>
///     <para>For gamepad control, use:</para>
///     <para>- <see cref="IMaaController.ClickKey"/>/<see cref="IMaaController.KeyDown"/>/<see cref="IMaaController.KeyUp"/>: For digital buttons (A, B, X, Y, LB, RB, etc.)
///         See <see cref="GamepadButton"/> for available buttons.</para>
///     <para>- <see cref="IMaaController.TouchDown"/>/<see cref="IMaaController.TouchMove"/>/<see cref="IMaaController.TouchUp"/>: For analog inputs (sticks and triggers)
///         See <see cref="GamepadTouch"/> for contact mapping.</para>
///     <para>Click and Swipe are not directly supported for gamepad.</para>
///     <para>InputText, StartApp, StopApp, Scroll are not supported.</para>
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaGamepadController : MaaController
{
    private readonly GamepadType _gamepadType;
    private readonly Win32ScreencapMethod _screencapMethod;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ GamepadType = {_gamepadType}, ScreencapMethod = {_screencapMethod} }}";

    /// <summary>
    ///     Creates a <see cref="MaaGamepadController"/> instance.
    /// </summary>
    /// <param name="hWnd">Window handle for screencap (optional, can be <see cref="nint.Zero"/> if screencap not needed).</param>
    /// <param name="gamepadType">Type of virtual gamepad (<see cref="GamepadType.Xbox360"/> or <see cref="GamepadType.DualShock4"/>).</param>
    /// <param name="screencapMethod">Win32 screencap method to use. Ignored if hWnd is <see cref="nint.Zero"/>.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGamepadControllerCreate"/>.
    /// </remarks>
    /// <exception cref="MaaJobStatusException"/>
    public MaaGamepadController(nint hWnd, GamepadType gamepadType, Win32ScreencapMethod screencapMethod, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        var handle = MaaGamepadControllerCreate(hWnd, (MaaGamepadType)gamepadType, (MaaWin32ScreencapMethod)screencapMethod);
        _ = MaaControllerAddSink(handle, MaaEventCallback, 5);
        SetHandle(handle, needReleased: true);

        _gamepadType = gamepadType;
        _screencapMethod = screencapMethod;

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, hWnd, gamepadType, screencapMethod);
    }
}
