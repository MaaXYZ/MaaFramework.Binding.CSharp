using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaTasker with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaTasker<T> : IMaaTasker, IMaaDisposableHandle<T>
{
    /// <inheritdoc cref="IMaaTasker.Resource"/>
    new IMaaResource<T> Resource { get; set; }

    /// <inheritdoc cref="IMaaTasker.Controller"/>
    new IMaaController<T> Controller { get; set; }
}

/// <summary>
///     An interface defining wrapped members for MaaTasker.
/// </summary>
public interface IMaaTasker : IMaaCommon, IMaaOption<TaskerOption>, IMaaPost, IMaaDisposable
{
    /// <summary>
    ///     Gets or sets whether disposes the <see cref="Resource"/> or the <see cref="Controller"/> when <see cref="IDisposable.Dispose"/> was invoked.
    /// </summary>
    DisposeOptions DisposeOptions { get; set; }

    /// <summary>
    ///     Gets or sets a resource that binds to the <see cref="IMaaTasker"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="MaaInteroperationException"/>
    IMaaResource Resource { get; set; }

    /// <summary>
    ///     Gets or sets a controller that binds to the <see cref="IMaaTasker"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="MaaInteroperationException"/>
    IMaaController Controller { get; set; }

    /// <summary>
    ///     Gets or sets a toolkit.
    /// </summary>
    IMaaToolkit Toolkit { get; set; }

    /// <summary>
    ///     Gets or sets a utility.
    /// </summary>
    IMaaUtility Utility { get; set; }

    /// <summary>
    ///     Gets whether the <see cref="IMaaTasker"/> is fully initialized.
    /// </summary>
    /// <returns><see langword="true"/> if the <see cref="IMaaTasker"/> was fully initialized; otherwise, <see langword="false"/>.</returns>
    bool IsInitialized { get; }

    /// <summary>
    ///     Appends a job of executing a task, could be called multiple times.
    /// </summary>
    /// <param name="entry">The entry name of the task.</param>
    /// <param name="pipelineOverride">The json used to override the pipeline.</param>
    /// <returns>A task job.</returns>
    MaaTaskJob AppendTask(string entry, string pipelineOverride = "{}");

    /// <summary>
    ///     Gets whether the <see cref="IMaaTasker"/> is running.
    /// </summary>
    /// <returns><see langword="true"/> if <see cref="IMaaTasker"/> is running; otherwise, <see langword="false"/>.</returns>
    bool IsRunning { get; }

    /// <summary>
    ///     Stops all appended tasks, the <see cref="IMaaResource"/> and the <see cref="IMaaController"/>. 
    /// </summary>
    /// <returns>A stop job.</returns>
    MaaTaskJob Abort();

    /// <summary>
    ///     Clear the runtime cache.
    /// </summary>
    /// <returns><see langword="true"/> if the cache cleared successfully; otherwise, <see langword="false"/>.</returns>
    bool ClearCache();

    /// <summary>
    ///     Gets the recognition detail.
    /// </summary>
    /// <param name="recognitionId">The recognition id.</param>
    /// <param name="nodeName">The node name.</param>
    /// <param name="algorithm">The algorithm name of the recognition.</param>
    /// <param name="hit">A value indicating whether the recognition hits.</param>
    /// <param name="hitBox">The hit box.</param>
    /// <param name="detailJson">The recognition detail.</param>
    /// <param name="raw">The raw image on the recognition completing.<para>Only valid in debug mode.</para></param>
    /// <param name="draws">The draw images on the recognition completed.<para>Only valid in debug mode.</para></param>
    /// <returns><see langword="true"/> if query was successful; otherwise, <see langword="false"/>.</returns>
    bool GetRecognitionDetail<T>(MaaRecoId recognitionId, out string nodeName, out string algorithm, out bool hit, IMaaRectBuffer? hitBox, out string detailJson, T? raw, IMaaListBuffer<T>? draws)
        where T : IMaaImageBuffer, new();

    /// <summary>
    ///     Gets the node detail.
    /// </summary>
    /// <param name="nodeId">The node id.</param>
    /// <param name="nodeName">The node name.</param>
    /// <param name="recognitionId">The recognition id.</param>
    /// <param name="actionCompleted">A value indicating whether the action run completed.</param>
    /// <returns><see langword="true"/> if query was successful; otherwise, <see langword="false"/>.</returns>
    bool GetNodeDetail(MaaNodeId nodeId, out string nodeName, out MaaRecoId recognitionId, out bool actionCompleted);

    /// <summary>
    ///     Gets the task detail.
    /// </summary>
    /// <param name="taskId">The task id.</param>
    /// <param name="entry">The name of task entry.</param>
    /// <param name="nodeIdList">The node id list.</param>
    /// <param name="status">The status of the task.</param>
    /// <returns><see langword="true"/> if query was successful; otherwise, <see langword="false"/>.</returns>
    bool GetTaskDetail(MaaTaskId taskId, out string entry, out MaaNodeId[] nodeIdList, out MaaJobStatus status);

    /// <summary>
    ///     Gets the latest node.
    /// </summary>
    /// <param name="nodeName">The name of the node in the task.</param>
    /// <param name="latestId">The latest node id.</param>
    /// <returns><see langword="true"/> if query was successful; otherwise, <see langword="false"/>.</returns>
    bool GetLatestNode(string nodeName, out MaaNodeId latestId);
}
