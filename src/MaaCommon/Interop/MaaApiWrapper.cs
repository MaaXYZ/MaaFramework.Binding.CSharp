using System.Runtime.InteropServices;
using MaaCommon.Common;
using MaaCommon.Enums;
using MaaCommon.Extensions;

namespace MaaCommon.Interop;

/// <summary>
///     MAA DLL interop proxy
/// </summary>
public static class MaaApiWrapper
{
    /// <summary>
    ///     MAA callback delegate
    /// </summary>
    public delegate void MaaCallback(string msg, string detailsJson, IntPtr callbackArgument);

    private static MaaApi.MaaCallback Wrap(this MaaCallback callback) => (msg, detail, arg) =>
        callback(Marshal.PtrToStringUTF8(msg) ?? string.Empty, Marshal.PtrToStringUTF8(detail) ?? "{}", arg);

    /// <summary>
    ///     Create Maa Resource instance
    /// </summary>
    /// <param name="userPath"></param>
    /// <param name="callback"></param>
    /// <param name="callbackArgument"></param>
    /// <returns></returns>
    public static IntPtr MaaResourceCreate(string userPath, MaaCallback callback, IntPtr callbackArgument)
    {
        ArgumentNullException.ThrowIfNull(userPath);
        
        var userPathNative = new NativeString(userPath);
        return MaaApi.MaaResourceCreate(userPathNative.Value, callback.Wrap(), callbackArgument);
    }

    /// <summary>
    ///     Destroy Maa Resource instance
    /// </summary>
    /// <param name="resourceHandle"></param>
    public static void MaaResourceDestroy(IntPtr resourceHandle)
    {
        MaaApi.MaaResourceDestroy(resourceHandle);
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="resourceHandle"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Int64 MaaResourcePostResource(IntPtr resourceHandle, string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        
        var pathNative = new NativeString(path);
        return MaaApi.MaaResourcePostResource(resourceHandle, pathNative.Value);
    }

    /// <summary>
    ///     Get resource status
    /// </summary>
    /// <param name="resourceHandle"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Int32 MaaResourceStatus(IntPtr resourceHandle, Int64 id)
    {
        return MaaApi.MaaResourceStatus(resourceHandle, id);
    }

    /// <summary>
    ///     Wait for resource
    /// </summary>
    /// <param name="resourceHandle"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Int32 MaaResourceWait(IntPtr resourceHandle, Int64 id)
    {
        return MaaApi.MaaResourceWait(resourceHandle, id);
    }

    /// <summary>
    ///     Resource loaded
    /// </summary>
    /// <param name="resourceHandle"></param>
    /// <returns></returns>
    public static bool MaaResourceLoaded(IntPtr resourceHandle)
    {
        return MaaApi.MaaResourceLoaded(resourceHandle) != 0;
    }

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
    public static IntPtr MaaAdbControllerCreate(string adbPath, string address, AdbControllerTypes type, string config, MaaCallback callback, IntPtr callbackArgument)
    {
        ArgumentNullException.ThrowIfNull(adbPath);
        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(config);
        
        return MaaApi.MaaAdbControllerCreate(
            adbPath.ToNativePtr(),
            address.ToNativePtr(),
            (int)type,
            config.ToNativePtr(),
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
            (int)ControllerOptions.ScreenshotTargetWidth,
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
            (int)ControllerOptions.ScreenshotTargetHeight, 
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
        var entryNative = entry.ToNative();
        return MaaApi.MaaControllerSetOption(
            handle,
            (int)ControllerOptions.DefaultAppPackageEntry, 
            entryNative.Value,
            (UInt64)entryNative.Length) != 0;
    }

    /// <summary>
    ///     Set default app package of MAA controller
    /// </summary>
    /// <param name="handle"></param>
    /// <param name="package"></param>
    /// <returns></returns>
    public static bool MaaControllerSetDefaultAppPackage(IntPtr handle, string package)
    {
        var packageNative = package.ToNative();
        return 0 != MaaApi.MaaControllerSetOption(
            handle,
            (Int32)ControllerOptions.DefaultAppPackage,
            packageNative.Value,
            (UInt64)packageNative.Length);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerHandler"></param>
    /// <returns></returns>
    public static Int64 MaaControllerPostConnection(IntPtr controllerHandler)
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
    public static Int64 MaaControllerPostClick(IntPtr controllerHandler, Int32 x, Int32 y)
    {
        return MaaApi.MaaControllerPostClick(controllerHandler, x, y);
    }

    // public static  Int64 MaaControllerPostSwipe(IntPtr controllerHandler, Int32* x_steps, Int32* y_steps, Int32* step_delays, UInt64 buff_size);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerHandler"></param>
    /// <returns></returns>
    public static Int64 MaaControllerPostScreencap(IntPtr controllerHandler)
    {
        return MaaApi.MaaControllerPostScreencap(controllerHandler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerHandler"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Int32 MaaControllerStatus(IntPtr controllerHandler, Int64 id)
    {
        return MaaApi.MaaControllerStatus(controllerHandler, id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="controllerHandler"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Int32 MaaControllerWait(IntPtr controllerHandler, Int64 id)
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
        var size = MaaApi.MaaControllerGetImage(controllerHandler, buf, (UInt64)bufferSize);
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
        var size = MaaApi.MaaControllerGetUUID(controllerHandler, buf, (UInt64)bufferSize);
        if (size == 0xFFFFFFFFFFFFFFFF)
        {
            Marshal.FreeHGlobal(buf);
            return null;
        }

        var ret = Marshal.PtrToStringUTF8(buf);
        Marshal.FreeHGlobal(buf);
        return ret;
    }

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
    public static Int64 MaaInstancePostTask(IntPtr instanceHandler, string name, string args)
    {
        return MaaApi.MaaInstancePostTask(instanceHandler, name.ToNativePtr(), args.ToNativePtr());
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="instanceHandler"></param>
    /// <param name="id"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static bool MaaSetTaskParam(IntPtr instanceHandler, Int64 id, string args)
    {
        return 0 != MaaApi.MaaSetTaskParam(instanceHandler, id, args.ToNativePtr());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instanceHandler"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Int32 MaaTaskStatus(IntPtr instanceHandler, Int64 id)
    {
        return MaaApi.MaaTaskStatus(instanceHandler, id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="instanceHandler"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Int32 MaaTaskWait(IntPtr instanceHandler, Int64 id)
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool MaaSetLogging(string path)
    {
        var pathNative = path.ToNative();
        return 0 != MaaApi.MaaSetGlobalOption(
            (Int32)GlobalOptions.Logging,
            pathNative.Value,
            (UInt64)pathNative.Length);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static string MaaVersion()
    {
        return Marshal.PtrToStringUTF8(MaaApi.MaaVersion()) ?? string.Empty;
    }
}
