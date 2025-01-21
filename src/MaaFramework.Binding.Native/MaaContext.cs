using MaaFramework.Binding.Buffers;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaContext;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaContext"/>.
/// </summary>
public class MaaContext : IMaaContext<nint>
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
        => TaskDetail.Query(
            taskId: MaaContextRunTask(Handle, entry, pipelineOverride),
            tasker: Tasker);

    /// <inheritdoc/>
    public RecognitionDetail? RunRecognition(string entry, string recognitionOverride, IMaaImageBuffer image)
        => RunRecognition(entry, recognitionOverride, (MaaImageBuffer)image);

    /// <inheritdoc/>
    public RecognitionDetail? RunRecognition(string entry, string recognitionOverride, IMaaImageBuffer<nint> image)
        => RunRecognition(entry, recognitionOverride, (MaaImageBuffer)image);

    /// <inheritdoc cref="IMaaContext.RunRecognition"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextRunRecognition"/>.
    /// </remarks>
    public RecognitionDetail? RunRecognition(string entry, string recognitionOverride, MaaImageBuffer image)
    {
        ArgumentNullException.ThrowIfNull(image);
        return RecognitionDetail.Query<MaaRectBuffer, MaaImageBuffer, MaaImageListBuffer>(
            recognitionId: MaaContextRunRecognition(Handle, entry, recognitionOverride, image.Handle),
            tasker: Tasker);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextRunAction"/>.
    /// </remarks>
    public NodeDetail? RunAction(string entry, string actionOverride, IMaaRectBuffer recognitionBox, string recognitionDetail)
        => RunAction(entry, actionOverride, (IMaaRectBuffer<nint>)recognitionBox, recognitionDetail);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextRunAction"/>.
    /// </remarks>
    public NodeDetail? RunAction(string entry, string actionOverride, IMaaRectBuffer<nint> recognitionBox, string recognitionDetail)
    {
        ArgumentNullException.ThrowIfNull(recognitionBox);
        return NodeDetail.Query(
            nodeId: MaaContextRunAction(Handle, entry, actionOverride, recognitionBox.Handle, recognitionDetail),
            tasker: Tasker);
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
        => MaaStringListBuffer.Set(nextList, listBuffer
            => MaaContextOverrideNext(Handle, nodeName, listBuffer));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextGetTaskId"/>.
    /// </remarks>
    public MaaTaskJob TaskJob => new(MaaContextGetTaskId(Handle), Tasker);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextGetTasker"/>.
    /// </remarks>
    IMaaTasker IMaaContext.Tasker => Tasker;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextGetTasker"/>.
    /// </remarks>
    public IMaaTasker<nint> Tasker => MaaTasker.Instances[MaaContextGetTasker(Handle)];

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextClone"/>.
    /// </remarks>
    public object Clone()
        => new MaaContext(MaaContextClone(Handle));
}
