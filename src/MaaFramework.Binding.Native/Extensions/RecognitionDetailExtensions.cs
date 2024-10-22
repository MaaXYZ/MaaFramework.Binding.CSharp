using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     A static class providing extension methods for the query of recognition detail.
/// </summary>
public static class RecognitionDetailExtensions
{
    /// <param name="nodeDetail">The node detail.</param>
    /// <param name="tasker">The maa tasker.</param>
    /// <inheritdoc cref="RecognitionDetail.Query{T1, T2, T3}"/>
    public static RecognitionDetail? QueryRecognitionDetail(this NodeDetail? nodeDetail, IMaaTasker tasker)
        => nodeDetail is null ? null : RecognitionDetail.Query<MaaRectBuffer, MaaImageBuffer, MaaImageListBuffer>(nodeDetail.RecognitionId, tasker);

    /// <param name="taskDetail">The task detail.</param>
    /// <param name="tasker">The maa tasker.</param>
    /// <param name="index">The index of <see cref="TaskDetail.NodeIdList"/>.</param>
    /// <inheritdoc cref="RecognitionDetail.Query{T1, T2, T3}"/>
    public static RecognitionDetail? QueryRecognitionDetail(this TaskDetail? taskDetail, IMaaTasker tasker, int index = 0)
        => taskDetail?.QueryNodeDetail(tasker, index).QueryRecognitionDetail(tasker);

    /// <param name="job">The maa task job.</param>
    /// <param name="index">The index of <see cref="TaskDetail.NodeIdList"/>.</param>
    /// <inheritdoc cref="RecognitionDetail.Query{T1, T2, T3}"/>
    public static RecognitionDetail? QueryRecognitionDetail(this MaaTaskJob? job, int index = 0)
        => job?.QueryNodeDetail(index).QueryRecognitionDetail(job.Tasker);
}
