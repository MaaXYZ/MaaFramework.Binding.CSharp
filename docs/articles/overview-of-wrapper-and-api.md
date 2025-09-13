# Overview of Wrapper (v4.5.0) and Api ([v4.5.3](https://github.com/MaaXYZ/MaaFramework/tree/v4.5.3))

## Enums

> [!TIP]
> If the name of an Enum ends in the ***plural***, it means that the Enum, with **Flags**Attribute, can contain more than one kind of information through **"bitwise OR"** operation.

- Enum Type

| Wrapper | MaaDef |
| --- | --- |
| MaaJobStatus | `MaaStatusEnum` |
| LoggingLevel | `MaaLoggingLevelEnum` |
| GlobalOption | `MaaGlobalOptionEnum` |
| ResourceOption | `MaaResOptionEnum` |
| InferenceDevice | `MaaInferenceDeviceEnum` |
| InferenceExecutionProvider | `MaaInferenceExecutionProviderEnum` |
| ControllerOption | `MaaCtrlOptionEnum` |
| TaskerOption | `MaaTaskerOptionEnum` |

- Macro Definition

| Wrapper | MaaDef |
| --- | --- |
| AdbScreencapMethods | `MaaAdbScreencapMethod` |
| AdbInputMethods | `MaaAdbInputMethod` |
| Win32ScreencapMethod | `MaaWin32ScreencapMethod` |
| Win32InputMethod | `MaaWin32InputMethod` |
| DbgControllerType | `MaaDbgControllerType` |

## MaaTaskJob : MaaJob

| Wrapper | Native API |
| --- | --- |
| MaaJob.Id | *The MaaId.* |
| MaaJob.Status | `MaaControllerStatus` <br> `MaaResourceStatus` <br> `MaaTaskerStatus` |
| MaaJob.Wait() <br> MaaTaskJob.WaitFor() | `MaaControllerWait` <br> `MaaResourceWait` <br> `MaaTaskerWait` |
| MaaTaskJob.Tasker | *A property used to simplify design of TaskDetail.Query* |

## IMaaCommon

Derived:
- IMaaController
- IMaaResource
- IMaaTasker
- IMaaToolkit.PI

| Wrapper | MaaDef |
| --- | --- |
| IMaaCommon.Callback | *Occurs when MaaFramework calls back.* |

## IMaaDisposableHandle : IMaaDisposable : IDisposable

IMaaDisposable Derived:
- IMaaDisposableHandle
  - IMaaAgentClient
  - IMaaController
  - IMaaResource
  - IMaaTasker
  - IMaaRectBuffer
  - IMaaImageBuffer
  - IMaaStringBuffer
  - IMaaListBuffer
    - MaaStringListBuffer
    - AdbDeviceListBuffer
    - DesktopWindowListBuffer
    - MaaImageListBuffer
- MaaImage

| Wrapper | MaaDef |
| --- | --- |
| IDisposable.Dispose() | *Destroys the unmanaged resources.* |
| IMaaDisposable.IsInvalid | *Indicates whether the unmanaged resources from MaaFramework are invalid.* |
| IMaaDisposable.ThrowOnInvalid | *Indicates whether an exception is thrown when current resources are invalid but still called.* |
| IMaaDisposable.IsStateless | *Indicates whether the current instance is stateless.* |
| IMaaDisposableHandle.Handle | *The unmanaged resource handle.* |
| IMaaDisposableHandle.SetHandleAsInvalid() | *Marks a handle as no longer available.* <br> *If you are not sure when to call it, please use IDisposable.Dispose() instead.* |

## MaaController : IMaaController

