using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Enums;
using MaaFramework.Binding.Exceptions;
using MaaFramework.Binding.Native.Interop;
using static MaaFramework.Binding.Native.Interop.Framework.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A class providing a reference implementation for <see cref="MaaFramework.Binding.Native.Interop.Framework.MaaController"/>.
/// </summary>
public class MaaController : MaaCommon<ControllerOption>
{
    internal MaaControllerHandle _handle;
    private bool disposed;

    internal MaaController()
    {
    }

    /// <inheritdoc cref="MaaController(string, string, AdbControllerTypes, string, string, MaaCallbackTransparentArg, CheckStatusOption, LinkOption)"/>
    public MaaController(string adbPath, string address, AdbControllerTypes type, string adbConfig, string agentPathh, MaaCallbackTransparentArg maaCallbackTransparentArg)
        : this(adbPath, address, type, adbConfig, agentPathh, maaCallbackTransparentArg, CheckStatusOption.ThrowIfNotSuccess, LinkOption.Start)
    {
    }

    /// <inheritdoc cref="MaaController(string, string, AdbControllerTypes, string, string, LinkOption)"/>
    public MaaController(string adbPath, string address, AdbControllerTypes type, string adbConfig, string agentPath)
        : this(adbPath, address, type, adbConfig, agentPath, LinkOption.Start)
    {
    }

    /// <inheritdoc cref="MaaController(string, string, AdbControllerTypes, string, string, CheckStatusOption, LinkOption)"/>
    public MaaController(string adbPath, string address, AdbControllerTypes type, string adbConfig, string agentPath, LinkOption link)
        : this(adbPath, address, type, adbConfig, agentPath, CheckStatusOption.ThrowIfNotSuccess, link)
    {
    }

    /// <inheritdoc cref="MaaController(string, string, AdbControllerTypes, string, string, MaaCallbackTransparentArg, CheckStatusOption, LinkOption)"/>
    public MaaController(string adbPath, string address, AdbControllerTypes type, string adbConfig, string agentPath, CheckStatusOption check, LinkOption link)
        : this(adbPath, address, type, adbConfig, agentPath, MaaCallbackTransparentArg.Zero, check, link)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaController"/> instance.
    /// </summary>
    /// <param name="adbPath">The path of adb executable file.</param>
    /// <param name="address">The device address.</param>
    /// <param name="type">The AdbControllerTypes including touch type, key type and screencap type.</param>
    /// <param name="adbConfig">The path of adb config file.</param>
    /// <param name="agentPath">The path of agent directory.</param>
    /// <param name="maaCallbackTransparentArg">The maaCallbackTransparentArg.</param>
    /// <param name="check">Checks LinkStart().Wait() status if true; otherwise, not check.</param>
    /// <param name="link">Executes <see cref="LinkStart"/> if true; otherwise, not link.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAdbControllerCreateV2"/>.
    /// </remarks>
    /// <exception cref="MaaJobStatusException" />
    public MaaController(string adbPath, string address, AdbControllerTypes type, string adbConfig, string agentPath, MaaCallbackTransparentArg maaCallbackTransparentArg, CheckStatusOption check, LinkOption link)
    {
        _handle = MaaAdbControllerCreateV2(adbPath, address, (int)type, adbConfig, agentPath, MaaApiCallback, maaCallbackTransparentArg);

        if (link == LinkOption.Start)
        {
            var status = LinkStart().Wait();
            if (check == CheckStatusOption.ThrowIfNotSuccess)
            {
                status.ThrowIfMaaControllerNotSuccess();
            }
        }
    }

