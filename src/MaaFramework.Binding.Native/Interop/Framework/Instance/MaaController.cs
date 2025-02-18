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

global using MaaCustomControllerCallbacksHandle = nint;

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MaaFramework.Binding.Interop.Native;

public static partial class MaaController
{
    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaControllerHandle MaaAdbControllerCreate(string adbPath, string address, MaaAdbScreencapMethod screencapMethods, MaaAdbInputMethod inputMethods, string config, string agentPath, MaaNotificationCallback notify, nint notifyTransArg);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaControllerHandle MaaWin32ControllerCreate(nint hWnd, MaaWin32ScreencapMethod screencapMethod, MaaWin32InputMethod inputMethod, MaaNotificationCallback notify, nint notifyTransArg);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaControllerHandle MaaCustomControllerCreate([MarshalUsing(typeof(MaaMarshaller))] Custom.IMaaCustomController controller, nint controllerArg, MaaNotificationCallback notify, nint notifyTransArg);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaControllerHandle MaaDbgControllerCreate(string readPath, string writePath, MaaDbgControllerType type, string config, MaaNotificationCallback notify, nint notifyTransArg);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial void MaaControllerDestroy(MaaControllerHandle ctrl);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool MaaControllerSetOption(MaaControllerHandle ctrl, MaaCtrlOption key, byte[] value, MaaOptionValueSize valSize);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaCtrlId MaaControllerPostConnection(MaaControllerHandle ctrl);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaCtrlId MaaControllerPostClick(MaaControllerHandle ctrl, int x, int y);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaCtrlId MaaControllerPostSwipe(MaaControllerHandle ctrl, int x1, int y1, int x2, int y2, int duration);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaCtrlId MaaControllerPostPressKey(MaaControllerHandle ctrl, int keycode);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaCtrlId MaaControllerPostInputText(MaaControllerHandle ctrl, string text);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaCtrlId MaaControllerPostStartApp(MaaControllerHandle ctrl, string intent);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaCtrlId MaaControllerPostStopApp(MaaControllerHandle ctrl, string intent);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaCtrlId MaaControllerPostTouchDown(MaaControllerHandle ctrl, int contact, int x, int y, int pressure);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaCtrlId MaaControllerPostTouchMove(MaaControllerHandle ctrl, int contact, int x, int y, int pressure);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaCtrlId MaaControllerPostTouchUp(MaaControllerHandle ctrl, int contact);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaCtrlId MaaControllerPostScreencap(MaaControllerHandle ctrl);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaStatus MaaControllerStatus(MaaControllerHandle ctrl, MaaCtrlId id);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    public static partial MaaStatus MaaControllerWait(MaaControllerHandle ctrl, MaaCtrlId id);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool MaaControllerConnected(MaaControllerHandle ctrl);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool MaaControllerCachedImage(MaaControllerHandle ctrl, MaaImageBufferHandle buffer);

    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool MaaControllerGetUuid(MaaControllerHandle ctrl, MaaStringBufferHandle buffer);
}