| Wrapper | Native API |
| --- | --- |
| MaaAdbController.ctor() | `MaaAdbControllerCreate` |
| MaaWin32Controller.ctor() | `MaaWin32ControllerCreate` |
| MaaCustomController.ctor() | `MaaCustomControllerCreate` |
| MaaDbgController.ctor() | `MaaDbgControllerCreate` |
| IDisposable.Dispose() | `MaaControllerDestroy` |
| IMaaOption.SetOption() | `MaaControllerSetOption` |
| IMaaController.LinkStart() | `MaaControllerPostConnection` |
| IMaaController.Click() | `MaaControllerPostClick` |
| IMaaController.Swipe() | `MaaControllerPostSwipe` |
| IMaaController.PressKey() | `MaaControllerPostPressKey` |
| IMaaController.InputText() | `MaaControllerPostInputText` |
| IMaaController.StartApp() | `MaaControllerPostStartApp` |
| IMaaController.StopApp() | `MaaControllerPostStopApp` |
| IMaaController.TouchDown() | `MaaControllerPostTouchDown` |
| IMaaController.TouchMove() | `MaaControllerPostTouchMove` |
| IMaaController.TouchUp() | `MaaControllerPostTouchUp` |
| IMaaController.Screencap() | `MaaControllerPostScreencap` |
| IMaaPost.GetStatus() | `MaaControllerStatus` |
| IMaaPost.Wait() | `MaaControllerWait` |
| IMaaController.LinkStop() | `MaaControllerConnected` |
| IMaaController.GetCachedImage() | `MaaControllerCachedImage` |
| IMaaController.Uuid | `MaaControllerGetUuid` |
| IMaaDisposableHandle.Handle | *The MaaControllerHandle.* |

## MaaTasker : IMaaTasker

| Wrapper | Native API |
| --- | --- |
| MaaTasker.ctor() | `MaaTaskerCreate` |
| IDisposable.Dispose() | `MaaTaskerDestroy` |
| IMaaOption.SetOption() | `MaaTaskerSetOption` |
| IMaaTasker.Resource | `MaaTaskerBindResource` <br> `MaaTaskerGetResource` |
| IMaaTasker.Controller | `MaaTaskerBindController` <br> `MaaTaskerGetController` |
| IMaaTasker.Toolkit | *Easy to call Toolkit.* |
| IMaaTasker.Utility | *Easy to call Utility.* |
| IMaaTasker.IsInitialized | `MaaTaskerInited` |
| IMaaTasker.AppendTask() | `MaaTaskerPostTask` |
| IMaaPost.GetStatus() | `MaaTaskerStatus` |
| IMaaPost.Wait() | `MaaTaskerWait` |
| IMaaTasker.IsRunning | `MaaTaskerRunning` |
| IMaaTasker.Stop() | `MaaTaskerPostStop` |
| IMaaTasker.IsStopping | `MaaTaskerStopping` |
| IMaaTasker.ClearCache() | `MaaTaskerClearCache` |
| IMaaTasker.GetRecognitionDetail() <br> RecognitionDetail.Query() <br> NodeDetail.QueryRecognitionDetail() <br> TaskDetail.QueryRecognitionDetail() <br> MaaTaskJob.QueryRecognitionDetail() | `MaaTaskerGetRecognitionDetail` |
| IMaaTasker.GetNodeDetail() <br> NodeDetail.Query() <br> TaskDetail.QueryNodeDetail() <br> MaaTaskJob.QueryNodeDetail() | `MaaTaskerGetNodeDetail` |
| IMaaTasker.GetTaskDetail() <br> TaskDetail.Query() <br> MaaTaskJob.QueryTaskDetail() | `MaaTaskerGetTaskDetail` |
| IMaaTasker.GetLatestNode() <br> NodeDetail.QueryLatest() | `MaaTaskerGetLatestNode` |
| IMaaTasker.DisposeOptions | *Disposes the Resource or the Controller when Dispose() was invoked.* |
| IMaaDisposableHandle.Handle | *The MaaInstanceHandle.* |

## MaaResource : IMaaResource

