namespace MaaFramework.Binding;

/// <summary>
///     A sealed record providing properties of task detail.
/// </summary>
/// <param name="Id">Gets the task id.</param>
/// <param name="Entry">Gets the name of task entry.</param>
/// <param name="NodeIdList">Gets the node id list.</param>
public sealed record TaskDetail(
    MaaTaskId Id,
    string Entry,
    IList<MaaNodeId> NodeIdList
)
{
    /// <summary>
    ///     Queries the task detail.
    /// </summary>
    /// <param name="taskId">The task id.</param>
    /// <param name="tasker">The maa tasker.</param>
    /// <returns>A <see cref="TaskDetail"/> if query was successful; otherwise, <see langword="null"/>.</returns>
    public static TaskDetail? Query(MaaTaskId taskId, IMaaTasker tasker)
    {
        ArgumentNullException.ThrowIfNull(tasker);
        return tasker.GetTaskDetail(taskId, out var entry, out var nodeIdList)
            ? new TaskDetail(
                Id: taskId,
                Entry: entry,
                NodeIdList: nodeIdList)
            : null;
    }
}

/// <summary>
///     A static class providing extension methods for the query of task detail.
/// </summary>
public static class TaskDetailExtensions
{
    /// <param name="job">The maa task job.</param>
    /// <inheritdoc cref="TaskDetail.Query"/>
    public static TaskDetail? QueryTaskDetail(this MaaTaskJob? job)
        => job is null ? null : TaskDetail.Query(job.Id, job.Tasker);
}
