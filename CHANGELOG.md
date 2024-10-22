### NuGet Link

[![](https://img.shields.io/badge/NuGet-Maa.Framework-%23004880)](https://www.nuget.org/packages/Maa.Framework/2.1.2) [![](https://img.shields.io/badge/NuGet-Maa.Framework.Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Native/2.1.2) [![](https://img.shields.io/badge/NuGet-Binding-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding/2.1.2) [![](https://img.shields.io/badge/NuGet-Native-%23004880)](https://www.nuget.org/packages/Maa.Framework.Binding.Native/2.1.2)

## What's Changed in v2.1.2

**Full Changelog**: https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/compare/v2.0.3...v2.1.2

### Breaking changes

- chore(UnitTests): update @moomiji
- refactor: rename to `MaaInteroperationException` from `MaaBindException` @moomiji
- chore: append `s` to `Extension` class @moomiji
- refactor: add `MaaImage` class as return value instead of generic `IMaaImageBuffer` @moomiji
- refactor: rename to `MaaMarshaller` from `MaaDefConverter` @moomiji

### New features

- add NodeDetail.QueryLatest() @moomiji
- add MaaMarshallingExtensions @moomiji
- standardized v2 interface design @moomiji
- make RecognitionDetail include `hit` @moomiji
- support ResourceOption.InferenceDevice @moomiji

### Other

- improve exception messages @moomiji
- update interop to framework 2.1.2 @moomiji
- use CustomMarshaller for string returned @moomiji
- use U1 Marshaller for MaaBool @moomiji
- use CustomMarshaller for custom controller @moomiji