| Wrapper | Native API |
| --- | --- |
| MaaResource.ctor() | `MaaResourceCreate` |
| IDisposable.Dispose() | `MaaResourceDestroy` |
| IMaaResource.Register() | `MaaResourceRegisterCustomRecognition` <br> `MaaResourceRegisterCustomAction` |
| IMaaResource.Unregister() | `MaaResourceUnregisterCustomRecognition`  <br> `MaaResourceUnregisterCustomAction` |
| IMaaResource.Clear() | `MaaResourceClearCustomRecognition`  <br> `MaaResourceClearCustomAction` <br> `MaaResourceClear` |
| IMaaResource.AppendBundle() | `MaaResourcePostBundle` |
| IMaaResource.OverridePipeline() | `MaaResourceOverridePipeline` |
| IMaaResource.OverrideNext() | `MaaResourceOverrideNext` |
| IMaaResource.GetNodeData() | `MaaResourceGetNodeData` |
| IMaaPost.GetStatus() | `MaaResourceStatus` |
| IMaaPost.Wait() | `MaaResourceWait` |
| IMaaResource.IsLoaded | `MaaResourceLoaded` |
| IMaaOption.SetOption() | `MaaResourceSetOption` |
| IMaaResource.Hash | `MaaResourceGetHash` |
| IMaaResource.NodeList | `MaaResourceGetNodeList` |
| IMaaDisposableHandle.Handle | *The MaaResourceHandle.* |

## MaaContext : IMaaContext

| Wrapper | Native API |
| --- | --- |
| IMaaContext.Handle | *The MaaContextHandle.* |
| IMaaContext.RunTask() | `MaaContextRunTask` |
| IMaaContext.RunRecognition() | `MaaContextRunRecognition` |
| IMaaContext.RunAction() | `MaaContextRunAction` |
| IMaaContext.OverridePipeline() | `MaaContextOverridePipeline` |
| IMaaContext.OverrideNext() | `MaaContextOverrideNext` |
| IMaaContext.GetNodeData() | `MaaContextGetNodeData` |
| IMaaContext.TaskJob | `MaaContextGetTaskId` |
| IMaaContext.Tasker | `MaaContextGetTasker` |
| IMaaContext.Clone() <br> ICloneable.Clone() | `MaaContextClone` |

## Buffers.MaaStringBuffer : Buffers.IMaaStringBuffer

| Wrapper | Native API |
| --- | --- |
| MaaStringBuffer.ctor() | `MaaStringBufferCreate` |
| IDisposable.Dispose() | `MaaStringBufferDestroy` |
| IMaaStringBuffer.IsEmpty | `MaaStringBufferIsEmpty` |
| IMaaStringBuffer.TryClear() | `MaaStringBufferClear` |
| IMaaStringBuffer.TryGetValue() <br> MaaStringBuffer.TryGetValue() <br> object.ToString() | `MaaStringBufferGet` |
| IMaaStringBuffer.Size | `MaaStringBufferSize` |
| IMaaStringBuffer.TrySetValue() <br> MaaStringBuffer.TrySetValue() | `MaaStringBufferSet` <br> `MaaStringBufferSetEx` |
| IMaaBuffer.TryCopyTo() | *Optimization method for copying the same type of buffer.* |
| IMaaDisposableHandle.Handle | *The MaaStringBufferHandle.* |

## Buffers.MaaStringListBuffer : Buffers.MaaListBuffer : Buffers.IMaaListBuffer

> [!TIP]
> Be aware that the underlying implementation of `MaaListBuffer` stores the values of the Buffer in a fixed, contiguous memory space, rather than storing the references of the Buffer.

| Wrapper | Native API |
| --- | --- |
| MaaStringListBuffer.ctor() | `MaaStringListBufferCreate` |
| IDisposable.Dispose() | `MaaStringListBufferDestroy` |
| IMaaListBuffer.IsEmpty | `MaaStringListBufferIsEmpty` |
| IMaaListBuffer.MaaSizeCount <br> ICollection<>.Count | `MaaStringListBufferSize` |
| IMaaListBuffer.this[] <br> IMaaListBuffer.TryIndexOf() <br> IMaaBuffer.TryCopyTo() <br> IList<>.this[] <br> IList<>.IndexOf() <br> ICollection<>.Contains() <br> ICollection<>.CopyTo() <br> ICollection<>.Remove() | `MaaStringListBufferAt` |
| IMaaListBuffer.TryAdd() <br> IMaaBuffer.TryCopyTo() <br> ICollection<>.Add() <br> ICollection<>.CopyTo() | `MaaStringListBufferAppend` |
| IMaaListBuffer.TryRemoveAt() <br> IList<>.RemoveAt() <br> ICollection<>.Remove() | `MaaStringListBufferRemove` |
| IMaaListBuffer.TryClear() <br> ICollection<>.Clear() | `MaaStringListBufferClear` |
| ICollection<>.IsReadOnly | `false` |
| IEnumerable<>.GetEnumerator() <br> IEnumerable.GetEnumerator() | *Implemented by class Buffers.MaaListBuffer.Enumerator* |
| IMaaDisposableHandle.Handle | *The MaaStringListBufferHandle.* |
| MaaStringListBuffer.TryGetList() <br> MaaStringListBuffer.TrySetList() | *Static utility methods used to avoid creating instances of the class.* |

