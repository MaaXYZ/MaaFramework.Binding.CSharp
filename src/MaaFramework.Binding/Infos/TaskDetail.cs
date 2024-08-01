namespace MaaFramework.Binding;

/// <summary>
///     A class providing properties of task detail.
/// </summary>
public sealed class TaskDetail
{
    /// <summary>
    ///     Gets or initializes the task id.
    /// </summary>
    /// <remarks>
    ///     From <see cref="MaaJob.Id"/>.
    /// </remarks>
    public required MaaTaskId Id { get; init; }

    /// <summary>
    ///     Gets or initializes the name of task entry.
    /// </summary>
    public required string Entry { get; init; }

    /// <summary>
    ///     Gets or initializes the node id list.
    /// </summary>
    public required IList<MaaNodeId> NodeIdList { get; init; }

    /// <summary>
    ///     Queries the task detail.
    /// </summary>
    /// <param name="taskId">The task id.</param>
    /// <param name="maa">The maa utility.</param>
    /// <returns>A <see cref="TaskDetail"/> if query was successful; otherwise, <see langword="null"/>.</returns>
    public static TaskDetail? Query(MaaTaskId taskId, IMaaUtility maa)
    {
        ArgumentNullException.ThrowIfNull(maa);
        if (!maa.QueryTaskDetail(taskId, out var entry, out var nodeIdList))
            return null;

        return new TaskDetail
        {
            Id = taskId,
            Entry = entry,
            NodeIdList = nodeIdList,
        };
    }
}

/// <summary>
///     A static class providing extension methods for the query of task detail.
/// </summary>
public static class TaskDetailExtension
{
    /// <param name="job">The maa task job.</param>
    /// <inheritdoc cref="TaskDetail.Query"/>
    public static TaskDetail? QueryTaskDetail(this MaaTaskJob? job)
        => job is null ? null : TaskDetail.Query(job.Id, job.Maa.Utility);
}
