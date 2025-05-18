### NuGet Link

[![](https://img.shields.io/badge/NuGet-Maa.Framework-%23004880)](https://www.nuget.org/packages/Maa.Framework/4.0.0) [![](https://img.shields.io/badge/NuGet-Maa.Framework.Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Native/4.0.0) [![](https://img.shields.io/badge/NuGet-Binding-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding/4.0.0) [![](https://img.shields.io/badge/NuGet-Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding.Native/4.0.0)

## What's Changed in v4.0.0

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
