### NuGet Link

[![](https://img.shields.io/badge/NuGet-Maa.Framework-%23004880)](https://www.nuget.org/packages/Maa.Framework/4.4.0) [![](https://img.shields.io/badge/NuGet-Maa.Framework.Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Native/4.4.0) [![](https://img.shields.io/badge/NuGet-Binding-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding/4.4.0) [![](https://img.shields.io/badge/NuGet-Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding.Native/4.4.0)

## What's Changed in v4.4.0

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v4.4.0-preview.1...v4.4.0

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
