using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaController with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaController<T> : IMaaController, IMaaDisposableHandle<T>;

/// <summary>
///     An interface defining wrapped members for MaaController.
/// </summary>
public interface IMaaController : IMaaCommon, IMaaOption<ControllerOption>, IMaaDisposable
{
    /// <summary>
    ///     Gets the last valid posted job.
    /// </summary>
    /// <returns>A <see cref="MaaJob"/> if any valid job has been posted; otherwise, <see langword="null"/>..</returns>
    MaaJob? LastJob { get; }

    /// <summary>
    ///     Connects the device specified by the constructor.
    /// </summary>
    /// <returns>A connection <see cref="MaaJob"/>.</returns>
    MaaJob LinkStart();

    /// <summary>
    ///     Clicks a point.
    /// </summary>
    /// <param name="x">The horizontal coordinate of the point.</param>
    /// <param name="y">The vertical coordinate of the point.</param>
    /// <returns>A click <see cref="MaaJob"/>.</returns>
    MaaJob Click(int x, int y);

    /// <summary>
    ///     Swipes from a starting point to an ending point with duration.
    /// </summary>
    /// <param name="x1">The horizontal coordinate of the starting point.</param>
    /// <param name="y1">The vertical coordinate of the starting point.</param>
    /// <param name="x2">The horizontal coordinate of the ending point.</param>
    /// <param name="y2">The vertical coordinate of the ending point.</param>
    /// <param name="duration">The millisecond of the swipe duration(ms).</param>
    /// <returns>A swipe <see cref="MaaJob"/>.</returns>
    MaaJob Swipe(int x1, int y1, int x2, int y2, int duration);

    /// <summary>
    ///     Presses a key.
    ///     <para>For adb controller, <paramref name="keyCode"/> is from <a href="https://developer.android.com/reference/android/view/KeyEvent">android key event</a>.</para>
    ///     <para>For win32 controller, <paramref name="keyCode"/> is from <a href="https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes">windows virtual key</a>.</para>
    /// </summary>
    /// <param name="keyCode">The code of the key.</param>
    /// <returns>A press key <see cref="MaaJob"/>.</returns>
    [Obsolete($"Use {nameof(ClickKey)}() instead.")]
    MaaJob PressKey(int keyCode);

    /// <summary>
    ///     Clicks a key.
    ///     <para>For adb controller, <paramref name="keyCode"/> is from <a href="https://developer.android.com/reference/android/view/KeyEvent">android key event</a>.</para>
    ///     <para>For win32 controller, <paramref name="keyCode"/> is from <a href="https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes">windows virtual key</a>.</para>
    /// </summary>
    /// <param name="keyCode">The code of the key.</param>
    /// <returns>A click key <see cref="MaaJob"/>.</returns>
    MaaJob ClickKey(int keyCode);

    /// <summary>
    ///     Inputs a text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>An input text <see cref="MaaJob"/>.</returns>
    MaaJob InputText(string text);

    /// <summary>
    ///     Starts an app.
    /// </summary>
    /// <param name="intent">The intent.
    /// <para>e.g., "com.hypergryph.arknights/com.u8.sdk.U8UnityContext".</para>
    /// </param>
    /// <returns>A start app <see cref="MaaJob"/>.</returns>
    MaaJob StartApp(string intent);

    /// <summary>
    ///     Stops an app.
    /// </summary>
    /// <param name="intent">The intent.
    /// <para>e.g., "com.hypergryph.arknights".</para>
    /// </param>
    /// <returns>A stop app <see cref="MaaJob"/>.</returns>
    MaaJob StopApp(string intent);

    /// <summary>
    ///     Usage: TouchDown -> TouchMove -> TouchUp.
    ///     <para>For adb controller, <paramref name="contact"/> means finger id (0 for first finger, 1 for second finger, etc.).</para>
    ///     <para>For win32 controller, <paramref name="contact"/> means mouse button id (0 for left, 1 for right, 2 for middle).</para>
    /// </summary>
    /// <param name="contact">The contact id.</param>
    /// <param name="x">The horizontal coordinate of the point.</param>
    /// <param name="y">The vertical coordinate of the point.</param>
    /// <param name="pressure">The pressure.</param>
    /// <returns>A touch down <see cref="MaaJob"/>.</returns>
    MaaJob TouchDown(int contact, int x, int y, int pressure);

