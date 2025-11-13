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
    private long _timeout = -1;
    private Process? _agentServerProcess;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ {nameof(Id)} = {Id ?? "<null>"}, {nameof(IsConnected)} = {IsConnected}, {nameof(IsAlive)} = {IsAlive}, Timeout = {_timeout} }}";

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
        => Create(string.Empty, resource);

    /// <summary>
    ///     Creates a <see cref="MaaAgentClient"/> instance, the callback (from tasker, resource and controller) will be forwarded to the agent server.
    /// </summary>
    /// <param name="identifier">The unique identifier used to communicate with the agent server.</param>
    /// <param name="maa">The tasker inclued resource and controller.</param>
    /// <returns>The <see cref="MaaAgentClient"/> instance.</returns>
    public static MaaAgentClient Create(string identifier, MaaTasker maa)
    {
        var client = new MaaAgentClient(identifier) { Resource = maa.Resource, };
        _ = MaaAgentClientRegisterTaskerSink(client.Handle, maa.Handle);
        _ = MaaAgentClientRegisterResourceSink(client.Handle, maa.Resource.Handle);
        _ = MaaAgentClientRegisterControllerSink(client.Handle, maa.Controller.Handle);
        return client;

    }

    /// <inheritdoc cref="Create(string, MaaResource)"/>
    public static MaaAgentClient Create(MaaTasker maa)
        => Create(string.Empty, maa);

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

    private void OnResourceReleasing(object? sender, EventArgs e)
        => Dispose();

    /// <inheritdoc/>
    public IMaaAgentClient AttachDisposeToResource()
    {
        Resource.Releasing += OnResourceReleasing;
        return this;
    }

    /// <inheritdoc/>
    public IMaaAgentClient DetachDisposeToResource()
    {
        Resource.Releasing -= OnResourceReleasing;
        return this;
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        KillAndDisposeAgentServerProcess(disposing); // disposing 无关
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle(MaaAgentClientHandle handle)
    {
        try
        {
            _ = MaaAgentClientDisconnect(handle);
        }
        finally
        {
            MaaAgentClientDestroy(handle);
        }
    }

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
            _ = Cancel(waitTask: linkStartTask);
            cts.Token.ThrowIfCancellationRequested();

            return completedTask != serverExitTask
                && linkStartTask.Result;
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

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientDisconnect"/>.
    /// </remarks>
    public bool LinkStop()
        => MaaAgentClientDisconnect(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientConnect"/>.
    /// </remarks>
    public bool IsConnected => MaaAgentClientConnected(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientAlive"/>.
    /// </remarks>
    public bool IsAlive => MaaAgentClientAlive(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientSetTimeout"/>.
    /// </remarks>
    public bool SetTimeout(long millisecondsDelay)
    {
        _timeout = millisecondsDelay;
        return MaaAgentClientSetTimeout(Handle, _timeout);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientSetTimeout"/>.
    /// </remarks>
    public bool SetTimeout(TimeSpan delay)
        => SetTimeout((long)delay.TotalMilliseconds);

    private static bool NeedToCancel(Func<bool>? waitFunc, Task<bool>? waitTask, MaaJob? waitJob)
    {
        if (waitFunc is not null)
            return true;
        if (waitTask is not null && !waitTask.IsCompleted)
            return true;
        if (waitJob is not null && !waitJob.Status.IsDone())
            return true;

        return false;
    }

    private static bool WaitForCancellation(Func<bool>? waitFunc = null, Task<bool>? waitTask = null, MaaJob? waitJob = null)
    {
        var ret = true;
        if (waitFunc is not null)
            ret &= waitFunc.Invoke();
        if (waitTask is not null)
            ret &= waitTask.GetAwaiter().GetResult();
        if (waitJob is not null)
            ret &= waitJob.Wait().IsSucceeded();
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientSetTimeout"/>.
    /// </remarks>
    public bool CancelWith(CancellationToken cancellationToken, Func<bool>? waitFunc = null, Task<bool>? waitTask = null, MaaJob? waitJob = null)
    {
        if (!NeedToCancel(waitFunc, waitTask, waitJob))
            return true;

        var ctr = cancellationToken.Register(() => SetTimeout(0));
        try
        {
            return WaitForCancellation(waitFunc, waitTask, waitJob);
        }
        finally
        {
            ctr.Dispose();
            _ = SetTimeout(_timeout).ThrowIfFalse();
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientSetTimeout"/>.
    /// </remarks>
    public bool Cancel(Func<bool>? waitFunc = null, Task<bool>? waitTask = null, MaaJob? waitJob = null)
    {
        if (!NeedToCancel(waitFunc, waitTask, waitJob))
            return true;

        _ = SetTimeout(0).ThrowIfFalse();
        try
        {
            return WaitForCancellation(waitFunc, waitTask, waitJob);
        }
        finally
        {
            _ = SetTimeout(_timeout).ThrowIfFalse();
        }
    }

    /// <inheritdoc/>
    public Process AgentServerProcess => _agentServerProcess
            ?? throw new InvalidOperationException($"The agent server process is unavailable or not managed by {nameof(MaaAgentClient)}.");

    private void KillAndDisposeAgentServerProcess(bool disposing)
    {
        if (_agentServerProcess is null)
            return;

        try
        {
            if (!_agentServerProcess.HasExited)
            {
                _agentServerProcess.Kill(entireProcessTree: true);
                _agentServerProcess.WaitForExit();
            }
        }
        catch (Exception ex)
        {
            Trace.TraceWarning("Failed to kill the agent server process: {0}.", _agentServerProcess.Id);
            Trace.TraceError(ex.ToString());
            if (disposing)
                throw;
        }
        finally
        {
            _agentServerProcess.Dispose();
            _agentServerProcess = null;
        }
    }
}
