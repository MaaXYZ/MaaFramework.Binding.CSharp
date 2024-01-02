using MaaFramework.Binding.Abstractions.Native;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaController"/>.
/// </summary>
public abstract class MaaController : MaaCommon, IMaaController<nint>
{
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

        var bytes = opt switch
        {
            ControllerOption.Invalid => throw new InvalidOperationException(),
            ControllerOption.ScreenshotTargetLongSide => value switch { int v => v.ToMaaOptionValues(), _ => throw new InvalidOperationException(), },
            ControllerOption.ScreenshotTargetShortSide => value switch { int v => v.ToMaaOptionValues(), _ => throw new InvalidOperationException(), },
            ControllerOption.DefaultAppPackageEntry => value switch { string v => v.ToMaaOptionValues(), _ => throw new InvalidOperationException(), },
            ControllerOption.DefaultAppPackage => value switch { string v => v.ToMaaOptionValues(), _ => throw new InvalidOperationException(), },
            ControllerOption.Recording => value switch { bool v => v.ToMaaOptionValues(), _ => throw new InvalidOperationException(), },
            _ => throw new NotImplementedException(),
        };

        return MaaControllerSetOption(Handle, (MaaCtrlOption)opt, ref bytes[0], (MaaOptionValueSize)bytes.Length).ToBoolean();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostConnection"/>.
    /// </remarks>
    public IMaaJob LinkStart()
    {
        var id = MaaControllerPostConnection(Handle);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostClick"/>.
    /// </remarks>
    public IMaaJob Click(int x, int y)
    {
        var id = MaaControllerPostClick(Handle, x, y);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostSwipe"/>.
    /// </remarks>
    public IMaaJob Swipe(int x1, int y1, int x2, int y2, int duration)
    {
        var id = MaaControllerPostSwipe(Handle, x1, y1, x2, y2, duration);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostPressKey"/>.
    /// </remarks>
    public IMaaJob PressKey(int keyCode)
    {
        var id = MaaControllerPostPressKey(Handle, keyCode);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostInputText"/>.
    /// </remarks>
    public IMaaJob InputText(string text)
    {
        var id = MaaControllerPostInputText(Handle, text);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostTouchDown"/>.
    /// </remarks>
    public IMaaJob TouchDown(int contact, int x, int y, int pressure)
    {
        var id = MaaControllerPostTouchDown(Handle, contact, x, y, pressure);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostTouchMove"/>.
    /// </remarks>
    public IMaaJob TouchMove(int contact, int x, int y, int pressure)
    {
        var id = MaaControllerPostTouchMove(Handle, contact, x, y, pressure);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostTouchUp"/>.
    /// </remarks>
    public IMaaJob TouchUp(int contact)
    {
        var id = MaaControllerPostTouchUp(Handle, contact);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostScreencap"/>.
    /// </remarks>
    public IMaaJob Screencap()
    {
        var id = MaaControllerPostScreencap(Handle);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    bool Abstractions.IMaaPost.SetParam(IMaaJob job, string param)
        => throw new InvalidOperationException();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(IMaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return (MaaJobStatus)MaaControllerStatus(Handle, job.Id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerWait"/>.
    /// </remarks>
    public MaaJobStatus Wait(IMaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return (MaaJobStatus)MaaControllerWait(Handle, job.Id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerConnected"/>.
    /// </remarks>
    public bool LinkStop()
        => MaaControllerConnected(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerGetImage"/>.
    /// </remarks>
    public bool GetImage(IMaaImageBuffer maaImage)
        => GetImage((IMaaImageBuffer<nint>)maaImage);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerGetImage"/>.
    /// </remarks>
    public bool GetImage(IMaaImageBuffer<nint> maaImage)
    {
        ArgumentNullException.ThrowIfNull(maaImage);

        return MaaControllerGetImage(Handle, maaImage.Handle).ToBoolean();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerGetUUID"/>.
    /// </remarks>
    public string? Uuid
    {
        get
        {
            using var buffer = new MaaStringBuffer();
            var ret = MaaControllerGetUUID(Handle, buffer.Handle).ToBoolean();
            return ret ? buffer.ToString() : null;
        }
    }
}
