using System.Runtime.InteropServices;

namespace MaaToolKit.Extensions.Interop.Framework.Instance;

#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///     The base P/Invoke methods for MaaController, use this class to call all the native methods.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="Extensions.MaaController"/> instead.
/// </summary>
public static partial class MaaController
{

    #region include/MaaFramework/Instance/MaaController.h, version: v1.1.0.

    [Obsolete("Replaced by MaaAdbControllerCreateV2")]
    [LibraryImport("MaaFramework")]
    public static partial MaaControllerHandle MaaAdbControllerCreate([MarshalAs(UnmanagedType.LPUTF8Str)] string adb_path, [MarshalAs(UnmanagedType.LPUTF8Str)] string address, MaaAdbControllerType type, [MarshalAs(UnmanagedType.LPUTF8Str)] string config, MaaControllerCallback callback, MaaCallbackTransparentArg callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial MaaControllerHandle MaaAdbControllerCreateV2([MarshalAs(UnmanagedType.LPUTF8Str)] string adb_path, [MarshalAs(UnmanagedType.LPUTF8Str)] string address, MaaAdbControllerType type, [MarshalAs(UnmanagedType.LPUTF8Str)] string config, [MarshalAs(UnmanagedType.LPUTF8Str)] string agent_path, MaaControllerCallback callback, MaaCallbackTransparentArg callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial MaaControllerHandle MaaCustomControllerCreate(ref MaaCustomControllerApi handle, MaaTransparentArg handle_arg, MaaControllerCallback callback, MaaCallbackTransparentArg callback_arg);

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
    public static partial MaaCtrlId MaaControllerPostSwipe(MaaControllerHandle ctrl, int32_t x1, int32_t y1, int32_t x2, int32_t y2, int32_t duration);

    [LibraryImport("MaaFramework")]
    public static partial MaaCtrlId MaaControllerPostPressKey(MaaControllerHandle ctrl, int32_t keycode);

    [LibraryImport("MaaFramework")]
    public static partial MaaCtrlId MaaControllerPostTouchDown(MaaControllerHandle ctrl, int32_t contact, int32_t x, int32_t y, int32_t pressure);

    [LibraryImport("MaaFramework")]
    public static partial MaaCtrlId MaaControllerPostTouchMove(MaaControllerHandle ctrl, int32_t contact, int32_t x, int32_t y, int32_t pressure);

    [LibraryImport("MaaFramework")]
    public static partial MaaCtrlId MaaControllerPostTouchUp(MaaControllerHandle ctrl, int32_t contact);

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

    #endregion

}
