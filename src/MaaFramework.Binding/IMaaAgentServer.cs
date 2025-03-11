using MaaFramework.Binding.Custom;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaAgentServer.
/// </summary>
public interface IMaaAgentServer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="custom"></param>
    /// <returns></returns>
    bool Register<T>(string name, T custom) where T : IMaaCustomResource;

    /// <inheritdoc cref="Register{T}(string, T)"/>
    bool Register<T>(T custom) where T : IMaaCustomResource;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    bool StartUp(string identifier);

    /// <summary>
    /// 
    /// </summary>
    void ShutDown();

    /// <summary>
    /// 
    /// </summary>
    void Join();

    /// <summary>
    /// 
    /// </summary>
    void Detach();
}
