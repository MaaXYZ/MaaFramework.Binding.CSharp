using System.Reflection;
using System.Runtime.InteropServices;
using SNL = System.Runtime.InteropServices.NativeLibrary;

namespace MaaToolKit.Extensions.Interop;

/// <inheritdoc cref="SNL" />
internal static partial class NativeLibrary
{
    /// <inheritdoc cref="SNL.Load(string)" />
    public static IntPtr Load(string libraryPath)
        => SNL.Load(libraryPath);

    /// <inheritdoc cref="SNL.Load(string, Assembly, DllImportSearchPath?)" />
    public static IntPtr Load(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        => SNL.Load(libraryName, assembly, searchPath);

    /// <inheritdoc cref="SNL.TryLoad(string, out nint)" />
    public static bool TryLoad(string libraryPath, out IntPtr handle)
        => SNL.TryLoad(libraryPath, out handle);

    /// <inheritdoc cref="SNL.TryLoad(string, Assembly, DllImportSearchPath?, out nint)" />
    public static bool TryLoad(string libraryName, Assembly assembly, DllImportSearchPath? searchPath, out IntPtr handle)
        => SNL.TryLoad(libraryName, assembly, searchPath, out handle);

    /// <inheritdoc cref="SNL.Free(nint)" />
    public static void Free(IntPtr handle)
        => SNL.Free(handle);

    /// <inheritdoc cref="SNL.GetExport(nint, string)" />
    public static IntPtr GetExport(IntPtr handle, string name)
        => SNL.GetExport(handle, name);

    /// <inheritdoc cref="SNL.TryGetExport(nint, string, out nint)" />
    public static bool TryGetExport(IntPtr handle, string name, out IntPtr address)
        => SNL.TryGetExport(handle, name, out address);

    /// <inheritdoc cref="SNL.SetDllImportResolver(Assembly, DllImportResolver)" />
    public static void SetDllImportResolver(Assembly assembly, DllImportResolver resolver)
        => SNL.SetDllImportResolver(assembly, resolver);

    /// <inheritdoc cref="SNL.GetMainProgramHandle()"/>
    public static IntPtr GetMainProgramHandle()
        => SNL.GetMainProgramHandle();
}


