<div align="center">

<img alt="LOGO" src="https://cdn.jsdelivr.net/gh/MaaAssistantArknights/design@main/logo/maa-logo_512x512.png" width="256" height="256" />

# MaaFramework.Binding.CSharp

_✨ The csharp binding of [MaaFramework](https://github.com/MaaAssistantArknights/MaaFramework/tree/v1.4.0) ✨_

_💫 A common interoperable API wrapper 💫_

![license](https://img.shields.io/github/license/MaaAssistantArknights/MaaFramework) ![language](https://img.shields.io/badge/.NET-≥%207-512BD4?logo=csharp) ![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blueviolet) [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Maa.Framework?logo=nuget&color=%23004880)](https://www.nuget.org/packages/Maa.Framework)

</div>

## Wiki

- [Overview of Wrapper and Api](https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/wiki#overview-of-wrapper-and-api)

## Get Started

### Install Dependents

`MaaFramework.Binding.CSharp` needs the following dependencies installed to run properly.

| Platform | Package Id | Dependent Download |
| :---: | :---: | :---: |
| Windows | `Maa.Framework` <br> `Maa.Framework.Binding.Native` | [Visual C++  Redistributable](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads) |

### Prepare Resources

See MaaFramework document ([en-us](https://github.com/MaaAssistantArknights/MaaFramework/blob/v1.4.0/docs/en_us/1.1-QuickStarted.md#prepare-resource-files) / [zh_cn](https://github.com/MaaAssistantArknights/MaaFramework/blob/v1.4.0/docs/zh_cn/1.1-%E5%BF%AB%E9%80%9F%E5%BC%80%E5%A7%8B.md#%E5%87%86%E5%A4%87%E8%B5%84%E6%BA%90%E6%96%87%E4%BB%B6)).

Like this [SampleResource](./src/MaaFramework.Binding.UnitTests/SampleResource) in MaaFramework.Binding.CSharp.

### Run Code

> Pre-work: `adb connect HOST[:PORT]`

```CSharp
using MaaFramework.Binding;

var maaTookit = new MaaToolkit();
var devices = maaTookit.Find();
if (devices.Length < 1 || !maaTookit.Init())
{
    throw new InvalidOperationException();
}

using var maa = new MaaInstance
{
                                    // From package Maa.Framework
    Controller = devices[0].ToAdbController("./MaaAgentBinary"),
    Resource = new MaaResource("./SampleResource"),
    DisposeOptions = DisposeOptions.All,
};

if (!maa.Initialized)
{
    throw new InvalidOperationException();
}

maa.AppendTask("EmptyTask")
   .Wait()
   .ThrowIfNot(MaaJobStatus.Success);

Console.WriteLine("EmptyTask Completed");
maaTookit.Uninit();
```

## Best Practices

- [MBA](https://github.com/MaaAssistantArknights/MBA) BA Assistant  
  A BA Assistant based on MAA's new architecture. Image technology + simulation control, no more clicking! Powered by MaaFramework.

- You can also find more examples in the [unit tests](./src/MaaFramework.Binding.UnitTests).

## License

`MaaFramework` is open-sourced under the [`LGPL-3.0`](./LICENSE.md) license.

## Discussion

- QQ Group: 595990173
