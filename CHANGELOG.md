### NuGet Link

[![](https://img.shields.io/badge/NuGet-Maa.Framework-%23004880)](https://www.nuget.org/packages/Maa.Framework/v1.8.8) [![](https://img.shields.io/badge/NuGet-Binding-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding/v1.8.8) [![](https://img.shields.io/badge/NuGet-Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding.Native/v1.8.8)

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v1.6.4...v1.8.8

## What's Changed in v1.8.8

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
