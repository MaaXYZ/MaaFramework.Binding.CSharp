using MaaToolKit.Extensions.Enums;
using MaaToolKit.Extensions.Exceptions;
using MaaToolKit.Extensions.Interfaces;
using MaaToolKit.Extensions.Interop;
using System.Runtime.InteropServices;
using static MaaToolKit.Extensions.Interop.MaaApi;

namespace MaaToolKit.Extensions.ComponentModel;

/// <summary>
///     A class providing a reference implementation for Maa Controller section of <see cref="MaaApi"/>.
/// </summary>
public class MaaController : IMaaNotify, IMaaPost, IDisposable
{
    internal MaaControllerHandle _handle;

    internal static readonly HashSet<MaaController> _controllers = new HashSet<MaaController>();
    internal static MaaController Get(IntPtr handle)
    {
        var ret = _controllers.FirstOrDefault(x => x._handle == handle);
        if (ret == null)
        {
            ret = new() { _handle = handle };
            _controllers.Add(ret);
        }
        return ret;
    }

    /// <inheritdoc/>
    public event IMaaNotify.MaaCallback? Callback;
    internal readonly MaaControllerCallback _callback;

    internal MaaController()
    {
        _callback = (msg, detail, arg) => Callback?.Invoke(
            Marshal.PtrToStringUTF8(msg) ?? string.Empty,
            Marshal.PtrToStringUTF8(detail) ?? "{}",
            arg);
    }

    /// <inheritdoc cref="MaaController(string, string, AdbControllerType, string, MaaCallbackTransparentArg, bool)"/>
    public MaaController(string adbPath, string address, AdbControllerType type, string adbConfig, bool linkStart = false)
        : this(adbPath, address, type, adbConfig, MaaCallbackTransparentArg.Zero, linkStart)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaController"/> instance.
    /// </summary>
    /// <param name="adbPath"></param>
    /// <param name="address"></param>
    /// <param name="type"></param>
    /// <param name="adbConfig"></param>
    /// <param name="maaCallbackTransparentArg"></param>
    /// <param name="linkStart">Whether to execute <see cref="LinkStart"/></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAdbControllerCreate"/>.
    /// </remarks>
    /// <exception cref="MaaJobStatusException" />
    public MaaController(string adbPath, string address, AdbControllerType type, string adbConfig, MaaCallbackTransparentArg maaCallbackTransparentArg, bool linkStart = false)
        : this()
    {
        _handle = MaaAdbControllerCreate(adbPath, address, (int)type, adbConfig, _callback, maaCallbackTransparentArg);
        _controllers.Add(this);

        if (linkStart)
        {
            this.LinkStart()
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
        if (disposing && _handle != MaaControllerHandle.Zero)
        {
            MaaControllerDestroy(_handle);
            _handle = MaaControllerHandle.Zero;
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
    /// <returns>true if the option was successfully setted; otherwise, false.</returns>
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
    ///     Swipe in steps.
    /// </summary>
    /// <param name="xSteps">The x-coordinate of the point in steps.</param>
    /// <param name="ySteps">The y-coordinate of the point in steps.</param>
    /// <param name="stepsDelay">The swipe delay between steps.</param>
    /// <param name="stepsLength">The length of steps.</param>
    /// <returns>A swipe job.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostSwipe"/>.
    /// </remarks>
    public MaaJob Swipe(int[] xSteps, int[] ySteps, int[] stepsDelay, ulong stepsLength)
    {
        var id = MaaControllerPostSwipe(_handle, ref xSteps[0], ref ySteps[0], ref stepsDelay[0], stepsLength);
        return new(id, this);
    }

    /// <summary>
    ///     Take a screenshot.
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
    /// <returns>true if the connection was successfully ended; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerConnected"/>.
    /// </remarks>
    public bool LinkStop()
        => MaaControllerConnected(_handle).ToBoolean();

    /// <summary>
    ///     Gets a image.
    /// </summary>
    /// <returns>Byte array of the image if got successfully; otherwise, null.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerGetImage"/>.
    /// </remarks>
    public byte[]? GetImage(MaaSize bufferSize = 3 << 20)
        => _handle.GetBytesFromFuncWithBuffer(
            MaaControllerGetImage,
            bufferSize);

    /// <summary>
    ///     Gets the uuid string of the <see cref="MaaController"/>.
    /// </summary>
    /// <value>
    ///     Null if failed to get uuid, or a UTF-8 string represent of uuid
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerGetUUID"/>.
    /// </remarks>
    public string? Uuid => _handle.GetStringFromFuncWithBuffer(
        MaaControllerGetUUID,
        bufferSize: 1 << 7);
}
