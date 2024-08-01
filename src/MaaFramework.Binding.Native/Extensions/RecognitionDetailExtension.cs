using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     A static class providing extension methods for the query of recognition detail.
/// </summary>
public static class RecognitionDetailExtension
{
    /// <param name="nodeDetail">The node detail.</param>
    /// <param name="maa">The maa utility.</param>
    /// <inheritdoc cref="RecognitionDetail&lt;T&gt;.Query&lt;T1, T2, T3&gt;"/>
    public static RecognitionDetail<MaaImageBuffer>? QueryRecognitionDetail(this NodeDetail? nodeDetail, IMaaUtility maa)
        => nodeDetail is null ? null : RecognitionDetail<MaaImageBuffer>.Query<MaaRectBuffer, MaaImageBuffer, MaaImageList>(nodeDetail.RecognitionId, maa);

    /// <param name="job">The maa task job.</param>
    /// <inheritdoc cref="RecognitionDetail&lt;T&gt;.Query&lt;T1, T2, T3&gt;"/>
    public static RecognitionDetail<MaaImageBuffer>? QueryRecognitionDetail(this MaaTaskJob? job)
        => job?.QueryNodeDetail().QueryRecognitionDetail(job.Maa.Utility);
}
