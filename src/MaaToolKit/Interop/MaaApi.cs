using MaaToolKit.Enums;
using System.Runtime.InteropServices;

// P/Invoke method should not be visible
#pragma warning disable CA1401
// Remove the underscores from member name
#pragma warning disable CA1707

// Missing XML comment for publicly visible type or member
#pragma warning disable CS1591

// Make this native method private and provide a wrapper
#pragma warning disable S4200

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace MaaToolKit.Interop;

/// <summary>
///     The base P/Invoke methods for MaaFramework, use this class to call all the native methods.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="MaaApiWrapper"/> instead.
/// </summary>
public static partial class MaaApi
{
    public delegate void MaaCallback(IntPtr msg, IntPtr details_json, IntPtr callback_arg);

    /* Resource */

    [LibraryImport("MaaFramework")]
    public static partial IntPtr MaaResourceCreate(MaaCallback callback, ref byte callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial void MaaResourceDestroy(IntPtr res_handle);

    [LibraryImport("MaaFramework")]
    public static partial long MaaResourcePostResource(IntPtr res_handle, [MarshalAs(UnmanagedType.LPUTF8Str)] string path);

    [LibraryImport("MaaFramework")]
    public static partial int MaaResourceStatus(IntPtr res_handle, long id);

    [LibraryImport("MaaFramework")]
    public static partial int MaaResourceWait(IntPtr res_handle, long id);

    [LibraryImport("MaaFramework")]
    public static partial byte MaaResourceLoaded(IntPtr res_handle);

    [LibraryImport("MaaFramework")]
    public static partial byte MaaResourceSetOption(IntPtr res_handle, int option, IntPtr value, ulong value_size);
    [LibraryImport("MaaFramework")]
    public static partial byte MaaResourceSetOption(IntPtr res_handle, int option, ref byte value, ulong value_size);

    [LibraryImport("MaaFramework")]
    public static partial ulong MaaResourceGetHash(IntPtr res_handle, IntPtr buff, ulong buff_size);

    /* Controller */

    [LibraryImport("MaaFramework")]
    public static partial IntPtr MaaAdbControllerCreate([MarshalAs(UnmanagedType.LPUTF8Str)] string adb_path, [MarshalAs(UnmanagedType.LPUTF8Str)] string address, int type,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string config, MaaCallback callback,
        IntPtr callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial void MaaControllerDestroy(IntPtr ctrl_handle);

    [LibraryImport("MaaFramework")]
    public static partial byte MaaControllerSetOption(IntPtr ctrl_handle, ControllerOption option, IntPtr value, ulong value_size);
    [LibraryImport("MaaFramework")]
    public static partial byte MaaControllerSetOption(IntPtr ctrl_handle, ControllerOption option, ref int value, ulong value_size);
    [LibraryImport("MaaFramework")]
    public static partial byte MaaControllerSetOption(IntPtr ctrl_handle, ControllerOption option, ref byte value, ulong value_size);

    [LibraryImport("MaaFramework")]
    public static partial long MaaControllerPostConnection(IntPtr ctrl_handle);

    [LibraryImport("MaaFramework")]
    public static partial long MaaControllerPostClick(IntPtr ctrl_handle, int x, int y);

    [LibraryImport("MaaFramework")]
    public static unsafe partial long MaaControllerPostSwipe(IntPtr ctrl_handle, int* x_steps, int* y_steps, int* step_delays, ulong buff_size);

    [LibraryImport("MaaFramework")]
    public static partial long MaaControllerPostScreencap(IntPtr ctrl_handle);

    [LibraryImport("MaaFramework")]
    public static partial int MaaControllerStatus(IntPtr ctrl_handle, long id);

    [LibraryImport("MaaFramework")]
    public static partial int MaaControllerWait(IntPtr ctrl_handle, long id);

    [LibraryImport("MaaFramework")]
    public static partial byte MaaControllerConnected(IntPtr ctrl_handle);

    [LibraryImport("MaaFramework")]
    public static partial ulong MaaControllerGetImage(IntPtr ctrl_handle, IntPtr buff, ulong buff_size);

    [LibraryImport("MaaFramework")]
    public static partial ulong MaaControllerGetUUID(IntPtr ctrl_handle, IntPtr buff, ulong buff_size);

    /* Instance */

    [LibraryImport("MaaFramework")]
    public static partial IntPtr MaaCreate(MaaCallback callback, IntPtr callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial void MaaDestroy(IntPtr inst_handle);

    [LibraryImport("MaaFramework")]
    public static partial byte MaaSetOption(IntPtr inst_handle, int option, IntPtr value, ulong value_size);

    [LibraryImport("MaaFramework")]
    public static partial byte MaaBindResource(IntPtr inst_handle, IntPtr resource_handle);

    [LibraryImport("MaaFramework")]
    public static partial byte MaaBindController(IntPtr inst_handle, IntPtr controller_handle);

    [LibraryImport("MaaFramework")]
    public static partial byte MaaInited(IntPtr inst_handle);

    [LibraryImport("MaaFramework")]
    public static partial void MaaRegisterCustomTask(IntPtr inst_handle, IntPtr name, IntPtr task);

    [LibraryImport("MaaFramework")]
    public static partial void MaaUnregisterCustomTask(IntPtr inst_handle, IntPtr name);

    [LibraryImport("MaaFramework")]
    public static partial void MaaClearCustomTask(IntPtr inst_handle);

    [LibraryImport("MaaFramework")]
    public static partial long MaaPostTask(IntPtr inst_handle, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string args);

    [LibraryImport("MaaFramework")]
    public static partial byte MaaSetTaskParam(IntPtr inst_handle, long id, [MarshalAs(UnmanagedType.LPUTF8Str)] string args);

    [LibraryImport("MaaFramework")]
    public static partial int MaaTaskStatus(IntPtr inst_handle, long id);

    [LibraryImport("MaaFramework")]
    public static partial int MaaWaitTask(IntPtr inst_handle, long id);

    [LibraryImport("MaaFramework")]
    public static partial byte MaaTaskAllFinished(IntPtr inst_handle);

    [LibraryImport("MaaFramework")]
    public static partial void MaaStop(IntPtr inst_handle);

    [LibraryImport("MaaFramework")]
    public static partial IntPtr MaaGetResource(IntPtr inst_handle);

    [LibraryImport("MaaFramework")]
    public static partial IntPtr MaaGetController(IntPtr inst_handle);

    /* Utils */

    [LibraryImport("MaaFramework")]
    public static partial byte MaaSetGlobalOption(int option, IntPtr value, ulong value_size);
    [LibraryImport("MaaFramework")]
    public static partial byte MaaSetGlobalOption(int option, ref byte value, ulong value_size);

    [LibraryImport("MaaFramework")]
    public static partial IntPtr MaaVersion();
}
