using System.Runtime.InteropServices;

namespace MaaToolKit.Extensions.Interop;

#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///     The base P/Invoke methods for MaaFramework, use this class to call all the native methods.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="ComponentModel"/> instead.
/// </summary>
public static partial class MaaApi
{
    static MaaApi()
    {
        NativeLibrary.Init();
    }

    #region include/MaaFramework/MaaAPI.h, commit title: chore: 移除struct API的导出宏, commit hash: c0630377a4c959f324d684106a001ef38807b4ca.

    [LibraryImport("MaaFramework")]
    public static partial MaaStringView MaaVersion();

    [LibraryImport("MaaFramework")]
    public static partial MaaStringBufferHandle MaaCreateStringBuffer();

    [LibraryImport("MaaFramework")]
    public static partial void MaaDestroyStringBuffer(MaaStringBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaStringView MaaGetString(MaaStringBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaSize MaaGetStringSize(MaaStringBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetString(MaaStringBufferHandle handle, [MarshalAs(UnmanagedType.LPUTF8Str)] string str);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetStringEx(MaaStringBufferHandle handle, [MarshalAs(UnmanagedType.LPUTF8Str)] string str, MaaSize size);

    [LibraryImport("MaaFramework")]
    public static partial MaaImageBufferHandle MaaCreateImageBuffer();

    [LibraryImport("MaaFramework")]
    public static partial void MaaDestroyImageBuffer(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaImageRawData MaaGetImageRawData(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial int32_t MaaGetImageWidth(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial int32_t MaaGetImageHeight(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial int32_t MaaGetImageType(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetImageRawData(MaaImageBufferHandle handle, MaaImageRawData data, int32_t width, int32_t height, int32_t type);

    [LibraryImport("MaaFramework")]
    public static partial MaaImageEncodedData MaaGetImageEncoded(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaSize MaaGetImageEncodedSize(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetImageEncoded(MaaImageBufferHandle handle, MaaImageEncodedData data, MaaSize size);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetGlobalOption(MaaGlobalOption key, ref MaaOptionValue value, MaaOptionValueSize val_size);

    [LibraryImport("MaaFramework")]
    public static partial MaaResourceHandle MaaResourceCreate(MaaResourceCallback callback, MaaCallbackTransparentArg callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial void MaaResourceDestroy(MaaResourceHandle res);

    [LibraryImport("MaaFramework")]
    public static partial MaaResId MaaResourcePostPath(MaaResourceHandle res, [MarshalAs(UnmanagedType.LPUTF8Str)] string path);

    [LibraryImport("MaaFramework")]
    public static partial MaaStatus MaaResourceStatus(MaaResourceHandle res, MaaResId id);

    [LibraryImport("MaaFramework")]
    public static partial MaaStatus MaaResourceWait(MaaResourceHandle res, MaaResId id);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaResourceLoaded(MaaResourceHandle res);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaResourceSetOption(MaaResourceHandle res, MaaResOption key, ref MaaOptionValue value, MaaOptionValueSize val_size);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaResourceGetHash(MaaResourceHandle res, /* out */ MaaStringBufferHandle buffer);

    [LibraryImport("MaaFramework")]
    public static partial MaaControllerHandle MaaAdbControllerCreate([MarshalAs(UnmanagedType.LPUTF8Str)] string adb_path, [MarshalAs(UnmanagedType.LPUTF8Str)] string address, MaaAdbControllerType type, [MarshalAs(UnmanagedType.LPUTF8Str)] string config, MaaControllerCallback callback, MaaCallbackTransparentArg callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial MaaControllerHandle MaaCustomControllerCreate(ref MaaCustomControllerApi handle, MaaControllerCallback callback, MaaCallbackTransparentArg callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial MaaControllerHandle MaaThriftControllerCreate([MarshalAs(UnmanagedType.LPUTF8Str)] string param, MaaControllerCallback callback, MaaCallbackTransparentArg callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial void MaaControllerDestroy(MaaControllerHandle ctrl);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaControllerSetOption(MaaControllerHandle ctrl, MaaCtrlOption key, ref MaaOptionValue value, MaaOptionValueSize val_size);

    [LibraryImport("MaaFramework")]
    public static partial MaaCtrlId MaaControllerPostConnection(MaaControllerHandle ctrl);

    [LibraryImport("MaaFramework")]
    public static partial MaaCtrlId MaaControllerPostClick(MaaControllerHandle ctrl, int32_t x, int32_t y);

    [LibraryImport("MaaFramework")]
    public static partial MaaCtrlId MaaControllerPostSwipe(MaaControllerHandle ctrl, ref int32_t x_steps_buff, ref int32_t y_steps_buff, ref int32_t step_delay_buff, MaaSize buff_size);

    [LibraryImport("MaaFramework")]
    public static partial MaaCtrlId MaaControllerPostScreencap(MaaControllerHandle ctrl);

    [LibraryImport("MaaFramework")]
    public static partial MaaStatus MaaControllerStatus(MaaControllerHandle ctrl, MaaCtrlId id);

    [LibraryImport("MaaFramework")]
    public static partial MaaStatus MaaControllerWait(MaaControllerHandle ctrl, MaaCtrlId id);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaControllerConnected(MaaControllerHandle ctrl);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaControllerGetImage(MaaControllerHandle ctrl, /* out */ MaaImageBufferHandle buffer);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaControllerGetUUID(MaaControllerHandle ctrl, /* out */ MaaStringBufferHandle buffer);

    [LibraryImport("MaaFramework")]
    public static partial MaaInstanceHandle MaaCreate(MaaInstanceCallback callback, MaaCallbackTransparentArg callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial void MaaDestroy(MaaInstanceHandle inst);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetOption(MaaInstanceHandle inst, MaaInstOption key, ref MaaOptionValue value, MaaOptionValueSize val_size);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaBindResource(MaaInstanceHandle inst, MaaResourceHandle res);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaBindController(MaaInstanceHandle inst, MaaControllerHandle ctrl);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaInited(MaaInstanceHandle inst);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaRegisterCustomRecognizer(MaaInstanceHandle inst, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, ref MaaCustomRecognizerApi recognizer);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaUnregisterCustomRecognizer(MaaInstanceHandle inst, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaClearCustomRecognizer(MaaInstanceHandle inst);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaRegisterCustomAction(MaaInstanceHandle inst, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, ref MaaCustomActionApi action);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaUnregisterCustomAction(MaaInstanceHandle inst, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaClearCustomAction(MaaInstanceHandle inst);

    [LibraryImport("MaaFramework")]
    public static partial MaaTaskId MaaPostTask(MaaInstanceHandle inst, [MarshalAs(UnmanagedType.LPUTF8Str)] string entry, [MarshalAs(UnmanagedType.LPUTF8Str)] string param);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetTaskParam(MaaInstanceHandle inst, MaaTaskId id, [MarshalAs(UnmanagedType.LPUTF8Str)] string param);

    [LibraryImport("MaaFramework")]
    public static partial MaaStatus MaaTaskStatus(MaaInstanceHandle inst, MaaTaskId id);

    [LibraryImport("MaaFramework")]
    public static partial MaaStatus MaaWaitTask(MaaInstanceHandle inst, MaaTaskId id);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaTaskAllFinished(MaaInstanceHandle inst);

    [LibraryImport("MaaFramework")]
    public static partial void MaaStop(MaaInstanceHandle inst);

    [LibraryImport("MaaFramework")]
    public static partial MaaResourceHandle MaaGetResource(MaaInstanceHandle inst);

    [LibraryImport("MaaFramework")]
    public static partial MaaControllerHandle MaaGetController(MaaInstanceHandle inst);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextRunTask(MaaSyncContextHandle sync_context, [MarshalAs(UnmanagedType.LPUTF8Str)] string task, [MarshalAs(UnmanagedType.LPUTF8Str)] string param);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextRunRecognizer(MaaSyncContextHandle sync_context, MaaImageBufferHandle image, [MarshalAs(UnmanagedType.LPUTF8Str)] string task, [MarshalAs(UnmanagedType.LPUTF8Str)] string task_param, ref MaaRectApi out_box, /* out */ MaaStringBufferHandle detail_buff);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextRunAction(MaaSyncContextHandle sync_context, [MarshalAs(UnmanagedType.LPUTF8Str)] string task, [MarshalAs(UnmanagedType.LPUTF8Str)] string task_param, MaaRectApi cur_box, [MarshalAs(UnmanagedType.LPUTF8Str)] string cur_rec_detail);

    [LibraryImport("MaaFramework")]
    public static partial void MaaSyncContextClick(MaaSyncContextHandle sync_context, int32_t x, int32_t y);

    [LibraryImport("MaaFramework")]
    public static partial void MaaSyncContextSwipe(MaaSyncContextHandle sync_context, ref int32_t x_steps_buff, ref int32_t y_steps_buff, ref int32_t step_delay_buff, MaaSize buff_size);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextScreencap(MaaSyncContextHandle sync_context, /* out */ MaaImageBufferHandle buffer);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextGetTaskResult(MaaSyncContextHandle sync_context, [MarshalAs(UnmanagedType.LPUTF8Str)] string task, /* out */ MaaStringBufferHandle buffer);

    #endregion

}
