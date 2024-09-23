using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaController with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaController<T> : IMaaController, IMaaDisposableHandle<T>
{
    /// <inheritdoc cref="IMaaController.GetCachedImage"/>
    bool GetCachedImage(IMaaImageBuffer<nint> maaImage);
}

/// <summary>
///     An interface defining wrapped members for MaaController.
/// </summary>
public interface IMaaController : IMaaCommon, IMaaOption<ControllerOption>, IMaaPost, IMaaDisposable
{
    /// <summary>
    ///     Connects the address specified by the constructor.
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
    /// </summary>
    /// <param name="keyCode">The code of the key.</param>
    /// <returns>A press key <see cref="MaaJob"/>.</returns>
    MaaJob PressKey(int keyCode);

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
    /// </summary>
    /// <remarks>
    ///     <para>For adb controller, contact means finger id (0 for first finger, 1 for second finger, etc.).</para>
    ///     <para>For win32 controller, contact means mouse button id (0 for left, 1 for right, 2 for middle).</para>
    /// </remarks>
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
    ///     Takes a screenshot.
    /// </summary>
    /// <returns>A screen capture <see cref="MaaJob"/>.</returns>
    MaaJob Screencap();

    /// <summary>
    ///     Ends the connection of the address specified by the constructor.
    /// </summary>
    /// <returns><see langword="true"/> if the connection was ended successfully; otherwise, <see langword="false"/>.</returns>
    bool LinkStop();

    /// <summary>
    ///     Gets the cached image.
    /// </summary>
    /// <returns>An <see cref="IMaaImageBuffer"/> if the hash was successfully got; otherwise, <see langword="null"/>.</returns>
    bool GetCachedImage(IMaaImageBuffer maaImage);

    /// <summary>
    ///     Gets the uuid string of the <see cref="IMaaController"/>.
    /// </summary>
    /// <returns>A <see cref="string"/> if the hash was successfully got; otherwise, <see langword="null"/>.</returns>
    string? Uuid { get; }
}
