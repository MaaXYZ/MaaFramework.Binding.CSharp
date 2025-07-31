using MaaFramework.Binding.Interop.Native;
using System.Diagnostics;

namespace MaaFramework.Binding;

/// <summary>
///     Provides information and configuration for native bindings in the MaaFramework.
/// </summary>
public static class NativeBindingContext
{
    /// <summary>
    ///     Gets a value indicating whether NativeLibrary is already loaded.
    /// </summary>
    public static bool IsLoaded => NativeLibrary.IsLoaded;

    /// <summary>
    ///     Gets the loaded directory path where native libraries are located.
    /// </summary>
    public static string LoadedNativeLibraryDirectory
    {
        get
        {
            if (!NativeLibrary.ApiInfo.HasFlag(ApiInfoFlags.UseDefaultResolver))
                return NativeLibrary.LoadedDirectory;

            var name = NativeLibrary.LoadedLibraryHandles.First().Key;
            foreach (var obj in Process.GetCurrentProcess().Modules)
            {
                if (obj is not ProcessModule module || Path.GetFileNameWithoutExtension(module.ModuleName) != name)
                    continue;
                return Path.GetDirectoryName(module.FileName) ?? string.Empty;
            }
            return string.Empty;
        }
    }

    /// <summary>
    ///     Gets the API information, which provides details about the current API context.
    /// </summary>
    public static ApiInfoFlags ApiInfo => NativeLibrary.ApiInfo;

    /// <summary>
    ///     Gets a value indicating whether the current API is interoperating in the stateless mode.
    ///     <para>Stateless mode is typically used in server contexts.</para>
    /// </summary>
    public static bool IsStatelessMode => ApiInfo.HasFlag(ApiInfoFlags.InAgentServerContext);

    /// <summary>
    ///     Switches the context to the MaaAgentServer context.
    /// </summary>
    /// <exception cref="InvalidOperationException">NativeLibrary is already loaded.</exception>
    public static void SwitchToAgentServerContext()
    {
        ThrowIfLoaded();
        NativeLibrary.ApiInfo = ApiInfoFlags.InAgentServerContext;
    }

    /// <summary>
    ///     Switches the context to the MaaFramework context.
    /// </summary>
    /// <exception cref="InvalidOperationException">NativeLibrary is already loaded.</exception>
    public static void SwitchToFrameworkContext()
    {
        ThrowIfLoaded();
        NativeLibrary.ApiInfo = ApiInfoFlags.InFrameworkContext;
    }

    /// <summary>
    ///    Appends the specified paths to the native library search paths.
    /// </summary>
    /// <param name="paths"></param>
    /// <exception cref="InvalidOperationException">NativeLibrary is already loaded.</exception>
    public static void AppendNativeLibrarySearchPaths(params IEnumerable<string> paths)
    {
        ThrowIfLoaded();
        NativeLibrary.SearchPaths.AddRange(paths);
    }

    /// <exception cref="InvalidOperationException">NativeLibrary is already loaded.</exception>
    internal static void ThrowIfLoaded()
    {
        if (NativeLibrary.IsLoaded)
            throw new InvalidOperationException("NativeLibrary is already loaded.");
    }
}
