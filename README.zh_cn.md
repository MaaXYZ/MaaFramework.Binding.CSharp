<div align="center">

<img alt="LOGO" src="https://cdn.jsdelivr.net/gh/MaaAssistantArknights/design@main/logo/maa-logo_512x512.png" width="256" height="256" />

# MaaFramework.Binding.CSharp

_✨ [MaaFramework](https://github.com/MaaXYZ/MaaFramework/blob/v4.0.0) 的 C# 绑定 ✨_

_💫 一个通用的互操作 API 封装 💫_

![license](https://img.shields.io/github/license/MaaXYZ/MaaFramework) ![language](https://img.shields.io/badge/.NET-≥%207-512BD4?logo=csharp) ![platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-blueviolet) [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Maa.Framework?logo=nuget&color=%23004880)](https://www.nuget.org/packages/Maa.Framework)

[English](./README.md) | [简体中文](./README.zh_cn.md)

</div>

## 文章
- [封装与 API 一览](https://maaxyz.github.io/MaaFramework.Binding.CSharp/articles/overview-of-wrapper-and-api.html)

## 快速入门

### 系统要求

使用 `MaaFramework.Binding.CSharp` 必须满足最低系统要求。以下未列出的平台可能也可运行。

| 操作系统版本 | 最低要求 / 受限原因 |
| :---: | :---: |
| Windows 10+ | 受限于 [.NET 7.0](https://github.com/dotnet/core/blob/main/release-notes/7.0/supported-os.md#windows) |
| macOS 12+ | 受限于 [MaaFramework](https://github.com/MaaXYZ/MaaFramework/issues/174) |
| Linux <br> Ubuntu 23.10+ | 需要 libc6 2.38+ <br> 及其他依赖 |
| Android | 未经测试 |

- 架构支持：x64, arm64

### 依赖项

`MaaFramework.Binding.CSharp` 的必需依赖项：

| 平台 | 包 ID | 依赖链接 |
| :---: | :---: | :---: |
| Windows | `Maa.Framework` <br> `Maa.Framework.Runtimes` <br> `Maa.Framework.Runtime.win-arm64` <br> `Maa.Framework.Runtime.win-x64` | [Visual C++ 可再发行程序包](https://learn.microsoft.com/zh-cn/cpp/windows/latest-supported-vc-redist) |

### 资源准备

参考 MaaFramework 文档：[English](https://github.com/MaaXYZ/MaaFramework/blob/v4.0.0/docs/en_us/1.1-QuickStarted.md#prepare-resource-files) | [简体中文](https://github.com/MaaXYZ/MaaFramework/blob/v4.0.0/docs/zh_cn/1.1-快速开始.md#准备资源文件)

实现示例可查看 [SampleResource](./src/MaaFramework.Binding.UnitTests/SampleResource)。

### 包安装

| 包 ID | 描述 |
| :---- | :---- |
| Maa.Framework | 引用了 Native 和 Runtimes 的元包 |
| Maa.Framework.Native | 引用了 Binding.Native 和 AgentBinary 的元包 |
| Maa.Framework.Binding | 抽象层 |
| Maa.Framework.Binding.Native | 本机 API 封装 |
| Maa.Framework.Runtimes <br> 平台专用包 | 预编译的 [MaaFramework](https://github.com/MaaXYZ/MaaFramework) 二进制文件。 <br> 默认不包含 Android 包。 |
| Maa.AgentBinary | 预编译的 [Agent 二进制文件](https://github.com/MaaXYZ/MaaAgentBinary) |

#### 正式版本

```ps1
dotnet add package Maa.Framework --prerelease
```

#### 夜间构建

- **添加包**

  ```ps1
  dotnet add package Maa.Framework --prerelease -s https://maaxyz.github.io/pkg/nuget/index.json
  ```

- **配置 NuGet 源**

  - .csproj
    ```xml
    <PropertyGroup>
    <RestoreSources>$(RestoreSources);https://api.nuget.org/v3/index.json;https://maaxyz.github.io/pkg/nuget/index.json</RestoreSources>
    </PropertyGroup>
    ```

  - NuGet.config

    参考 [示例配置](./NuGet.config) 查看具体实现；完整指南请查阅 [夜间构建版使用说明](https://maaxyz.github.io/MaaFramework.Binding.CSharp/articles/nightly-builds.html)。

#### 指定运行时标识符 (RID)

支持的运行时标识符列表详见[此处](https://github.com/MaaXYZ/MaaFramework/tree/main/tools/nupkgs)。

例如 Windows 平台需手动引用以下包：
- `Maa.Framework.Native`
- `Maa.Framework.Runtime.win-arm64`
- `Maa.Framework.Runtime.win-x64`

### 代码示例

> 准备工作：`adb connect HOST[:PORT]`

从 sample 文件夹中的 [powershell](sample\csharp\QuickStart.ps1) 或 [bash](sample\csharp\QuickStart.sh) 脚本快速开始。

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

#### 自定义

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

#### 更多资源
- 要查看与 MaaFramework 主仓库功能一致的实现示例，请访问 [示例目录](./sample)。
- 如需获取覆盖框架绝大多数功能的示例代码，请参考 [单元测试集](./src/MaaFramework.Binding.UnitTests)。

## 最佳实践
- [MFAWPF](https://github.com/SweetSmellFox/MFAWPF) MFA 任务管理器
  基于 MAA 全新架构的 通用 GUI。 由 MaaFramework 强力驱动！

## 文档

我们在源码中编写了详细的注释说明。

您也可以访问 [API 参考](https://maaxyz.github.io/MaaFramework.Binding.CSharp/api/MaaFramework.Binding.html) 和 [单元测试](./src/MaaFramework.Binding.UnitTests) 获取更多信息。

如需查看特定版本 MaaFramework.Binding.CSharp 的 API 参考，请访问项目发布页面并下载附带的 `docs.zip` 文件。

## 参与贡献

欢迎为 MaaFramework.Binding.CSharp 贡献力量。如果您发现 Bug 或有功能建议，请在 GitHub 仓库提交 Issue。如果您想贡献代码，欢迎 Fork 仓库并提交 Pull Request。

## 开源协议

`MaaFramework` 采用 [`LGPL-3.0`](./LICENSE.md) 协议开源。

## 讨论
- QQ 群：595990173
