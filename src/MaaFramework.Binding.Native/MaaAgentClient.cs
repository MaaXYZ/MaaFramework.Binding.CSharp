using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Interop.Native;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaAgentClient;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaAgentClient"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaAgentClient : MaaDisposableHandle<MaaAgentClientHandle>, IMaaAgentClient<MaaAgentClientHandle>
{
    private bool _isConnected;
    private Process? _agentServerProcess;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ Id = {Id}, IsConnected = {_isConnected} }}";

    /// <summary>
    ///     Creates a <see cref="MaaAgentClient"/> instance.
    /// </summary>
    /// <param name="identifier">The unique identifier used to communicate with the agent server.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientCreate"/>.
    /// </remarks>
    public MaaAgentClient(string identifier = "")
        : base(invalidHandleValue: MaaAgentClientHandle.Zero)
    {
        var handle = MaaAgentClientCreate();
        SetHandle(handle, needReleased: true);
        Id = CreateSocket(identifier).ThrowIfNull();

        if (!string.IsNullOrEmpty(identifier))
            _ = Id.ThrowIfNotEquals(identifier);
    }

    /// <param name="identifier">The unique identifier used to communicate with the agent server.</param>
    /// <param name="resource">The resource.</param>
    /// <inheritdoc cref="MaaAgentClient(string)"/>
    [SetsRequiredMembers]
    public MaaAgentClient(string identifier, MaaResource resource)
        : this(identifier)
    {
        Resource = resource;
    }

    /// <inheritdoc cref="MaaAgentClient(string, MaaResource)"/>
    [SetsRequiredMembers]
    public MaaAgentClient(MaaResource resource)
        : this("", resource)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaAgentClient"/> instance.
    /// </summary>
    /// <param name="identifier">The unique identifier used to communicate with the agent server.</param>
    /// <param name="resource">The resource.</param>
    /// <returns>The <see cref="MaaAgentClient"/> instance.</returns>
    public static MaaAgentClient Create(string identifier, MaaResource resource)
        => new(identifier, resource);

    /// <inheritdoc cref="Create(string, MaaResource)"/>
    public static MaaAgentClient Create(MaaResource resource)
        => new(resource);

    /// <inheritdoc/>
    public string Id { get; }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        KillAndDisposeAgentServerProcess();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaAgentClientDestroy(Handle);

    /// <inheritdoc/>
    IMaaResource IMaaAgentClient.Resource
    {
        get => Resource;
        set => Resource = (MaaResource)value;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientBindResource"/>.
    /// </remarks>
    public required MaaResource Resource
    {
        get => field;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _ = MaaAgentClientBindResource(Handle, value.Handle).ThrowIfFalse(MaaInteroperationException.ResourceBindingFailedMessage);
            field = value;
        }
    }

    /// <summary>
    ///     Creates a socket connection with the specified identifier.
    /// </summary>
    /// <param name="identifier">The specified identifier.</param>
    /// <returns><see langword="true"/> if the socket was created successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientCreateSocket"/>.
    /// </remarks>
    protected string? CreateSocket(string identifier = "")
        => MaaStringBuffer.TryGetValue(out var socketId, handle
            => MaaStringBuffer.TrySetValue(handle, identifier)
            && MaaAgentClientCreateSocket(Handle, handle))
        ? socketId
        : null;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientConnect"/>.
    /// </remarks>
    public bool LinkStart()
        => _isConnected = MaaAgentClientConnect(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientConnect"/>.
    /// </remarks>
    public bool LinkStart(ProcessStartInfo info, CancellationToken cancellationToken = default)
    {
        if (_agentServerProcess is null or { HasExited: true })
        {
            _agentServerProcess?.Dispose();
            _agentServerProcess = Process.Start(info);

            if (_agentServerProcess is null or { HasExited: true })
                return false;
        }

        return LinkStartUnlessProcessExit(_agentServerProcess, cancellationToken).GetAwaiter().GetResult();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientConnect"/>.
    /// </remarks>
    public bool LinkStart(IMaaAgentClient.AgentServerStartupMethod method, CancellationToken cancellationToken = default)
    {
        if (_agentServerProcess is null or { HasExited: true })
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(NativeBindingInfo.NativeAssemblyDirectory))
            {
                throw new InvalidOperationException(
                    $"The {nameof(Id)}({Id ?? "<null>"})" +
                    $" or {nameof(NativeBindingInfo.NativeAssemblyDirectory)}({NativeBindingInfo.NativeAssemblyDirectory ?? "<null>"})" +
                    $" is invalid.");
            }

            _agentServerProcess?.Dispose();
            _agentServerProcess = method.Invoke(Id, NativeBindingInfo.NativeAssemblyDirectory);

            if (_agentServerProcess is null or { HasExited: true })
                return false;
        }

        return LinkStartUnlessProcessExit(_agentServerProcess, cancellationToken).GetAwaiter().GetResult();
    }

    /// <inheritdoc/>
    public async Task<bool> LinkStartUnlessProcessExit(Process process, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(process);
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        if (process.HasExited)
            return false;

        var serverExitTask = process.WaitForExitAsync(cts.Token);
        var linkStartTask = Task.Run(LinkStart, cts.Token);
        var completedTask = await Task.WhenAny(linkStartTask, serverExitTask).ConfigureAwait(false);

        try
        {
            cts.Token.ThrowIfCancellationRequested();
            if (completedTask == serverExitTask)
                return false;

            return linkStartTask.Result;
        }
        finally
        {
#if NET8_0_OR_GREATER
            await cts.CancelAsync();
#else
            cts.Cancel();
#endif
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientDisconnect"/>.
    /// </remarks>
    public bool LinkStop()
    {
        if (_isConnected)
        {
            _isConnected = false;
            if (!MaaAgentClientDisconnect(Handle))
            {
                _isConnected = true;
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc/>
    public Process AgentServerProcess => _agentServerProcess
            ?? throw new InvalidOperationException($"The agent server process is unavailable or not managed by {nameof(MaaAgentClient)}.");

    private void KillAndDisposeAgentServerProcess()
    {
        if (_agentServerProcess is null)
            return;

        if (!_agentServerProcess.HasExited)
        {
            _agentServerProcess.Kill(entireProcessTree: true);
            _agentServerProcess.WaitForExit();
        }

        _agentServerProcess.Dispose();
        _agentServerProcess = null;
    }
}
