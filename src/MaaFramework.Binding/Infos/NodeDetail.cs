namespace MaaFramework.Binding;

/// <summary>
///     A sealed record providing properties of node detail.
/// </summary>
/// <param name="Id">Gets the node id.</param>
/// <param name="NodeName">Gets the node name.</param>
/// <param name="RecognitionId">Gets the recognition id.</param>
/// <param name="ActionId">Gets the action id.</param>
/// <param name="ActionCompleted">Gets a value indicating whether the action run completed.</param>
public sealed record NodeDetail(
    MaaNodeId Id,
    string NodeName,
    MaaRecoId RecognitionId,
    MaaActId ActionId,
    bool ActionCompleted
)
{
    /// <summary>
    ///     Queries the node detail.
    /// </summary>
    /// <param name="nodeId">The node id.</param>
    /// <param name="tasker">The maa tasker.</param>
    /// <returns>A <see cref="NodeDetail"/> if query was successful; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    public static NodeDetail? Query(MaaNodeId nodeId, IMaaTasker tasker)
    {
        ArgumentNullException.ThrowIfNull(tasker);
        return tasker.GetNodeDetail(nodeId, out var nodeName, out var recognitionId, out var actionId, out var actionCompleted)
            ? new NodeDetail(
                Id: nodeId,
                NodeName: nodeName,
                RecognitionId: recognitionId,
                ActionId: actionId,
                ActionCompleted: actionCompleted)
            : null;
    }

    /// <summary>
    ///     Queries the latest node detail.
    /// </summary>
    /// <param name="nodeName">The node name.</param>
    /// <param name="tasker">The maa tasker.</param>
    /// <returns>A <see cref="NodeDetail"/> if query was successful; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    public static NodeDetail? QueryLatest(string nodeName, IMaaTasker tasker)
    {
        ArgumentNullException.ThrowIfNull(tasker);
        return tasker.GetLatestNode(nodeName, out var nodeId)
            ? Query(nodeId, tasker)
            : null;
    }

    /// <inheritdoc cref="QueryLatest(string, IMaaTasker)"/>
    public NodeDetail? QueryLatest(IMaaTasker tasker)
    {
        ArgumentNullException.ThrowIfNull(tasker);
        if (!tasker.GetLatestNode(NodeName, out var nodeId)) return null;
        return nodeId == Id ? this : Query(nodeId, tasker);
    }
}

/// <summary>
///     A static class providing extension methods for the query of node detail.
/// </summary>
public static class NodeDetailExtensions
{
    /// <param name="taskDetail">The task detail.</param>
    /// <param name="tasker">The maa tasker.</param>
    /// <param name="index">The index of <see cref="TaskDetail.NodeIdList"/>.</param>
    /// <inheritdoc cref="NodeDetail.Query"/>
    public static NodeDetail? QueryNodeDetail(this TaskDetail? taskDetail, IMaaTasker tasker, int index = 0)
        => taskDetail?.NodeIdList.Count > index ? NodeDetail.Query(taskDetail.NodeIdList[index], tasker) : null;

    /// <param name="job">The maa task job.</param>
    /// <param name="index">The index of <see cref="TaskDetail.NodeIdList"/>.</param>
    /// <inheritdoc cref="NodeDetail.Query"/>
    public static NodeDetail? QueryNodeDetail(this MaaTaskJob? job, int index = 0)
        => job?.QueryTaskDetail().QueryNodeDetail(job.Tasker, index);
}
