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
