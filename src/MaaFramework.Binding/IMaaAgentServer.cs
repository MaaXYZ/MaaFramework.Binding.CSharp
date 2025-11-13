using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Custom;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaAgentServer.
/// </summary>
public interface IMaaAgentServer : IMaaCommon
{
    /// <summary>
    ///     Configures the unique identifier used to communicate with the agent client.
    /// </summary>
    /// <param name="identifier">The unique identifier used to communicate with the agent client.</param>
    /// <remarks>
    ///     Uses this method before <see cref="StartUp"/>; otherwise, throws an <see cref="InvalidOperationException"/>.
    /// </remarks>
    /// <exception cref="InvalidOperationException">MaaAgentServer has started up.</exception>
    IMaaAgentServer WithIdentifier(string identifier);

    /// <summary>
    ///     Registers a <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/> in the <see cref="IMaaAgentServer"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</typeparam>
    /// <param name="name">The new name that will be used to reference the custom resource.</param>
    /// <param name="custom">The custom resource instance to register.</param>
    /// <exception cref="MaaInteroperationException">Thrown if the registration fails.</exception>
    IMaaAgentServer Register<T>(string name, T custom) where T : IMaaCustomResource;

    /// <inheritdoc cref="Register{T}(string, T)"/>
    IMaaAgentServer Register<T>(string? name = null) where T : IMaaCustomResource, new();

    /// <inheritdoc cref="Register{T}(string, T)"/>
    IMaaAgentServer Register<T>(T custom) where T : IMaaCustomResource;

    /// <summary>
    ///     Starts up the agent server to prepare for receiving client messages from the specified connection.
    /// </summary>
    /// <exception cref="MaaInteroperationException">Thrown if the registration fails.</exception>
    IMaaAgentServer StartUp();

    /// <summary>
    ///     Shuts down the agent server.
    /// </summary>
    IMaaAgentServer ShutDown();

    /// <summary>
    ///     Blocks the calling thread until the thread for receiving client messages finishes its execution.
    /// </summary>
    IMaaAgentServer Join();

    /// <summary>
    ///     Separates the thread for receiving client messages, allowing execution to continue independently.
    /// </summary>
    IMaaAgentServer Detach();
}
