using MaaFramework.Binding.Interop.Native;

namespace MaaFramework.Binding;

/// <summary>
///     Provides information and configuration for native bindings in the MaaFramework.
/// </summary>
public static class NativeBindingInfo
{
    /// <summary>
    ///     Gets the directory path where native assemblies are located.
    /// </summary>
    public static string? NativeAssemblyDirectory { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the framework is operating in stateless mode.
    /// <para>Stateless mode is typically used in server contexts.</para>
    /// </summary>
    public static bool IsStatelessMode { get; internal set; }

    /// <summary>
    ///     Gets or sets the API information string, which provides details about the current API context.
    /// </summary>
    public static string ApiInfo { get; internal set; } = string.Empty;

    /// <summary>
    ///     Initializes the native library with the specified configuration.
    ///     This method sets up the native library resolver and prepares the environment for native interop.
    /// </summary>
    /// <param name="isAgentServer">Indicates whether the application is running in an agent server context.</param>
    /// <param name="dllSearchPaths">Directory paths to search for native libraries.</param>
    /// <exception cref="InvalidOperationException">Thrown if the native library is already loaded.</exception>
    public static void Set(bool isAgentServer, params string[] dllSearchPaths)
        => NativeLibrary.Init(isAgentServer, dllSearchPaths);
}
