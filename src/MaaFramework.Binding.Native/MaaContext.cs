using MaaFramework.Binding.Buffers;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaContext;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaContext"/>.
/// </summary>
public class MaaContext : IMaaContext<MaaContextHandle>
{
    /// <inheritdoc/>
    public required MaaContextHandle Handle { get; init; }

    /// <summary>
    ///     Creates a <see cref="MaaContext"/> instance.
    /// </summary>
    /// <param name="contextHandle">The MaaContextHandle.</param>
    [SetsRequiredMembers]
    public MaaContext(MaaContextHandle contextHandle)
    {
        if (contextHandle == MaaContextHandle.Zero)
            throw new ArgumentException($"Value cannot be {MaaContextHandle.Zero}.", nameof(contextHandle));
        Handle = contextHandle;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextRunTask"/>.
    /// </remarks>
    public TaskDetail? RunTask(string entry, string pipelineOverride)
    {
        var taskId = MaaContextRunTask(Handle, entry, pipelineOverride);
        return taskId == Interop.Native.MaaDef.MaaInvalidId
            ? null
            : TaskDetail.Query(taskId, Tasker);
    }

    /// <inheritdoc/>
    public RecognitionDetail? RunRecognition(string entry, string pipelineOverride, IMaaImageBuffer image)
        => RunRecognition(entry, pipelineOverride, (MaaImageBuffer)image);

    /// <inheritdoc cref="IMaaContext.RunRecognition"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextRunRecognition"/>.
    /// </remarks>
    public RecognitionDetail? RunRecognition(string entry, string pipelineOverride, MaaImageBuffer image)
    {
        ArgumentNullException.ThrowIfNull(image);
        var recognitionId = MaaContextRunRecognition(Handle, entry, pipelineOverride, image.Handle);
        return recognitionId == Interop.Native.MaaDef.MaaInvalidId
            ? null
            : RecognitionDetail.Query<MaaRectBuffer, MaaImageBuffer, MaaImageListBuffer>(recognitionId, Tasker);
    }

    /// <inheritdoc/>
    public NodeDetail? RunAction(string entry, string pipelineOverride, IMaaRectBuffer recognitionBox, string recognitionDetail)
        => RunAction(entry, pipelineOverride, (MaaRectBuffer)recognitionBox, recognitionDetail);

    /// <inheritdoc cref="IMaaContext.RunAction"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextRunAction"/>.
    /// </remarks>
    public NodeDetail? RunAction(string entry, string pipelineOverride, MaaRectBuffer recognitionBox, string recognitionDetail)
    {
        ArgumentNullException.ThrowIfNull(recognitionBox);
        var nodeId = MaaContextRunAction(Handle, entry, pipelineOverride, recognitionBox.Handle, recognitionDetail);
        return nodeId == Interop.Native.MaaDef.MaaInvalidId
            ? null
            : NodeDetail.Query(nodeId, Tasker);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextOverridePipeline"/>.
    /// </remarks>
    public bool OverridePipeline(string pipelineOverride)
        => MaaContextOverridePipeline(Handle, pipelineOverride);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextOverrideNext"/>.
    /// </remarks>
    public bool OverrideNext(string nodeName, IEnumerable<string> nextList)
        => MaaStringListBuffer.TrySetList(nextList, listBuffer
            => MaaContextOverrideNext(Handle, nodeName, listBuffer));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextGetTaskId"/>.
    /// </remarks>
    public MaaTaskJob TaskJob => new(MaaContextGetTaskId(Handle), Tasker);

    IMaaTasker IMaaContext.Tasker => Tasker;

    /// <inheritdoc cref="IMaaContext.Tasker"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextGetTasker"/>.
    /// </remarks>
    public MaaTasker Tasker => MaaTasker.Instances[MaaContextGetTasker(Handle)];

    object ICloneable.Clone()
        => Clone();
    IMaaContext IMaaContext.Clone()
        => Clone();

    /// <inheritdoc cref="IMaaContext.Clone"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextClone"/>.
    /// </remarks>
    public MaaContext Clone()
        => new(MaaContextClone(Handle));
}
