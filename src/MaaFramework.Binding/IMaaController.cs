using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaController with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaController<T> : IMaaController, IMaaDisposableHandle<T>
{
    /// <inheritdoc cref="IMaaController.GetImage"/>
    bool GetImage(IMaaImageBuffer<T> maaImage);
}

/// <summary>
///     An interface defining wrapped members for MaaController.
/// </summary>
public interface IMaaController : IMaaCommon, IMaaOption<ControllerOption>, IMaaPost, IMaaDisposable
{
    /// <summary>
    ///     Connects the address specified by the constructor.
    /// </summary>
    /// <returns>A connection job.</returns>
    IMaaJob LinkStart();

    /// <summary>
    ///     Clicks a point.
    /// </summary>
    /// <param name="x">The horizontal coordinate of the point.</param>
    /// <param name="y">The vertical coordinate of the point.</param>
    /// <returns>A click job.</returns>
    IMaaJob Click(int x, int y);

    /// <summary>
    ///     Swipes from a starting point to a ending point with duration.
    /// </summary>
    /// <param name="x1">The horizontal coordinate of the starting point.</param>
    /// <param name="y1">The vertical coordinate of the starting point.</param>
    /// <param name="x2">The horizontal coordinate of the ending point.</param>
    /// <param name="y2">The horizontal coordinate of the ending point.</param>
    /// <param name="duration">The swipe duration(ms).</param>
    /// <returns>A swipe job.</returns>
    IMaaJob Swipe(int x1, int y1, int x2, int y2, int duration);

    /// <summary>
    ///     Presses a key.
    /// </summary>
    /// <param name="keyCode">The code of the key.</param>
    /// <returns>A press key job.</returns>
    IMaaJob PressKey(int keyCode);

    /// <summary>
    ///     Inputs a text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>A input text job.</returns>
    IMaaJob InputText(string text);

    /// <summary>
    ///     Starts an app.
    /// </summary>
    /// <param name="intent">The intent. eg: "com.hypergryph.arknights/com.u8.sdk.U8UnityContext".</param>
    /// <returns>A start app job.</returns>
    IMaaJob StartApp(string intent);

    /// <summary>
    ///     Stops an app.
    /// </summary>
    /// <param name="intent">The intent. eg: "com.hypergryph.arknights/com.u8.sdk.U8UnityContext".</param>
    /// <returns>A stop app job.</returns>
    IMaaJob StopApp(string intent);

    /// <summary>
    ///     Usage: TouchDown -> TouchMove -> TouchUp.
    /// </summary>
    /// <param name="contact">The contact id.</param>
    /// <param name="x">The horizontal coordinate of the starting point.</param>
    /// <param name="y">The vertical coordinate of the starting point.</param>
    /// <param name="pressure">The pressure.</param>
    /// <returns>A touch down job.</returns>
    IMaaJob TouchDown(int contact, int x, int y, int pressure);

    /// <summary>
    ///     Usage: TouchDown -> TouchMove -> TouchUp.
    /// </summary>
    /// <param name="contact">The contact id.</param>
    /// <param name="x">The horizontal coordinate of the ending point.</param>
    /// <param name="y">The vertical coordinate of the ending point.</param>
    /// <param name="pressure">The pressure.</param>
    /// <returns>A touch move job.</returns>
    IMaaJob TouchMove(int contact, int x, int y, int pressure);

    /// <summary>
    ///     Usage: TouchDown -> TouchMove -> TouchUp.
    /// </summary>
    /// <param name="contact">The contact id.</param>
    /// <returns>A touch up job.</returns>
    IMaaJob TouchUp(int contact);

    /// <summary>
    ///     Takes a screenshot.
    /// </summary>
    /// <returns>A screen capture job.</returns>
    IMaaJob Screencap();

    /// <summary>
    ///     Ends the connection of the address specified by the constructor.
    /// </summary>
    /// <returns>true if the connection was ended successfully; otherwise, false.</returns>
    bool LinkStop();

    /// <summary>
    ///     Gets a image.
    /// </summary>
    /// <param name="maaImage">The MaaImageBuffer.</param>
    /// <returns>true if the image was got successfully; otherwise, false.</returns>
    bool GetImage(IMaaImageBuffer maaImage);

    /// <summary>
    ///     Gets the uuid string of the <see cref="IMaaController"/>.
    /// </summary>
    /// <value>
    ///     A string if the hash was successfully got; otherwise, null.
    /// </value>
    string? Uuid { get; }
}
