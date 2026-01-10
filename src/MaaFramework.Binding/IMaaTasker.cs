using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaTasker with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaTasker<T> : IMaaTasker, IMaaDisposableHandle<T>;

/// <summary>
///     An interface defining wrapped members for MaaTasker.
/// </summary>
public interface IMaaTasker : IMaaCommon, IMaaOption<TaskerOption>, IMaaDisposable
{
    /// <summary>
    ///     Gets the last valid posted job.
    /// </summary>
    /// <returns>A <see cref="MaaTaskJob"/> if any valid job has been posted; otherwise, <see langword="null"/>..</returns>
    MaaTaskJob? LastJob { get; }

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

#pragma warning disable CA1716 // 标识符不应与关键字匹配
    /// <summary>
    ///     Gets or sets a global.
    /// </summary>
    IMaaGlobal Global { get; set; }
#pragma warning restore CA1716 // 标识符不应与关键字匹配

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
    MaaTaskJob AppendTask(string entry, [StringSyntax("Json")] string pipelineOverride = "{}");

    /// <summary>
    ///     Appends a job of executing a recognition.
    /// </summary>
    /// <param name="recoType">The recognition type.</param>
    /// <param name="recoParam">The recognition parameters json.</param>
    /// <param name="image">The image to be recognized.</param>
    /// <returns>A task job.</returns>
    MaaTaskJob AppendRecognition(string recoType, [StringSyntax("Json")] string recoParam, IMaaImageBuffer image);

    /// <summary>
    ///     Appends a job of executing an action.
    /// </summary>
    /// <param name="actionType">The action type.</param>
    /// <param name="actionParam">The action parameters json.</param>
    /// <param name="box">The recognition position.</param>
    /// <param name="recoDetail">The recognition details.</param>
    /// <returns>A task job.</returns>
    MaaTaskJob AppendAction(string actionType, [StringSyntax("Json")] string actionParam, IMaaRectBuffer box, [StringSyntax("Json")] string recoDetail);

    /// <summary>
    ///     Gets whether the <see cref="IMaaTasker"/> is running.
    /// </summary>
    /// <returns><see langword="true"/> if <see cref="IMaaTasker"/> is running; otherwise, <see langword="false"/>.</returns>
    bool IsRunning { get; }

    /// <summary>
    ///     Gets whether the <see cref="IMaaTasker"/> is stopping.
    /// </summary>
    /// <returns><see langword="true"/> if <see cref="IMaaTasker"/> is stopping; otherwise, <see langword="false"/>.</returns>
    bool IsStopping { get; }

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable CA1716 // 标识符不应与关键字匹配
    /// <summary>
    ///     Stops all appended tasks, the <see cref="IMaaResource"/> and the <see cref="IMaaController"/>. 
    /// </summary>
    /// <returns>A stop job.</returns>
    MaaTaskJob Stop();

    /// <inheritdoc cref="Stop"/>
    [Obsolete($"Use {nameof(Stop)}() instead.", error: false)]
    MaaTaskJob Abort();
#pragma warning restore CA1716 // 标识符不应与关键字匹配
#pragma warning restore S1133 // Deprecated code should be removed

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
    bool GetRecognitionDetail<T>(MaaRecoId recognitionId, out string nodeName, out string algorithm, out bool hit, IMaaRectBuffer? hitBox, out string detailJson, IMaaImageBuffer? raw, IMaaListBuffer<T>? draws)
        where T : IMaaImageBuffer;

    /// <summary>
    ///     Gets the action detail.
    /// </summary>
    /// <param name="actionId">The action id.</param>
    /// <param name="nodeName">The node name.</param>
    /// <param name="action">The action.</param>
    /// <param name="box">The hit box.</param>
    /// <param name="isSucceeded">A value indicating whether the action is succeeded.</param>
    /// <param name="detailJson">the action detail</param>
    /// <returns><see langword="true"/> if query was successful; otherwise, <see langword="false"/>.</returns>
    bool GetActionDetail(MaaActId actionId, out string nodeName, out string action, IMaaRectBuffer? box, out bool isSucceeded, out string detailJson);

    /// <summary>
    ///     Gets the node detail.
    /// </summary>
    /// <param name="nodeId">The node id.</param>
    /// <param name="nodeName">The node name.</param>
    /// <param name="recognitionId">The recognition id.</param>
    /// <param name="actionId">The action id.</param>
    /// <param name="actionCompleted">A value indicating whether the action is completed.</param>
    /// <returns><see langword="true"/> if query was successful; otherwise, <see langword="false"/>.</returns>
    bool GetNodeDetail(MaaNodeId nodeId, out string nodeName, out MaaRecoId recognitionId, out MaaActId actionId, out bool actionCompleted);

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
