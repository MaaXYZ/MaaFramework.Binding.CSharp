using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaUtility.
/// </summary>
public interface IMaaUtility : IMaaOption<GlobalOption>
{
    /// <summary>
    ///     Gets version of MaaFramework.
    /// </summary>
    string Version { get; }

    /// <summary>
    ///     Queries the task detail.
    /// </summary>
    /// <param name="taskId">The task id.</param>
    /// <param name="entry">The name of task entry.</param>
    /// <param name="nodeIdList">The node id list.</param>
    /// <returns><see langword="true"/> if query was successful; otherwise, <see langword="false"/>.</returns>
    bool QueryTaskDetail(MaaTaskId taskId, out string entry, out MaaNodeId[] nodeIdList);

    /// <summary>
    ///     Queries the node detail.
    /// </summary>
    /// <param name="nodeId">The node id.</param>
    /// <param name="name">The node name.</param>
    /// <param name="recognitionId">The recognition id.</param>
    /// <param name="runCompleted">A value indicating whether the action run completed.</param>
    /// <returns><see langword="true"/> if query was successful; otherwise, <see langword="false"/>.</returns>
    bool QueryNodeDetail(MaaNodeId nodeId, out string name, out MaaRecoId recognitionId, out bool runCompleted);

    /// <summary>
    ///     Queries the recognition detail.
    /// </summary>
    /// <param name="recognitionId">The recognition id.</param>
    /// <param name="name">The recognition name.</param>
    /// <param name="hit">A value indicating whether the recognition hits.</param>
    /// <param name="hitBox">The hit box.</param>
    /// <param name="detailJson">The recognition detail.</param>
    /// <param name="raw">The raw image on the recognition completing.</param>
    /// <param name="draws">The draw images on the recognition completed.</param>
    /// <returns><see langword="true"/> if query was successful; otherwise, <see langword="false"/>.</returns>
    bool QueryRecognitionDetail<T>(MaaRecoId recognitionId, out string name, out bool hit, IMaaRectBuffer? hitBox, out string detailJson, T? raw, IMaaList<T>? draws)
        where T : IMaaImageBuffer, new();
}

