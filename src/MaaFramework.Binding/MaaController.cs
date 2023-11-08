using MaaFramework.Binding.Enums;
using MaaFramework.Binding.Exceptions;
using MaaFramework.Binding.Interfaces;
using MaaFramework.Binding.Interop;
using System.Runtime.InteropServices;
using static MaaFramework.Binding.Interop.Framework.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A class providing a reference implementation for <see cref="Interop.Framework.MaaController"/>.
/// </summary>
public class MaaController : IMaaNotify, IMaaPost, IDisposable
{
    internal MaaControllerHandle _handle;
    private bool disposed;

    internal MaaController()
    {
        _callback = (msg, detail, arg) => Callback?.Invoke(
            Marshal.PtrToStringUTF8(msg) ?? string.Empty,
            Marshal.PtrToStringUTF8(detail) ?? "{}",
            arg);
    }

    /// <inheritdoc cref="MaaController(string, string, AdbControllerType, string, string, MaaCallbackTransparentArg, bool)"/>
    public MaaController(string adbPath, string address, AdbControllerType type, string adbConfig, string agentPath, bool linkStart = false)
        : this(adbPath, address, type, adbConfig, agentPath, MaaCallbackTransparentArg.Zero, linkStart)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaController"/> instance.
    /// </summary>
    /// <param name="adbPath"></param>
    /// <param name="address"></param>
    /// <param name="type"></param>
    /// <param name="adbConfig"></param>
    /// <param name="agentPath"></param>
    /// <param name="maaCallbackTransparentArg"></param>
    /// <param name="linkStart">Whether to execute <see cref="LinkStart"/></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAdbControllerCreate"/>.
    /// </remarks>
    /// <exception cref="MaaJobStatusException" />
    public MaaController(string adbPath, string address, AdbControllerType type, string adbConfig, string agentPath, MaaCallbackTransparentArg maaCallbackTransparentArg, bool linkStart = false)
        : this()
    {
        _handle = MaaAdbControllerCreateV2(adbPath, address, (int)type, adbConfig, agentPath, _callback, maaCallbackTransparentArg);

        if (linkStart)
        {
            LinkStart()
                .Wait()
                .ThrowIfNot(MaaJobStatus.Success);
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Disposes the <see cref="MaaController"/> instance.
    /// </summary>
    /// <param name="disposing"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerDestroy"/>.
    /// </remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            // if (disposing) Dispose managed resources.
            MaaControllerDestroy(_handle);
            _handle = MaaControllerHandle.Zero;
            disposed = true;
        }
    }

    /// <summary>
    ///     Sets <paramref name="value"/> to a option of the <see cref="MaaController"/>.
    /// </summary>
    /// <param name="option">The option.</param>
    /// <param name="value">The value.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerSetOption"/>.
    /// </remarks>
    /// <returns>true if the option was setted successfully; otherwise, false.</returns>
    private bool SetOption(ControllerOption option, MaaOptionValue[] value)
        => MaaControllerSetOption(_handle, (MaaCtrlOption)option, ref value[0], (MaaOptionValueSize)value.Length).ToBoolean();

    /// <inheritdoc cref="SetOption(ControllerOption, MaaOptionValue[])"/>
    public bool SetOption(ControllerOption option, int value)
        => SetOption(option, value.ToMaaOptionValues());

    /// <inheritdoc cref="SetOption(ControllerOption, MaaOptionValue[])"/>
    /// <exception cref="ArgumentException" />
    public bool SetOption(ControllerOption option, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        return SetOption(option, value.ToMaaOptionValues());
    }

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
    /// <param name="x">The x-coordinate of the point.</param>
    /// <param name="y">The y-coordinate of the point.</param>
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
    /// <param name="x1">The x-coordinate of the starting point.</param>
    /// <param name="y1">The y-coordinate of the starting point.</param>
    /// <param name="x2">The x-coordinate of the ending point.</param>
    /// <param name="y2">The x-coordinate of the ending point.</param>
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
    /// <param name="x">The x-coordinate of the starting point.</param>
    /// <param name="y">The y-coordinate of the starting point.</param>
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
    /// <param name="x">The x-coordinate of the ending point.</param>
    /// <param name="y">The y-coordinate of the ending point.</param>
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
    public bool SetParam(MaaJob job, string param)
        => false;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(MaaJob job)
        => (MaaJobStatus)MaaControllerStatus(_handle, job);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerWait"/>.
    /// </remarks>
    public MaaJobStatus Wait(MaaJob job)
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
