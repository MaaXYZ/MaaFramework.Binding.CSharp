using MaaFramework.Binding.Abstractions;

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
    ///     Gets or sets whether disposes the <see cref="Resource"/> when <see cref="IDisposable.Dispose"/> was invoked.
    /// </summary>
    DisposeOptions DisposeOptions { get; set; }

    /// <summary>
    ///     Gets or sets a resource that binds to the <see cref="IMaaAgentClient"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="MaaInteroperationException"/>
    IMaaResource Resource { get; set; }

    /// <summary>
    ///     Creates a socket connection with the specified identifier.
    /// </summary>
    /// <param name="identifier">The connection identifier.</param>
    /// <returns><see langword="true"/> if the socket was created successfully; otherwise, <see langword="false"/>.</returns>
    string? CreateSocket(string identifier = "");

    /// <summary>
    ///     Starts the connection.
    /// </summary>
    /// <returns><see langword="true"/> if the connection was started successfully; otherwise, <see langword="false"/>.</returns>
    bool LinkStart();

    /// <summary>
    ///     Stops the connection.
    /// </summary>
    /// <returns><see langword="true"/> if the connection was stopped successfully; otherwise, <see langword="false"/>.</returns>
    bool LinkStop();
}
