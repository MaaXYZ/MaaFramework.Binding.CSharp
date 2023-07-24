using MaaToolKit.Enums;
using MaaToolKit.Extensions.Interfaces;
using MaaToolKit.Interop;
using static MaaToolKit.Interop.MaaApiWrapper;

namespace MaaToolKit.Extensions.ComponentModel;

/// <summary>
///     A class providing a reference implementation for Maa Controller section of <see cref="MaaApiWrapper"/>.
/// </summary>
public class MaaController : IMaaNotify, IMaaPost, IDisposable
{
    internal IntPtr _handle;

    private static readonly HashSet<MaaController> _controllers = new HashSet<MaaController>();
    internal static MaaController GetMaaController(IntPtr handle) => _controllers.First(x => x._handle == handle);

    /// <inheritdoc/>
    public event MaaCallback? Notify;

    /// <inheritdoc/>
    public void OnNotify(string msg, string detailsJson, IntPtr identifier)
    {
        Notify?.Invoke(msg, detailsJson, identifier);
    }

    /// <summary>
    ///     Creates a <see cref="MaaController"/> instance.
    /// </summary>
    /// <param name="adbPath"></param>
    /// <param name="address"></param>
    /// <param name="type"></param>
    /// <param name="adbConfig"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAdbControllerCreate"/>.
    /// </remarks>
    public MaaController(string adbPath, string address, AdbControllerType type, string adbConfig)
    {
        _handle = MaaAdbControllerCreate(adbPath, address, type, adbConfig, OnNotify, IntPtr.Zero);
        _controllers.Add(this);
    }

    /// <summary>
    ///     Creates a <see cref="MaaController"/> instance.
    /// </summary>
    /// <param name="adbPath"></param>
    /// <param name="address"></param>
    /// <param name="type"></param>
    /// <param name="adbConfig"></param>
    /// <param name="identifier"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAdbControllerCreate"/>.
    /// </remarks>
    public MaaController(string adbPath, string address, AdbControllerType type, string adbConfig, IntPtr identifier)
    {
        _handle = MaaAdbControllerCreate(adbPath, address, type, adbConfig, OnNotify, identifier);
        _controllers.Add(this);
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
        if (disposing)
        {
            MaaControllerDestroy(_handle);
            _handle = IntPtr.Zero;
        }
    }

    private int _screenshotWidth;
    private int _screenshotHeight;

    /// <summary>
    ///     Get or sets the target width of screenshots with original aspect ratio.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerSetScreenshotTargetWidth"/>.
    /// </remarks>
    public int ScreenshotWidth
    {
        get => _screenshotWidth;
        set
        {
            var setted = MaaControllerSetScreenshotTargetWidth(_handle, value);
            if (setted)
            {
                _screenshotWidth = value;
                _screenshotHeight = 0;
            }
        }
    }

    /// <summary>
    ///     Get or sets the target height of screenshots with original aspect ratio.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerSetScreenshotTargetHeight"/>.
    /// </remarks>
    public int ScreenshotHeight
    {
        get => _screenshotHeight;
        set
        {
            var setted = MaaControllerSetScreenshotTargetHeight(_handle, value);
            if (setted)
            {
                _screenshotWidth = 0;
                _screenshotHeight = value;
            }
        }
    }

    private string _defaultAppPackageEntry = string.Empty;
    private string _defaultAppPackage = string.Empty;

    /// <summary>
    ///     Get or sets a default app package name for starting the app.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerSetDefaultAppPackageEntry"/>.
    /// </remarks>
    public string DefaultAppPackageEntry
    {
        get => _defaultAppPackageEntry;
        set
        {
            var setted = MaaControllerSetDefaultAppPackageEntry(_handle, value);
            if (setted)
            {
                _defaultAppPackageEntry = value;
            }
        }
    }

    /// <summary>
    ///     Get or sets a default app package name for stopping the app.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerSetDefaultAppPackage"/>.
    /// </remarks>
    public string DefaultAppPackage
    {
        get => _defaultAppPackage;
        set
        {
            var setted = MaaControllerSetDefaultAppPackage(_handle, value);
            if (setted)
            {
                _defaultAppPackage = value;
            }
        }
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

    // public MaaJob Swipe()

    /// <summary>
    ///     
    /// </summary>
    /// <returns>A screencap job.</returns>
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
    /// <returns></returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerConnected"/>.
    /// </remarks>
    public bool LinkStop()
        => MaaControllerConnected(_handle);

    /// <summary>
    ///     Gets a image.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerGetImage"/>.
    /// </remarks>
    public byte[]? GetImage(int bufferSize = 3 << 20)
        => MaaControllerGetImage(_handle, bufferSize);

    /// <summary>
    ///     Gets the uuid string of the <see cref="MaaController"/>.
    /// </summary>
    /// <value>
    ///     Null if failed to get uuid, or a UTF-8 string represent of uuid
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerGetUuid"/>.
    /// </remarks>
    public string? Uuid => MaaControllerGetUuid(_handle);
}
