<div align="center">

<img alt="LOGO" src="https://cdn.jsdelivr.net/gh/MaaAssistantArknights/design@main/logo/maa-logo_512x512.png" width="256" height="256" />

# MaaFramework.Binding.CSharp

_✨ The csharp binding of [MaaFramework](https://github.com/MaaXYZ/MaaFramework/blob/v2.0.1) ✨_

_💫 A common interoperable API wrapper 💫_

![license](https://img.shields.io/github/license/MaaXYZ/MaaFramework) ![language](https://img.shields.io/badge/.NET-≥%207-512BD4?logo=csharp) ![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blueviolet) [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Maa.Framework?logo=nuget&color=%23004880)](https://www.nuget.org/packages/Maa.Framework)

[English](./README.md) | [简体中文](./README_zh.md)

</div>

## Articles

- [Overview of Wrapper and Api](https://maaxyz.github.io/MaaFramework.Binding.CSharp/articles/overview-of-wrapper-and-api.html)

## Get Started

### System Requirements

Your computer should meet the minimum system requirements before you run and use `MaaFramework.Binding.CSharp`, which might run on other platforms or versions not listed here.

| OS Version | Minimum Requirements / Reason |
| :---: | :---: |
| Windows 10+ | Restricted from [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/supported-os.md#windows) |
| macOS 12+ | Restricted from [MaaFramework](https://github.com/MaaXYZ/MaaFramework/issues/174) |
| Linux <br> Ubuntu 23.10+ | libc6 2.38+ <br> and more |
| Android | Unknown |

- Architectures: X64, Arm64

### Install Dependents

`MaaFramework.Binding.CSharp` needs the following dependencies installed to run properly.

| Platform | Package Id | Dependent Download |
| :---: | :---: | :---: |
| Windows | `Maa.Framework` <br> `Maa.Framework.Runtimes` <br> `Maa.Framework.Runtime.win-arm64` <br> `Maa.Framework.Runtime.win-x64` | [Visual C++  Redistributable](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads) |

### Prepare Resources

See MaaFramework document ( [English](https://github.com/MaaXYZ/MaaFramework/blob/v2.0.1/docs/en_us/1.1-QuickStarted.md#prepare-resource-files) | [简体中文](https://github.com/MaaXYZ/MaaFramework/blob/v2.0.1/docs/zh_cn/1.1-%E5%BF%AB%E9%80%9F%E5%BC%80%E5%A7%8B.md#%E5%87%86%E5%A4%87%E8%B5%84%E6%BA%90%E6%96%87%E4%BB%B6) ) .

Like this [SampleResource](./src/MaaFramework.Binding.UnitTests/SampleResource) in `MaaFramework.Binding.CSharp`.

### Add Packages

| Package Id | Descriptions |
| :---- | :---- |
| Maa.Framework | A metapackage with references to Native, Runtimes. |
| Maa.Framework.Native | A metapackage with references to Binding.Native, AgentBinary. |
| Maa.Framework.Binding | The abstractions of binding. |
| Maa.Framework.Binding.Native | The native api wrapper implementation of binding.|
| Maa.Framework.Runtimes <br> Maa.Framework.Runtime.win-x64 <br> Maa.Framework.Runtime.win-arm64 <br> Maa.Framework.Runtime.linux-x64 <br> Maa.Framework.Runtime.linux-arm64 <br> Maa.Framework.Runtime.osx-x64  <br> Maa.Framework.Runtime.osx-arm64 <br> Maa.Framework.Runtime.android-x64 <br> Maa.Framework.Runtime.android-arm64 | Native binaries of [MaaFramework](https://github.com/MaaXYZ/MaaFramework). <br> **No** reference to android packages in Runtimes. |
| Maa.AgentBinary | Pre-built [agent](https://github.com/MaaXYZ/MaaAgentBinary) binaries, including minitouch, maatouch and minicap. |

#### Release

``` ps1
dotnet add package Maa.Framework --prerelease
```

#### Nightly Build

Download `nupkgs.zip` from [CI Action](https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/actions/workflows/ci.yml) and extract files to `.\nupkgs\`.

- shell
``` ps1
dotnet add package Maa.Framework --prerelease -s .\nupkgs\
```

- or .csproj
``` xml
  <PropertyGroup>
    <RestoreSources>$(RestoreSources);$(FullPath of .\nupkgs\)</RestoreSources>
  </PropertyGroup>
```

#### Specifying RIDs

The supported Runtime IDs for `MaaFramework` can be found [here](https://github.com/MaaXYZ/MaaFramework/tree/main/tools/nupkgs).

To use specific RIDs, such as the supported `win` platform packages, manually reference the following packages:
- `Maa.Framework.Native`
- `Maa.Framework.Runtime.win-arm64`
- `Maa.Framework.Runtime.win-x64`

### Run Code

> Pre-work: `adb connect HOST[:PORT]`

```CSharp
using MaaFramework.Binding;

var devices = new MaaToolkit(true).Device.Find();
if (devices.Length < 1)
    throw new InvalidOperationException();

using var maa = new MaaInstance
{
    Controller = devices[0].ToAdbController(),
    Resource = new MaaResource("./SampleResource"),
    DisposeOptions = DisposeOptions.All,
};

if (!maa.Initialized)
    throw new InvalidOperationException();

maa.AppendTask("EmptyTask")
   .Wait()
   .ThrowIfNot(MaaJobStatus.Success);

Console.WriteLine("EmptyTask Completed");
```

#### Custom

```CSharp
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;

var taskName = "MyCustomTask";
var param = $$"""
{
  "{{taskName}}": {
      "recognition": "Custom",
      "custom_recognition": "MyRec",
      "custom_recognition_param": {
          "my_rec_key": "my_rec_value"
      },
      "action": "Custom",
      "custom_action": "MyAct",
      "custom_action_param": {
          "my_act_key": "my_act_value"
      }
  }
}
""";

maa.Register(new MyRec());
maa.Register(new MyAct());
maa.AppendTask(taskName, param)
    .Wait()
    .ThrowIfNot(MaaJobStatus.Success);

internal sealed class MyRec : IMaaCustomRecognition
{
    public string Name { get; set; } = nameof(MyRec);

    public bool Analyze(in IMaaSyncContext syncContext, IMaaImageBuffer image, string taskName, string customRecognitionParam, in IMaaRectBuffer outBox, in IMaaStringBuffer outDetail)
    {
        outBox.SetValues(0, 0, 100, 100);
        outDetail.SetValue("Hello World!");
        return true;
    }
}

internal sealed class MyAct : IMaaCustomAction
{
    public string Name { get; set; } = nameof(MyAct);

    public void Abort() { }

    public bool Run(in IMaaSyncContext syncContext, string taskName, string customActionParam, IMaaRectBuffer curBox, string curRecDetail)
    {
        return true;
    }
}
```

## Best Practices

- [MBA](https://github.com/MaaXYZ/MBA) BA Assistant
  A BA Assistant based on MAA's new architecture. Image technology + simulation control, no more clicking! Powered by MaaFramework.
- [MFAWPF](https://github.com/SweetSmellFox/MFAWPF) MFA Task Manager
  A Universal GUI based on MAA's new architecture. Powered by MaaFramework.

- You can also find more examples in the [Unit Tests](./src/MaaFramework.Binding.UnitTests).

## Documentation

We have written detailed documentation comments in source code files.

You can also visit [API Reference](https://maaxyz.github.io/MaaFramework.Binding.CSharp/api/MaaFramework.Binding.html) and [Unit Tests](./src/MaaFramework.Binding.UnitTests) for more information.

If you still intend to use a API Reference specific to your preferred version of MaaFramework.Binding.CSharp, you may refer to the releases page of the project and download the attached `docs.zip` file.

## Contributing

We welcome contributions to the MaaFramework.Binding.CSharp. If you find a bug or have a feature request, please open an issue on the GitHub repository. If you want to contribute code, feel free to fork the repository and submit a pull request.

## License

`MaaFramework` is open-sourced under the [`LGPL-3.0`](./LICENSE.md) license.

## Discussion

- QQ Group: 595990173
