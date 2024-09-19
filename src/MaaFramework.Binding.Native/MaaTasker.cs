using MaaFramework.Binding.Abstractions.Native;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Interop.Native;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaTasker;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaTasker"/>.
/// </summary>
public class MaaTasker : MaaCommon, IMaaTasker<nint>
{
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
    protected internal static Dictionary<MaaTaskerHandle, MaaTasker> Instances { get; } = [];

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
        SetHandle(handle, needReleased: true);
        Instances.Add(handle, this);

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
        throw new InvalidOperationException();
        /*
        var optValue = (value, opt) switch
        {
            (int vvvv, TaskerOption.Invalid) => vvvv.ToMaaOptionValue(),
            _ => throw new InvalidOperationException(),
        };

        return MaaTaskerSetOption(Handle, (MaaTaskerOption)opt, optValue, (MaaOptionValueSize)optValue.Length).ToBoolean();
        */
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
            MaaBindException.ThrowIf(
                MaaTaskerGetResource(Handle) != _resource.Handle,
                MaaBindException.ResourceModifiedMessage);
            return _resource;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            MaaBindException.ThrowIf(
                !MaaTaskerBindResource(Handle, value.Handle).ToBoolean(),
                MaaBindException.ResourceMessage);
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
            MaaBindException.ThrowIf(
                MaaTaskerGetController(Handle) != _controller.Handle,
                MaaBindException.ControllerModifiedMessage);
            return _controller;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            MaaBindException.ThrowIf(
                !MaaTaskerBindController(Handle, value.Handle).ToBoolean(),
                MaaBindException.ControllerMessage);
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
    public bool Initialized => MaaTaskerInited(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerPostPipeline"/>.
    /// </remarks>
    public MaaTaskJob AppendPipeline(string entry, string pipelineOverride = "{}")
    {
        var id = MaaTaskerPostPipeline(Handle, entry, pipelineOverride);
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
    public bool Running => MaaTaskerRunning(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerPostStop"/>.
    /// </remarks>
    public bool Abort()
        => MaaTaskerPostStop(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerClearCache"/>.
    /// </remarks>
    public bool ClearCache()
        => MaaTaskerClearCache(Handle).ToBoolean();

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

        var ret = MaaTaskerGetRecognitionDetail(Handle, recognitionId, nameBuffer.Handle, algorithmBuffer.Handle, out var hitByte, hitBoxHandle, detailJsonBuffer.Handle, rawHandle, drawsHandle).ToBoolean();

        name = nameBuffer.ToString();
        algorithm = algorithmBuffer.ToString();
        hit = hitByte.ToBoolean();
        detailJson = detailJsonBuffer.ToString();
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerGetNodeDetail"/>.
    /// </remarks>
    public bool GetNodeDetail(MaaNodeId nodeId, out string name, out MaaRecoId recognitionId, out MaaSize times, out bool actionCompleted)
    {
        using var nameBuffer = new MaaStringBuffer();

        var ret = MaaTaskerGetNodeDetail(Handle, nodeId, nameBuffer.Handle, out recognitionId, out times, out var completed).ToBoolean();

        name = nameBuffer.ToString();
        actionCompleted = completed.ToBoolean();
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerGetTaskDetail"/>.
    /// </remarks>
    public bool GetTaskDetail(MaaTaskId taskId, out string entry, out MaaNodeId[] nodeIdList)
    {
        entry = string.Empty;
        nodeIdList = [];

        MaaSize nodeIdListSize = 0;
        using var entryBuffer = new MaaStringBuffer();

        if (!MaaTaskerGetTaskDetail(Handle, taskId, entryBuffer.Handle, null, ref nodeIdListSize).ToBoolean())
            return false;

        entry = entryBuffer.ToString();
        nodeIdList = new MaaNodeId[nodeIdListSize];
        return MaaTaskerGetTaskDetail(Handle, taskId, entryBuffer.Handle, nodeIdList, ref nodeIdListSize).ToBoolean();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerGetLatestNode"/>.
    /// </remarks>
    public bool GetLatestNode(string taskName, out MaaNodeId latestId)
        => MaaTaskerGetLatestNode(Handle, taskName, out latestId).ToBoolean();
}
