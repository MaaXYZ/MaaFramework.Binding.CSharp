# v1.6.4

### NuGet Link

[![](https://img.shields.io/badge/NuGet-Maa.Framework-%23004880)](https://www.nuget.org/packages/Maa.Framework/v1.6.4) [![](https://img.shields.io/badge/NuGet-Binding-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding/v1.6.4) [![](https://img.shields.io/badge/NuGet-Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding.Native/v1.6.4) [![](https://img.shields.io/badge/NuGet-Grpc-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding.Grpc/v1.6.4)

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v1.4.0...v1.6.4

## What's Changed in v1.6.4

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

# v1.4.0

> ### ***Great refactoring***

## New features

- rename MaaToolKit.Extensions to MaaFramework.Binding.CSharp @moomiji
- `Maa.Framework.Binding` : separate abstraction & implementation @moomiji
- `Maa.Framework.Binding.Native` : add Native project & move implementation @moomiji
- `Maa.Framework.Binding.Grpc` : add Grpc interop & implementations @moomiji
- `Maa.Framework` : add Native metapackage @moomiji
- support framework `net8.0` now @moomiji

## Other

- LICENSE: Update license to LGPL v3.0 @moomiji
- update interop to framework 1.4.0 @moomiji

# v1.0.0-beta.2.1

## Breaking changes

- move namespace Extensions.ComponentModel to Extensions @moomiji

## New features

- add Maa ToolKit device section @moomiji

## Other

- Todo: MaaFramework GetHash return null @moomiji
- follow the change of MaaFramework from v0.6.0-beta.1 to v1.0.0-beta.2 @moomiji
