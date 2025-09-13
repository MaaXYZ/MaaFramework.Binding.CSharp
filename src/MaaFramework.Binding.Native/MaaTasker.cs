using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Interop.Native;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static MaaFramework.Binding.Interop.Native.MaaTasker;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaTasker"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaTasker : MaaCommon, IMaaTasker<MaaTaskerHandle>, IMaaPost
{
    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ {nameof(IsRunning)} = {IsRunning}, {nameof(IsInitialized)} = {IsInitialized}, {nameof(DisposeOptions)} = {DisposeOptions} }}";

    /// <summary>
    ///     Gets all maa tasker instances.
    /// </summary>
    /// <remarks>
    ///     A property used to simplify design of <see cref="MaaContext.Tasker"/>.
    /// </remarks>
    protected internal static ConcurrentDictionary<MaaTaskerHandle, MaaTasker> Instances { get; } = [];

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    [SetsRequiredMembers]
    [ExcludeFromCodeCoverage(Justification = "Test for stateful mode.")]
    internal MaaTasker(MaaTaskerHandle handle)
    {
        SetHandle(handle, needReleased: false);
        _resource = new MaaResource(MaaTaskerGetResource(handle));
        _controller = new MaaController(MaaTaskerGetController(handle));
        DisposeOptions = DisposeOptions.None;
        Toolkit = MaaToolkit.Shared;
        Utility = MaaUtility.Shared;
    }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。

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
            // Always returns true, but non-atomic operation may fail to add.
            throw new InvalidOperationException($"This {nameof(MaaTasker)} already added to {nameof(Instances)}.");
        SetHandle(handle, needReleased: true);

        Toolkit = MaaToolkit.Shared;
        Utility = MaaUtility.Shared;
        if (toolkitInit)
            _ = Toolkit.Config.InitOption().ThrowIfFalse();
    }

    /// <param name="controller">The controller.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="disposeOptions">The dispose options.</param>
    /// <param name="toolkitInit">Whether initializes the <see cref="Toolkit"/>.</param>
    /// <inheritdoc cref="Binding.MaaTasker(bool)"/>
    [SetsRequiredMembers]
    public MaaTasker(MaaController controller, MaaResource resource, DisposeOptions disposeOptions, bool toolkitInit = false)
        : this(toolkitInit)
    {
        Resource = resource;
        Controller = controller;
        DisposeOptions = disposeOptions;
    }

    /// <inheritdoc/>
    public required DisposeOptions DisposeOptions { get; set; }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing) return;

        if (DisposeOptions.HasFlag(DisposeOptions.Controller))
            Controller.Dispose();

        if (DisposeOptions.HasFlag(DisposeOptions.Resource))
            Resource.Dispose();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle(MaaTaskerHandle handle)
    {
        try
        {
            if (LastJob != null)
                _ = MaaTaskerWait(handle, LastJob.Id);
            _ = Instances.TryRemove(new KeyValuePair<MaaTaskerHandle, MaaTasker>(handle, this));
        }
        finally
        {
            MaaTaskerDestroy(handle);
        }
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

    private MaaResource _resource = null!;
    private MaaController _controller = null!;

    IMaaResource IMaaTasker.Resource
    {
        get => Resource;
        set => Resource = (MaaResource)value;
    }

    IMaaController IMaaTasker.Controller
    {
        get => Controller;
        set => Controller = (MaaController)value;
    }

    /// <inheritdoc cref="IMaaTasker.Resource"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerBindResource"/> and <see cref="MaaTaskerGetResource"/>.
    /// </remarks>
    public required MaaResource Resource
    {
        get
        {
            if (!IsInvalid)
                _ = MaaTaskerGetResource(Handle).ThrowIfNotEquals(_resource.Handle, MaaInteroperationException.ResourceModifiedMessage);
            return _resource;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _ = MaaTaskerBindResource(Handle, value.Handle).ThrowIfFalse(MaaInteroperationException.ResourceBindingFailedMessage);
            _resource = value;
        }
    }

    /// <inheritdoc cref="IMaaTasker.Controller"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerBindController"/> and <see cref="MaaTaskerGetController"/>.
    /// </remarks>
    public required MaaController Controller
    {
        get
        {
            if (!IsInvalid)
                _ = MaaTaskerGetController(Handle).ThrowIfNotEquals(_controller.Handle, MaaInteroperationException.ControllerModifiedMessage);
            return _controller;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _ = MaaTaskerBindController(Handle, value.Handle).ThrowIfFalse(MaaInteroperationException.ControllerBindingFailedMessage);
            _controller = value;
        }
    }

    IMaaToolkit IMaaTasker.Toolkit { get; set; } = default!;
    IMaaUtility IMaaTasker.Utility { get; set; } = default!;

    /// <inheritdoc cref="IMaaTasker.Toolkit"/>
    public MaaToolkit Toolkit { get => field; set => ((IMaaTasker)this).Toolkit = field = value; }

    /// <inheritdoc cref="IMaaTasker.Utility"/>
    public MaaUtility Utility { get => field; set => ((IMaaTasker)this).Utility = field = value; }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerInited"/>.
    /// </remarks>
    public bool IsInitialized => MaaTaskerInited(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerPostTask"/>.
    /// </remarks>
    public MaaTaskJob AppendTask(string entry, [StringSyntax("Json")] string pipelineOverride = "{}")
        => CreateJob(MaaTaskerPostTask(Handle, entry, pipelineOverride));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerStatus"/>.
    /// </remarks>
    [Obsolete("Deprecated from v4.5.0.")]
    public MaaJobStatus GetStatus(MaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        var id = job.Id;
        var handle = Handle;
        return IsInvalid ? MaaJobStatus.Invalid : (MaaJobStatus)MaaTaskerStatus(handle, id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerWait"/>.
    /// </remarks>
    [Obsolete("Deprecated from v4.5.0.")]
    public MaaJobStatus Wait(MaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        var id = job.Id;
        var handle = Handle;
        return IsInvalid ? MaaJobStatus.Invalid : (MaaJobStatus)MaaTaskerWait(handle, id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private MaaTaskJob CreateJob(MaaTaskId id)
    {
        var job = new MaaTaskJob(id, this, this);
        if (id != MaaDef.MaaInvalidId)
            LastJob = job;
        return job;
    }

    /// <inheritdoc/>
    public MaaTaskJob? LastJob { get; private set; }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerRunning"/>.
    /// </remarks>
    public bool IsRunning => MaaTaskerRunning(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerStopping"/>.
    /// </remarks>
    public bool IsStopping => MaaTaskerStopping(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerPostStop"/>.
    /// </remarks>
    public MaaTaskJob Stop()
        => CreateJob(MaaTaskerPostStop(Handle));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerPostStop"/>.
    /// </remarks>
    [Obsolete("Use Stop() instead.")]
    [ExcludeFromCodeCoverage(Justification = "Use Stop() instead.")]
    public MaaTaskJob Abort() => Stop();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerClearCache"/>.
    /// </remarks>
    public bool ClearCache()
        => MaaTaskerClearCache(Handle);

    /// <inheritdoc/>
    public bool GetRecognitionDetail<T>(long recognitionId, out string nodeName, out string algorithm, out bool hit, IMaaRectBuffer? hitBox, out string detailJson, IMaaImageBuffer? raw, IMaaListBuffer<T>? draws)
        where T : IMaaImageBuffer
        => GetRecognitionDetail(recognitionId, out nodeName, out algorithm, out hit, (MaaRectBuffer?)hitBox, out detailJson, (MaaImageBuffer?)raw, (MaaImageListBuffer?)draws);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerGetRecognitionDetail"/>.
    /// </remarks>
    public bool GetRecognitionDetail(MaaRecoId recognitionId, out string nodeName, out string algorithm, out bool hit, MaaRectBuffer? hitBox, out string detailJson, MaaImageBuffer? raw, MaaImageListBuffer? draws)
    {
        var hitBoxHandle = hitBox?.Handle ?? MaaRectHandle.Zero;
        var rawHandle = raw?.Handle ?? MaaImageBufferHandle.Zero;
        var drawsHandle = draws?.Handle ?? MaaImageListBufferHandle.Zero;

        using var nodeNameBuffer = new MaaStringBuffer();
        using var algorithmBuffer = new MaaStringBuffer();
        using var detailJsonBuffer = new MaaStringBuffer();

        var ret = MaaTaskerGetRecognitionDetail(Handle, recognitionId, nodeNameBuffer.Handle, algorithmBuffer.Handle, out hit, hitBoxHandle, detailJsonBuffer.Handle, rawHandle, drawsHandle);

        nodeName = nodeNameBuffer.ToString();
        algorithm = algorithmBuffer.ToString();
        detailJson = detailJsonBuffer.ToString();
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerGetNodeDetail"/>.
    /// </remarks>
    public bool GetNodeDetail(MaaNodeId nodeId, out string nodeName, out MaaRecoId recognitionId, out bool actionCompleted)
    {
        using var nodeNameBuffer = new MaaStringBuffer();

        var ret = MaaTaskerGetNodeDetail(Handle, nodeId, nodeNameBuffer.Handle, out recognitionId, out actionCompleted);

        nodeName = nodeNameBuffer.ToString();
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskerGetTaskDetail"/>.
    /// </remarks>
    public bool GetTaskDetail(MaaTaskId taskId, out string entry, out MaaNodeId[] nodeIdList, out MaaJobStatus status)
    {
        MaaSize nodeIdListSize = 0;
        using var entryBuffer = new MaaStringBuffer();

        if (!MaaTaskerGetTaskDetail(Handle, taskId, entryBuffer.Handle, null, ref nodeIdListSize, out var statusInt))
        {
            entry = string.Empty;
            nodeIdList = [];
            status = (MaaJobStatus)statusInt;
            return false;
        }

        entry = entryBuffer.ToString();
        nodeIdList = nodeIdListSize == 0 ? [] : new MaaNodeId[nodeIdListSize];
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
