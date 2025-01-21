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

    /// <inheritdoc cref="IMaaContext.RunRecognition"/>
    RecognitionDetail? RunRecognition(string entry, string recognitionOverride, IMaaImageBuffer<T> image);

    /// <inheritdoc cref="IMaaContext.RunAction"/>
    NodeDetail? RunAction(string entry, string actionOverride, IMaaRectBuffer<T> recognitionBox, string recognitionDetail);

    /// <inheritdoc cref="IMaaContext.Tasker"/>
    new IMaaTasker<T> Tasker { get; }
}

/// <summary>
///     An interface defining wrapped members for MaaContext.
/// </summary>
public interface IMaaContext : ICloneable
{
    /// <summary>
    ///     Runs a pipeline.
    /// </summary>
    /// <param name="entry">The pipeline entry name.</param>
    /// <param name="pipelineOverride">The json used to override the pipeline.</param>
    /// <returns><see cref="TaskDetail"/> if the operation was executed successfully; otherwise, <see langword="null"/>.</returns>
    TaskDetail? RunTask(string entry, string pipelineOverride);

    /// <summary>
    ///     Run a recognition.
    /// </summary>
    /// <param name="entry">The recognition entry name.</param>
    /// <param name="recognitionOverride">The json used to override the recognition.</param>
    /// <param name="image">The image to be recognized.</param>
    /// <returns><see cref="RecognitionDetail"/> if the operation was executed successfully; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    RecognitionDetail? RunRecognition(string entry, string recognitionOverride, IMaaImageBuffer image);

    /// <summary>
    ///     Run an action.
    /// </summary>
    /// <param name="entry">The action entry name.</param>
    /// <param name="actionOverride">The json used to override the action.</param>
    /// <param name="recognitionBox">The rect buffer containing current rect in the recognition result.</param>
    /// <param name="recognitionDetail">The rect detail in the recognition result.</param>
    /// <returns><see cref="NodeDetail"/> if the operation was executed successfully; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    NodeDetail? RunAction(string entry, string actionOverride, IMaaRectBuffer recognitionBox, string recognitionDetail);

    /// <summary>
    ///     Override a pipeline.
    /// </summary>
    /// <param name="pipelineOverride">The json used to override the pipeline.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool OverridePipeline(string pipelineOverride);

    /// <summary>
    ///     Override the property field "next" in a task.
    /// </summary>
    /// <param name="nodeName">The task name.</param>
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
}
