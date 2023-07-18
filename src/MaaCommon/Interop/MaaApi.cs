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

namespace MaaCommon.Interop;

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
    public static partial IntPtr MaaResourceCreate([MarshalAs(UnmanagedType.LPUTF8Str)] string user_path, MaaCallback callback, IntPtr callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial void MaaResourceDestroy(IntPtr res_handle);

    [LibraryImport("MaaFramework")]
    public static partial Int64 MaaResourcePostResource(IntPtr res_handle, [MarshalAs(UnmanagedType.LPUTF8Str)] string path);

    [LibraryImport("MaaFramework")]
    public static partial Int32 MaaResourceStatus(IntPtr res_handle, Int64 id);

    [LibraryImport("MaaFramework")]
    public static partial Int32 MaaResourceWait(IntPtr res_handle, Int64 id);

    [LibraryImport("MaaFramework")]
    public static partial Byte MaaResourceLoaded(IntPtr res_handle);

    [LibraryImport("MaaFramework")]
    public static partial Byte MaaResourceSetOption(IntPtr res_handle, Int32 option, IntPtr value, UInt64 value_size);

    [LibraryImport("MaaFramework")]
    public static partial UInt64 MaaResourceGetHash(IntPtr res_handle, IntPtr buff, UInt64 buff_size);

    /* Controller */

    [LibraryImport("MaaFramework")]
    public static partial IntPtr MaaAdbControllerCreate([MarshalAs(UnmanagedType.LPUTF8Str)] string adb_path, [MarshalAs(UnmanagedType.LPUTF8Str)] string address, Int32 type,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string config, MaaCallback callback,
        IntPtr callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial void MaaControllerDestroy(IntPtr ctrl_handle);

    [LibraryImport("MaaFramework")]
    public static partial Byte MaaControllerSetOption(IntPtr ctrl_handle, Int32 option, IntPtr value, UInt64 value_size);
    [LibraryImport("MaaFramework")]
    public static partial Byte MaaControllerSetOption(IntPtr ctrl_handle, Int32 option, ref byte value, UInt64 value_size);

    [LibraryImport("MaaFramework")]
    public static partial Int64 MaaControllerPostConnection(IntPtr ctrl_handle);

    [LibraryImport("MaaFramework")]
    public static partial Int64 MaaControllerPostClick(IntPtr ctrl_handle, Int32 x, Int32 y);

    [LibraryImport("MaaFramework")]
    public static unsafe partial Int64 MaaControllerPostSwipe(IntPtr ctrl_handle, Int32* x_steps, Int32* y_steps, Int32* step_delays, UInt64 buff_size);

    [LibraryImport("MaaFramework")]
    public static partial Int64 MaaControllerPostScreencap(IntPtr ctrl_handle);

    [LibraryImport("MaaFramework")]
    public static partial Int32 MaaControllerStatus(IntPtr ctrl_handle, Int64 id);

    [LibraryImport("MaaFramework")]
    public static partial Int32 MaaControllerWait(IntPtr ctrl_handle, Int64 id);

    [LibraryImport("MaaFramework")]
    public static partial Byte MaaControllerConnected(IntPtr ctrl_handle);

    [LibraryImport("MaaFramework")]
    public static partial UInt64 MaaControllerGetImage(IntPtr ctrl_handle, IntPtr buff, UInt64 buff_size);

    [LibraryImport("MaaFramework")]
    public static partial UInt64 MaaControllerGetUUID(IntPtr ctrl_handle, IntPtr buff, UInt64 buff_size);

    /* Instance */

    [LibraryImport("MaaFramework")]
    public static partial IntPtr MaaInstanceCreate(MaaCallback callback, IntPtr callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial void MaaInstanceDestroy(IntPtr inst_handle);

    [LibraryImport("MaaFramework")]
    public static partial Byte MaaInstanceSetOption(IntPtr inst_handle, Int32 option, IntPtr value, UInt64 value_size);

    [LibraryImport("MaaFramework")]
    public static partial Byte MaaBindResource(IntPtr inst_handle, IntPtr resource_handle);

    [LibraryImport("MaaFramework")]
    public static partial Byte MaaBindController(IntPtr inst_handle, IntPtr controller_handle);

    [LibraryImport("MaaFramework")]
    public static partial Byte MaaInited(IntPtr inst_handle);

    [LibraryImport("MaaFramework")]
    public static partial void MaaRegisterCustomTask(IntPtr inst_handle, IntPtr name, IntPtr task);

    [LibraryImport("MaaFramework")]
    public static partial void MaaUnregisterCustomTask(IntPtr inst_handle, IntPtr name);

    [LibraryImport("MaaFramework")]
    public static partial void MaaClearCustomTask(IntPtr inst_handle);

    [LibraryImport("MaaFramework")]
    public static partial Int64 MaaInstancePostTask(IntPtr inst_handle, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string args);

    [LibraryImport("MaaFramework")]
    public static partial Byte MaaSetTaskParam(IntPtr inst_handle, long id, [MarshalAs(UnmanagedType.LPUTF8Str)] string args);

    [LibraryImport("MaaFramework")]
    public static partial Int32 MaaTaskStatus(IntPtr inst_handle, Int64 id);

    [LibraryImport("MaaFramework")]
    public static partial Int32 MaaTaskWait(IntPtr inst_handle, Int64 id);

    [LibraryImport("MaaFramework")]
    public static partial Byte MaaTaskAllFinished(IntPtr inst_handle);

    [LibraryImport("MaaFramework")]
    public static partial void MaaStop(IntPtr inst_handle);

    [LibraryImport("MaaFramework")]
    public static partial IntPtr MaaGetResource(IntPtr inst_handle);

    [LibraryImport("MaaFramework")]
    public static partial IntPtr MaaGetController(IntPtr inst_handle);

    /* Utils */

    [LibraryImport("MaaFramework")]
    public static partial Byte MaaSetGlobalOption(Int32 option, IntPtr value, UInt64 value_size);
    [LibraryImport("MaaFramework")]
    public static partial Byte MaaSetGlobalOption(int option, ref byte value, ulong value_size);

    [LibraryImport("MaaFramework")]
    public static partial IntPtr MaaVersion();
}
