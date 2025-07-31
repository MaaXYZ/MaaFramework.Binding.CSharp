using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Native;

internal static partial class NativeLibrary
{
    private static readonly Assembly s_assembly = typeof(NativeLibrary).Assembly;

#pragma warning disable CA2255 // 不应在库中使用 “ModuleInitializer” 属性
#pragma warning disable S2223 // Non-constant static fields should not be visible

    internal static bool IsLoaded;
    internal static string LoadedDirectory = string.Empty;
    internal static readonly Dictionary<string, nint> LoadedLibraryHandles = [];

    internal static ApiInfoFlags ApiInfo;
    internal static readonly List<string> SearchPaths = [];

    [ModuleInitializer]
    internal static void SetNativeLibraryResolver() => SetDllImportResolver(s_assembly, NativeLibraryResolver);

#pragma warning restore S2223 // Non-constant static fields should not be visible
#pragma warning restore CA2255 // 不应在库中使用 “ModuleInitializer” 属性

    public static nint NativeLibraryResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath) => libraryName switch
    {
        "MaaFramework" or "MaaToolkit"
        or "MaaAgentServer" or "MaaAgentClient" => GetLibraryHandle(libraryName, searchPath),

        _ => IntPtr.Zero,
    };

    private static nint GetLibraryHandle(string libraryName, DllImportSearchPath? searchPath)
    {
        if (LoadedLibraryHandles.TryGetValue(libraryName, out var libraryHandle))
            return libraryHandle;

        IsLoaded = true;
        UseBindingResolverToLoad(libraryName, searchPath, out libraryHandle);
        LoadedLibraryHandles.Add(libraryName, libraryHandle);

        return libraryHandle;
    }

    private static void UseBindingResolverToLoad(string libraryName, DllImportSearchPath? searchPath, out nint libraryHandle)
    {
        libraryName = GetFullLibraryName(libraryName);
        var dllPath = GetLibraryPaths(libraryName).FirstOrDefault(File.Exists, string.Empty);
        var dllDir = string.Empty;
        var resolver = ApiInfoFlags.UseDefaultResolver;

        if (!string.IsNullOrEmpty(dllPath) && TryLoad(dllPath, out libraryHandle))
        {
            resolver = ApiInfoFlags.UseBindingResolver;
            dllDir = Path.GetDirectoryName(dllPath)!;
        }
        else if (!TryLoad(libraryName, s_assembly, searchPath, out libraryHandle)) // Not found
        {
            IsLoaded = false;
            return;
        }

        SetBindingContext(resolver, dllDir, isFirst: LoadedLibraryHandles.Count == 0);
    }

    private static void SetBindingContext(ApiInfoFlags resolver, string dllDir, bool isFirst)
    {
        if (ApiInfo.HasFlag_ResolverExcept(resolver))
            throw new InvalidOperationException($"The resolver '{ApiInfo}' was attempted to switch to '{resolver}'.");

        if (isFirst)
            LoadedDirectory = dllDir;

        else if (LoadedDirectory != dllDir)
            throw new InvalidOperationException($"The native library directory '{LoadedDirectory}' was attempted to switch to '{dllDir}'.");

        ApiInfo |= resolver;
        if (!ApiInfo.HasFlag_Context())
            ApiInfo |= ApiInfoFlags.InFrameworkContext;
    }

    [UnconditionalSuppressMessage("SingleFile", "IL3000: Avoid accessing Assembly file path when publishing as a single file",
        Justification = "The code handles the Assembly.Location being null/empty by falling back to AppContext.BaseDirectory.")]
    private static IEnumerable<string> GetLibraryPaths(string libraryFullName)
    {
        // For single-file deployments, the assembly location is an empty string so we fall back
        // to AppContext.BaseDirectory which is the directory containing the single-file executable.
        var assemblyDirectory = string.IsNullOrEmpty(s_assembly.Location)
            ? AppContext.BaseDirectory
            : Path.GetDirectoryName(s_assembly.Location)!;

        string?[] basePaths =
        [
            ..SearchPaths,
            Environment.GetEnvironmentVariable("MAAFW_BINARY_PATH"),
            assemblyDirectory,
            Environment.CurrentDirectory,
        ];

        string[] runtimesPaths =
        [
            "./",
            $"./runtimes/{GetArchitectureName()}/native/",
        ];

        return from basePath in basePaths.Distinct()
               where !string.IsNullOrWhiteSpace(basePath)
               from runtimesPath in runtimesPaths
               let libraryPath = Path.Combine(basePath, runtimesPath, libraryFullName)
               select Path.GetFullPath(libraryPath);
    }

#pragma warning disable IDE0072 // 添加缺失的事例
    private static string GetArchitectureName() => RuntimeInformation.OSArchitecture switch
    {
        Architecture.X64 when IsWindows => "win-x64",
        // Architecture.Arm64 when IsWindows => "win-arm64",
        Architecture.X64 when IsLinux => "linux-x64",
        Architecture.Arm64 when IsLinux => "linux-arm64",
        Architecture.X64 when IsOSX => "osx-x64",
        Architecture.Arm64 when IsOSX => "osx-arm64",
        Architecture.X64 when IsAndroid => "android-x64",
        Architecture.Arm64 when IsAndroid => "android-arm64",
        _ => throw new PlatformNotSupportedException(),
    };
#pragma warning restore IDE0072 // 添加缺失的事例

    private static string GetFullLibraryName(string libraryName)
    {
        if (libraryName == "MaaFramework" && ApiInfo.HasFlag(ApiInfoFlags.InAgentServerContext))
            libraryName = "MaaAgentServer";

        if (IsWindows)
            return $"{libraryName}.dll";
        if (IsLinux || IsAndroid)
            return $"lib{libraryName}.so";
        if (IsOSX)
            return $"lib{libraryName}.dylib";

        throw new PlatformNotSupportedException();
    }

    private static bool IsWindows => OperatingSystem.IsWindows();
    private static bool IsLinux => OperatingSystem.IsLinux();
    private static bool IsOSX => OperatingSystem.IsMacOS();
    private static bool IsAndroid => OperatingSystem.IsAndroid();
}
