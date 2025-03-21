using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Interop.Native;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaAgentClient;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaAgentClient"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaAgentClient : MaaDisposableHandle<MaaAgentClientHandle>, IMaaAgentClient<MaaAgentClientHandle>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)] private string DebuggerDisplay => $"{{{GetType().Name} {{ Disposed = {IsInvalid} }}}}";

    /// <summary>
    ///     Creates a <see cref="MaaAgentClient"/> instance.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientCreate"/>.
    /// </remarks>
    public MaaAgentClient()
        : base(invalidHandleValue: MaaAgentClientHandle.Zero)
    {
        var handle = MaaAgentClientCreate();
        SetHandle(handle, needReleased: true);
    }

    /// <param name="resource">The resource.</param>
    /// <param name="disposeOptions">The dispose options.</param>
    /// <inheritdoc cref="MaaAgentClient()"/>
    [SetsRequiredMembers]
    public MaaAgentClient(MaaResource resource, DisposeOptions disposeOptions)
        : this()
    {
        Resource = resource;
        DisposeOptions = disposeOptions;
    }

    /// <inheritdoc/>
    public required DisposeOptions DisposeOptions { get; set; }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
    {
        // Cannot destroy Instance before disposing Resource.

        if (DisposeOptions.HasFlag(DisposeOptions.Resource))
        {
            Resource.Dispose();
        }

        MaaAgentClientDestroy(Handle);
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
    ///     Wrapper of <see cref="MaaAgentClientCreateSocket"/>.
    /// </remarks>
    public bool CreateSocket(string identifier)
    {
        var handle = MaaStringBufferCreate();
        var ret = MaaStringBufferSetEx(Handle, identifier) && MaaAgentClientCreateSocket(Handle, handle);
        MaaStringBufferDestroy(handle);
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientConnect"/>.
    /// </remarks>
    public bool LinkStart()
        => MaaAgentClientConnect(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentClientDisconnect"/>.
    /// </remarks>
    public bool LinkStop()
        => MaaAgentClientDisconnect(Handle);
}

