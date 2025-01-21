# Overview of Wrapper and Api in [v1.4.0](https://github.com/MaaXYZ/MaaFramework/tree/v1.4.0)

## Enums

> [!TIP]
> If the name of an Enum ends in the ***plural***, it means that the Enum, with **Flags**Attribute, can contain more than one kind of information through **"bitwise OR"** operation.

| Wrapper | MaaDef |
| --- | --- |
| MaaJobStatus | MaaStatusEnum |
| LoggingLevel | MaaLoggingLevelEunm |
| GlobalOption | MaaGlobalOptionEnum |
| ResourceOption | MaaResOptionEnum |
| ControllerOption | MaaCtrlOptionEnum |
| InstanceOption | MaaInstOptionEnum |
| AdbControllerTypes | MaaAdbControllerTypeEnum |
| DbgControllerType | MaaDbgControllerTypeEnum |
| ThriftControllerType | MaaThriftControllerTypeEnum |
| Win32ControllerTypes | MaaWin32ControllerTypeEnum |

## MaaJob : IMaaJob

| Wrapper | Native API |
| --- | --- |
| IMaaJob.Status | MaaControllerStatus <br> MaaResourceStatus <br> MaaTaskStatus |
| IMaaJob.Wait() | MaaControllerWait <br> MaaResourceWait <br> MaaWaitTask |
| IMaaJob.SetParam() | MaaSetTaskParam |

## MaaController : IMaaController

| Wrapper | Native API |
| --- | --- |
| MaaWin32Controller | MaaWin32ControllerCreate |
| MaaAdbController | MaaAdbControllerCreateV2 |
| MaaCustomController | MaaCustomControllerCreate |
| MaaThriftController | MaaThriftControllerCreate |
| MaaDbgController | MaaDbgControllerCreate |
| IDisposable.Dispose() | MaaControllerDestroy |
| IMaaOption.SetOption() | MaaControllerSetOption |
| IMaaController.LinkStart() | MaaControllerPostConnection |
| IMaaController.Click() | MaaControllerPostClick |
| IMaaController.Swipe() | MaaControllerPostSwipe |
| IMaaController.PressKey() | MaaControllerPostPressKey |
| IMaaController.InputText() | MaaControllerPostInputText |
| IMaaController.TouchDown() | MaaControllerPostTouchDown |
| IMaaController.TouchMove() | MaaControllerPostTouchMove |
| IMaaController.TouchUp() | MaaControllerPostTouchUp |
| IMaaController.Screencap() | MaaControllerPostScreencap |
| IMaaPost.SetParam() | *Invalid operation.* |
| IMaaPost.GetStatus() | MaaControllerStatus |
| IMaaPost.Wait() | MaaControllerWait |
| IMaaController.LinkStop() | MaaControllerConnected |
| IMaaController.GetImage() | MaaControllerGetImage |
| IMaaController.Uuid | MaaControllerGetUUID |
| IMaaCommon.Callback | *Occurs when MaaFramework calls back.* |
| IMaaDisposable.IsInvalid | *Indicates whether the unmanaged resources from MaaFramework are invalid.* |
| IMaaDisposableHandle.Handle | *The MaaControllerHandle.* |
| IMaaDisposableHandle.SetHandleAsInvalid() | *Marks a handle as no longer used.* |

## MaaInstance : IMaaInstance

| Wrapper | Native API |
| --- | --- |
| MaaInstance | MaaCreate |
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
| IMaaInstance.Abort() | MaaStop |
| IMaaCommon.Callback | *Occurs when MaaFramework calls back.* |
| IMaaDisposable.IsInvalid | *Indicates whether the unmanaged resources from MaaFramework are invalid.* |
| IMaaDisposableHandle.Handle | *The MaaInstanceHandle.* |
| IMaaDisposableHandle.SetHandleAsInvalid() | *Marks a handle as no longer used.* |

## MaaResource : IMaaResource

