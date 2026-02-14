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

    internal sealed class NullAgentClient : MaaAgentClient { internal NullAgentClient() : base(MaaAgentClientHandle.Zero) { } }

    /// <summary>
    ///     Represents a null instance of the <see cref="MaaAgentClient"/> type.
    /// </summary>
    public static MaaAgentClient Null { get; } = new NullAgentClient() { Resource = MaaResource.Null, };

    [ExcludeFromCodeCoverage(Justification = "Test for stateful mode.")]
    internal MaaAgentClient(MaaAgentClientHandle handle)
        : base(invalidHandleValue: MaaAgentClientHandle.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

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
        ArgumentNullException.ThrowIfNull(maa);

        var client = new MaaAgentClient(identifier)
        {
            Tasker = maa,
            Controller = maa.Controller,
            Resource = maa.Resource,
        };
        return client;
    }

    /// <inheritdoc cref="Create(string, MaaTasker)"/>
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

#pragma warning disable S1121 // Assignments should not be made from within sub-expressions

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        // disposing 无关
        Tasker?.Releasing -= OnReleasing;
        Controller?.Releasing -= OnReleasing;
        Resource.Releasing -= OnReleasing;
        KillAndDisposeAgentServerProcess(disposing);
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

    private void OnReleasing(object? sender, EventArgs e)
        => Dispose();

    /// <inheritdoc/>
    IMaaTasker? IMaaAgentClient.Tasker
    {
        get => Tasker;
        set => Tasker = (MaaTasker?)value;
    }

    /// <inheritdoc/>
    public MaaTasker? Tasker
    {
        get;
        set
        {
            field?.Releasing -= OnReleasing;
            if (value is not null and not MaaTasker.NullTasker)
            {
                _ = MaaAgentClientRegisterTaskerSink(Handle, value.Handle).ThrowIfFalse();
                value.Releasing += OnReleasing;
            }
            else if (field is not null and not MaaTasker.NullTasker)
                throw new InvalidOperationException("Null instance or null can only be used for init.");
            field = value;
        }
    }

    /// <inheritdoc/>
    IMaaController? IMaaAgentClient.Controller
    {
        get => Controller;
        set => Controller = (MaaController?)value;
    }

    /// <inheritdoc/>
    public MaaController? Controller
    {
        get;
        set
        {
            field?.Releasing -= OnReleasing;
            if (value is not null and not MaaController.NullController)
            {
                _ = MaaAgentClientRegisterControllerSink(Handle, value.Handle).ThrowIfFalse();
                value.Releasing += OnReleasing;
            }
            else if (field is not null and not MaaController.NullController)
                throw new InvalidOperationException("Null instance or null can only be used for init.");

            field = value;
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
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            field?.Releasing -= OnReleasing;
            if (value is not MaaResource.NullResource)
            {
                _ = MaaAgentClientBindResource(Handle, value.Handle).ThrowIfFalse(MaaInteroperationException.ResourceBindingFailedMessage);
                _ = MaaAgentClientRegisterResourceSink(Handle, value.Handle).ThrowIfFalse();
                value.Releasing += OnReleasing;
            }
            else if (field is not null and not MaaResource.NullResource)
                throw new InvalidOperationException("Null instance can only be used for init.");

            field = value;
        }
    }

#pragma warning restore S1121 // Assignments should not be made from within sub-expressions

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
        ArgumentNullException.ThrowIfNull(method);

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
                && await linkStartTask;
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

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientGetCustomRecognitionList"/>.
    /// </remarks>
    public IList<string> CustomRecognitionList
    {
        get
        {
            _ = MaaStringListBuffer.TryGetList(out var list, h => MaaAgentClientGetCustomRecognitionList(Handle, h)).ThrowIfFalse();
            return list!;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientGetCustomActionList"/>.
    /// </remarks>
    public IList<string> CustomActionList
    {
        get
        {
            _ = MaaStringListBuffer.TryGetList(out var list, h => MaaAgentClientGetCustomActionList(Handle, h)).ThrowIfFalse();
            return list!;
        }
    }
}
