using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Native.Interop;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Native.Interop.MaaSyncContext;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Native.Interop.MaaSyncContext"/>.
/// </summary>
public class MaaSyncContext : IMaaSyncContext<nint>
{
    /// <inheritdoc/>
    public required MaaSyncContextHandle Handle { get; init; }

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

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextRunTask"/>.
    /// </remarks>
    public bool RunTask(string task, string param)
        => MaaSyncContextRunTask(Handle, task, param).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextRunRecognizer"/>.
    /// </remarks>
    public bool RunRecognizer(IMaaImageBuffer image, string task, string taskParam, IMaaRectBuffer outBox, IMaaStringBuffer detailBuff)
        => RunRecognizer((IMaaImageBuffer<nint>)image, task, taskParam, (IMaaRectBuffer<nint>)outBox, (IMaaStringBuffer<nint>)detailBuff);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextRunRecognizer"/>.
    /// </remarks>
    public bool RunRecognizer(IMaaImageBuffer<nint> image, string task, string taskParam, IMaaRectBuffer<nint> outBox, IMaaStringBuffer<nint> detailBuff)
        => MaaSyncContextRunRecognizer(Handle, image.Handle, task, taskParam, outBox.Handle, detailBuff.Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextRunAction"/>.
    /// </remarks>
    public bool RunAction(string task, string taskParam, IMaaRectBuffer curBox, string curRecDetail)
        => RunAction(task, taskParam, (IMaaRectBuffer<nint>)curBox, curRecDetail);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextRunAction"/>.
    /// </remarks>
    public bool RunAction(string task, string taskParam, IMaaRectBuffer<nint> curBox, string curRecDetail)
        => MaaSyncContextRunAction(Handle, task, taskParam, curBox.Handle, curRecDetail).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextClick"/>.
    /// </remarks>
    public bool Click(int x, int y)
        => MaaSyncContextClick(Handle, x, y).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextSwipe"/>.
    /// </remarks>
    public bool Swipe(int x1, int y1, int x2, int y2, int duration)
        => MaaSyncContextSwipe(Handle, x1, y1, x2, y2, duration).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextPressKey"/>.
    /// </remarks>
    public bool PressKey(int keyCode)
        => MaaSyncContextPressKey(Handle, keyCode).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextTouchDown"/>.
    /// </remarks>
    public bool TouchDown(int contact, int x, int y, int pressure)
        => MaaSyncContextTouchDown(Handle, contact, x, y, pressure).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextTouchMove"/>.
    /// </remarks>
    public bool TouchMove(int contact, int x, int y, int pressure)
        => MaaSyncContextTouchMove(Handle, contact, x, y, pressure).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextTouchUp"/>.
    /// </remarks>
    public bool TouchUp(int contact)
        => MaaSyncContextTouchUp(Handle, contact).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextScreencap"/>.
    /// </remarks>
    public bool Screencap(IMaaImageBuffer buffer)
        => Screencap((IMaaImageBuffer<nint>)buffer);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextScreencap"/>.
    /// </remarks>
    public bool Screencap(IMaaImageBuffer<nint> buffer)
        => MaaSyncContextScreencap(Handle, buffer.Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextGetTaskResult"/>.
    /// </remarks>
    public bool GetTaskResult(string task, IMaaStringBuffer buffer)
        => GetTaskResult(task, (IMaaStringBuffer<nint>)buffer);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextGetTaskResult"/>.
    /// </remarks>
    public bool GetTaskResult(string task, IMaaStringBuffer<nint> buffer)
        => MaaSyncContextGetTaskResult(Handle, task, buffer.Handle).ToBoolean();
}