| Wrapper | Native API |
| --- | --- |
| MaaResource | MaaResourceCreate |
| IDisposable.Dispose() | MaaResourceDestroy |
| IMaaResource.AppendBundle() | MaaResourcePostBundle |
| IMaaPost.SetParam() | *Invalid operation.* |
| IMaaPost.GetStatus() | MaaResourceStatus |
| IMaaPost.Wait() | MaaResourceWait |
| IMaaResource.Loaded | MaaResourceLoaded |
| IMaaOption.SetOption() | MaaResourceSetOption |
| IMaaResource.Hash | MaaResourceGetHash |
| IMaaResource.NodeList | MaaResourceGetNodeList |
| IMaaCommon.Callback | *Occurs when MaaFramework calls back.* |
| IMaaDisposable.IsInvalid | *Indicates whether the unmanaged resources from MaaFramework are invalid.* |
| IMaaDisposableHandle.Handle | *The MaaResourceHandle.* |
| IMaaDisposableHandle.SetHandleAsInvalid() | *Marks a handle as no longer used.* |

## MaaSyncContext : IMaaSyncContext

| Wrapper | Native API |
| --- | --- |
| IMaaSyncContext.Handle | *The MaaSyncContextHandle.* |
| IMaaSyncContext.RunTask() | MaaSyncContextRunTask |
| IMaaSyncContext.RunRecognizer() | MaaSyncContextRunRecognizer |
| IMaaSyncContext.RunAction() | MaaSyncContextRunAction |
| IMaaSyncContext.Click() | MaaSyncContextClick |
| IMaaSyncContext.Swipe() | MaaSyncContextSwipe |
| IMaaSyncContext.PressKey() | MaaSyncContextPressKey |
| IMaaSyncContext.InputText() | MaaSyncContextInputText |
| IMaaSyncContext.TouchDown() | MaaSyncContextTouchDown |
| IMaaSyncContext.TouchMove() | MaaSyncContextTouchMove |
| IMaaSyncContext.TouchUp() | MaaSyncContextTouchUp |
| IMaaSyncContext.Screencap() | MaaSyncContextScreencap |
| IMaaSyncContext.GetTaskResult() | MaaSyncContextGetTaskResult |

## Buffers.MaaStringBuffer : Buffers.IMaaStringBuffer

| Wrapper | Native API |
| --- | --- |
| MaaStringBuffer | MaaCreateStringBuffer |
| IDisposable.Dispose() | MaaDestroyStringBuffer |
| IMaaStringBuffer.IsEmpty | MaaIsStringEmpty |
| IMaaStringBuffer.Clear() | MaaClearString |
| IMaaStringBuffer.GetValue() <br> MaaStringBuffer.Get() | MaaGetString |
| IMaaStringBuffer.Size | MaaGetStringSize |
| IMaaStringBuffer.SetValue() <br> MaaStringBuffer.Set() | MaaSetString <br> MaaSetStringEx |
| object.ToString() | MaaGetString |
| IMaaDisposable.IsInvalid | *Indicates whether the unmanaged resources from MaaFramework are invalid.* |
| IMaaDisposableHandle.Handle | *The MaaStringBufferHandle.* |
| IMaaDisposableHandle.SetHandleAsInvalid() | *Marks a handle as no longer used.* |

## Buffers.MaaImageBuffer : Buffers.IMaaImageBuffer

