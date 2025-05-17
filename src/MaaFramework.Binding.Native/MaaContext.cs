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

        var taskerHandle = MaaContextGetTasker(Handle);
        Tasker = NativeBindingContext.IsStatelessMode ? new MaaTasker(taskerHandle) : MaaTasker.Instances[taskerHandle];
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextRunTask"/>.
    /// </remarks>
    public TaskDetail? RunTask(string entry, [StringSyntax("Json")] string pipelineOverride = "{}")
    {
        var taskId = MaaContextRunTask(Handle, entry, pipelineOverride);
        return taskId == Interop.Native.MaaDef.MaaInvalidId
            ? null
            : TaskDetail.Query(taskId, Tasker);
    }

    /// <inheritdoc/>
    public RecognitionDetail? RunRecognition(string entry, IMaaImageBuffer image, [StringSyntax("Json")] string pipelineOverride = "{}")
        => RunRecognition(entry, (MaaImageBuffer)image, pipelineOverride);

    /// <inheritdoc cref="IMaaContext.RunRecognition"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextRunRecognition"/>.
    /// </remarks>
    public RecognitionDetail? RunRecognition(string entry, MaaImageBuffer image, [StringSyntax("Json")] string pipelineOverride = "{}")
    {
        ArgumentNullException.ThrowIfNull(image);
        var recognitionId = MaaContextRunRecognition(Handle, entry, pipelineOverride, image.Handle);
        return recognitionId == Interop.Native.MaaDef.MaaInvalidId
            ? null
            : RecognitionDetail.Query<MaaRectBuffer, MaaImageBuffer, MaaImageListBuffer>(recognitionId, Tasker);
    }

    /// <inheritdoc/>
    public NodeDetail? RunAction(string entry, IMaaRectBuffer recognitionBox, string recognitionDetail, [StringSyntax("Json")] string pipelineOverride = "{}")
        => RunAction(entry, (MaaRectBuffer)recognitionBox, recognitionDetail, pipelineOverride);

    /// <inheritdoc cref="IMaaContext.RunAction"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextRunAction"/>.
    /// </remarks>
    public NodeDetail? RunAction(string entry, MaaRectBuffer recognitionBox, string recognitionDetail, [StringSyntax("Json")] string pipelineOverride = "{}")
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
    public bool OverridePipeline([StringSyntax("Json")] string pipelineOverride)
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
    public MaaTasker Tasker { get; }

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
