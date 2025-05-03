using MaaFramework.Binding.Abstractions;
using System.Diagnostics;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaAgentClient with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaAgentClient<out T> : IMaaAgentClient, IMaaDisposableHandle<T>;

/// <summary>
///     An interface defining wrapped members for MaaAgentClient.
/// </summary>
public interface IMaaAgentClient : IMaaDisposable
{
    /// <summary>
    ///     Gets the unique identifier used to communicate with the agent server.
    /// </summary>
    string Id { get; }

    /// <summary>
    ///     Gets or sets a resource that binds to the <see cref="IMaaAgentClient"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="MaaInteroperationException"/>
    IMaaResource Resource { get; set; }

    /// <summary>
    ///     Starts the connection.
    /// </summary>
    /// <returns><see langword="true"/> if the connection was started successfully; otherwise, <see langword="false"/>.</returns>
    bool LinkStart();

    /// <summary>
    ///     Starts the agent server process using the specified <see cref="ProcessStartInfo"/> and connects to the agent server.
    /// </summary>
    /// <param name="info">The process start info.</param>
    /// <returns><see langword="true"/> if the connection was started successfully; otherwise, <see langword="false"/>.</returns>
    bool LinkStart(ProcessStartInfo info);

    /// <summary>
    ///     Starts the agent server process using the specified method and connects to the agent server.
    /// </summary>
    /// <param name="method">The delegate method that defines how to start the agent server process.</param>
    /// <returns><see langword="true"/> if the connection was started successfully; otherwise, <see langword="false"/>.</returns>
    bool LinkStart(AgentServerStartupMethod method);

    /// <summary>
    ///     Stops the connection.
    /// </summary>
    /// <returns><see langword="true"/> if the connection was stopped successfully; otherwise, <see langword="false"/>.</returns>
    bool LinkStop();

    /// <summary>
    ///     Represents a method that starts the agent server process.
    /// </summary>
    /// <param name="identifier">The unique identifier used to communicate with the agent server.</param>
    /// <param name="nativeAssemblyDirectory">The directory path where the <see cref="MaaFramework"/> native assemblies are located.</param>
    /// <returns>
    ///     A <see cref="Process"/> instance representing the started agent server process, 
    ///     or <see langword="null"/> if the method is used to synchronize with unmanaged processes.
    /// </returns>
    delegate Process? AgentServerStartupMethod(string identifier, string nativeAssemblyDirectory);

    /// <summary>
    ///     Gets the agent server process managed by <see cref="IMaaAgentClient"/> from method LinkStart.
    /// </summary>
    /// <exception cref="InvalidOperationException">The process is unavailable or not managed by <see cref="IMaaAgentClient"/>.</exception>
    Process AgentServerProcess { get; }
}
