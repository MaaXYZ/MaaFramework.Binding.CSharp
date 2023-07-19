using System.Runtime.InteropServices;
using System.Text;
using MaaCommon.Enums;

namespace MaaCommon.Interop;

/// <summary>
///     A wrapper of <see cref="MaaApi"/>. This class is intended to make the API more .NET style.
///     Prefer to use this class instead of <see cref="MaaApi"/> directly if you want to use MAA API.
/// </summary>
public static class MaaApiWrapper
{
    /// <summary>
    ///     MAA callback delegate
    /// </summary>
    public delegate void MaaCallback(string msg, string detailsJson, IntPtr identifier);

    private static MaaApi.MaaCallback Wrap(this MaaCallback callback) => (msg, detail, arg) =>
        callback(
            Marshal.PtrToStringUTF8(msg) ?? string.Empty, 
            Marshal.PtrToStringUTF8(detail) ?? "{}", 
            arg);

    #region Miscellaneous

    /// <summary>
    ///     Get Maa version
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaApi.MaaVersion"/>
    /// </remarks>
    public static string GetMaaVersion()
    {
        var ptr = MaaApi.MaaVersion();
        var version = Marshal.PtrToStringUTF8(ptr);
        Marshal.FreeHGlobal(ptr);
        return version!;
    }

    /// <summary>
    ///     Set global option
    /// </summary>
    /// <param name="option">The option name</param>
    /// <param name="value">The option value</param>
    /// <returns></returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaApi.MaaSetGlobalOption(int, ref byte, ulong)"/>
    /// </remarks>
    public static bool SetGlobalOption(GlobalOption option, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        return MaaApi.MaaSetGlobalOption((int)option, ref bytes[0], (ulong)bytes.Length) != 0;
    }
    
    #endregion
    
    #region Resource
    
    /// <summary>
    ///     Create Maa Resource instance
    /// </summary>
    /// <param name="userPath"></param>
    /// <param name="callback"></param>
    /// <param name="identifier"></param>
    /// <returns></returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaApi.MaaResourceCreate"/>
    /// </remarks>
    public static IntPtr CreateMaaResource(string userPath, MaaCallback callback, string identifier)
    {
        ArgumentNullException.ThrowIfNull(userPath);
        var bytes = Encoding.UTF8.GetBytes(identifier);
        return MaaApi.MaaResourceCreate(userPath, callback.Wrap(), ref bytes[0]);
    }

    /// <summary>
    ///     Dispose Maa Resource instance
    /// </summary>
    /// <param name="resourceHandle">The MAA Resource instance handle</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaApi.MaaResourceDestroy"/>
    /// </remarks>
    public static void DisposeMaaResource(IntPtr resourceHandle)
    {
        MaaApi.MaaResourceDestroy(resourceHandle);
    }

    /// <summary>
    ///     Append a load resource async job, could be called multiple times
    /// </summary>
    /// <param name="resourceHandle">The Maa Resource instance handle</param>
    /// <param name="path">The resource path</param>
    /// <returns>A resource load task id</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaApi.MaaResourcePostResource"/>
    /// </remarks>
    public static long AppendAddResourceJob(IntPtr resourceHandle, string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        
        return MaaApi.MaaResourcePostResource(resourceHandle, path);
    }

    /// <summary>
    ///     Get resource loading status
    /// </summary>
    /// <param name="resourceHandle">The Maa Resource instance handle</param>
    /// <param name="id">The load resource async job id you got from <see cref="AppendAddResourceJob"/></param>
    /// <returns>The status of the resource loading job</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaApi.MaaResourceStatus"/>
    /// </remarks>
    public static int GetResourceStatus(IntPtr resourceHandle, long id)
    {
        return MaaApi.MaaResourceStatus(resourceHandle, id);
    }

    /// <summary>
    ///     Wait for load resource job
    /// </summary>
    /// <param name="resourceHandle">The Maa Resource instance handle</param>
    /// <param name="id">The load resource async job id you got from <see cref="AppendAddResourceJob"/></param>
    /// <returns>The status of the resource loading job</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaApi.MaaResourceWait"/>
    /// </remarks>
    public static int WaitResourceJob(IntPtr resourceHandle, long id)
    {
        return MaaApi.MaaResourceWait(resourceHandle, id);
    }

    /// <summary>
    ///     Check if resource is fully loaded
    /// </summary>
    /// <param name="resourceHandle">The Maa Resource instance handle</param>
    /// <returns></returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaApi.MaaResourceLoaded"/>
    /// </remarks>
    public static bool IsResourceLoaded(IntPtr resourceHandle)
    {
        return MaaApi.MaaResourceLoaded(resourceHandle) != 0;
    }

