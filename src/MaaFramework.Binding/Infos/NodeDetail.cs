namespace MaaFramework.Binding;

/// <summary>
///     A class providing properties of node detail.
/// </summary>
public sealed class NodeDetail
{
    /// <summary>
    ///     Gets or initializes the node id.
    /// </summary>
    /// <remarks>
    ///     From <see cref="TaskDetail.NodeIdList"/>.
    /// </remarks>
    public required MaaNodeId Id { get; init; }

    /// <summary>
    ///     Gets or initializes the node name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Gets or initializes the recognition id.
    /// </summary>
    public required MaaRecoId RecognitionId { get; init; }

    /// <summary>
    ///     Gets or initializes a value indicating whether the action run completed.
    /// </summary>
    public required bool RunCompleted { get; init; }

    /// <summary>
    ///     Queries the node detail.
    /// </summary>
    /// <param name="nodeId">The node id.</param>
    /// <param name="maa">The maa utility.</param>
    /// <returns>A <see cref="NodeDetail"/> if query was successful; otherwise, <see langword="null"/>.</returns>
    public static NodeDetail? Query(MaaNodeId nodeId, IMaaUtility maa)
    {
        ArgumentNullException.ThrowIfNull(maa);
        if (!maa.QueryNodeDetail(nodeId, out var name, out var recognitionId, out var runCompleted))
            return null;

        return new NodeDetail
        {
            Id = nodeId,
            Name = name,
            RecognitionId = recognitionId,
            RunCompleted = runCompleted,
        };
    }
}

/// <summary>
///     A static class providing extension methods for the query of node detail.
/// </summary>
public static class NodeDetailExtension
{
    /// <param name="taskDetail">The task detail.</param>
    /// <param name="maa">The maa utility.</param>
    /// <param name="index">The index of <see cref="TaskDetail.NodeIdList"/>.</param>
    /// <inheritdoc cref="NodeDetail.Query"/>
    public static NodeDetail? QueryNodeDetail(this TaskDetail? taskDetail, IMaaUtility maa, int index = 0)
        => taskDetail?.NodeIdList.Count > index ? NodeDetail.Query(taskDetail.NodeIdList[index], maa) : null;

    /// <param name="job">The maa task job.</param>
    /// <param name="index">The index of <see cref="TaskDetail.NodeIdList"/>.</param>
    /// <inheritdoc cref="NodeDetail.Query"/>
    public static NodeDetail? QueryNodeDetail(this MaaTaskJob? job, int index = 0)
        => job?.QueryTaskDetail().QueryNodeDetail(job.Maa.Utility, index);
}
