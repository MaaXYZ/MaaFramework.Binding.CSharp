using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     A static class providing extension methods for the query of action detail.
/// </summary>
public static class ActionDetailExtensions
{
    /// <param name="nodeDetail">The node detail.</param>
    /// <param name="tasker">The maa tasker.</param>
    /// <inheritdoc cref="ActionDetail.Query{TRect}"/>
    public static ActionDetail? QueryActionDetail(this NodeDetail? nodeDetail, IMaaTasker tasker)
        => nodeDetail is null ? null : ActionDetail.Query<MaaRectBuffer>(nodeDetail.ActionId, tasker);

    /// <param name="taskDetail">The task detail.</param>
    /// <param name="tasker">The maa tasker.</param>
    /// <param name="index">The index of <see cref="TaskDetail.NodeIdList"/>.</param>
    /// <inheritdoc cref="ActionDetail.Query{TRect}"/>
    public static ActionDetail? QueryActionDetail(this TaskDetail? taskDetail, IMaaTasker tasker, int index = 0)
        => taskDetail?.QueryNodeDetail(tasker, index).QueryActionDetail(tasker);

    /// <param name="job">The maa task job.</param>
    /// <param name="index">The index of <see cref="TaskDetail.NodeIdList"/>.</param>
    /// <inheritdoc cref="ActionDetail.Query{TRect}"/>
    public static ActionDetail? QueryActionDetail(this MaaTaskJob? job, int index = 0)
        => job?.QueryNodeDetail(index).QueryActionDetail(job.Tasker);
}