    /// <summary>
    ///     Set resource option
    /// </summary>
    /// <param name="resourceHandle">The Maa Resource instance handle</param>
    /// <param name="option">The option id</param>
    /// <param name="value">The option value</param>
    /// <returns></returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaApi.MaaResourceSetOption(IntPtr, int, ref byte, ulong)"/>
    /// </remarks>
    public static bool SetResourceOption(IntPtr resourceHandle, ResourceOption option, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        return MaaApi.MaaResourceSetOption(resourceHandle, (int)option, ref bytes[0], (ulong)bytes.Length) != 0;
    }

    /// <summary>
    ///     Get resource hash
    /// </summary>
    /// <param name="resourceHandle">The Maa Resource instance handle</param>
    /// <returns>Null if failed to get hash, or a UTF-8 string represent of hash</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaApi.MaaResourceGetHash"/>
    /// </remarks>
    public static string? GetResourceHash(IntPtr resourceHandle)
    {
        const int buffSize = 1024;
        var buff = Marshal.AllocHGlobal(buffSize);
        var size = MaaApi.MaaResourceGetHash(resourceHandle, buff, buffSize);
        if (size == ulong.MaxValue)
        {
            return null;
        }

        var hash = Marshal.PtrToStringUTF8(buff, (int)size);
        Marshal.FreeHGlobal(buff);
        return hash;
    }

    #endregion

    #region Controller

    /// <summary>
    ///     Create an MAA ADB Controller instance
    /// </summary>
    /// <param name="adbPath"></param>
    /// <param name="address"></param>
    /// <param name="type"></param>
    /// <param name="config"></param>
    /// <param name="callback"></param>
    /// <param name="callbackArgument"></param>
    /// <returns></returns>
    public static IntPtr MaaAdbControllerCreate(string adbPath, string address, AdbControllerType type, string config, MaaCallback callback, IntPtr callbackArgument)
    {
        ArgumentNullException.ThrowIfNull(adbPath);
        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(config);
        
        return MaaApi.MaaAdbControllerCreate(
            adbPath,
            address,
            (int)type,
            config,
            callback.Wrap(),
            callbackArgument);
    }

    /// <summary>
    ///     Destroy an MAA Controller instance
    /// </summary>
    /// <param name="handle"></param>
    public static void MaaControllerDestroy(IntPtr handle)
    {
        MaaApi.MaaControllerDestroy(handle);
    }

    /// <summary>
    ///     Set screenshot target width of MAA controller
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public static bool MaaControllerSetScreenshotTargetWidth(IntPtr handle, int width)
    {
        return MaaApi.MaaControllerSetOption(
            handle,
            ControllerOption.ScreenshotTargetWidth,
            width, 
            sizeof(int)) != 0;
    }

