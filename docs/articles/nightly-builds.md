# Early access to MaaFramework.Binding.CSharp packages

Stable versions (and selected previews) of MaaFramework.Binding.CSharp, and related packages, are distributed through <https://nuget.org> (see [add packages](../README.md#add-packages)).

We also publish every successful merge to main branches to our preview NuGet channel called `nightly-builds`.

To use this channel, you will need to add or edit your [NuGet.Config](https://learn.microsoft.com/nuget/reference/nuget-config-file) file with the following content:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="maaxyz.github.io" value="https://maaxyz.github.io/pkg/nuget/index.json" />
  </packageSources>
</configuration>
```

You can also browse the available versions using `https://github.com/MaaXYZ/pkg/tree/main/nuget/flatcontainer/<PackageName>`, where `<PackageName>` is the name of the package you are looking for. For example, for Maa.Framework meta package, the link is <https://github.com/MaaXYZ/pkg/tree/main/nuget/flatcontainer/maa.framework>.

## Warranty

Packages from `nightly-builds` feed are considered experimental. They might not have the usual quality, may contain experimental and breaking changes, and come without warranty.

## Feed information

### NuGet.config placement

NuGet.Config file can be placed next to solution file, or next to project file when you don't have solution file. But in cases where you have solution file, you should always place it next to solution file, to ensure consistent behavior in Visual Studio and in command line.

### Usage with central package management

Solutions that use central package management through `Directory.Packages.props` will see `NU1507` warnings about multiple package sources. To solve this add this section to your `NuGet.Config` file:

```xml
<packageSourceMapping>
  <!-- key value for <packageSource> should match key values from <packageSources> element -->
  <packageSource key="nuget.org">
    <package pattern="*" />
    <package pattern="Maa.*" />
  </packageSource>
  <packageSource key="maaxyz.github.io">
    <package pattern="Maa.*" />
  </packageSource>
</packageSourceMapping>
```


Full documentation of package source mapping can be [found here](https://learn.microsoft.com/nuget/consume-packages/package-source-mapping#enable-by-manually-editing-nugetconfig).

## Version Number Definition

The `Core` version number follows Maa.Framework.Runtimes, whose [release version](https://github.com/MaaXYZ/MaaFramework/issues/208) is `Major.Minor.Patch[-Preview][-Post]`.

Therefore, the version number of MaaFramework.Binding.CSharp is defined as follows:

| Channel | Definition | Examples | Descriptions |
| :---: | :---: | :---: | :---: |
| stable-releases | Core[.Revision] | 2.0.0 & 2.0.0-alpha.1 <br> 2.0.0.2 & 2.0.0-alpha.1.2 | official version <br> second revision |
| preview-releases | Major.Minor.Patch-preview.X | 2.0.0-preview.2 | second preview |
| nightly-builds | Major.Minor.Patch-preview.Date.BuildTimes | 2.0.0-preview.24501.2 | second build <br> on October 1, 2024 |

> The Date 24501 calculates from **(Year 2024 - 2000) * 1000 + Month 10 * 50 + Day 1**.
