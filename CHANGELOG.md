### NuGet Link

[![](https://img.shields.io/badge/NuGet-Maa.Framework-%23004880)](https://www.nuget.org/packages/Maa.Framework/5.1.0) [![](https://img.shields.io/badge/NuGet-Maa.Framework.Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Native/5.1.0) [![](https://img.shields.io/badge/NuGet-Binding-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding/5.1.0) [![](https://img.shields.io/badge/NuGet-Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding.Native/5.1.0)

## What's Changed in v5.1.0

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v4.5.0...v5.1.0

### Breaking changes

- [Breaking change] feat: more win32 screencap methods @moomiji
- [Breaking change] chore: method ToControllerWith() instead of null check in ToController() @moomiji
- [Breaking change] feat: controller features @moomiji
- [Breaking change] refactor: split win32 mouse and keyboard @moomiji
- [Breaking change] chore: remove obsolete method @moomiji
- [Breaking change] feat: LoadPlugin() & rename IMaaUtility -> IMaaGlobal @moomiji
- [Breaking change] chore: remove MaaToolkitProjectInterface @moomiji

### New features

- feat: add Win32InputMethod (#20) @SweetSmellFox
- feat: 同步 MaaFW v5.1 接口及回调消息 (#19) @MistEO
- feat: add context param on NotificationHandler @moomiji
- feat: override image @moomiji
- feat: forward callback to the agent server @moomiji
- feat: support query action detail @moomiji

### Fix

- fix: MaaJobStatusException message formatting @moomiji
- fix: library directory resolution logic @moomiji
- fix: handle is null when add sink @moomiji

### Other

- chore: update obsolete messages @moomiji
- refactor: auto attach & detach agentClient.Dispose @moomiji
- chore: update interop to framework v5.1.4 @moomiji
- chore: fix code quality analysis warnings @moomiji
- chore: MaaAgentServer add exception document @moomiji
- refactor: callback event part.2 @moomiji
- refactor: callback event @moomiji
