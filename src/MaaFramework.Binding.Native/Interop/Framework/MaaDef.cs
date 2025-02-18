﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable CS1573 // 参数在 XML 注释中没有匹配的 param 标记
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

global using MaaBool = System.Byte;
global using MaaSize = System.UInt64;
global using MaaId = System.Int64;
global using MaaCtrlId = System.Int64;
global using MaaResId = System.Int64;
global using MaaTaskId = System.Int64;
global using MaaRecoId = System.Int64;
global using MaaNodeId = System.Int64;
global using MaaStringBufferHandle = nint;
global using MaaImageBufferHandle = nint;
global using MaaStringListBufferHandle = nint;
global using MaaImageListBufferHandle = nint;
global using MaaResourceHandle = nint;
global using MaaControllerHandle = nint;
global using MaaTaskerHandle = nint;
global using MaaContextHandle = nint;
global using MaaStatus = System.Int32;
global using MaaLoggingLevel = System.Int32;
global using MaaOption = System.Int32;
global using MaaOptionValue = nint;
global using MaaOptionValueSize = System.UInt64;
global using MaaGlobalOption = System.Int32;
global using MaaResOption = System.Int32;
global using MaaInferenceDevice = System.Int32;
global using MaaInferenceExecutionProvider = System.Int32;
global using MaaCtrlOption = System.Int32;
global using MaaTaskerOption = System.Int32;
// Use bitwise OR to set the method you need, MaaFramework will test their speed and use the fastest one.
global using MaaAdbScreencapMethod = System.UInt64;
// Use bitwise OR to set the method you need, MaaFramework will select the available ones according to priority. The priority is: EmulatorExtras > Maatouch > MinitouchAndAdbKey > AdbShell
global using MaaAdbInputMethod = System.UInt64;
// No bitwise OR, just set it
global using MaaWin32ScreencapMethod = System.UInt64;
// No bitwise OR, just set it
global using MaaWin32InputMethod = System.UInt64;
// No bitwise OR, just set it
global using MaaDbgControllerType = System.UInt64;
global using MaaRectHandle = nint;

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MaaFramework.Binding.Interop.Native;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void MaaNotificationCallback([MarshalAs(UnmanagedType.LPUTF8Str)] string message, [MarshalAs(UnmanagedType.LPUTF8Str)] string detailsJson, nint notifyTransArg);

[return: MarshalAs(UnmanagedType.U1)]
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate bool MaaCustomRecognitionCallback(MaaContextHandle context, MaaTaskId taskId, [MarshalAs(UnmanagedType.LPUTF8Str)] string nodeName, [MarshalAs(UnmanagedType.LPUTF8Str)] string customRecognitionName, [MarshalAs(UnmanagedType.LPUTF8Str)] string customRecognitionParam, MaaImageBufferHandle image, MaaRectHandle roi, nint transArg, MaaRectHandle outBox, MaaStringBufferHandle outDetail);

[return: MarshalAs(UnmanagedType.U1)]
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate bool MaaCustomActionCallback(MaaContextHandle context, MaaTaskId taskId, [MarshalAs(UnmanagedType.LPUTF8Str)] string nodeName, [MarshalAs(UnmanagedType.LPUTF8Str)] string customActionName, [MarshalAs(UnmanagedType.LPUTF8Str)] string customActionParam, MaaRecoId recoId, MaaRectHandle box, nint transArg);

public static partial class MaaDef
{
    internal const MaaSize MaaNullSize = MaaSize.MaxValue;
    internal const MaaId MaaInvalidId = 0;
}
