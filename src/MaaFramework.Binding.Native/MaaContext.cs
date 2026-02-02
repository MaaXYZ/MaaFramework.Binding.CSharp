using MaaFramework.Binding.Buffers;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaContext;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaContext"/>.
/// </summary>
public class MaaContext : IMaaContext<MaaContextHandle>, IEquatable<MaaContext>, IEqualityComparer<MaaContext>
{
    /// <inheritdoc/>
    public required MaaContextHandle Handle { get; init; }

    /// <inheritdoc cref="IMaaContext.Tasker"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextGetTasker"/>.
    /// </remarks>
    public required MaaTasker Tasker { get; init; }

    /// <summary>
    ///     Creates a <see cref="MaaContext"/> instance.
    /// </summary>
    /// <param name="contextHandle">The MaaContextHandle.</param>
    [SetsRequiredMembers]
    public MaaContext(MaaContextHandle contextHandle)
    {
        var taskerHandle = MaaContextGetTasker(contextHandle);
        Handle = contextHandle;
        Tasker = (NativeBindingContext.IsStatelessMode || taskerHandle == MaaTaskerHandle.Zero)
            ? new MaaTasker(taskerHandle)
            : MaaTasker.Instances[taskerHandle];
    }

    /// <summary>
    ///     Creates a <see cref="MaaContext"/> instance.
    /// </summary>
    /// <param name="contextHandle">The MaaContextHandle.</param>
    /// <param name="tasker">The MaaTasker.</param>
    [SetsRequiredMembers]
    public MaaContext(MaaContextHandle contextHandle, MaaTasker tasker)
    {
        Handle = contextHandle;
        Tasker = tasker;
    }

    #region Override equality
    /// <inheritdoc/>
    public int GetHashCode(MaaContext obj) { ArgumentNullException.ThrowIfNull(obj); return obj.Handle.GetHashCode(); }
    /// <inheritdoc/>
    public override int GetHashCode() => Handle.GetHashCode();
    /// <inheritdoc/>
    public virtual bool Equals(MaaContext? other) => other is not null && Handle == other.Handle;
    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is MaaContext other && Handle == other.Handle;
    /// <inheritdoc/>
    public bool Equals(MaaContext? x, MaaContext? y) => x is null ? y is null : x.Equals(y);
    /// <summary>
    ///     Compares two values to determine equality.
    /// </summary>
    /// <param name="left">The left value.</param>
    /// <param name="right">The right value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(MaaContext? left, MaaContext? right) => left is null ? right is null : left.Equals(right);
    /// <summary>
    ///     Compares two values to determine inequality.
    /// </summary>
    /// <param name="left">The left value.</param>
    /// <param name="right">The right value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(MaaContext? left, MaaContext? right) => !(left == right);
    #endregion

    /// <inheritdoc/>
    public bool IsCancellationRequested => Tasker.IsStopping;

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
    public ActionDetail? RunAction(string entry, IMaaRectBuffer recognitionBox, string recognitionDetail, [StringSyntax("Json")] string pipelineOverride = "{}")
        => RunAction(entry, (MaaRectBuffer)recognitionBox, recognitionDetail, pipelineOverride);

    /// <inheritdoc cref="IMaaContext.RunAction"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextRunAction"/>.
    /// </remarks>
    public ActionDetail? RunAction(string entry, MaaRectBuffer recognitionBox, string recognitionDetail, [StringSyntax("Json")] string pipelineOverride = "{}")
    {
        ArgumentNullException.ThrowIfNull(recognitionBox);
        var actionId = MaaContextRunAction(Handle, entry, pipelineOverride, recognitionBox.Handle, recognitionDetail);
        return actionId == Interop.Native.MaaDef.MaaInvalidId
            ? null
            : ActionDetail.Query<MaaRectBuffer>(actionId, Tasker);
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
    public bool OverrideImage(string imageName, IMaaImageBuffer image)
        => OverrideImage(imageName, (MaaImageBuffer)image);

    /// <inheritdoc cref="IMaaResource.OverrideImage"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextOverrideImage"/>.
    /// </remarks>
    public bool OverrideImage(string imageName, MaaImageBuffer image)
    {
        ArgumentNullException.ThrowIfNull(image);

        return MaaContextOverrideImage(Handle, imageName, image.Handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextGetNodeData"/>.
    /// </remarks>
    public bool GetNodeData(string nodeName, [MaybeNullWhen(false)][StringSyntax("Json")] out string data)
        => MaaStringBuffer.TryGetValue(out data, buffer
            => MaaContextGetNodeData(Handle, nodeName, buffer));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextGetTaskId"/>.
    /// </remarks>
    public MaaTaskJob TaskJob => new(MaaContextGetTaskId(Handle), Tasker, Tasker);

    IMaaTasker IMaaContext.Tasker => Tasker;

    object ICloneable.Clone()
        => Clone();
    IMaaContext IMaaContext.Clone()
        => Clone();

    /// <inheritdoc cref="IMaaContext.Clone"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextClone"/>.
    /// </remarks>
    public virtual MaaContext Clone()
        => new(MaaContextClone(Handle), Tasker);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextSetAnchor"/>.
    /// </remarks>
    public bool SetAnchor(string anchorName, string nodeName)
        => MaaContextSetAnchor(Handle, anchorName, nodeName);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextGetAnchor"/>.
    /// </remarks>
    public bool GetAnchor(string anchorName, [MaybeNullWhen(false)] out string nodeName)
        => MaaStringBuffer.TryGetValue(out nodeName, buffer
            => MaaContextGetAnchor(Handle, anchorName, buffer));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextGetHitCount"/>.
    /// </remarks>
    public bool GetHitCount(string nodeName, out ulong count)
        => MaaContextGetHitCount(Handle, nodeName, out count);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaContextClearHitCount"/>.
    /// </remarks>
    public bool ClearHitCount(string nodeName)
        => MaaContextClearHitCount(Handle, nodeName);
}
