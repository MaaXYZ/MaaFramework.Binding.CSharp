<div align="center">

<img alt="LOGO" src="https://cdn.jsdelivr.net/gh/MaaAssistantArknights/design@main/logo/maa-logo_512x512.png" width="256" height="256" />

# MaaFramework.Binding.CSharp

_✨ The csharp binding of [MaaFramework](https://github.com/MaaAssistantArknights/MaaFramework/tree/v1.1.1) ✨_

_💫 A common interoperable API wrapper 💫_

![license](https://img.shields.io/github/license/MaaAssistantArknights/MaaFramework)
![language](https://img.shields.io/badge/.NET-7-512BD4?logo=csharp)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Maa.Framework.Runtimes?logo=nuget&color=%23004880)
![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blueviolet)

</div>

## Best Practices

- [MBA](https://github.com/MaaAssistantArknights/MBA) BA Assistant  
  A BA Assistant based on MAA's new architecture. Image technology + simulation control, no more clicking! Powered by MaaFramework.

## Overview of Wrapper and Api

### Enums

- Tip: If the name of an Enum ends in the ***plural***, it means that the Enum, with **Flags**Attribute, can contain more than one kind of information through **"bitwise OR"** operation.

| Wrapper | MaaDef |
| --- | --- |
| AdbControllerTypes | MaaAdbControllerTypeEnum |
| ControllerOption | MaaCtrlOptionEnum |
| DebuggingControllerTypes | MaaDebuggingControllerTypeEnum |
| GlobalOption | MaaGlobalOptionEnum |
| InstanceOption | MaaInstOptionEnum |
| MaaJobStatus | MaaStatusEnum |
| ResourceOption | MaaResOptionEnum |

### IMaaCommon

| Wrapper | Native API |
| --- | --- |
| IMaaCommon.Callback | *Occurs when MaaFramework calls back.* |

### MaaJob : IMaaJob

| Wrapper | Native API |
| --- | --- |
| IMaaJob.Status | MaaControllerStatus <br> MaaResourceStatus <br> MaaTaskStatus |
| IMaaJob.Wait() | MaaControllerWait <br> MaaResourceWait <br> MaaWaitTask |
| IMaaJob.SetParam() | MaaSetTaskParam |

### MaaController : IMaaController

| Wrapper | Native API |
| --- | --- |
| MaaController.MaaController() | MaaAdbControllerCreateV2 |
| MaaCustomController.MaaCustomController() | MaaCustomControllerCreate |
| MaaThriftController.MaaThriftController() | MaaThriftControllerCreate |
| MaaDebuggingController.MaaDebuggingController() | MaaDebuggingControllerCreate |
| IDisposable.Dispose() | MaaControllerDestroy |
| IMaaOption.SetOption() | MaaControllerSetOption |
| IMaaController.LinkStart() | MaaControllerPostConnection |
| IMaaController.Click() | MaaControllerPostClick |
| IMaaController.Swipe() | MaaControllerPostSwipe |
| IMaaController.PressKey() | MaaControllerPostPressKey |
| IMaaController.TouchDown() | MaaControllerPostTouchDown |
| IMaaController.TouchMove() | MaaControllerPostTouchMove |
| IMaaController.TouchUp() | MaaControllerPostTouchUp |
| IMaaController.Screencap() | MaaControllerPostScreencap |
| IMaaPost.SetParam() | *Always return false.* |
| IMaaPost.GetStatus() | MaaControllerStatus |
| IMaaPost.Wait() | MaaControllerWait |
| IMaaController.LinkStop() | MaaControllerConnected |
| IMaaController.GetImage() | MaaControllerGetImage |
| IMaaController.Uuid | MaaControllerGetUUID |
| IMaaCommon.Callback | *Occurs when MaaFramework calls back.* |
| IMaaDisposableHandle.Handle | *The MaaResourceHandle.* |
| IMaaDisposableHandle.SetHandleAsInvalid() | *Marks a handle as no longer used.* |

### MaaInstance : IMaaInstance

| Wrapper | Native API |
| --- | --- |
| MaaInstance.MaaInstance() | MaaCreate |
| IDisposable.Dispose() | MaaDestroy |
| IMaaOption.SetOption() | MaaSetOption |
| IMaaInstance.Resource | MaaBindResource <br> MaaGetResource |
| IMaaInstance.Controller | MaaBindController <br> MaaGetController |
| IMaaInstance.Initialized | MaaInited |
| IMaaInstance.Register() | MaaRegisterCustomRecognizer <br> MaaRegisterCustomAction |
| IMaaInstance.Unregister() | MaaUnregisterCustomRecognizer <br> MaaUnregisterCustomAction |
| IMaaInstance.Clear() | MaaClearCustomRecognizer <br> MaaClearCustomAction |
| IMaaInstance.AppendTask() | MaaPostTask |
| IMaaPost.SetParam() | MaaSetTaskParam |
| IMaaPost.GetStatus() | MaaTaskStatus |
| IMaaPost.Wait() | MaaWaitTask |
| IMaaInstance.AllTasksFinished | MaaTaskAllFinished |
| IMaaInstance.Stop() | MaaStop |
| IMaaCommon.Callback | *Occurs when MaaFramework calls back.* |
| IMaaDisposableHandle.Handle | *The MaaResourceHandle.* |
| IMaaDisposableHandle.SetHandleAsInvalid() | *Marks a handle as no longer used.* |

### MaaResource : IMaaResource

| Wrapper | Native API |
| --- | --- |
| MaaResource.MaaResource() | MaaResourceCreate |
| IDisposable.Dispose() | MaaResourceDestroy |
| IMaaResource.AppendPath() | MaaResourcePostPath |
| IMaaPost.SetParam() | *Always return false.* |
| IMaaPost.GetStatus() | MaaResourceStatus |
| IMaaPost.Wait() | MaaResourceWait |
| IMaaResource.Loaded | MaaResourceLoaded |
| IMaaOption.SetOption() | MaaResourceSetOption |
| IMaaResource.Hash | MaaResourceGetHash |
| IMaaCommon.Callback | *Occurs when MaaFramework calls back.* |
| IMaaDisposableHandle.Handle | *The MaaResourceHandle.* |
| IMaaDisposableHandle.SetHandleAsInvalid() | *Marks a handle as no longer used.* |

### MaaSyncContext : IMaaSyncContext

| Wrapper | Native API |
| --- | --- |
| IMaaSyncContext.Handle | *The MaaSyncContextHandle.* |
| IMaaSyncContext.RunTask() | MaaSyncContextRunTask |
| IMaaSyncContext.RunRecognizer() | MaaSyncContextRunRecognizer |
| IMaaSyncContext.RunAction() | MaaSyncContextRunAction |
| IMaaSyncContext.Click() | MaaSyncContextClick |
| IMaaSyncContext.Swipe() | MaaSyncContextSwipe |
| IMaaSyncContext.PressKey() | MaaSyncContextPressKey |
| IMaaSyncContext.TouchDown() | MaaSyncContextTouchDown |
| IMaaSyncContext.TouchMove() | MaaSyncContextTouchMove |
| IMaaSyncContext.TouchUp() | MaaSyncContextTouchUp |
| IMaaSyncContext.Screencap() | MaaSyncContextScreencap |
| IMaaSyncContext.GetTaskResult() | MaaSyncContextGetTaskResult |

### MaaToolkit : IMaaToolkit

| Wrapper | Native API |
| --- | --- |
| IMaaToolkit.Init() | MaaToolKitInit |
| IMaaToolkit.Uninit() | MaaToolKitUninit |
| IMaaToolKit.Find() | *The DeviceInfo Array.* |
| MaaToolKit.FindDevice() | MaaToolKitFindDevice <br> MaaToolKitFindDeviceWithAdb |
| MaaToolKit.GetDeviceName() | MaaToolKitGetDeviceName |
| MaaToolKit.GetDeviceAdbPath() | MaaToolKitGetDeviceAdbPath |
| MaaToolKit.GetDeviceAdbSerial() | MaaToolKitGetDeviceAdbSerial |
| MaaToolKit.GetDeviceAdbControllerTypes() | MaaToolKitGetDeviceAdbControllerType |
| MaaToolKit.GetDeviceAdbConfig() | MaaToolKitGetDeviceAdbConfig |

### MaaUtility : IMaaUtility

| Wrapper | Native API |
| --- | --- |
| IMaaUtility.Version | MaaVersion |
| IMaaOption.SetOption() | MaaSetGlobalOption |

### MaaImageBuffer : IMaaImageBuffer

| Wrapper | FFI API |
| --- | --- |
| MaaImageBuffer.MaaImageBuffer() | MaaCreateImageBuffer |
| IDisposable.Dispose() | MaaDestroyImageBuffer |
| IMaaImageBuffer.IsEmpty() | MaaIsImageEmpty |
| IMaaImageBuffer.Clear() | MaaClearImage |
| IMaaImageBuffer.GetRawData() | MaaGetImageRawData |
| IMaaImageBuffer.Width | MaaGetImageWidth |
| IMaaImageBuffer.Height | MaaGetImageHeight |
| IMaaImageBuffer.Type | MaaGetImageType |
| IMaaImageBuffer.SetRawData() | MaaSetImageRawData |
| IMaaImageBuffer.GetEncodedData() | MaaGetImageEncoded |
| IMaaImageBuffer.Size | MaaGetImageEncodedSize |
| IMaaImageBuffer.SetEncodedData() | MaaSetImageEncoded |
| IMaaDisposableHandle.Handle | *The MaaImageBufferHandle.* |
| IMaaDisposableHandle.SetHandleAsInvalid() | *Marks a handle as no longer used.* |

### MaaStringBuffer : IMaaStringBuffer

| Wrapper | FFI API |
| --- | --- |
| MaaStringBuffer.MaaStringBuffer() | MaaCreateStringBuffer |
| IDisposable.Dispose() | MaaDestroyStringBuffer |
| IMaaStringBuffer.IsEmpty() | MaaIsStringEmpty |
| IMaaStringBuffer.Clear() | MaaClearString |
| IMaaStringBuffer.Get() | MaaGetString |
| IMaaStringBuffer.Size | MaaGetStringSize |
| IMaaStringBuffer.Set() | MaaSetString <br> MaaSetStringEx |
| IMaaStringBuffer.String | MaaGetString <br> MaaSetStringEx |
| object.ToString() | MaaGetString |
| IMaaDisposableHandle.Handle | *The MaaStringBufferHandle.* |
| IMaaDisposableHandle.SetHandleAsInvalid() | *Marks a handle as no longer used.* |

### MaaRectBuffer : IMaaRectBuffer

| Wrapper | FFI API |
| --- | --- |
| MaaRectBuffer.MaaRectBuffer() | MaaCreateRectBuffer |
| IDisposable.Dispose() | MaaDestroyRectBuffer |
| IMaaRectBuffer.X | MaaGetRectX <br> MaaSetRectX |
| IMaaRectBuffer.Y | MaaGetRectY <br> MaaSetRectY |
| IMaaRectBuffer.Width | MaaGetRectW <br> MaaSetRectW |
| IMaaRectBuffer.Height | MaaGetRectH <br> MaaSetRectH |
| IMaaRectBuffer.Set() | MaaSetRectX <br> MaaSetRectY <br> MaaSetRectW <br> MaaSetRectH |
| IMaaRectBuffer.Get() | MaaGetRectX <br> MaaGetRectY <br> MaaGetRectW <br> MaaGetRectH |
| IMaaDisposableHandle.Handle | *The MaaRectHandle.* |
| IMaaDisposableHandle.SetHandleAsInvalid() | *Marks a handle as no longer used.* |
