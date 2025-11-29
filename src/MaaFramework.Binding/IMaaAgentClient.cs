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
    /// <returns>A <see cref="string"/> if the identifier was got successfully; otherwise, <see langword="null"/>.</returns>
    string? Id { get; }

    /// <summary>
    ///     Gets or sets a resource that binds to the <see cref="IMaaAgentClient"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="MaaInteroperationException"/>
    IMaaResource Resource { get; set; }

    /// <summary>
    ///     Attaches <see langword="this"/> instance's Dispose() to the <see cref="Resource"/>'s Releasing event.
    /// </summary>
    IMaaAgentClient AttachDisposeToResource();

    /// <summary>
    ///     Detaches <see langword="this"/> instance's Dispose() from the <see cref="Resource"/>'s Releasing event.
    /// </summary>
    IMaaAgentClient DetachDisposeToResource();

    /// <summary>
    ///     Starts the connection.
    /// </summary>
    /// <returns><see langword="true"/> if the connection was started successfully; otherwise, <see langword="false"/>.</returns>
    bool LinkStart();

    /// <summary>
    ///     Starts the agent server process using the specified <see cref="ProcessStartInfo"/> and connects to the agent server.
    ///     <para>To start a new process, the current <see cref="AgentServerProcess"/> must have exited first.</para>
    ///     <para>The agent server process will be killed when <see cref="IDisposable.Dispose"/> is called.</para>
    /// </summary>
    /// <param name="info">The process start info.</param>
    /// <param name="cancellationToken">An optional token to cancel the asynchronous operation waiting for the connection.</param>
    /// <returns><see langword="true"/> if the connection was started successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="OperationCanceledException">The <paramref name="cancellationToken"/> has had cancellation requested.</exception>
    bool LinkStart(ProcessStartInfo info, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Starts the agent server process using the specified <see cref="ProcessStartInfo"/> and connects to the agent server.
    ///     <para>To start a new process, the current <see cref="AgentServerProcess"/> must have exited first.</para>
    ///     <para>The agent server process will be killed when <see cref="IDisposable.Dispose"/> is called.</para>
    /// </summary>
    /// <param name="method">The delegate method that defines how to start the agent server process.</param>
    /// <param name="cancellationToken">An optional token to cancel the asynchronous operation waiting for the connection.</param>
    /// <returns><see langword="true"/> if the connection was started successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="OperationCanceledException">The <paramref name="cancellationToken"/> has had cancellation requested.</exception>
    bool LinkStart(AgentServerStartupMethod method, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Starts the connection asynchronously unless the process has exited.
    /// </summary>
    /// <param name="process">The process to monitor for exit status.</param>
    /// <param name="cancellationToken">An optional token to cancel the asynchronous operation waiting for the connection.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains 
    ///     <see langword="true"/> if the connection was started successfully; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="OperationCanceledException">The <paramref name="cancellationToken"/> has had cancellation requested.</exception>
    Task<bool> LinkStartUnlessProcessExit(Process process, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Stops the connection.
    /// </summary>
    /// <returns><see langword="true"/> if the connection was stopped successfully; otherwise, <see langword="false"/>.</returns>
    bool LinkStop();

    /// <summary>
    ///     Gets whether the <see cref="IMaaAgentClient"/> is connected to the agent server.
    /// </summary>
    /// <returns><see langword="true"/> if the <see cref="IMaaAgentClient"/> is connected; otherwise, <see langword="false"/>.</returns>
    bool IsConnected { get; }

    /// <summary>
    ///     Gets whether the connection is alive.
    /// </summary>
    /// <returns><see langword="true"/> if the connection is alive; otherwise, <see langword="false"/>.</returns>
    bool IsAlive { get; }

    /// <summary>
    ///    Sets the timeout for the agent server to respond.
    /// </summary>
    /// <param name="millisecondsDelay">The time span to wait before the agent server response.</param>
    /// <returns><see langword="true"/> if the timeout was set successfully; otherwise, <see langword="false"/>.</returns>
    bool SetTimeout(long millisecondsDelay);

    /// <param name="delay">The time span to wait before the agent server response.</param>
    /// <inheritdoc cref="SetTimeout(long)"/>
    bool SetTimeout(TimeSpan delay);

    /// <summary>
    ///   Cancels the agent operation with a specified token.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the waiting operation.</param>
    /// <param name="waitFunc">The func that needs to be canceled.</param>
    /// <param name="waitTask">The task that needs to be canceled.</param>
    /// <param name="waitJob">The job that needs to be canceled.</param>
    /// <returns>The return value of <paramref name="waitFunc"/> AND <paramref name="waitTask"/>.Result AND <paramref name="waitJob"/>.IsSucceeded().</returns>
    bool CancelWith(CancellationToken cancellationToken, Func<bool>? waitFunc = null, Task<bool>? waitTask = null, MaaJob? waitJob = null);

    /// <summary>
    ///   Cancels the agent operation.
    /// </summary>
    /// <inheritdoc cref="CancelWith"/>
    bool Cancel(Func<bool>? waitFunc = null, Task<bool>? waitTask = null, MaaJob? waitJob = null);

    /// <summary>
    ///     Represents a method that starts the agent server process.
    /// </summary>
    /// <param name="identifier">The unique identifier used to communicate with the agent server.</param>
    /// <param name="nativeLibraryDirectory">The directory path where the <see cref="MaaFramework"/> native libraries are located.</param>
    /// <returns>
    ///     A new <see cref="Process"/> that is associated with the process resource, or <see langword="null"/> if no process resource is started.
    /// </returns>
    /// <remarks>
    ///     <para>The implementation of this delegate is responsible for validating the provided parameters.</para>
    ///     <para>Ensure that <paramref name="identifier"/> and <paramref name="nativeLibraryDirectory"/> are valid and meet the requirements of the agent server process.</para>
    /// </remarks>
    delegate Process? AgentServerStartupMethod(string identifier, string nativeLibraryDirectory);

    /// <summary>
    ///     Gets the agent server process managed by <see cref="IMaaAgentClient"/> from method LinkStart.
    /// </summary>
    /// <exception cref="InvalidOperationException">The process is unavailable or not managed by <see cref="IMaaAgentClient"/>.</exception>
    Process AgentServerProcess { get; }

    /// <summary>
    ///     Gets the list of custom recognitions registered by the agent server.
    /// </summary>
    IList<string> CustomRecognitionList { get; }

    /// <summary>
    ///     Gets the list of custom actions registered by the agent server.
    /// </summary>
    IList<string> CustomActionList { get; }
}
