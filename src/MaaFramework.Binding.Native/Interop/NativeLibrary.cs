using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Native;

internal static partial class NativeLibrary
{
    private static readonly Assembly s_assembly = typeof(NativeLibrary).Assembly;

    private static bool s_isAgentServer;
    private static readonly List<string> s_searchPath = [];
    private static readonly Dictionary<string, nint> s_libraryHandles = [];

#pragma warning disable CA2255 // 不应在库中使用 “ModuleInitializer” 属性
    [ModuleInitializer]
    internal static void SetNativeAssemblyResolver() => SetDllImportResolver(s_assembly, NativeAssemblyResolver);
#pragma warning restore CA2255 // 不应在库中使用 “ModuleInitializer” 属性

    public static void Init(bool isAgentServer, params string[] paths)
    {
        if (s_libraryHandles.Count != 0)
            throw new InvalidOperationException("NativeLibrary is already loaded.");

        s_isAgentServer = isAgentServer;
        s_searchPath.AddRange(paths);
    }

    public static IntPtr NativeAssemblyResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath) => libraryName switch
    {
        "MaaFramework" or "MaaToolkit"
        or "MaaAgentServer" or "MaaAgentClient" => GetLibraryHandle(libraryName),

        _ => IntPtr.Zero,
    };

    private static nint GetLibraryHandle(string libraryName)
    {
        if (s_libraryHandles.TryGetValue(libraryName, out var libraryHandle))
            return libraryHandle;

        if (!TryGetRuntimesPath(libraryName, out var dllPath) || !TryLoad(dllPath, out libraryHandle))
        {
            s_libraryHandles.Add(libraryName, nint.Zero);
            NativeBindingInfo.NativeAssemblyDirectory = null;
            NativeBindingInfo.IsStatelessMode = false;
            NativeBindingInfo.ApiInfo = "Using default dll resolver.";
            return nint.Zero;
        }

        s_libraryHandles.Add(libraryName, libraryHandle);

        var dllDir = Path.GetDirectoryName(dllPath);
        if (s_libraryHandles.Count == 1)
        {
            NativeBindingInfo.NativeAssemblyDirectory ??= dllDir;
            NativeBindingInfo.IsStatelessMode = s_isAgentServer;
            NativeBindingInfo.ApiInfo = s_isAgentServer ? "In MaaAgentServer context." : "In MaaFramework context.";
        }

        if (NativeBindingInfo.NativeAssemblyDirectory != dllDir)
            throw new InvalidOperationException($"The native assembly directory '{NativeBindingInfo.NativeAssemblyDirectory}' was switched to '{dllDir}'.");

        return libraryHandle;
    }

    private static bool TryGetRuntimesPath(string libraryName, out string dllPath)
    {
        libraryName = GetFullLibraryName(libraryName);
        dllPath = GetRuntimesPaths(libraryName).FirstOrDefault(File.Exists, string.Empty);
        return !string.IsNullOrEmpty(dllPath);
    }

    private static IEnumerable<string> GetRuntimesPaths(string libraryFullName)
    {
        var searchPaths = s_searchPath.Concat(
        [
            Environment.GetEnvironmentVariable("MAAFW_BINARY_PATH"),
            Path.GetDirectoryName(s_assembly.Location),
            Environment.CurrentDirectory,
        ]).Where(path => !string.IsNullOrWhiteSpace(path));

        var runtimePaths = new[]
        {
            $"./runtimes/{GetArchitectureName()}/native/",
            "./"
        };

        return from searchPath in searchPaths
               from runtimePath in runtimePaths
               select Path.GetFullPath(
                   Path.Combine(searchPath, runtimePath, libraryFullName));
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
        if (s_isAgentServer && libraryName == "MaaFramework")
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
