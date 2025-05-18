# 关于示例

文档语言: [简体中文](README.zh_cn.md) / [English](README.md)

## 使用指南

本示例与 MaaFramework 主仓库中的 [C# 示例](https://github.com/MaaXYZ/MaaFramework/tree/main/sample/csharp) 保持一致，功能实现参考 [Python 示例](https://github.com/MaaXYZ/MaaFramework/tree/main/sample/python)。

如需深入理解接口用法，请查阅：
- [单元测试案例](./src/MaaFramework.Binding.UnitTests)
- [C# API 参考文档](https://maaxyz.github.io/MaaFramework.Binding.CSharp/api/MaaFramework.Binding.html)

### 环境准备

1. 启动安卓模拟器；或通过 `adb connect HOST[:PORT]` 连接实体设备
2. 确认 ADB 设备已就绪（可通过 `adb devices` 验证连接）

### 运行示例

- [powershell](sample\csharp\QuickStart.ps1)
- [bash](sample\csharp\QuickStart.sh)

### 资源配置

根据 [官方快速入门指南](https://github.com/MaaXYZ/MaaFramework/blob/main/docs/zh_cn/1.1-快速开始.md#准备资源文件) 配置资源文件：

1. 确保 `resource` 目录包含：
   - `image/App.png` - 应用图标文件
   - `model/ocr` 目录下包含 OCR 模型文件
2. 按需调整 `sample.json` 任务配置
3. 修改 `main.csx`

> [!TIP]
> 若运行报错，请优先检查资源文件完整性及路径配置。