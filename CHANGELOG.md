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
