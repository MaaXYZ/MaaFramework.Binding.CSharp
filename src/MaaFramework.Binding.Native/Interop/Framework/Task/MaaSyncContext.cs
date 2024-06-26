﻿using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Native;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1707 // 标识符不应包含下划线

/// <summary>
///     The base P/Invoke methods for MaaSyncContext, use this class to call all the native methods.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="Binding.MaaSyncContext"/> instead.
/// </summary>
public static partial class MaaSyncContext
{

    #region include/MaaFramework/Task/MaaSyncContext.h, version: v1.6.4.

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextRunTask(MaaSyncContextHandle sync_context, [MarshalAs(UnmanagedType.LPUTF8Str)] string task_name, [MarshalAs(UnmanagedType.LPUTF8Str)] string param);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextRunRecognizer(MaaSyncContextHandle sync_context, MaaImageBufferHandle image, [MarshalAs(UnmanagedType.LPUTF8Str)] string task_name, [MarshalAs(UnmanagedType.LPUTF8Str)] string task_param, MaaRectHandle out_box /* [out] */, MaaStringBufferHandle out_detail /* [out] */);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextRunAction(MaaSyncContextHandle sync_context, [MarshalAs(UnmanagedType.LPUTF8Str)] string task_name, [MarshalAs(UnmanagedType.LPUTF8Str)] string task_param, MaaRectHandle cur_box, [MarshalAs(UnmanagedType.LPUTF8Str)] string cur_rec_detail);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextClick(MaaSyncContextHandle sync_context, int32_t x, int32_t y);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextSwipe(MaaSyncContextHandle sync_context, int32_t x1, int32_t y1, int32_t x2, int32_t y2, int32_t duration);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextPressKey(MaaSyncContextHandle sync_context, int32_t keycode);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextInputText(MaaSyncContextHandle sync_context, [MarshalAs(UnmanagedType.LPUTF8Str)] string text);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextTouchDown(MaaSyncContextHandle sync_context, int32_t contact, int32_t x, int32_t y, int32_t pressure);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextTouchMove(MaaSyncContextHandle sync_context, int32_t contact, int32_t x, int32_t y, int32_t pressure);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextTouchUp(MaaSyncContextHandle sync_context, int32_t contact);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextScreencap(MaaSyncContextHandle sync_context, MaaImageBufferHandle out_image /* [out] */);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSyncContextGetTaskResult(MaaSyncContextHandle sync_context, [MarshalAs(UnmanagedType.LPUTF8Str)] string task_name, MaaStringBufferHandle out_task_result /* [out] */);

    #endregion

}
