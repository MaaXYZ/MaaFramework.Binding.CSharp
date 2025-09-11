using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;

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
    [Obsolete("Use ClickKey() instead.")]
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
    ///     Gets whether the <see cref="IMaaController"/> is connected to the device specified by the constructor.
    /// </summary>
    /// <returns><see langword="true"/> if the <see cref="IMaaController"/> is connected to the device; otherwise, <see langword="false"/>.</returns>
    bool IsConnected { get; }

    /// <summary>
    ///     Gets the cached image.
    /// </summary>
    /// <param name="maaImage">An <see cref="IMaaImageBuffer"/> used to get the cached image.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    bool GetCachedImage(IMaaImageBuffer maaImage);

    /// <summary>
    ///     Gets the uuid string of the <see cref="IMaaController"/>.
    /// </summary>
    /// <returns>A <see cref="string"/> if the hash was successfully got; otherwise, <see langword="null"/>.</returns>
    string? Uuid { get; }
}
