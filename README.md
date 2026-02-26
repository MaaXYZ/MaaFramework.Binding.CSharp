<div align="center">

<img alt="LOGO" src="https://cdn.jsdelivr.net/gh/MaaAssistantArknights/design@main/logo/maa-logo_512x512.png" width="256" height="256" />

# MaaFramework.Binding.CSharp

_✨ C# Binding for [MaaFramework](https://github.com/MaaXYZ/MaaFramework/blob/v5.6.0) ✨_

_💫 A universal interop API wrapper 💫_

![license](https://img.shields.io/github/license/MaaXYZ/MaaFramework) ![language](https://img.shields.io/badge/.NET-≥%207-512BD4?logo=csharp) ![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blueviolet) [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Maa.Framework?logo=nuget&color=%23004880)](https://www.nuget.org/packages/Maa.Framework)

[English](./README.md) | [简体中文](./README.zh_cn.md)

</div>

## Articles
- [Overview of Wrapper and Api](https://maaxyz.github.io/MaaFramework.Binding.CSharp/articles/overview-of-wrapper-and-api.html)

## Quick Start

### System Requirements

Your system must meet minimum requirements to use `MaaFramework.Binding.CSharp`. The framework may work on other platforms not listed here.

| OS Version | Minimum Requirement / Limitation |
| :---: | :---: |
| Windows 10+ | Limited by [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/supported-os.md#windows) |
| macOS 12+ | Limited by [MaaFramework](https://github.com/MaaXYZ/MaaFramework/issues/174) |
| Linux <br> Ubuntu 23.10+ | libc6 2.38+ required <br> and more |
| Android | Untested |

- Architecture: x64, arm64

### Dependencies

Required dependencies for `MaaFramework.Binding.CSharp`:

| Platform | Package ID | Dependency Link |
| :---: | :---: | :---: |
| Windows | `Maa.Framework` <br> `Maa.Framework.Runtimes` <br> `Maa.Framework.Runtime.win-arm64` <br> `Maa.Framework.Runtime.win-x64` | [Visual C++  Redistributable](https://learn.microsoft.com/en-us/cpp/windows/latest-supported-vc-redist) |

### Resource Setup

Refer to MaaFramework docs: [English](https://github.com/MaaXYZ/MaaFramework/blob/v5.6.0/docs/en_us/1.1-QuickStarted.md#prepare-resource-files) | [简体中文](https://github.com/MaaXYZ/MaaFramework/blob/v5.6.0/docs/zh_cn/1.1-快速开始.md#准备资源文件)

See [SampleResource](./src/MaaFramework.Binding.UnitTests/SampleResource) for implementation.

### Package Installation

| Package ID | Description |
| :---- | :---- |
| Maa.Framework | Meta package (Native + Runtimes) |
| Maa.Framework.Native | Meta package (Binding.Native + AgentBinary) |
| Maa.Framework.Binding | Abstraction layer |
| Maa.Framework.Binding.Native | Native API wrapper |
| Maa.Framework.Runtimes <br> Platform-specific packages | Prebuilt [MaaFramework](https://github.com/MaaXYZ/MaaFramework) binaries.<br>Android packages excluded by default. |
| Maa.AgentBinary | Prebuilt [Agent binaries](https://github.com/MaaXYZ/MaaAgentBinary) |

#### Stable Releases

```ps1
dotnet add package Maa.Framework --prerelease
```

#### Nightly Builds

- **Add package**

  ```ps1
  dotnet add package Maa.Framework --prerelease -s https://maaxyz.github.io/pkg/nuget/index.json
  ```

- **Configure NuGet sources**

  - .csproj
    ```xml
    <PropertyGroup>
    <RestoreSources>$(RestoreSources);https://api.nuget.org/v3/index.json;https://maaxyz.github.io/pkg/nuget/index.json</RestoreSources>
    </PropertyGroup>
    ```

  - NuGet.config

    See [sample configuration](./NuGet.config) for implementation examples; Refer to [nightly builds guide](https://maaxyz.github.io/MaaFramework.Binding.CSharp/articles/nightly-builds.html) for full details.

#### RID Specification

Supported Runtime IDs are listed [here](https://github.com/MaaXYZ/MaaFramework/tree/main/tools/nupkgs).

For example, on the Windows platform, you need to manually reference the following packages:
- `Maa.Framework.Native`
- `Maa.Framework.Runtime.win-arm64`
- `Maa.Framework.Runtime.win-x64`

### Code Example

> Pre-work: `adb connect HOST[:PORT]`

Start quickly from the [powershell](sample\csharp\QuickStart.ps1) or [bash](sample\csharp\QuickStart.sh) script in the sample folder.

```csharp
// using MaaFramework.Binding;

MaaToolkit.Shared.Config.InitOption(".cache");

var devices = MaaToolkit.Shared.AdbDevice.Find();
if (devices.IsEmpty)
    throw new InvalidOperationException();

using var maa = new MaaTasker
{
    Controller = devices[0].ToAdbController(),
    Resource = new MaaResource("../../src/MaaFramework.Binding.UnitTests/SampleResource"),
    DisposeOptions = DisposeOptions.All,
};

if (!maa.IsInitialized)
    throw new InvalidOperationException();

maa.AppendTask("EmptyNode")
   .Wait()
   .ThrowIfNot(MaaJobStatus.Succeeded);

Console.WriteLine("EmptyNode Completed");
```

#### Custom

```csharp
// using MaaFramework.Binding.Buffers;
// using MaaFramework.Binding.Custom;

var nodeName = "MyCustomTask";
var param = $$"""
{
  "{{nodeName}}": {
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

// Register custom components
maa.Resource.Register(new MyRec());
maa.Resource.Register(new MyAct());
maa.AppendTask(nodeName, param)
    .Wait()
    .ThrowIfNot(MaaJobStatus.Succeeded);

internal sealed class MyRec : IMaaCustomRecognition
{
    public string Name { get; set; } = nameof(MyRec);
    public bool Analyze(in IMaaContext context, in AnalyzeArgs args, in AnalyzeResults results)
    {
        Console.WriteLine($"Enter {Name}");
        return results.Box.TrySetValues(0, 0, 100, 100)
            && results.Detail.TrySetValue("Hello World!");
    }
}
internal sealed class MyAct : IMaaCustomAction
{
    public string Name { get; set; } = nameof(MyAct);
    public bool Run(in IMaaContext context, in RunArgs args, in RunResults results)
    {
        Console.WriteLine($"Enter {Name}");
        return true;
    }
}
```

#### Additional Resources
- To view implementation examples with identical functionality to the main MaaFramework repository, visit the [Sample Directory](./sample).
- For code examples covering the vast majority of framework features, explore the [Unit Test Suite](./src/MaaFramework.Binding.UnitTests).

## Best Practices
- [MFAWPF](https://github.com/SweetSmellFox/MFAWPF) MFA Task Manager
  A Universal GUI based on MAA's new architecture. Powered by MaaFramework.

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