## Buffers.MaaImageBuffer : Buffers.IMaaImageBuffer

> [!TIP]
> We designed a tool record `MaaFramework.Binding.MaaImage`, for quickly loading external images, as well as in RecognitionDetail.

| Wrapper | Native API |
| --- | --- |
| MaaImageBuffer.ctor() | `MaaImageBufferCreate` |
| IDisposable.Dispose() | `MaaImageBufferDestroy` |
| IMaaImageBuffer.IsEmpty | `MaaImageBufferIsEmpty` |
| IMaaImageBuffer.TryClear() | `MaaImageBufferClear` |
| MaaImageBuffer.TryGetRawData() | `MaaImageBufferGetRawData` |
| MaaImageBuffer.TrySetRawData() | `MaaImageBufferSetRawData` |
| IMaaImageBuffer.GetInfo() <br> MaaImageBuffer.Width <br> MaaImageBuffer.Height <br> MaaImageBuffer.Channels <br> MaaImageBuffer.Type | `MaaImageBufferWidth` <br> `MaaImageBufferHeight` <br> `MaaImageBufferChannels` <br> `MaaImageBufferType` |
| IMaaImageBuffer.TryGetEncodedData() <br> MaaImageBuffer.TryGetEncodedData() | `MaaImageBufferGetEncoded` <br> `MaaImageBufferGetEncodedSize` |
| IMaaImageBuffer.TrySetEncodedData() <br> MaaImageBuffer.TrySetEncodedData() | `MaaImageBufferSetEncoded` |
| IMaaBuffer.TryCopyTo() | *Optimization method for copying the same type of buffer.* |
| IMaaDisposableHandle.Handle | *The MaaImageBufferHandle.* |

## Buffers.MaaImageListBuffer : Buffers.MaaListBuffer : Buffers.IMaaListBuffer

> [!TIP]
> Be aware that the underlying implementation of `MaaListBuffer` stores the values of the Buffer in a fixed, contiguous memory space, rather than storing the references of the Buffer.

> [!IMPORTANT]
> Please note that after using the `Clear`, `Remove`, or `Dispose` methods, all (or part, if using `Remove`) `MaaImageBuffer` instances will become unavailable.
>
> This is because the image types in CSharp have different implementations depending on the chosen framework. Therefore, `MaaImageListBuffer` is based on `MaaImageBuffer` and borrows the memory space from `MaaFramework`.
>
> This means that the CSharp Binding violates the design principles of MaaFramework Binding, but it avoids additional space and time consumption.

| Wrapper | Native API |
| --- | --- |
| MaaImageListBuffer.ctor() | `MaaImageListBufferCreate` |
| IDisposable.Dispose() | `MaaImageListBufferDestroy` |
| IMaaListBuffer.IsEmpty | `MaaImageListBufferIsEmpty` |
| IMaaListBuffer.MaaSizeCount <br> ICollection<>.Count | `MaaImageListBufferSize` |
| IMaaListBuffer.this[] <br> IMaaListBuffer.TryIndexOf() <br> IMaaBuffer.TryCopyTo() <br> IList<>.this[] <br> IList<>.IndexOf() <br> ICollection<>.Contains() <br> ICollection<>.CopyTo() <br> ICollection<>.Remove() | `MaaImageListBufferAt` |
| IMaaListBuffer.TryAdd() <br> IMaaBuffer.TryCopyTo() <br> ICollection<>.Add() <br> ICollection<>.CopyTo() | `MaaImageListBufferAppend` |
| IMaaListBuffer.TryRemoveAt() <br> IList<>.RemoveAt() <br> ICollection<>.Remove() | `MaaImageListBufferRemove` |
| IMaaListBuffer.TryClear() <br> ICollection<>.Clear() | `MaaImageListBufferClear` |
| ICollection<>.IsReadOnly | `false` |
| IEnumerable<>.GetEnumerator() <br> IEnumerable.GetEnumerator() | *Implemented by class Buffers.MaaListBuffer.Enumerator* |
| IMaaDisposableHandle.Handle | *The MaaImageListBufferHandle.* |
| MaaImageListBuffer.TryGetEncodedDataList() <br> MaaImageListBuffer.TryGetEncodedDataList() | *Static utility methods used to avoid creating instances of the class.* |

