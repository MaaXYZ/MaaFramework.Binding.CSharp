using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Native.Abstractions;
using MaaFramework.Binding.Native.Interop;
using static MaaFramework.Binding.Native.Interop.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Native.Interop.MaaController"/>.
/// </summary>
public class MaaController : MaaCommon<ControllerOption>, IMaaController<nint>
{
    /// <summary>
    ///     Converts a <see cref="IMaaController{nint}"/> instance to a <see cref="MaaController"/>.
    /// </summary>
    /// <param name="maaController">The <see cref="IMaaController{nint}"/> instance.</param>
    public MaaController(IMaaController<nint> maaController)
    {
        SetHandle(maaController.Handle, needReleased: true);
    }

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
    sealed protected override bool SetOption(ControllerOption option, MaaOptionValue[] value)
        => MaaControllerSetOption(Handle, (MaaCtrlOption)option, ref value[0], (MaaOptionValueSize)value.Length).ToBoolean();

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
    /// <remarks>
    ///     Always return false.
    /// </remarks>
    public bool SetParam(IMaaJob job, string param)
        => false;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(IMaaJob job)
        => (MaaJobStatus)MaaControllerStatus(Handle, job.Id);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerWait"/>.
    /// </remarks>
    public MaaJobStatus Wait(IMaaJob job)
        => (MaaJobStatus)MaaControllerWait(Handle, job.Id);

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
        => MaaControllerGetImage(Handle, maaImage.Handle).ToBoolean();

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
