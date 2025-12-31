## v5.1.0

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v4.5.0...v5.1.0

### Breaking changes

- [Breaking change] feat: more win32 screencap methods @moomiji
- [Breaking change] chore: method ToControllerWith() instead of null check in ToController() @moomiji
- [Breaking change] feat: controller features @moomiji
- [Breaking change] refactor: split win32 mouse and keyboard @moomiji
- [Breaking change] chore: remove obsolete method @moomiji
- [Breaking change] feat: LoadPlugin() & rename IMaaUtility -> IMaaGlobal @moomiji
- [Breaking change] chore: remove MaaToolkitProjectInterface @moomiji

### New features

- feat: add Win32InputMethod (#20) @SweetSmellFox
- feat: 同步 MaaFW v5.1 接口及回调消息 (#19) @MistEO
- feat: add context param on NotificationHandler @moomiji
- feat: override image @moomiji
- feat: forward callback to the agent server @moomiji
- feat: support query action detail @moomiji

### Fix

- fix: MaaJobStatusException message formatting @moomiji
- fix: library directory resolution logic @moomiji
- fix: handle is null when add sink @moomiji

### Other

- chore: update obsolete messages @moomiji
- refactor: auto attach & detach agentClient.Dispose @moomiji
- chore: update interop to framework v5.1.4 @moomiji
- chore: fix code quality analysis warnings @moomiji
- chore: MaaAgentServer add exception document @moomiji
- refactor: callback event part.2 @moomiji
- refactor: callback event @moomiji

## v4.5.0

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v4.4.0...v4.5.0

### New features

- feat: IMaaAgentClient.DetachDisposeToResource() @moomiji
- feat: deprecated `Recording` @moomiji
- feat(Extensions): support `param` in Notification @moomiji
- refactor & feat: Controller.ClickKey() & KeyDown() & KeyUp() @moomiji

### Fix

- fix: exception occurs in Dispose() @moomiji
- fix: potential bugs in Dispose(bool disposing) @moomiji
- fix: remove saved tasker instance after released @moomiji

### Other

- chore: update interop to framework v4.5.3 @moomiji
- chore: use try-finally in ReleaseHandle()
- refactor: implementations of interface `IMaaPost` @moomiji
- refactor: MaaListBuffer.Enumerator @moomiji
- chore: resupport win-arm64 @moomiji
- chore: IMaaDisposable.Releasing use empty arg @moomiji
- [Breaking change] chore: rename event IMaaDisposable.Disposing & Disposed (4.4.1) to Releasing & Released (4.4.2) @moomiji

## v4.4.0

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v4.2.0...v4.4.0

### New features

- agent client disposure attached to resource @moomiji
- event IMaaDisposable.Disposing & Disposed @moomiji
- IMaaResource.SetInference_xxx() @moomiji
- register custom resources created by binding with new constraint @moomiji
- support fallback to use DllImportSearchPath @moomiji

### Perfect

- avoid bytes GC in MaaStringBufferSetEx() @moomiji

### Fix

- improve the usability of DefaultResolver @moomiji
- wait last job before disposing @moomiji
- handle was still used after released @moomiji

### Other

- update interop to framework 4.4.1 @moomiji

## v4.2.0

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v4.0.0...v4.2.0

### Breaking changes

- [Breaking change] fix: return value of TryGetEncodedData() using invalid handle @moomiji
- [Breaking change] chore: use IMaaTasker.Stop() instead of Abort() @moomiji
- [Breaking change] refactor: new agent client api @moomiji

### New features

- Extensions: any focus @moomiji
- IMaaResource.OverridePipeline() OverrideNext() @moomiji
- IMaaTasker.IsStopping @moomiji

### Other

- new version number definition @moomiji
- update interop to framework 4.2.0 @moomiji

## v4.0.0

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v3.0.4...v4.0.0

### Breaking changes

- [Breaking change] feat(IMaaCustomAction): add results parameter to Run method @moomiji
- [Breaking change] refactor(IMaaContext): make pipelineOverride an optional trailing parameter @moomiji
- [Breaking change] chore: improve readability for methods of MaaOptionExtensions @moomiji

### New features

- add IMaaDisposable.IsStateless @moomiji
- add `Shared` instance for toolkit & utility @moomiji
- NativeBindingContext @moomiji
- support env variable "MAAFW_BINARY_PATH" @moomiji
- add wrapper of MaaAgent @moomiji

### Other

- update Maa.AgentBinary to 1.1.0 @moomiji
- load native libraries using `ModuleInitializer` @moomiji
- update interop to framework 4.0.0 @moomiji

## v3.0.4

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v2.3.1...v3.0.4

### Breaking changes

- [Breaking change] fix: rename to `IMaaController.IsConnected` from `LinkStop()` @moomiji
- [Breaking change] refactor: modify member naming (#8) @moomiji
- [Breaking change] refactor: buffer wrapper (#8) @moomiji
- [Breaking change] chore: update interop to framework 3.0.1 (#13) @MistEO
- feat: modify target framework net8.0 -> net9.0 @moomiji

### New features

- IMaaRectBuffer.Deconstruct() @moomiji
- (Binding.Extensions) NotificationHandlerRegistry @moomiji
- identify json using StringSyntaxAttribute @moomiji

### Fix

- check for invalid MaaId (#11) @moomiji
- buffer boundary judgment @moomiji
- error entry point in MaaStringBuffer.CopyTo() @moomiji

### Other

- change invalid exception to ObjectDisposedException @moomiji
- check for validity in `DebuggerDisplay` `ToString` `IMaaPost` @moomiji
- refactor controller constructors @moomiji
- implement SRP for ReleaseHandle @moomiji
- improve README content @moomiji
- README for MaaFramework.Binding.Extensions @moomiji
- update articles @moomiji
- optimize "try" handling statements @moomiji
- "ThrowIf" extensions return valid value @moomiji
- refactor: generic Handle class @moomiji
- update interop to framework 3.0.4 @moomiji
- modify package description @moomiji

## v2.3.1

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v2.1.2...v2.3.1

### Breaking changes

- feat: add `MaaStatus` to `MaaTaskerGetTaskDetail` @moomiji
- feat: `MaaTaskerPostStop` return `MaaTaskId` @moomiji
- chore: unify namespaces `MaaFramework.Binding.Messages` to `MaaFramework.Binding.Notification` @moomiji

### New features

- support ResourceOption.InferenceExecutionProvider @moomiji
- support ControllerOption.ScreenshotUseRawSize @moomiji
- MaaWin32Controller allows hWnd to be Zero @moomiji
- add notification extensions as a new project @moomiji
- add `MaaTaskJob MaaTaskJob.WaitFor()` @moomiji
- add `Prefix` properties in MaaMsg @moomiji

### Fix

- unexpected value from ListBuffer.IsEmpty @moomiji

### Other

- update interop to framework 2.3.1 @moomiji

## v2.1.2

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v2.0.3...v2.1.2

### Breaking changes

- chore(UnitTests): update @moomiji
- refactor: rename to `MaaInteroperationException` from `MaaBindException` @moomiji
- chore: append `s` to `Extension` class @moomiji
- refactor: add `MaaImage` class as return value instead of generic `IMaaImageBuffer` @moomiji
- refactor: rename to `MaaMarshaller` from `MaaDefConverter` @moomiji

### New features

- add NodeDetail.QueryLatest() @moomiji
- add MaaMarshallingExtensions @moomiji
- standardized v2 interface design @moomiji
- make RecognitionDetail include `hit` @moomiji
- support ResourceOption.InferenceDevice @moomiji

### Other

- improve exception messages @moomiji
- update interop to framework 2.1.2 @moomiji
- use CustomMarshaller for string returned @moomiji
- use U1 Marshaller for MaaBool @moomiji
- use CustomMarshaller for custom controller @moomiji

## v2.0.3

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v1.8.9.2...v2.0.3

### New features

- support android native library @moomiji
- support platform rid differential nupkgs @moomiji
- support MaaToolkit project interface @moomiji

### Perfect

- pretty debugger display @moomiji

### Fix

- NuGet package metadata errors @moomiji

### Other

- remove custom executors @moomiji
- Create README_zh.md & add MFAWPF (#7) @SweetSmellFox
- add solution builder @moomiji
- add preview article @moomiji
- update package version @moomiji
- update interop to framework 2.0.3 @moomiji

## v1.8.9.2

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v1.8.9.1...v1.8.9.2

### New features

- add query extension for MaaTaskJob & wrap query api @moomiji

## v1.8.9.1

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v1.8.9...v1.8.9.1

### Fix

- null custom handle caused by unpinned object @moomiji
- a callback was made on a garbage collected delegate @moomiji

## v1.8.9

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v1.8.8.1...v1.8.9

### New features

- add IMaaImageBuffer.EncodedDataStream @moomiji

### Fix

- incorrect default userPath of MaaToolkit.Config.InitOption @moomiji

### Other

- update interop to framework 1.8.9 @moomiji

## v1.8.8.1

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v1.8.8...v1.8.8.1

### Fix

- [MarshalAs(UnmanagedType.LPUTF8Str)] missing in delegates @moomiji

## v1.8.8

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v1.6.4...v1.8.8

### Breaking changes

- remove interface IMaaJob & add MaaTaskJob @moomiji
- deprecated MaaFramework.Binding.Grpc @moomiji
- rename namespace Message -> Messages @moomiji

### Fix

- MaaCustomTask P/Invoke @moomiji

### Other

- use MaaXYZ/MaaApiConverter @moomiji
- update interop to framework 1.8.8 @moomiji
- remove abstract class MaaDisposable @moomiji
- update binding implementation to 1.8.7 step.3 @moomiji
- update interop to framework 1.8.7 step.2 @moomiji
- use MaaApiConverter to auto-generate interop @moomiji
- abstract out the marshaled api cache class @moomiji
- MaaCustom use interface methods to replace delegate properties @moomiji
- rename MaaAssistantArknights to MaaXYZ @moomiji
- add API Reference @moomiji

## v1.6.4

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v1.4.0...v1.6.4

### Breaking changes

- MaaToolkit follow MaaFramework structure @moomiji
- change parameter order of method DeviceInfo.ToAdbController @moomiji
- rename Custom.Api to Custom.Task @moomiji

### New features

- add default value of agentPath @moomiji
- overload method IMaaInstance.Register with IMaaCustom.Name @moomiji
- add FindAsync in IMaaToolkit @moomiji
- add Toolkit and Utility to IMaaInstance @moomiji
- MaaJobStatusException with key arguments @moomiji
- create MaaResource with `IEnumerable<string>` @moomiji

### Fix

- disable Grpc unit tests @moomiji

### Other

- transfer repository ownership to MaaXYZ @moomiji
- update interop to framework 1.6.4 @moomiji
- use NotSupportedException in Grpc @moomiji
- typo(fw): rename ToolKit -> Toolkit @moomiji

## v1.4.0

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v1.0.0-beta.2.1...v1.4.0

> #### ***Great refactoring***

### New features

- rename MaaToolKit.Extensions to MaaFramework.Binding.CSharp @moomiji
- `Maa.Framework.Binding` : separate abstraction & implementation @moomiji
- `Maa.Framework.Binding.Native` : add Native project & move implementation @moomiji
- `Maa.Framework.Binding.Grpc` : add Grpc interop & implementations @moomiji
- `Maa.Framework` : add Native metapackage @moomiji
- support framework `net8.0` now @moomiji

### Other

- LICENSE: Update license to LGPL v3.0 @moomiji
- update interop to framework 1.4.0 @moomiji

## v1.0.0-beta.2.1

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/0662a7e...v1.0.0-beta.2.1

### Breaking changes

- move namespace Extensions.ComponentModel to Extensions @moomiji

### New features

- add Maa ToolKit device section @moomiji

### Other

- Todo: MaaFramework GetHash return null @moomiji
- follow the change of MaaFramework from v0.6.0-beta.1 to v1.0.0-beta.2 @moomiji