## Buffers.MaaRectBuffer : Buffers.IMaaRectBuffer

| Wrapper | Native API |
| --- | --- |
| MaaRectBuffer.ctor() | `MaaRectCreate` |
| IDisposable.Dispose() | `MaaRectDestroy` |
| IMaaRectBuffer.X <br> IMaaRectBuffer.Y <br> IMaaRectBuffer.Width <br> IMaaRectBuffer.Height <br> IMaaRectBuffer.TryGetValues() <br> IMaaRectBuffer.GetValues() <br> MaaRectBuffer.TryGetValues() <br> MaaRectBuffer.GetValues() | `MaaGetRectX` <br> `MaaGetRectY` <br> `MaaGetRectW` <br> `MaaGetRectH` |
| IMaaRectBuffer.TrySetValues() <br> MaaRectBuffer.TrySetValues() | `MaaRectSet` |
| IMaaBuffer.TryCopyTo() | *Optimization method for copying the same type of buffer.* |
| IMaaDisposableHandle.Handle | *The MaaRectHandle.* |

## MaaUtility : IMaaUtility

| Wrapper | Native API |
| --- | --- |
| IMaaUtility.Version | `MaaVersion` |
| IMaaOption.SetOption() | `MaaSetGlobalOption` |

## MaaToolkit : IMaaToolkit

| Wrapper | Native API |
| --- | --- |
| MaaToolkit.ctor() <br> IMaaToolkit.Config.InitOption() | `MaaToolkitConfigInitOption` |
| IMaaToolkit.AdbDevice.Find() <br> IMaaToolkit.AdbDevice.FindAsync() | `MaaToolkitAdbDeviceFind` <br> `MaaToolkitAdbDeviceFindSpecified` |
| IMaaToolkit.Desktop.Window.Find() | `MaaToolkitDesktopWindowFindAll` |
| IMaaToolkit.PI.Register() | `MaaToolkitProjectInterfaceRegisterCustomAction` <br> `MaaToolkitProjectInterfaceRegisterCustomRecognition` |
| IMaaToolkit.PI.RunCli() | `MaaToolkitProjectInterfaceRunCli` |
| IMaaToolkit.PI.this[] | *Gets or creates a PI instance.* |

## Buffers.AdbDeviceListBuffer : Buffers.MaaListBuffer : Buffers.IMaaListBuffer

> [!TIP]
> Be aware that the underlying implementation of `MaaListBuffer` stores the values of the Buffer in a fixed, contiguous memory space, rather than storing the references of the Buffer.

| Wrapper | Native API |
| --- | --- |
| AdbDeviceListBuffer.MaaToolkitAdbDeviceInfo <br>  : AdbDeviceInfo | `MaaToolkitAdbDeviceGetName` <br> `MaaToolkitAdbDeviceGetAdbPath` <br> `MaaToolkitAdbDeviceGetAddress` <br> `MaaToolkitAdbDeviceGetScreencapMethods` <br> `MaaToolkitAdbDeviceGetInputMethods` <br> `MaaToolkitAdbDeviceGetConfig` |
| AdbDeviceListBuffer.ctor() | `MaaToolkitAdbDeviceListCreate` |
| IDisposable.Dispose() | `MaaToolkitAdbDeviceListDestroy` |
| IMaaListBuffer.IsEmpty <br> IMaaListBuffer.MaaSizeCount <br> ICollection<>.Count | `MaaToolkitAdbDeviceListSize` |
| IMaaListBuffer.this[] <br> IMaaListBuffer.TryIndexOf() <br> IList<>.this[] <br> IList<>.IndexOf() <br> ICollection<>.Contains() | `MaaToolkitAdbDeviceListAt` |
| ICollection<>.IsReadOnly | `true` |
| IEnumerable<>.GetEnumerator() <br> IEnumerable.GetEnumerator() | *Implemented by class Buffers.MaaListBuffer.Enumerator* |
| IMaaDisposableHandle.Handle | *The MaaStringListBufferHandle.* |
| AdbDeviceListBuffer.TryGetList() | *Static utility methods used to avoid creating instances of the class.* |