    /// <returns>A touch move <see cref="MaaJob"/>.</returns>
    /// <inheritdoc cref="TouchDown"/>
    MaaJob TouchMove(int contact, int x, int y, int pressure);

    /// <returns>A touch up <see cref="MaaJob"/>.</returns>
    /// <inheritdoc cref="TouchDown"/>
    MaaJob TouchUp(int contact);

    /// <summary>
    ///     Usage: KeyDown -> KeyUp.
    ///     <para>For adb controller, <paramref name="keyCode"/> is from <a href="https://developer.android.com/reference/android/view/KeyEvent">android key event</a>.</para>
    ///     <para>For win32 controller, <paramref name="keyCode"/> is from <a href="https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes">windows virtual key</a>.</para>
    /// </summary>
    /// <param name="keyCode">The code of the key.</param>
    /// <returns>A key down <see cref="MaaJob"/>.</returns>
    MaaJob KeyDown(int keyCode);

    /// <returns>A key up <see cref="MaaJob"/>.</returns>
    /// <inheritdoc cref="KeyDown"/>
    MaaJob KeyUp(int keyCode);

    /// <summary>
    ///     Takes a screenshot.
    /// </summary>
    /// <returns>A screen capture <see cref="MaaJob"/>.</returns>
    MaaJob Screencap();

    /// <summary>
    ///     Appends a job for scroll action.
    /// </summary>
    /// <param name="dx">The horizontal scroll delta. Positive values scroll right, negative values scroll left.</param>
    /// <param name="dy">The vertical scroll delta. Positive values scroll up, negative values scroll down.</param>
    /// <returns>A scroll <see cref="MaaJob"/>.</returns>
    /// <remarks>
    ///     <para>Not all controllers support scroll. If not supported, the action will fail.</para>
    ///     <para>The dx/dy values are sent directly as scroll increments. Using multiples of 120 (WHEEL_DELTA) is
    ///         recommended for best compatibility.
    ///     </para>
    /// </remarks>
    MaaJob Scroll(int dx, int dy);

    /// <summary>
    ///     Appends a job for shell command action.
    /// </summary>
    /// <param name="cmd">The shell command to execute.</param>
    /// <param name="timeout">The timeout in milliseconds. Default is 20000 (20 seconds).</param>
    /// <returns>A shell <see cref="MaaJob"/>.</returns>
    /// <remarks>
    ///     <para>This is only valid for ADB controllers. If the controller is not an ADB controller, the action will fail.</para>
    ///     <para>See also <see cref="GetShellOutput"/>.</para>
    /// </remarks>
    MaaJob Shell(string cmd, long timeout = 20000);

    /// <summary>
    ///     Gets the cached shell command output.
    /// </summary>
    /// <returns>A <see cref="string"/> if the output is available; otherwise, <see langword="null"/>.</returns>
    /// <remarks>This returns the output from the most recent shell command execution.</remarks>
    bool GetShellOutput([MaybeNullWhen(false)] out string output);

    /// <summary>
    ///     Gets whether the <see cref="IMaaController"/> is connected to the device specified by the constructor.
    /// </summary>
    /// <returns><see langword="true"/> if the <see cref="IMaaController"/> is connected to the device; otherwise, <see langword="false"/>.</returns>
    bool IsConnected { get; }

    /// <summary>
    ///     Gets the cached image.
    /// </summary>
    /// <param name="image">An <see cref="IMaaImageBuffer"/> used to get the cached image.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    bool GetCachedImage(IMaaImageBuffer image);

    /// <summary>
    ///     Gets the uuid string of the <see cref="IMaaController"/>.
    /// </summary>
    /// <returns>A <see cref="string"/> if the hash was successfully got; otherwise, <see langword="null"/>.</returns>
    string? Uuid { get; }
}
