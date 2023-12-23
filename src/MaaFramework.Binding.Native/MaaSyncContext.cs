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
    public bool RunTask(string taskName, string param)
        => MaaSyncContextRunTask(Handle, taskName, param).ToBoolean();

    /// <inheritdoc/>
    bool IMaaSyncContext.RunRecognizer(IMaaImageBuffer image, string taskName, string taskParam, IMaaRectBuffer outBox, IMaaStringBuffer outDetail)
        => RunRecognizer((IMaaImageBuffer<nint>)image, taskName, taskParam, (IMaaRectBuffer<nint>)outBox, (IMaaStringBuffer<nint>)outDetail);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextRunRecognizer"/>.
    /// </remarks>
    public bool RunRecognizer(IMaaImageBuffer<nint> image, string taskName, string taskParam, IMaaRectBuffer<nint> outBox, IMaaStringBuffer<nint> outDetail)
    {
        ArgumentNullException.ThrowIfNull(image);
        ArgumentNullException.ThrowIfNull(outBox);
        ArgumentNullException.ThrowIfNull(outDetail);

        return MaaSyncContextRunRecognizer(Handle, image.Handle, taskName, taskParam, outBox.Handle, outDetail.Handle).ToBoolean();
    }

    /// <inheritdoc/>
    bool IMaaSyncContext.RunAction(string taskName, string taskParam, IMaaRectBuffer curBox, string curRecDetail)
        => RunAction(taskName, taskParam, (IMaaRectBuffer<nint>)curBox, curRecDetail);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextRunAction"/>.
    /// </remarks>
    public bool RunAction(string taskName, string taskParam, IMaaRectBuffer<nint> curBox, string curRecDetail)
    {
        ArgumentNullException.ThrowIfNull(curBox);
        return MaaSyncContextRunAction(Handle, taskName, taskParam, curBox.Handle, curRecDetail).ToBoolean();
    }

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
    ///     Wrapper of <see cref="MaaSyncContextInputText"/>.
    /// </remarks>
    public bool InputText(string text)
        => MaaSyncContextInputText(Handle, text).ToBoolean();

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
    bool IMaaSyncContext.Screencap(IMaaImageBuffer outImage)
        => Screencap((IMaaImageBuffer<nint>)outImage);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextScreencap"/>.
    /// </remarks>
    public bool Screencap(IMaaImageBuffer<nint> outImage)
    {
        ArgumentNullException.ThrowIfNull(outImage);
        return MaaSyncContextScreencap(Handle, outImage.Handle).ToBoolean();
    }

    /// <inheritdoc/>
    bool IMaaSyncContext.GetTaskResult(string taskName, IMaaStringBuffer outTaskResult)
        => GetTaskResult(taskName, (IMaaStringBuffer<nint>)outTaskResult);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSyncContextGetTaskResult"/>.
    /// </remarks>
    public bool GetTaskResult(string taskName, IMaaStringBuffer<nint> outTaskResult)
    {
        ArgumentNullException.ThrowIfNull(outTaskResult);
        return MaaSyncContextGetTaskResult(Handle, taskName, outTaskResult.Handle).ToBoolean();
    }
}
