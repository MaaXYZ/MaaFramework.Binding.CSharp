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
///     <para>- <c>ClickKey</c> / <c>KeyDown</c> / <c>KeyUp</c> :
///         For digital buttons (A, B, X, Y, LB, RB, etc.)<br/>
///         See <see cref="GamepadButton"/> for available buttons.</para>
///     <para>- <c>TouchDown</c> / <c>TouchMove</c> / <c>TouchUp</c> :
///         For analog inputs (sticks and triggers)<br/>
///         See <see cref="GamepadTouch"/> for contact mapping.</para>
///     <para>- <c>Click</c> / <c>Swipe</c> :
///         Not directly supported for gamepad.</para>
///     <para>- <c>InputText</c> / <c>StartApp</c> / <c>StopApp</c> / <c>Scroll</c> :
///         Not supported.</para>
/// </remarks>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaGamepadController : MaaController
{
    private readonly GamepadType _debugGamepadType;
    private readonly DesktopWindowInfo? _debugInfo;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ {DebuggerInfoString}, GamepadType = {_debugGamepadType} }}";

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerInfoString => _debugInfo is null
        ? "No Screencap"
        : $"{nameof(_debugInfo.Name)} = {_debugInfo.Name}, {nameof(_debugInfo.ClassName)} = {_debugInfo.ClassName}, ScreencapMethod = {_debugInfo.ScreencapMethod}";

    /// <summary>
    ///     Creates a <see cref="MaaGamepadController"/> instance.
    /// </summary>
    /// <param name="gamepadType">The type of virtual gamepad.</param>
    /// <param name="info">The desktop window info (null if screencap not needed).</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGamepadControllerCreate"/>.
    /// </remarks>
    /// <exception cref="MaaJobStatusException"/>
    public MaaGamepadController(GamepadType gamepadType, DesktopWindowInfo? info = null, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        var handle = info is null
            ? MaaGamepadControllerCreate(nint.Zero, (MaaGamepadType)gamepadType, 0UL)
            : MaaGamepadControllerCreate(info.Handle, (MaaGamepadType)gamepadType, (MaaWin32ScreencapMethod)info.ScreencapMethod);
        _ = MaaControllerAddSink(handle, MaaEventCallback, 5);
        SetHandle(handle, needReleased: true);

        _debugGamepadType = gamepadType;
        _debugInfo = info;

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, info, gamepadType);
    }

    /// <param name="hWnd">Window handle for screencap (optional, can be <see cref="nint.Zero"/> if screencap not needed).</param>
    /// <param name="gamepadType">Type of virtual gamepad (<see cref="GamepadType.Xbox360"/> or <see cref="GamepadType.DualShock4"/>).</param>
    /// <param name="screencapMethod">Win32 screencap method to use. Ignored if hWnd is <see cref="nint.Zero"/>.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <inheritdoc cref="Binding.MaaGamepadController(GamepadType, DesktopWindowInfo, LinkOption, CheckStatusOption)"/>
    public MaaGamepadController(nint hWnd, GamepadType gamepadType, Win32ScreencapMethod screencapMethod, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
        : this(gamepadType, hWnd == nint.Zero ? null : new(hWnd, string.Empty, string.Empty, screencapMethod, Win32InputMethod.None, Win32InputMethod.None), link, check)
    {
    }

    /// <summary>
    ///     Clicks a key.
    /// </summary>
    /// <param name="key">The <see cref="GamepadButton"/> value.</param>
    /// <returns>A click key <see cref="MaaJob"/>.</returns>
    public MaaJob ClickKey(GamepadButton key)
        => ClickKey(unchecked((int)key)); // GamepadButton < 0x1_0000_0000

    /// <summary>
    ///     Usage: KeyDown -> KeyUp.
    /// </summary>
    /// <param name="key">The <see cref="GamepadButton"/> value.</param>
    /// <returns>A key down <see cref="MaaJob"/>.</returns>
    public MaaJob KeyDown(GamepadButton key)
        => KeyDown(unchecked((int)key)); // GamepadButton < 0x1_0000_0000

    /// <returns>A key up <see cref="MaaJob"/>.</returns>
    /// <inheritdoc cref="KeyDown"/>
    public MaaJob KeyUp(GamepadButton key)
        => KeyUp(unchecked((int)key)); // GamepadButton < 0x1_0000_0000

    /// <summary>
    ///     Usage: StickStart -> StickMove -> StickEnd.
    /// </summary>
    /// <param name="contact">The <see cref="GamepadTouch"/> value.</param>
    /// <param name="x">The horizontal coordinate of the point.</param>
    /// <param name="y">The vertical coordinate of the point.</param>
    /// <returns>A touch down <see cref="MaaJob"/>.</returns>
    public MaaJob StickStart(GamepadTouch contact, short x, short y)
        => TouchDown(unchecked((int)contact), x, y, 0); // GamepadTouch < 0x1_0000_0000

    /// <returns>A touch move <see cref="MaaJob"/>.</returns>
    /// <inheritdoc cref="StickStart"/>
    public MaaJob StickMove(GamepadTouch contact, short x, short y)
        => TouchMove(unchecked((int)contact), x, y, 0); // GamepadTouch < 0x1_0000_0000

    /// <returns>A touch up <see cref="MaaJob"/>.</returns>
    /// <inheritdoc cref="StickStart"/>
    public MaaJob StickEnd(GamepadTouch contact)
        => TouchUp(unchecked((int)contact)); // GamepadTouch < 0x1_0000_0000

    /// <summary>
    ///     Usage: TriggerStart -> TriggerMove -> TriggerEnd.
    /// </summary>
    /// <param name="contact">The <see cref="GamepadTouch"/> value.</param>
    /// <param name="pressure">The pressure.</param>
    /// <returns>A touch down <see cref="MaaJob"/>.</returns>
    public MaaJob TriggerStart(GamepadTouch contact, byte pressure)
        => TouchDown(unchecked((int)contact), 0, 0, pressure); // GamepadTouch < 0x1_0000_0000

    /// <returns>A touch move <see cref="MaaJob"/>.</returns>
    /// <inheritdoc cref="StickStart"/>
    public MaaJob TriggerMove(GamepadTouch contact, byte pressure)
        => TouchMove(unchecked((int)contact), 0, 0, pressure); // GamepadTouch < 0x1_0000_0000

    /// <returns>A touch up <see cref="MaaJob"/>.</returns>
    /// <inheritdoc cref="StickStart"/>
    public MaaJob TriggerEnd(GamepadTouch contact)
        => TouchUp(unchecked((int)contact)); // GamepadTouch < 0x1_0000_0000
}
