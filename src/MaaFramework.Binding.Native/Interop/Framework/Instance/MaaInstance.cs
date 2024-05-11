using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Native;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1707 // 标识符不应包含下划线

/// <summary>
///     The base P/Invoke methods for MaaInstance, use this class to call all the native methods.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="Binding.MaaInstance"/> instead.
/// </summary>
public static partial class MaaInstance
{

    #region include/MaaFramework/Instance/MaaInstance.h, version: v1.6.4.

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
#pragma warning disable SYSLIB1051 // 源生成的 P/Invoke 不支持指定的类型
    public static partial MaaBool MaaRegisterCustomRecognizer(MaaInstanceHandle inst, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, MaaCustomRecognizerApi recognizer, MaaTransparentArg recognizer_arg);
#pragma warning restore SYSLIB1051 // 源生成的 P/Invoke 不支持指定的类型

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaUnregisterCustomRecognizer(MaaInstanceHandle inst, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaClearCustomRecognizer(MaaInstanceHandle inst);

    [LibraryImport("MaaFramework")]
#pragma warning disable SYSLIB1051 // 源生成的 P/Invoke 不支持指定的类型
    public static partial MaaBool MaaRegisterCustomAction(MaaInstanceHandle inst, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, MaaCustomActionApi action, MaaTransparentArg action_arg);
#pragma warning restore SYSLIB1051 // 源生成的 P/Invoke 不支持指定的类型

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
    public static partial MaaBool MaaPostStop(MaaInstanceHandle inst);

    [Obsolete("This API MaaStop is about to be deprecated. Please use MaaPostStop instead.")]
    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaStop(MaaInstanceHandle inst);

    [LibraryImport("MaaFramework")]
    public static partial MaaResourceHandle MaaGetResource(MaaInstanceHandle inst);

    [LibraryImport("MaaFramework")]
    public static partial MaaControllerHandle MaaGetController(MaaInstanceHandle inst);

    #endregion

}
