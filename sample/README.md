# About Example

Document Language: [简体中文](README.zh_cn.md) / [English](README.md)

## Usage Guide

This example aligns with the [C# sample](https://github.com/MaaXYZ/MaaFramework/tree/main/sample/csharp) from the MaaFramework main repository. The implementation references the [Python sample](https://github.com/MaaXYZ/MaaFramework/tree/main/sample/python).

For detailed interface usage, see:
- [Unit test cases](./src/MaaFramework.Binding.UnitTests)
- [C# API reference documentation](https://maaxyz.github.io/MaaFramework.Binding.CSharp/api/MaaFramework.Binding.html)

### Environment Setup

1. Start an Android emulator; or connect a physical device using `adb connect HOST[:PORT]`
2. Verify ADB devices are ready (check connection with `adb devices`)

### Run Samples

- [powershell](sample\csharp\QuickStart.ps1)
- [bash](sample\csharp\QuickStart.sh)

### Resource Configuration

Configure resource files following the [official quickstart guide](https://github.com/MaaXYZ/MaaFramework/blob/main/docs/en_us/1.1-GettingStarted.md#prepare-resource-files):

1. Ensure `resource` directory contains:
   - `image/App.png` - Application icon file
   - OCR model files under `model/ocr` directory
2. Modify `sample.json` task configuration as needed
3. Edit `main.csx`

> [!TIP]
> If encountering errors, first verify resource file integrity and path configurations.