    /// <summary>
    ///     Set screenshot target height of MAA controller
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static bool MaaControllerSetScreenshotTargetHeight(IntPtr handle, int height)
    {
        return MaaApi.MaaControllerSetOption(
            handle,
            ControllerOption.ScreenshotTargetHeight, 
            height, 
            sizeof(int)) != 0;
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="entry"></param>
    /// <returns></returns>
    public static bool MaaControllerSetDefaultAppPackageEntry(IntPtr handle, string entry)
    {
        var entryNative = Encoding.UTF8.GetBytes(entry);
        return MaaApi.MaaControllerSetOption(
            handle,
            ControllerOption.DefaultAppPackageEntry, 
            ref entryNative[0],
            (ulong)entryNative.Length) != 0;
    }

    /// <summary>
    ///     Set default app package of MAA controller
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="package"></param>
    /// <returns></returns>
    public static bool MaaControllerSetDefaultAppPackage(IntPtr handle, string package)
    {
        var packageNative = Encoding.UTF8.GetBytes(package);
        return 0 != MaaApi.MaaControllerSetOption(
            handle,
            ControllerOption.DefaultAppPackage,
            ref packageNative[0],
            (ulong)packageNative.Length);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerHandler"></param>
    /// <returns></returns>
    public static long MaaControllerPostConnection(IntPtr controllerHandler)
    {
        return MaaApi.MaaControllerPostConnection(controllerHandler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerHandler"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static long MaaControllerPostClick(IntPtr controllerHandler, int x, int y)
    {
        return MaaApi.MaaControllerPostClick(controllerHandler, x, y);
    }

    // public static  Int64 MaaControllerPostSwipe(IntPtr controllerHandler, Int32* x_steps, Int32* y_steps, Int32* step_delays, UInt64 buff_size);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerHandler"></param>
    /// <returns></returns>
    public static long MaaControllerPostScreencap(IntPtr controllerHandler)
    {
        return MaaApi.MaaControllerPostScreencap(controllerHandler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerHandler"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int MaaControllerStatus(IntPtr controllerHandler, long id)
    {
        return MaaApi.MaaControllerStatus(controllerHandler, id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerHandler"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int MaaControllerWait(IntPtr controllerHandler, long id)
    {
        return MaaApi.MaaControllerWait(controllerHandler, id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerHandler"></param>
    /// <returns></returns>
    public static bool MaaControllerConnected(IntPtr controllerHandler)
    {
        return 0 != MaaApi.MaaControllerConnected(controllerHandler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerHandler"></param>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public static byte[]? MaaControllerGetImage(IntPtr controllerHandler, int bufferSize = 3 << 20)
    {
        var buf = Marshal.AllocHGlobal(bufferSize);
        var size = MaaApi.MaaControllerGetImage(controllerHandler, buf, (ulong)bufferSize);
        if (size == 0xFFFFFFFFFFFFFFFF)
        {
            Marshal.FreeHGlobal(buf);
            return null;
        }

        var ret = new byte[size];
        Marshal.Copy(buf, ret, 0, (int)size);
        Marshal.FreeHGlobal(buf);
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerHandler"></param>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public static string? MaaControllerGetUuid(IntPtr controllerHandler, int bufferSize = 64) // 16
    {
        var buf = Marshal.AllocHGlobal(bufferSize);
        var size = MaaApi.MaaControllerGetUUID(controllerHandler, buf, (ulong)bufferSize);
        if (size == 0xFFFFFFFFFFFFFFFF)
        {
            Marshal.FreeHGlobal(buf);
            return null;
        }

        var ret = Marshal.PtrToStringUTF8(buf);
        Marshal.FreeHGlobal(buf);
        return ret;
    }

    #endregion

    #region Instance

    /// <summary>
    /// 
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="callbackArgument"></param>
    /// <returns></returns>
    public static IntPtr MaaInstanceCreate(MaaCallback callback, IntPtr callbackArgument)
    {
        return MaaApi.MaaInstanceCreate(callback.Wrap(), callbackArgument);
    }

    /// <summary>
    ///     Destroy MAA instance
    /// </summary>
    /// <param name="instanceHandler"></param>
    public static void MaaInstanceDestroy(IntPtr instanceHandler)
    {
        MaaApi.MaaInstanceDestroy(instanceHandler);
    }

    /// <summary>
    ///     Bind MAA instance to resource
    /// </summary>
    /// <param name="instanceHandler"></param>
    /// <param name="resourceHandler"></param>
    /// <returns></returns>
    public static bool MaaBindResource(IntPtr instanceHandler, IntPtr resourceHandler)
    {
        return MaaApi.MaaBindResource(instanceHandler, resourceHandler) != 0;
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="instanceHandler"></param>
    /// <param name="controllerHandle"></param>
    /// <returns></returns>
    public static bool MaaBindController(IntPtr instanceHandler, IntPtr controllerHandle)
    {
        return MaaApi.MaaBindController(instanceHandler, controllerHandle) != 0;
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="instanceHandler"></param>
    /// <returns></returns>
    public static bool MaaInitialized(IntPtr instanceHandler)
    {
        return MaaApi.MaaInited(instanceHandler) != 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instanceHandler"></param>
    /// <param name="name"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static long MaaInstancePostTask(IntPtr instanceHandler, string name, string args)
    {
        return MaaApi.MaaInstancePostTask(instanceHandler, name, args);
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="instanceHandler"></param>
    /// <param name="id"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static bool MaaSetTaskParam(IntPtr instanceHandler, long id, string args)
    {
        return 0 != MaaApi.MaaSetTaskParam(instanceHandler, id, args);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instanceHandler"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int MaaTaskStatus(IntPtr instanceHandler, long id)
    {
        return MaaApi.MaaTaskStatus(instanceHandler, id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instanceHandler"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int MaaTaskWait(IntPtr instanceHandler, long id)
    {
        return MaaApi.MaaTaskWait(instanceHandler, id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instanceHandler"></param>
    /// <returns></returns>
    public static bool MaaTaskAllFinished(IntPtr instanceHandler)
    {
        return 0 != MaaApi.MaaTaskAllFinished(instanceHandler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instanceHandler"></param>
    public static void MaaStop(IntPtr instanceHandler)
    {
        MaaApi.MaaStop(instanceHandler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instanceHandler"></param>
    /// <returns></returns>
    public static IntPtr MaaGetResource(IntPtr instanceHandler)
    {
        return MaaApi.MaaGetResource(instanceHandler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instanceHandler"></param>
    /// <returns></returns>
    public static IntPtr MaaGetController(IntPtr instanceHandler)
    {
        return MaaApi.MaaGetController(instanceHandler);
    }

    #endregion

}
