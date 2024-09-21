using System.Reflection;
using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Native;

internal static partial class NativeLibrary
{
    private static readonly Assembly s_assembly = typeof(NativeLibrary).Assembly;

    public static void Init()
        => SetDllImportResolver(s_assembly, NativeAssemblyResolver);

    public static IntPtr NativeAssemblyResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        var libHandle = IntPtr.Zero;
        if (!libraryName.Equals("MaaFramework", StringComparison.Ordinal)
         && !libraryName.Equals("MaaToolkit", StringComparison.Ordinal))
        {
            return libHandle;
        }

        if (TryGetRuntimesPath(libraryName, out var dllPath))
        {
            _ = TryLoad(dllPath, assembly, searchPath, out libHandle);
        }
        return libHandle;
    }

    private static bool TryGetRuntimesPath(string libraryName, out string dllPath)
    {
        dllPath = GetRuntimesPaths(libraryName).FirstOrDefault(File.Exists, string.Empty);
        return !string.IsNullOrEmpty(dllPath);
    }

    private static IEnumerable<string> GetRuntimesPaths(string libraryName)
    {
        GetArchitectureNameAndExtensionName(out var arch, out var ext);
        var args1 = new string[]
        {
            Path.GetDirectoryName(s_assembly.Location) ?? "./",
            Environment.CurrentDirectory,
        };
        var args2 = new string[]
        {
            $"/runtimes/{arch}/native/",
            "/"
        };
        var args3 = new string[]
        {
            $"{libraryName}.{ext}",
            $"lib{libraryName}.{ext}"
        };

        return from arg1 in args1
               from arg2 in args2
               from arg3 in args3
               select Path.GetFullPath(
                   string.Concat(arg1, arg2, arg3));
    }

    private static void GetArchitectureNameAndExtensionName(out string arch, out string ext)
    {
        if (IsWindows) arch = "win";
        else if (IsLinux) arch = "linux";
        else if (IsOSX) arch = "osx";
        else if (IsAndroid) arch = "android";
        else throw new PlatformNotSupportedException();

        if (IsX64) arch += "-x64";
        else if (IsArm64) arch += "-arm64";
        else throw new PlatformNotSupportedException();

        if (IsWindows) ext = "dll";
        else if (IsLinux || IsAndroid) ext = "so";
        else if (IsOSX) ext = "dylib";
        else throw new PlatformNotSupportedException();
    }

    private static bool IsWindows => OperatingSystem.IsWindows();
    private static bool IsLinux => OperatingSystem.IsLinux();
    private static bool IsOSX => OperatingSystem.IsMacOS();
    private static bool IsAndroid => OperatingSystem.IsAndroid();
    private static bool IsX64 => RuntimeInformation.OSArchitecture == Architecture.X64;
    private static bool IsArm64 => RuntimeInformation.OSArchitecture == Architecture.Arm64;
}
