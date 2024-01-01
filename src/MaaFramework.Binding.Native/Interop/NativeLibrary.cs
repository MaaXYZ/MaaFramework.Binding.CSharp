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
         && !libraryName.Equals("MaaToolKit", StringComparison.Ordinal)
         && !libraryName.Equals("MaaRpc", StringComparison.Ordinal))
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
        if (string.IsNullOrEmpty(dllPath))
        {
            return false;
        }

        return true;
    }

    private static IEnumerable<string> GetRuntimesPaths(string libraryName)
    {
        var (arch, exten) = GetArchitectureNameAndExtensionName();
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
            $"{libraryName}.{exten}",
            $"lib{libraryName}.{exten}"
        };

        return from arg1 in args1
               from arg2 in args2
               from arg3 in args3
               select Path.GetFullPath(
                   string.Concat(arg1, arg2, arg3));
    }

    private static (string arch, string exten) GetArchitectureNameAndExtensionName()
    {
        var sb = new System.Text.StringBuilder();
        if (IsWindows)
            sb.Append("win");
        else if (IsLinux)
            sb.Append("linux");
        else if (IsOSX)
            sb.Append("osx");
        else
            throw new PlatformNotSupportedException();

        sb.Append('-');
        if (IsX64)
            sb.Append("x64");
        else if (IsArm64)
            sb.Append("arm64");
        else
            throw new PlatformNotSupportedException();

        string exten;
        if (IsWindows)
            exten = "dll";
        else if (IsLinux)
            exten = "so";
        else if (IsOSX)
            exten = "dylib";
        else
            throw new PlatformNotSupportedException();

        return (sb.ToString(), exten);
    }

    private static bool IsWindows
        => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    private static bool IsLinux
        => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    private static bool IsOSX
        => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    private static bool IsX64
        => RuntimeInformation.OSArchitecture == Architecture.X64;
    private static bool IsArm64
        => RuntimeInformation.OSArchitecture == Architecture.Arm64;
}
