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
    private Process? _agentServerProcess;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ {nameof(Id)} = {Id ?? "<null>"}, {nameof(IsConnected)} = {IsConnected} }}";

    /// <summary>
    ///     Creates a <see cref="MaaAgentClient"/> instance.
    /// </summary>
    /// <param name="identifier">The unique identifier used to communicate with the agent server.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientCreateV2"/>.
    /// </remarks>
    protected MaaAgentClient(string identifier = "")
        : base(invalidHandleValue: MaaAgentClientHandle.Zero)
    {
        _ = MaaStringBuffer.TrySetValue(identifier, true, buffer =>
        {
            var handle = MaaAgentClientCreateV2(buffer);
            SetHandle(handle, needReleased: true);
            return true;
        }).ThrowIfFalse();
    }

    /// <summary>
    ///     Creates a <see cref="MaaAgentClient"/> instance.
    /// </summary>
    /// <param name="identifier">The unique identifier used to communicate with the agent server.</param>
    /// <param name="resource">The resource.</param>
    /// <returns>The <see cref="MaaAgentClient"/> instance.</returns>
    public static MaaAgentClient Create(string identifier, MaaResource resource)
        => new(identifier) { Resource = resource, };

    /// <inheritdoc cref="Create(string, MaaResource)"/>
    public static MaaAgentClient Create(MaaResource resource)
        => new() { Resource = resource, };

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientIdentifier"/>.
    /// </remarks>
    public string? Id
    {
        get
        {
            _ = MaaStringBuffer.TryGetValue(out var str, buffer => MaaAgentClientIdentifier(Handle, buffer));
            return str;
        }
    }

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

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientConnect"/>.
    /// </remarks>
    public bool LinkStart()
        => MaaAgentClientConnect(Handle);

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
            ArgumentException.ThrowIfNullOrEmpty(Id);
            _agentServerProcess?.Dispose();
            _agentServerProcess = method.Invoke(Id, NativeBindingContext.LoadedNativeLibraryDirectory);

            if (_agentServerProcess is null or { HasExited: true })
                return false;
        }

        return LinkStartUnlessProcessExit(_agentServerProcess, cancellationToken).GetAwaiter().GetResult();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientConnect"/>.
    /// </remarks>
    public async Task<bool> LinkStartUnlessProcessExit(Process process, CancellationToken cancellationToken = default)
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
            await cts.CancelAsync().ConfigureAwait(false);
#else
            cts.Cancel();
#endif
        }
    }

    private int _stopping;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientDisconnect"/>.
    /// </remarks>
    public bool LinkStop()
    {
        if (IsConnected && Interlocked.CompareExchange(ref _stopping, 1, 0) == 0)
        {
            return MaaAgentClientDisconnect(Handle);
#pragma warning disable CS0162 // 检测到无法访问的代码
            try
            {
                return MaaAgentClientDisconnect(Handle);
            }
            finally
            {
                _stopping = 0;
            }
#pragma warning restore CS0162 // 检测到无法访问的代码
        }
        return true;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientConnect"/>.
    /// </remarks>
    public bool IsConnected => MaaAgentClientConnected(Handle);

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
