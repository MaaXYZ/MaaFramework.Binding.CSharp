using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaContext with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaContext<T> : IMaaContext
{
    /// <summary>
    ///     Gets or initializes a MaaContextHandle.
    /// </summary>
    T Handle { get; init; }
}

/// <summary>
///     An interface defining wrapped members for MaaContext.
/// </summary>
public interface IMaaContext : ICloneable
{
    /// <summary>
    ///     Runs a task.
    /// </summary>
    /// <param name="entry">The entry name of the task.</param>
    /// <param name="pipelineOverride">The json used to override the pipeline.</param>
    /// <returns><see cref="TaskDetail"/> if the operation was executed successfully; otherwise, <see langword="null"/>.</returns>
    TaskDetail? RunTask(string entry, string pipelineOverride);

    /// <summary>
    ///     Run a recognition.
    /// </summary>
    /// <param name="entry">The recognition entry name.</param>
    /// <param name="pipelineOverride">The json used to override the pipeline.</param>
    /// <param name="image">The image to be recognized.</param>
    /// <returns><see cref="RecognitionDetail"/> if the operation was executed successfully; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    RecognitionDetail? RunRecognition(string entry, string pipelineOverride, IMaaImageBuffer image);

    /// <summary>
    ///     Run an action.
    /// </summary>
    /// <param name="entry">The action entry name.</param>
    /// <param name="pipelineOverride">The json used to override the pipeline.</param>
    /// <param name="recognitionBox">The rect buffer containing current rect in the recognition result.</param>
    /// <param name="recognitionDetail">The rect detail in the recognition result.</param>
    /// <returns><see cref="NodeDetail"/> if the operation was executed successfully; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    NodeDetail? RunAction(string entry, string pipelineOverride, IMaaRectBuffer recognitionBox, string recognitionDetail);

    /// <summary>
    ///     Override a pipeline.
    /// </summary>
    /// <param name="pipelineOverride">The json used to override the pipeline.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool OverridePipeline(string pipelineOverride);

    /// <summary>
    ///     Override the property field "next" in a node.
    /// </summary>
    /// <param name="nodeName">The node name.</param>
    /// <param name="nextList">The next list.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool OverrideNext(string nodeName, IEnumerable<string> nextList);

    /// <summary>
    ///     Gets the task job from the <see cref="IMaaContext"/>.
    /// </summary>
    /// <returns>The <see cref="MaaTaskJob"/>.</returns>
    MaaTaskJob TaskJob { get; }

    /// <summary>
    ///     Gets the tasker from the <see cref="IMaaContext"/>.
    /// </summary>
    /// <returns>The <see cref="IMaaTasker"/>.</returns>
    IMaaTasker Tasker { get; }

    /// <summary>
    ///     Creates a new <see cref="IMaaContext"/> that is a deep copy of the current instance.
    /// </summary>
    /// <returns>The <see cref="IMaaContext"/></returns>
    new IMaaContext Clone();
}