## Buffers.DesktopWindowListBuffer : Buffers.MaaListBuffer : Buffers.IMaaListBuffer

> [!TIP]
> Be aware that the underlying implementation of `MaaListBuffer` stores the values of the Buffer in a fixed, contiguous memory space, rather than storing the references of the Buffer.

| Wrapper | Native API |
| --- | --- |
| DesktopWindowListBuffer.MaaToolkitDesktopWindowInfo <br>  : DesktopWindowInfo | `MaaToolkitDesktopWindowGetHandle` <br> `MaaToolkitDesktopWindowGetClassName` <br> `MaaToolkitDesktopWindowGetWindowName` |
| DesktopWindowListBuffer.ctor() | `MaaToolkitDesktopWindowListCreate` |
| IDisposable.Dispose() | `MaaToolkitDesktopWindowListDestroy` |
| IMaaListBuffer.IsEmpty <br> IMaaListBuffer.MaaSizeCount <br> ICollection<>.Count | `MaaToolkitDesktopWindowListSize` |
| IMaaListBuffer.this[] <br> IMaaListBuffer.TryIndexOf() <br> IList<>.this[] <br> IList<>.IndexOf() <br> ICollection<>.Contains() | `MaaToolkitDesktopWindowListAt` |
| ICollection<>.IsReadOnly | `true` |
| IEnumerable<>.GetEnumerator() <br> IEnumerable.GetEnumerator() | *Implemented by class Buffers.MaaListBuffer.Enumerator* |
| IMaaDisposableHandle.Handle | *The MaaStringListBufferHandle.* |
| DesktopWindowListBuffer.TryGetList() | *Static utility methods used to avoid creating instances of the class.* |

## MaaAgentClient : IMaaAgentClient

| Wrapper | Native API |
| --- | --- |
| MaaAgentClient.Create() | `MaaAgentClientCreateV2` |
| IDisposable.Dispose() | `MaaAgentClientDestroy` |
| IMaaAgentClient.Id | `MaaAgentClientIdentifier` |
| IMaaAgentClient.Resource | `MaaAgentClientBindResource` |
| IMaaAgentClient.LinkStart() <br> IMaaAgentClient.LinkStartUnlessProcessExit() | `MaaAgentClientConnect` |
| IMaaAgentClient.LinkStop() | `MaaAgentClientDisconnect` |
| IMaaAgentClient.IsConnected | `MaaAgentClientConnected` |
| IMaaAgentClient.IsAlive | `MaaAgentClientAlive` |
| IMaaAgentClient.SetTimeout() <br> IMaaAgentClient.Cancel() <br> IMaaAgentClient.CancelWith() | `MaaAgentClientSetTimeout` |
| IMaaAgentClient.AgentServerProcess | *A process created by LinkStart(), whose lifecycle is managed by the current class.* |
| IMaaAgentClient.AgentServerStartupMethod | *A delegate used to start the agent server process.* |
| MaaDisposableHandle.Handle | *The MaaAgentClientHandle.* |

## MaaAgentServer : IMaaAgentServer

| Wrapper | Native API |
| --- | --- |
| MaaAgentServer.CurrentId <br> IMaaAgentServer.WithIdentifier() | *Used to get and modify the identifier.* |
| IMaaAgentServer.Register() | `MaaAgentServerRegisterCustomRecognition` <br> `MaaAgentServerRegisterCustomAction` |
| IMaaAgentServer.StartUp() | `MaaAgentServerStartUp` |
| IMaaAgentServer.ShutDown() | `MaaAgentServerShutDown` |
| IMaaAgentServer.Join() | `MaaAgentServerJoin` |
| IMaaAgentServer.Detach() | `MaaAgentServerDetach` |
