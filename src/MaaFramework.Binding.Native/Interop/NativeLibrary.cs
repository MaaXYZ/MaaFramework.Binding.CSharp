using System.Diagnostics;
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
        ResolveLibraryLoading(libraryName, searchPath, out libraryHandle);
        LoadedLibraryHandles.Add(libraryName, libraryHandle);

        return libraryHandle;
    }

    private static void ResolveLibraryLoading(string libraryName, DllImportSearchPath? searchPath, out nint libraryHandle)
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
        else if (TryLoad(libraryName, s_assembly, searchPath, out libraryHandle))
        {
            dllDir = IsOSX
                ? DyldHelper.PathFromHandle(libraryHandle)
                : GetLoadedLibraryPath(libraryHandle);
        }
        else
        {
            // Not found
        }

        SetBindingContext(resolver, dllDir, isFirst: LoadedLibraryHandles.Count == 0);
    }

    private static void SetBindingContext(ApiInfoFlags resolver, string dllDir, bool isFirst)
    {
        ApiInfo |= resolver;
        if (!ApiInfo.HasFlag_Context())
            ApiInfo |= ApiInfoFlags.InFrameworkContext;

        if (isFirst)
            LoadedDirectory = dllDir;

        else if (LoadedDirectory != dllDir)
            throw new InvalidOperationException($"The native library directory '{LoadedDirectory}' was attempted to switch to '{dllDir}'.\nCurrent api info is '{ApiInfo}'.");
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

    // macOS / iOS
    // https://stackoverflow.com/questions/54201384
    private static partial class DyldHelper
    {
#pragma warning disable IDE1006
        [LibraryImport("libSystem.dylib", StringMarshalling = StringMarshalling.Utf8)]
        private static partial uint _dyld_image_count();

        [LibraryImport("libSystem.dylib", StringMarshalling = StringMarshalling.Utf8)]
        private static partial string? _dyld_get_image_name(uint idx);

        [LibraryImport("libSystem.dylib", StringMarshalling = StringMarshalling.Utf8)]
        private static partial nint dlopen(string path, int mode);

        [LibraryImport("libSystem.dylib", StringMarshalling = StringMarshalling.Utf8)]
        private static partial int dlclose(nint handle);
#pragma warning restore IDE1006

        private const int RTLD_LAZY = 0x1;

        public static string PathFromHandle(nint handle)
        {
            for (var i = _dyld_image_count() - 1; i >= 0; i--)
            {
                var imageName = _dyld_get_image_name(i);
                if (imageName == null)
                    continue;

                var probeHandle = dlopen(imageName, RTLD_LAZY);
                _ = dlclose(probeHandle);

                if (handle == probeHandle)
                    return imageName;
            }
            return string.Empty;
        }
    }

    private static string GetLoadedLibraryPath(nint handle)
    {
        var modules = Process.GetCurrentProcess().Modules;
        for (var i = modules.Count - 1; i >= 0; i--)
        {
            var module = modules[i];
            if (handle == module.BaseAddress)
                return module.FileName;
        }
        return string.Empty;
    }
}
