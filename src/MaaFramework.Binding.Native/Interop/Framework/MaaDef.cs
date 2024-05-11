#pragma warning disable IDE0005 // Using 指令是不需要的。

global using MaaStringBufferHandle = nint;
global using MaaImageBufferHandle = nint;

global using MaaResourceHandle = nint;
global using MaaControllerHandle = nint;
global using MaaInstanceHandle = nint;

global using MaaBool = System.Byte;
global using MaaSize = System.UInt64;
// const MaaNullSize

global using MaaStringView = nint;

global using MaaStatus = System.Int32;
// enum MaaJobStatus
global using MaaLoggingLevel = System.Int32;
// enum LoggingLevel

global using MaaId = System.Int64; // Also global using in MaaFramework.Binding
global using MaaCtrlId = System.Int64;
global using MaaResId = System.Int64;
global using MaaTaskId = System.Int64;
// const MaaInvalidId

global using MaaOption = System.Int32;
global using MaaOptionValue = System.Byte;
global using MaaOptionValueSize = System.UInt64;

global using MaaGlobalOption = System.Int32;
// enum GlobalOption
global using MaaResOption = System.Int32;
// enum ResourceOption
global using MaaCtrlOption = System.Int32;
// enum ResourceOption
global using MaaInstOption = System.Int32;
// enum InstanceOption
// const MaaTaskParam_Empty
global using MaaAdbControllerType = System.Int32;
// enum AdbControllerTypes
global using MaaDbgControllerType = System.Int32;
// enum DbgControllerType
global using MaaThriftControllerType = System.Int32;
// enum ThriftControllerType
global using MaaWin32ControllerType = System.Int32;
// enum Win32ControllerTypes
global using MaaWin32Hwnd = nint;

global using MaaTransparentArg = nint;
global using MaaCallbackTransparentArg = nint;

global using MaaResourceCallback = MaaFramework.Binding.Interop.Native.MaaAPICallback;
global using MaaControllerCallback = MaaFramework.Binding.Interop.Native.MaaAPICallback;
global using MaaInstanceCallback = MaaFramework.Binding.Interop.Native.MaaAPICallback;

global using MaaCustomControllerHandle = nint;
global using MaaCustomRecognizerHandle = nint;
global using MaaCustomActionHandle = nint;
global using MaaSyncContextHandle = nint;
global using MaaRectHandle = nint;

global using int32_t = System.Int32;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1707 // 标识符不应包含下划线

using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Native;

public static class MaaDef
{

    #region include/MaaFramework/MaaAPI.h, version: v1.6.4.

    #endregion

    internal const MaaSize MaaNullSize = MaaSize.MaxValue;
    internal const MaaId MaaInvalidId = 0;
    internal const string EmptyMaaTaskParam = "{}";
}

#region include/MaaFramework/MaaDef.h, version: v1.6.4.

/// <summary>
///     The callback delegate.
/// </summary>
/// <param name="msg">The message. See MaaMsg.h</param>
/// <param name="details_json">The details in JSON format. See doc in MaaMsg.h</param>
/// <param name="callback_arg"></param>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void MaaAPICallback(MaaStringView msg, MaaStringView details_json, MaaTransparentArg callback_arg);

#endregion
