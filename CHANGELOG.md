### NuGet Link

[![](https://img.shields.io/badge/NuGet-Maa.Framework-%23004880)](https://www.nuget.org/packages/Maa.Framework/4.5.0) [![](https://img.shields.io/badge/NuGet-Maa.Framework.Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Native/4.5.0) [![](https://img.shields.io/badge/NuGet-Binding-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding/4.5.0) [![](https://img.shields.io/badge/NuGet-Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding.Native/4.5.0)

## What's Changed in v4.5.0

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
