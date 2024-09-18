<div align="center">

<img alt="LOGO" src="https://cdn.jsdelivr.net/gh/MaaAssistantArknights/design@main/logo/maa-logo_512x512.png" width="256" height="256" />

# MaaFramework.Binding.CSharp

_✨ [MaaFramework](https://github.com/MaaXYZ/MaaFramework/tree/v1.6.4) 的 C# 绑定 ✨_

_💫 一个通用的可互操作 API 封装 💫_

![license](https://img.shields.io/github/license/MaaXYZ/MaaFramework) ![language](https://img.shields.io/badge/.NET-≥%207-512BD4?logo=csharp) ![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blueviolet) [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Maa.Framework?logo=nuget&color=%23004880)](https://www.nuget.org/packages/Maa.Framework)

</div>

## 文章

- [封装和 API 概述](https://maaxyz.github.io/MaaFramework.Binding.CSharp/articles/overview-of-wrapper-and-api.html)

## 快速开始

### 系统要求

在运行和使用 `MaaFramework.Binding.CSharp`  之前，您的计算机应满足最低系统要求，可能在其他未列出的平台或版本上运行。

| 操作系统版本 |                                             最低要求 / 原因                                              |
| :---: |:--------------------------------------------------------------------------------------------------:|
| Windows 10+ | 限制于 [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/supported-os.md#windows) |
| macOS 12+ |               限制于 [MaaFramework](https://github.com/MaaXYZ/MaaFramework/issues/174)                |
| Linux <br> Ubuntu 23.10+ |                                           libc6 2.38+ 等                                            |

- 架构: x64, Arm64

### 安装依赖

`MaaFramework.Binding.CSharp` 需要安装以下依赖项以正常运行。

| 平台 | 包 Id | 依赖下载 |
| :---: | :---: | :---: |
| Windows | `Maa.Framework` <br> `Maa.Framework.Binding.Native` | [Visual C++  Redistributable](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads) |

### 准备资源

请参阅 MaaFramework 文档 ([en-us](https://github.com/MaaXYZ/MaaFramework/blob/v1.4.0/docs/en_us/1.1-QuickStarted.md#prepare-resource-files) / [zh_cn](https://github.com/MaaXYZ/MaaFramework/blob/v1.4.0/docs/zh_cn/1.1-%E5%BF%AB%E9%80%9F%E5%BC%80%E5%A7%8B.md#%E5%87%86%E5%A4%87%E8%B5%84%E6%BA%90%E6%96%87%E4%BB%B6)).

如同在 MaaFramework.Binding.CSharp 的 [SampleResource](./src/MaaFramework.Binding.UnitTests/SampleResource) .

### 添加包

#### 正式版

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Maa.Framework?logo=nuget&color=%23004880)](https://www.nuget.org/packages/Maa.Framework)

``` ps1
dotnet add package Maa.Framework --prerelease
```

#### 每夜构建

从 [CI Action](https://github.com/MaaXYZ/MaaFramework.Binding.CSharp/actions/workflows/ci.yml) 下载 `nupkgs.zip` 并解压文件到 `.\nupkgs\`.

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
### 运行代码

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

#### 自定义

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

internal sealed class MyRec : IMaaCustomRecognizer
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

## 最佳实践

- [MBA](https://github.com/MaaXYZ/MBA) BA 小助手
  基于 MAA 全新架构的 BA 小助手. 图像技术 + 模拟控制，解放双手，不再点点点！由 MaaFramework 强力驱动！
- [MFAWPF](https://github.com/SweetSmellFox/MFAWPF) MFA 任务管理器
  基于 MAA 全新架构的 通用 GUI. 由 MaaFramework 强力驱动！

- 您还可以在 [单元测试](./src/MaaFramework.Binding.UnitTests) 中找到更多示例.

## 文档

我们在源代码文件中编写了详细的文档注释。

您还可以访问 [API 参考](https://maaxyz.github.io/MaaFramework.Binding.CSharp/api/MaaFramework.Binding.html) 和 [单元测试](./src/MaaFramework.Binding.UnitTests) 获取更多信息。

如果您仍然打算使用特定版本的 MaaFramework.Binding.CSharp 的 API 参考，可以参考项目的发行页面并下载附加的 docs.zip 文件。

## 贡献

我们欢迎对 MaaFramework.Binding.CSharp 的贡献。如果您发现了 bug 或有功能请求，请在 GitHub 仓库中打开一个 issue。如果您想贡献代码，可以随时 fork 仓库并提交 pull request。

## 许可证

`MaaFramework` 采用 [`LGPL-3.0`](./LICENSE.md) 许可证开源。

## 讨论

- QQ 群: 595990173
