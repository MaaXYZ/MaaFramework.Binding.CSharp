using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     A static class providing extension methods for the query of recognition detail.
/// </summary>
public static class RecognitionDetailExtension
{
    /// <param name="nodeDetail">The node detail.</param>
    /// <param name="tasker">The maa tasker.</param>
    /// <inheritdoc cref="RecognitionDetail{T}.Query{T1, T2, T3}"/>
    public static RecognitionDetail<MaaImageBuffer>? QueryRecognitionDetail(this NodeDetail? nodeDetail, IMaaTasker tasker)
        => nodeDetail is null ? null : RecognitionDetail<MaaImageBuffer>.Query<MaaRectBuffer, MaaImageBuffer, MaaImageListBuffer>(nodeDetail.RecognitionId, tasker);

    /// <param name="job">The maa task job.</param>
    /// <inheritdoc cref="RecognitionDetail{T}.Query{T1, T2, T3}"/>
    public static RecognitionDetail<MaaImageBuffer>? QueryRecognitionDetail(this MaaTaskJob? job)
        => job?.QueryNodeDetail().QueryRecognitionDetail(job.Tasker);
}
