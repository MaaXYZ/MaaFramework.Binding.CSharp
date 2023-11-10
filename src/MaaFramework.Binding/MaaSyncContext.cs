using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Interop;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Framework.MaaSyncContext;

namespace MaaFramework.Binding;

/// <summary>
///     A class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Framework.MaaSyncContext"/>.
/// </summary>
public class MaaSyncContext
{
    /// <inheritdoc cref="MaaSyncContext(MaaSyncContextHandle)"/>
    public MaaSyncContext()
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaSyncContext"/> instance.
    /// </summary>
    /// <param name="syncContext">The MaaSyncContextHandle.</param>
    [SetsRequiredMembers]
    public MaaSyncContext(MaaSyncContextHandle syncContext)
    {
        Handle = syncContext;
    }

    /// <summary>
    ///     Gets or inits a MaaSyncContextHandle.
    /// </summary>
    public required MaaSyncContextHandle Handle { get; init; }

    /// <summary>
    ///     Runs a task.
    /// </summary>
    /// <param name="task">The task name.</param>
    /// <param name="param">The task parameters.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextRunTask"/>.
    /// </remarks>
    public bool RunTask(string task, string param)
        => MaaSyncContextRunTask(Handle, task, param).ToBoolean();

    /// <summary>
    ///     Run a recognizer.
    /// </summary>
    /// <param name="image">The images to be recognized.</param>
    /// <param name="task">The task name.</param>
    /// <param name="taskParam">The task parameters.</param>
    /// <param name="outBox">The rect buffer to receive the rect in the recognition result.</param>
    /// <param name="detailBuff">The string buffer to receive the rect detail in the recognition result.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextRunRecognizer"/>.
    /// </remarks>
    public bool RunRecognizer(MaaImageBuffer image, string task, string taskParam, MaaRectBuffer outBox, MaaStringBuffer detailBuff)
        => MaaSyncContextRunRecognizer(Handle, image._handle, task, taskParam, outBox._handle, detailBuff._handle).ToBoolean();

    /// <summary>
    ///     Run an action.
    /// </summary>
    /// <param name="task">The task name.</param>
    /// <param name="taskParam">The task parameters.</param>
    /// <param name="curBox">The rect buffer containing curent rect in the recognition result.</param>
    /// <param name="curRecDetail">The rect detail in the recognition result.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextRunAction"/>.
    /// </remarks>
    public bool RunAction(string task, string taskParam, MaaRectBuffer curBox, string curRecDetail)
        => MaaSyncContextRunAction(Handle, task, taskParam, curBox._handle, curRecDetail).ToBoolean();

    /// <summary>
    ///     Clicks a point.
    /// </summary>
    /// <param name="x">The horizontal coordinate of the point.</param>
    /// <param name="y">The vertical coordinate of the point.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextClick"/>.
    /// </remarks>
    public bool Click(int x, int y)
        => MaaSyncContextClick(Handle, x, y).ToBoolean();

    /// <summary>
    ///     Swipes from a starting point to a ending point with duration.
    /// </summary>
    /// <param name="x1">The horizontal coordinate of the starting point.</param>
    /// <param name="y1">The vertical coordinate of the starting point.</param>
    /// <param name="x2">The horizontal coordinate of the ending point.</param>
    /// <param name="y2">The horizontal coordinate of the ending point.</param>
    /// <param name="duration">The duration.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextSwipe"/>.
    /// </remarks>
    public bool Swipe(int x1, int y1, int x2, int y2, int duration)
        => MaaSyncContextSwipe(Handle, x1, y1, x2, y2, duration).ToBoolean();

    /// <summary>
    ///     Presses a key.
    /// </summary>
    /// <param name="keyCode">The code of the key.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextPressKey"/>.
    /// </remarks>
    public bool PressKey(int keyCode)
        => MaaSyncContextPressKey(Handle, keyCode).ToBoolean();

    /// <summary>
    ///     Usage: TouchDown -> TouchMove -> TouchUp.
    /// </summary>
    /// <param name="contact">The contact id.</param>
    /// <param name="x">The horizontal coordinate of the starting point.</param>
    /// <param name="y">The vertical coordinate of the starting point.</param>
    /// <param name="pressure">The pressure.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextTouchDown"/>.
    /// </remarks>
    public bool TouchDown(int contact, int x, int y, int pressure)
        => MaaSyncContextTouchDown(Handle, contact, x, y, pressure).ToBoolean();

    /// <summary>
    ///     Usage: TouchDown -> TouchMove -> TouchUp.
    /// </summary>
    /// <param name="contact">The contact id.</param>
    /// <param name="x">The horizontal coordinate of the ending point.</param>
    /// <param name="y">The vertical coordinate of the ending point.</param>
    /// <param name="pressure">The pressure.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextTouchMove"/>.
    /// </remarks>
    public bool TouchMove(int contact, int x, int y, int pressure)
        => MaaSyncContextTouchMove(Handle, contact, x, y, pressure).ToBoolean();

    /// <summary>
    ///     Usage: TouchDown -> TouchMove -> TouchUp.
    /// </summary>
    /// <param name="contact">The contact id.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextTouchUp"/>.
    /// </remarks>
    public bool TouchUp(int contact)
        => MaaSyncContextTouchUp(Handle, contact).ToBoolean();

    /// <summary>
    ///     Takes a screenshot.
    /// </summary>
    /// <param name="buffer">The image buffer to receive the screenshot.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextScreencap"/>.
    /// </remarks>
    public bool Screencap(MaaImageBuffer buffer)
        => MaaSyncContextScreencap(Handle, buffer._handle).ToBoolean();

    /// <summary>
    ///     Gets a task result.
    /// </summary>
    /// <param name="task">The task name.</param>
    /// <param name="buffer">The string buffer to receive the task result.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextGetTaskResult"/>.
    /// </remarks>
    public bool GetTaskResult(string task, MaaStringBuffer buffer)
        => MaaSyncContextGetTaskResult(Handle, task, buffer._handle).ToBoolean();
}
