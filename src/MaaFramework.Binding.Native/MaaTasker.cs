using MaaFramework.Binding.Abstractions.Native;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Interop.Native;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaTasker;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaTasker"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaTasker : MaaCommon, IMaaTasker<nint>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)] private string DebuggerDisplay => $"{{{GetType().Name} {{ Disposed = {IsInvalid} }}}}";

#pragma warning disable CA2213
    private IMaaResource<nint> _resource = default!;
    private IMaaController<nint> _controller = default!;
#pragma warning restore CA2213

    /// <summary>
    ///     Gets all maa tasker instances.
    /// </summary>
    /// <remarks>
    ///     A property used to simplify design of <see cref="MaaContext.Tasker"/>.
    /// </remarks>
    protected internal static ConcurrentDictionary<MaaTaskerHandle, MaaTasker> Instances { get; } = [];

    /// <summary>
    ///     Creates a <see cref="MaaTasker"/> instance.
    /// </summary>
    /// <param name="toolkitInit">Whether initializes the <see cref="Toolkit"/>.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerCreate"/>.
    /// </remarks>
    public MaaTasker(bool toolkitInit = false)
    {
        var handle = MaaTaskerCreate(MaaNotificationCallback, nint.Zero);
        if (!Instances.TryAdd(handle, this))
            throw new InvalidOperationException($"This {nameof(MaaTasker)} already added to {nameof(Instances)}."); // Always returns true, but non-atomic operation may fail to add.
        SetHandle(handle, needReleased: true);

        Toolkit = new MaaToolkit(toolkitInit);
        Utility = new MaaUtility();
    }

    /// <param name="controller">The controller.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="disposeOptions">The dispose options.</param>
    /// <param name="toolkitInit">Whether initializes the <see cref="Toolkit"/>.</param>
    /// <inheritdoc cref="MaaTasker(bool)"/>
    [SetsRequiredMembers]
    public MaaTasker(IMaaController<nint> controller, IMaaResource<nint> resource, DisposeOptions disposeOptions, bool toolkitInit = false)
        : this(toolkitInit)
    {
        Resource = resource;
        Controller = controller;
        DisposeOptions = disposeOptions;
    }

    /// <inheritdoc/>
    public required DisposeOptions DisposeOptions { get; set; }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
    {
        // Cannot destroy Instance before disposing Controller and Resource.

        if (DisposeOptions.HasFlag(DisposeOptions.Controller))
        {
            Controller.Dispose();
        }

        if (DisposeOptions.HasFlag(DisposeOptions.Resource))
        {
            Resource.Dispose();
        }

        MaaTaskerDestroy(Handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerSetOption"/>.
    /// </remarks>
    public bool SetOption<T>(TaskerOption opt, T value)
    {
        ArgumentNullException.ThrowIfNull(value);
        throw new NotSupportedException($"'{nameof(TaskerOption)}.{opt}' or type '{typeof(T)}' is not supported.");

#pragma warning disable
        var optValue = (value, opt) switch
        {
            (int vvvv, TaskerOption.Invalid) => vvvv.ToMaaOptionValue(),
            _ => throw new NotSupportedException($"'{nameof(TaskerOption)}.{opt}' or type '{typeof(T)}' is not supported."),
        };

        return MaaTaskerSetOption(Handle, (MaaTaskerOption)opt, optValue, (MaaOptionValueSize)optValue.Length);
#pragma warning restore
    }

    /// <inheritdoc/>
    IMaaResource IMaaTasker.Resource
    {
        get => Resource;
        set => Resource = (IMaaResource<nint>)value;
    }

    /// <inheritdoc/>
    IMaaController IMaaTasker.Controller
    {
        get => Controller;
        set => Controller = (IMaaController<nint>)value;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerBindResource"/> and <see cref="MaaTaskerGetResource"/>.
    /// </remarks>
    public required IMaaResource<nint> Resource
    {
        get
        {
            MaaTaskerGetResource(Handle).ThrowIfNotEquals(_resource.Handle, MaaInteroperationException.ResourceModifiedMessage);
            return _resource;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            MaaTaskerBindResource(Handle, value.Handle).ThrowIfFalse(MaaInteroperationException.ResourceBindingFailedMessage);
            _resource = value;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerBindController"/> and <see cref="MaaTaskerGetController"/>.
    /// </remarks>
    public required IMaaController<nint> Controller
    {
        get
        {
            MaaTaskerGetController(Handle).ThrowIfNotEquals(_controller.Handle, MaaInteroperationException.ControllerModifiedMessage);
            return _controller;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            MaaTaskerBindController(Handle, value.Handle).ThrowIfFalse(MaaInteroperationException.ControllerBindingFailedMessage);
            _controller = value;
        }
    }

    /// <inheritdoc/>
    public IMaaToolkit Toolkit { get; set; }

    /// <inheritdoc/>
    public IMaaUtility Utility { get; set; }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerInited"/>.
    /// </remarks>
    public bool Initialized => MaaTaskerInited(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerPostTask"/>.
    /// </remarks>
    public MaaTaskJob AppendTask(string entry, string pipelineOverride = "{}")
    {
        var id = MaaTaskerPostTask(Handle, entry, pipelineOverride);
        return new MaaTaskJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(MaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return (MaaJobStatus)MaaTaskerStatus(Handle, job.Id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerWait"/>.
    /// </remarks>
    public MaaJobStatus Wait(MaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return (MaaJobStatus)MaaTaskerWait(Handle, job.Id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerRunning"/>.
    /// </remarks>
    public bool Running => MaaTaskerRunning(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerPostStop"/>.
    /// </remarks>
    public MaaTaskJob Abort()
    {
        var id = MaaTaskerPostStop(Handle);
        return new MaaTaskJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerClearCache"/>.
    /// </remarks>
    public bool ClearCache()
        => MaaTaskerClearCache(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerGetRecognitionDetail"/>.
    /// </remarks>
    public bool GetRecognitionDetail<T>(MaaRecoId recognitionId, out string name, out string algorithm, out bool hit, IMaaRectBuffer? hitBox, out string detailJson, T? raw, IMaaListBuffer<T>? draws)
        where T : IMaaImageBuffer, new()
    {
        var hitBoxHandle = (hitBox as IMaaRectBuffer<nint>)?.Handle ?? MaaRectHandle.Zero;
        var rawHandle = (raw as IMaaImageBuffer<nint>)?.Handle ?? MaaImageBufferHandle.Zero;
        var drawsHandle = (draws as IMaaListBuffer<nint, T>)?.Handle ?? MaaImageListBufferHandle.Zero;

        using var nameBuffer = new MaaStringBuffer();
        using var algorithmBuffer = new MaaStringBuffer();
        using var detailJsonBuffer = new MaaStringBuffer();

        var ret = MaaTaskerGetRecognitionDetail(Handle, recognitionId, nameBuffer.Handle, algorithmBuffer.Handle, out hit, hitBoxHandle, detailJsonBuffer.Handle, rawHandle, drawsHandle);

        name = nameBuffer.ToString();
        algorithm = algorithmBuffer.ToString();
        detailJson = detailJsonBuffer.ToString();
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerGetNodeDetail"/>.
    /// </remarks>
    public bool GetNodeDetail(MaaNodeId nodeId, out string name, out MaaRecoId recognitionId, out bool actionCompleted)
    {
        using var nameBuffer = new MaaStringBuffer();

        var ret = MaaTaskerGetNodeDetail(Handle, nodeId, nameBuffer.Handle, out recognitionId, out actionCompleted);

        name = nameBuffer.ToString();
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerGetTaskDetail"/>.
    /// </remarks>
    public bool GetTaskDetail(MaaTaskId taskId, out string entry, out MaaNodeId[] nodeIdList, out MaaJobStatus status)
    {
        entry = string.Empty;
        nodeIdList = [];

        MaaSize nodeIdListSize = 0;
        using var entryBuffer = new MaaStringBuffer();

        if (!MaaTaskerGetTaskDetail(Handle, taskId, entryBuffer.Handle, null, ref nodeIdListSize, out var statusInt))
        {
            status = (MaaJobStatus)statusInt;
            return false;
        }

        entry = entryBuffer.ToString();
        nodeIdList = new MaaNodeId[nodeIdListSize];
        var ret = MaaTaskerGetTaskDetail(Handle, taskId, entryBuffer.Handle, nodeIdList, ref nodeIdListSize, out statusInt);
        status = (MaaJobStatus)statusInt;
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerGetLatestNode"/>.
    /// </remarks>
    public bool GetLatestNode(string nodeName, out MaaNodeId latestId)
        => MaaTaskerGetLatestNode(Handle, nodeName, out latestId);
}
