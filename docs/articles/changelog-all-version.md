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
