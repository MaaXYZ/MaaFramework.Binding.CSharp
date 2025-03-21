using MaaFramework.Binding.Custom;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaAgentServer.
/// </summary>
public interface IMaaAgentServer
{
    /// <summary>
    ///     Registers a <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/> in the <see cref="IMaaAgentServer"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</typeparam>
    /// <param name="name">The new name that will be used to reference the custom resource.</param>
    /// <param name="custom">The custom resource instance to register.</param>
    /// <returns><see langword="true"/> if the registration was successful; otherwise, <see langword="false"/>.</returns>
    bool Register<T>(string name, T custom) where T : IMaaCustomResource;

    /// <inheritdoc cref="Register{T}(string, T)"/>
    bool Register<T>(T custom) where T : IMaaCustomResource;

    /// <summary>
    ///     Starts up the agent server to prepare for receiving client messages from the specified connection.
    /// </summary>
    /// <param name="identifier">The connection identifier.</param>
    /// <returns><see langword="true"/> if the server started successfully; otherwise, <see langword="false"/>.</returns>
    bool StartUp(string identifier);

    /// <summary>
    ///     Shuts down the agent server.
    /// </summary>
    void ShutDown();

    /// <summary>
    ///     Blocks the calling thread until the thread for receiving client messages finishes its execution.
    /// </summary>
    void Join();

    /// <summary>
    ///     Separates the thread for receiving client messages, allowing execution to continue independently.
    /// </summary>
    void Detach();
}
