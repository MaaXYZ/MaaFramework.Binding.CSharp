using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaSyncContext with generic handle.
/// </summary>
public interface IMaaSyncContext<T> : IMaaSyncContext
{
    /// <summary>
    ///     Gets or inits a MaaSyncContextHandle.
    /// </summary>
    T Handle { get; init; }

    /// <inheritdoc cref="IMaaSyncContext.RunRecognizer"/>
    bool RunRecognizer(IMaaImageBuffer<T> image, string task, string taskParam, IMaaRectBuffer<T> outBox, IMaaStringBuffer<T> detailBuff);

    /// <inheritdoc cref="IMaaSyncContext.RunAction"/>
    bool RunAction(string task, string taskParam, IMaaRectBuffer<T> curBox, string curRecDetail);

    /// <inheritdoc cref="IMaaSyncContext.Screencap"/>
    bool Screencap(IMaaImageBuffer<T> buffer);

    /// <inheritdoc cref="IMaaSyncContext.GetTaskResult"/>
    bool GetTaskResult(string task, IMaaStringBuffer<T> buffer);
}

/// <summary>
///     An interface defining wrapped members for MaaSyncContext.
/// </summary>
public interface IMaaSyncContext
{
    /// <summary>
    ///     Runs a task.
    /// </summary>
    /// <param name="task">The task name.</param>
    /// <param name="param">The task parameters.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    bool RunTask(string task, string param);

    /// <summary>
    ///     Run a recognizer.
    /// </summary>
    /// <param name="image">The images to be recognized.</param>
    /// <param name="task">The task name.</param>
    /// <param name="taskParam">The task parameters.</param>
    /// <param name="outBox">The rect buffer to receive the rect in the recognition result.</param>
    /// <param name="detailBuff">The string buffer to receive the rect detail in the recognition result.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    bool RunRecognizer(IMaaImageBuffer image, string task, string taskParam, IMaaRectBuffer outBox, IMaaStringBuffer detailBuff);

    /// <summary>
    ///     Run an action.
    /// </summary>
    /// <param name="task">The task name.</param>
    /// <param name="taskParam">The task parameters.</param>
    /// <param name="curBox">The rect buffer containing curent rect in the recognition result.</param>
    /// <param name="curRecDetail">The rect detail in the recognition result.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    bool RunAction(string task, string taskParam, IMaaRectBuffer curBox, string curRecDetail);

    /// <summary>
    ///     Clicks a point.
    /// </summary>
    /// <param name="x">The horizontal coordinate of the point.</param>
    /// <param name="y">The vertical coordinate of the point.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    bool Click(int x, int y);

    /// <summary>
    ///     Swipes from a starting point to a ending point with duration.
    /// </summary>
    /// <param name="x1">The horizontal coordinate of the starting point.</param>
    /// <param name="y1">The vertical coordinate of the starting point.</param>
    /// <param name="x2">The horizontal coordinate of the ending point.</param>
    /// <param name="y2">The horizontal coordinate of the ending point.</param>
    /// <param name="duration">The duration.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    bool Swipe(int x1, int y1, int x2, int y2, int duration);

    /// <summary>
    ///     Presses a key.
    /// </summary>
    /// <param name="keyCode">The code of the key.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    bool PressKey(int keyCode);

    /// <summary>
    ///     Usage: TouchDown -> TouchMove -> TouchUp.
    /// </summary>
    /// <param name="contact">The contact id.</param>
    /// <param name="x">The horizontal coordinate of the starting point.</param>
    /// <param name="y">The vertical coordinate of the starting point.</param>
    /// <param name="pressure">The pressure.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    bool TouchDown(int contact, int x, int y, int pressure);

    /// <summary>
    ///     Usage: TouchDown -> TouchMove -> TouchUp.
    /// </summary>
    /// <param name="contact">The contact id.</param>
    /// <param name="x">The horizontal coordinate of the ending point.</param>
    /// <param name="y">The vertical coordinate of the ending point.</param>
    /// <param name="pressure">The pressure.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    bool TouchMove(int contact, int x, int y, int pressure);

    /// <summary>
    ///     Usage: TouchDown -> TouchMove -> TouchUp.
    /// </summary>
    /// <param name="contact">The contact id.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    bool TouchUp(int contact);

    /// <summary>
    ///     Takes a screenshot.
    /// </summary>
    /// <param name="buffer">The image buffer to receive the screenshot.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    bool Screencap(IMaaImageBuffer buffer);

    /// <summary>
    ///     Gets a task result.
    /// </summary>
    /// <param name="task">The task name.</param>
    /// <param name="buffer">The string buffer to receive the task result.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    bool GetTaskResult(string task, IMaaStringBuffer buffer);
}
