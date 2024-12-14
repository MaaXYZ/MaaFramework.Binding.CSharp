using System.Diagnostics;
using MaaFramework.Binding.Abstractions.Native;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaController"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class MaaController : MaaCommon, IMaaController<nint>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)] private string DebuggerDisplay => $"{{{GetType().Name} {{ Disposed = {IsInvalid} }}}}";

    /// <summary>
    ///     Creates a <see cref="MaaController"/> instance.
    /// </summary>
    protected MaaController()
    {
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaControllerDestroy(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerSetOption"/>.
    /// </remarks>
    public bool SetOption<T>(ControllerOption opt, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var optValue = (value, opt) switch
        {
            (int vvvv, ControllerOption.ScreenshotTargetLongSide
                    or ControllerOption.ScreenshotTargetShortSide) => vvvv.ToMaaOptionValue(),
            (bool vvv, ControllerOption.Recording
                    or ControllerOption.ScreenshotUseRawSize) => vvv.ToMaaOptionValue(),

            _ => throw new NotSupportedException($"'{nameof(ControllerOption)}.{opt}' or type '{typeof(T)}' is not supported."),
        };

        return MaaControllerSetOption(Handle, (MaaCtrlOption)opt, optValue, (MaaOptionValueSize)optValue.Length);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostConnection"/>.
    /// </remarks>
    public MaaJob LinkStart()
    {
        var id = MaaControllerPostConnection(Handle);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostClick"/>.
    /// </remarks>
    public MaaJob Click(int x, int y)
    {
        var id = MaaControllerPostClick(Handle, x, y);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostSwipe"/>.
    /// </remarks>
    public MaaJob Swipe(int x1, int y1, int x2, int y2, int duration)
    {
        var id = MaaControllerPostSwipe(Handle, x1, y1, x2, y2, duration);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostPressKey"/>.
    /// </remarks>
    public MaaJob PressKey(int keyCode)
    {
        var id = MaaControllerPostPressKey(Handle, keyCode);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostInputText"/>.
    /// </remarks>
    public MaaJob InputText(string text)
    {
        var id = MaaControllerPostInputText(Handle, text);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostStartApp"/>.
    /// </remarks>
    public MaaJob StartApp(string intent)
    {
        var id = MaaControllerPostStartApp(Handle, intent);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostStopApp"/>.
    /// </remarks>
    public MaaJob StopApp(string intent)
    {
        var id = MaaControllerPostStopApp(Handle, intent);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostTouchDown"/>.
    /// </remarks>
    public MaaJob TouchDown(int contact, int x, int y, int pressure)
    {
        var id = MaaControllerPostTouchDown(Handle, contact, x, y, pressure);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostTouchMove"/>.
    /// </remarks>
    public MaaJob TouchMove(int contact, int x, int y, int pressure)
    {
        var id = MaaControllerPostTouchMove(Handle, contact, x, y, pressure);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostTouchUp"/>.
    /// </remarks>
    public MaaJob TouchUp(int contact)
    {
        var id = MaaControllerPostTouchUp(Handle, contact);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostScreencap"/>.
    /// </remarks>
    public MaaJob Screencap()
    {
        var id = MaaControllerPostScreencap(Handle);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(MaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return (MaaJobStatus)MaaControllerStatus(Handle, job.Id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerWait"/>.
    /// </remarks>
    public MaaJobStatus Wait(MaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return (MaaJobStatus)MaaControllerWait(Handle, job.Id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerConnected"/>.
    /// </remarks>
    public bool LinkStop()
        => MaaControllerConnected(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerCachedImage"/>.
    /// </remarks>
    public bool GetCachedImage(IMaaImageBuffer maaImage)
        => GetCachedImage((IMaaImageBuffer<nint>)maaImage);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerCachedImage"/>.
    /// </remarks>
    public bool GetCachedImage(IMaaImageBuffer<nint> maaImage)
    {
        ArgumentNullException.ThrowIfNull(maaImage);

        return MaaControllerCachedImage(Handle, maaImage.Handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerGetUuid"/>.
    /// </remarks>
    public string? Uuid
    {
        get
        {
            using var buffer = new MaaStringBuffer();
            return MaaControllerGetUuid(Handle, buffer.Handle)
                ? buffer.ToString()
                : null;
        }
    }
}
