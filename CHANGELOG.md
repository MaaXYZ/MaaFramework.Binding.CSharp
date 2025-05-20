### NuGet Link

[![](https://img.shields.io/badge/NuGet-Maa.Framework-%23004880)](https://www.nuget.org/packages/Maa.Framework/4.2.0) [![](https://img.shields.io/badge/NuGet-Maa.Framework.Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Native/4.2.0) [![](https://img.shields.io/badge/NuGet-Binding-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding/4.2.0) [![](https://img.shields.io/badge/NuGet-Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding.Native/4.2.0)

## What's Changed in v4.2.0

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
