using MaaFramework.Binding.Interop.Native;

namespace MaaFramework.Binding;

/// <summary>
/// 
/// </summary>
public static class NativeBindingInfo
{
    /// <summary>
    /// 
    /// </summary>
    public static string? NativeAssemblyDirectory { get; internal set; }

    /// <summary>
    /// 
    /// </summary>
    public static bool IsStatelessMode { get; internal set; }

    /// <summary>
    /// 
    /// </summary>
    public static string ApiInfo { get; internal set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isAgentServer"></param>
    /// <param name="dllSearchPaths"></param>
    /// <returns></returns>
    public static void Set(bool isAgentServer, params string[] dllSearchPaths)
    {
        NativeLibrary.Init(isAgentServer, dllSearchPaths);
    }
}
