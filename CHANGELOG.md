### NuGet Link

[![](https://img.shields.io/badge/NuGet-Maa.Framework-%23004880)](https://www.nuget.org/packages/Maa.Framework/2.3.1) [![](https://img.shields.io/badge/NuGet-Maa.Framework.Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Native/2.3.1) [![](https://img.shields.io/badge/NuGet-Binding-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding/2.3.1) [![](https://img.shields.io/badge/NuGet-Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding.Native/2.3.1)

## What's Changed in v2.3.1

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