| Wrapper | Native API |
| --- | --- |
| MaaImageBuffer | MaaCreateImageBuffer |
| IDisposable.Dispose() | MaaDestroyImageBuffer |
| IMaaImageBuffer.IsEmpty | MaaIsImageEmpty |
| IMaaImageBuffer.Clear() | MaaClearImage |
| IMaaImageBuffer.Info.Width | MaaGetImageWidth |
| IMaaImageBuffer.Info.Height | MaaGetImageHeight |
| IMaaImageBuffer.Info.Type | MaaGetImageType |
| IMaaImageBuffer.GetEncodedData() <br> MaaImageBuffer.Get() | MaaGetImageEncoded <br> MaaGetImageEncodedSize |
| IMaaImageBuffer.SetEncodedData() <br> MaaImageBuffer.Set() | MaaSetImageEncoded |
| IMaaDisposable.IsInvalid | *Indicates whether the unmanaged resources from MaaFramework are invalid.* |
| IMaaDisposableHandle.Handle | *The MaaImageBufferHandle.* |
| IMaaDisposableHandle.SetHandleAsInvalid() | *Marks a handle as no longer used.* |
| MaaImageBuffer.Width | MaaGetImageWidth |
| MaaImageBuffer.Height | MaaGetImageHeight |
| MaaImageBuffer.Type | MaaGetImageType |
| MaaImageBuffer.GetRawData() | MaaGetImageRawData |
| MaaImageBuffer.SetRawData() | MaaSetImageRawData |

## Buffers.MaaRectBuffer : Buffers.IMaaRectBuffer

| Wrapper | Native API |
| --- | --- |
| MaaRectBuffer | MaaCreateRectBuffer |
| IDisposable.Dispose() | MaaDestroyRectBuffer |
| IMaaRectBuffer.X | MaaGetRectX <br> MaaSetRectX |
| IMaaRectBuffer.Y | MaaGetRectY <br> MaaSetRectY |
| IMaaRectBuffer.Width | MaaGetRectW <br> MaaSetRectW |
| IMaaRectBuffer.Height | MaaGetRectH <br> MaaSetRectH |
| IMaaRectBuffer.SetValues() <br> MaaRectBuffer.Set() | MaaSetRect |
| IMaaRectBuffer.GetValues() <br> MaaRectBuffer.Get() | MaaGetRectX <br> MaaGetRectY <br> MaaGetRectW <br> MaaGetRectH |
| IMaaDisposable.IsInvalid | *Indicates whether the unmanaged resources from MaaFramework are invalid.* |
| IMaaDisposableHandle.Handle | *The MaaRectHandle.* |
| IMaaDisposableHandle.SetHandleAsInvalid() | *Marks a handle as no longer used.* |

## MaaToolkit : IMaaToolkit

| Wrapper | Native API |
| --- | --- |
| IMaaToolkit.Init() | MaaToolKitInit |
| IMaaToolkit.Uninit() | MaaToolKitUninit |
| IMaaToolkit.Find() | *The DeviceInfo Array.* |
| MaaToolkit.FindDevice() | MaaToolKitFindDevice <br> MaaToolKitFindDeviceWithAdb |
| MaaToolkit.GetDeviceName() | MaaToolKitGetDeviceName |
| MaaToolkit.GetDeviceAdbPath() | MaaToolKitGetDeviceAdbPath |
| MaaToolkit.GetDeviceAdbSerial() | MaaToolKitGetDeviceAdbSerial |
| MaaToolkit.GetDeviceAdbControllerTypes() | MaaToolKitGetDeviceAdbControllerType |
| MaaToolkit.GetDeviceAdbConfig() | MaaToolKitGetDeviceAdbConfig |
| MaaToolkit.FindWindow() | MaaToolKitFindWindow |
| MaaToolkit.SearchWindow() | MaaToolKitSearchWindow |
| MaaToolkit.GetWindow() | MaaToolKitGetWindow |
| MaaToolkit.GetCursorWindow() | MaaToolKitGetCursorWindow |
| MaaToolkit.RegisterCustomRecognizerExecutor() | MaaToolKitRegisterCustomRecognizerExecutor |
| MaaToolkit.UnregisterCustomRecognizerExecutor() | MaaToolKitUnregisterCustomRecognizerExecutor |
| MaaToolkit.RegisterCustomActionExecutor() | MaaToolKitRegisterCustomActionExecutor |
| MaaToolkit.UnregisterCustomActionExecutor() | MaaToolKitUnregisterCustomActionExecutor |

## MaaUtility : IMaaUtility

| Wrapper | Native API |
| --- | --- |
| IMaaUtility.Version | MaaVersion |
| IMaaOption.SetOption() | MaaSetGlobalOption |