    /// <summary>
    ///     Disposes the <see cref="MaaController"/> instance.
    /// </summary>
    /// <param name="disposing"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerDestroy"/>.
    /// </remarks>
    protected override void Dispose(bool disposing)
    {
        if (!disposed)
        {
            // if (disposing) Dispose managed resources.
            MaaControllerDestroy(_handle);
            _handle = MaaControllerHandle.Zero;
            disposed = true;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerSetOption"/>.
    /// </remarks>
    internal override bool SetOption(ControllerOption option, MaaOptionValue[] value)
        => MaaControllerSetOption(_handle, (MaaCtrlOption)option, ref value[0], (MaaOptionValueSize)value.Length).ToBoolean();

    /// <summary>
    ///     Connects the address specified by the constructor.
    /// </summary>
    /// <returns>A connection job.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostConnection"/>.
    /// </remarks>
    public MaaJob LinkStart()
    {
        var id = MaaControllerPostConnection(_handle);
        return new(id, this);
    }

    /// <summary>
    ///     Clicks a point.
    /// </summary>
    /// <param name="x">The horizontal coordinate of the point.</param>
    /// <param name="y">The vertical coordinate of the point.</param>
    /// <returns>A click job.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostClick"/>.
    /// </remarks>
    public MaaJob Click(int x, int y)
    {
        var id = MaaControllerPostClick(_handle, x, y);
        return new(id, this);
    }

    /// <summary>
    ///     Swipes from a starting point to a ending point with duration.
    /// </summary>
    /// <param name="x1">The horizontal coordinate of the starting point.</param>
    /// <param name="y1">The vertical coordinate of the starting point.</param>
    /// <param name="x2">The horizontal coordinate of the ending point.</param>
    /// <param name="y2">The horizontal coordinate of the ending point.</param>
    /// <param name="duration">The duration.</param>
    /// <returns>A swipe job.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostSwipe"/>.
    /// </remarks>
    public MaaJob Swipe(int x1, int y1, int x2, int y2, int duration)
    {
        var id = MaaControllerPostSwipe(_handle, x1, y1, x2, y2, duration);
        return new(id, this);
    }

    /// <summary>
    ///     Presses a key.
    /// </summary>
    /// <param name="keyCode">The code of the key.</param>
    /// <returns>A press key job.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostPressKey"/>.
    /// </remarks>
    public MaaJob PressKey(int keyCode)
    {
        var id = MaaControllerPostPressKey(_handle, keyCode);
        return new(id, this);
    }

    /// <summary>
    ///     Usage: TouchDown -> TouchMove -> TouchUp.
    /// </summary>
    /// <param name="contact">The contact id.</param>
    /// <param name="x">The horizontal coordinate of the starting point.</param>
    /// <param name="y">The vertical coordinate of the starting point.</param>
    /// <param name="pressure">The pressure.</param>
    /// <returns>A touch down job.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostTouchDown"/>.
    /// </remarks>
    public MaaJob TouchDown(int contact, int x, int y, int pressure)
    {
        var id = MaaControllerPostTouchDown(_handle, contact, x, y, pressure);
        return new(id, this);
    }

    /// <summary>
    ///     Usage: TouchDown -> TouchMove -> TouchUp.
    /// </summary>
    /// <param name="contact">The contact id.</param>
    /// <param name="x">The horizontal coordinate of the ending point.</param>
    /// <param name="y">The vertical coordinate of the ending point.</param>
    /// <param name="pressure">The pressure.</param>
    /// <returns>A touch move job.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostTouchMove"/>.
    /// </remarks>
    public MaaJob TouchMove(int contact, int x, int y, int pressure)
    {
        var id = MaaControllerPostTouchMove(_handle, contact, x, y, pressure);
        return new(id, this);
    }

    /// <summary>
    ///     Usage: TouchDown -> TouchMove -> TouchUp.
    /// </summary>
    /// <param name="contact">The contact id.</param>
    /// <returns>A touch up job.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostTouchUp"/>.
    /// </remarks>
    public MaaJob TouchUp(int contact)
    {
        var id = MaaControllerPostTouchUp(_handle, contact);
        return new(id, this);
    }

    /// <summary>
    ///     Takes a screenshot.
    /// </summary>
    /// <returns>A screen capture job.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostScreencap"/>.
    /// </remarks>
    public MaaJob Screencap()
    {
        var id = MaaControllerPostScreencap(_handle);
        return new(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Always return false.
    /// </remarks>
    public override bool SetParam(MaaJob job, string param)
        => false;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerStatus"/>.
    /// </remarks>
    public override MaaJobStatus GetStatus(MaaJob job)
        => (MaaJobStatus)MaaControllerStatus(_handle, job);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerWait"/>.
    /// </remarks>
    public override MaaJobStatus Wait(MaaJob job)
        => (MaaJobStatus)MaaControllerWait(_handle, job);

    /// <summary>
    ///     Ends the connection of the address specified by the constructor.
    /// </summary>
    /// <returns>true if the connection was ended successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerConnected"/>.
    /// </remarks>
    public bool LinkStop()
        => MaaControllerConnected(_handle).ToBoolean();

    /// <summary>
    ///     Gets a image.
    /// </summary>
    /// <returns>true if the image was got successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerGetImage"/>.
    /// </remarks>
    public bool GetImage(MaaImageBuffer maaImage)
        => MaaControllerGetImage(_handle, maaImage._handle).ToBoolean();

    /// <summary>
    ///     Gets the uuid string of the <see cref="MaaController"/>.
    /// </summary>
    /// <value>
    ///     A string if the hash was successfully got; otherwise, null.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerGetUUID"/>.
    /// </remarks>
    public string? Uuid
    {
        get
        {
            using var buffer = new MaaStringBuffer();
            var ret = MaaControllerGetUUID(_handle, buffer._handle).ToBoolean();
            return ret ? buffer.ToString() : null;
        }
    }
}
