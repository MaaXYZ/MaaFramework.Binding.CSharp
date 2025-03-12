using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaAgentClient with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaAgentClient<T> : IMaaAgentClient, IMaaDisposableHandle<T>
{
    /// <inheritdoc cref="IMaaAgentClient.Resource"/>
    new IMaaResource<T> Resource { get; set; }
}

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
    /// 
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    string? CreateSocket(string identifier = "");

    /// <summary>
    ///     Connects the address specified by the constructor.
    /// </summary>
    /// <returns><see langword="true"/> if the connection was ended successfully; otherwise, <see langword="false"/>.</returns>
    bool LinkStart();

    /// <summary>
    ///     Ends the connection of the address specified by the constructor.
    /// </summary>
    /// <returns><see langword="true"/> if the connection was ended successfully; otherwise, <see langword="false"/>.</returns>
    bool LinkStop();
}
