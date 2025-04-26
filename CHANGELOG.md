### NuGet Link

[![](https://img.shields.io/badge/NuGet-Maa.Framework-%23004880)](https://www.nuget.org/packages/Maa.Framework/3.0.4) [![](https://img.shields.io/badge/NuGet-Maa.Framework.Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Native/3.0.4)

[![](https://img.shields.io/badge/NuGet-Binding-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding/3.0.4) [![](https://img.shields.io/badge/NuGet-Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding.Native/3.0.4) [![](https://img.shields.io/badge/NuGet-Binding.Extensions-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding.Extensions/3.0.4)

## What's Changed in v3.0.4

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v2.3.1...v3.0.4

### Breaking changes

- [Breaking change] fix: rename to `IMaaController.IsConnected` from `LinkStop()` @moomiji
- [Breaking change] refactor: modify member naming (#8) @moomiji
- [Breaking change] refactor: buffer wrapper (#8) @moomiji
- [Breaking change] chore: update interop to framework 3.0.1 (#13) @MistEO
- feat: modify target framework net8.0 -> net9.0 @moomiji

### New features

- IMaaRectBuffer.Deconstruct() @moomiji
- NotificationHandlerRegistry @moomiji
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
