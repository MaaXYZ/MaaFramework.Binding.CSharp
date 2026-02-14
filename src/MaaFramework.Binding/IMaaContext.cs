using MaaFramework.Binding.Buffers;
using System.Diagnostics.CodeAnalysis;

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
    ///     Gets whether cancellation has been requested for this context.
    /// </summary>
    /// <returns><see langword="true"/> if cancellation has been requested; otherwise, <see langword="false"/>.</returns>
    bool IsCancellationRequested { get; }

    /// <summary>
    ///     Runs a task.
    /// </summary>
    /// <param name="entry">The entry name of the task.</param>
    /// <param name="pipelineOverride">The json used to override the pipeline.</param>
    /// <returns><see cref="TaskDetail"/> if the operation was executed successfully; otherwise, <see langword="null"/>.</returns>
    TaskDetail? RunTask(string entry, [StringSyntax("Json")] string pipelineOverride = "{}");

    /// <summary>
    ///     Runs a recognition.
    /// </summary>
    /// <param name="entry">The recognition entry name.</param>
    /// <param name="image">The image to be recognized.</param>
    /// <param name="pipelineOverride">The json used to override the pipeline.</param>
    /// <returns><see cref="RecognitionDetail"/> if the operation was executed successfully; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    RecognitionDetail? RunRecognition(string entry, IMaaImageBuffer image, [StringSyntax("Json")] string pipelineOverride = "{}");

    /// <summary>
    ///     Runs an action.
    /// </summary>
    /// <param name="entry">The action entry name.</param>
    /// <param name="recognitionBox">The rect buffer containing current rect in the recognition result.</param>
    /// <param name="recognitionDetail">The rect detail in the recognition result.</param>
    /// <param name="pipelineOverride">The json used to override the pipeline.</param>
    /// <returns><see cref="ActionDetail"/> if the operation was executed successfully; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    ActionDetail? RunAction(string entry, IMaaRectBuffer recognitionBox, [StringSyntax("Json")] string recognitionDetail = "", [StringSyntax("Json")] string pipelineOverride = "{}");

    /// <summary>
    ///     Runs recognition directly with type and parameters, without requiring a pipeline entry.
    /// </summary>
    /// <param name="type">The recognition type.<para>(e.g., "OCR", "TemplateMatch")</para></param>
    /// <param name="param">The recognition parameters json.</param>
    /// <param name="image">The image to be recognized.</param>
    /// <returns><see cref="RecognitionDetail"/> if the operation was executed successfully; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    RecognitionDetail? RunRecognitionDirect(string type, [StringSyntax("Json")] string param, IMaaImageBuffer image);

    /// <summary>
    ///     Runs action directly with type and parameters, without requiring a pipeline entry.
    /// </summary>
    /// <param name="type">The action type.<para>(e.g., "Click", "Swipe")</para></param>
    /// <param name="param">The action parameters json.</param>
    /// <param name="recognitionBox">The rect buffer containing current rect in the recognition result.</param>
    /// <param name="recognitionDetail">The rect detail in the recognition result.</param>
    /// <returns><see cref="ActionDetail"/> if the operation was executed successfully; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    ActionDetail? RunActionDirect(string type, [StringSyntax("Json")] string param, IMaaRectBuffer recognitionBox, [StringSyntax("Json")] string recognitionDetail = "");

    /// <summary>
    ///     Waits for screen to stabilize (freeze).
    /// </summary>
    /// <param name="millisecondsTime">The wait time in milliseconds.</param>
    /// <param name="recognitionBox">The recognition hit box, used when target is Self to calculate ROI.</param>
    /// <param name="waitFreezesParam">The wait parameters.<para>Supports time, target, target_offset, threshold, method, rate_limit, timeout.</para></param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    ///     <paramref name="millisecondsTime"/> and <paramref name="waitFreezesParam"/>.time are mutually exclusive.
    /// </remarks>
    /// <exception cref="ArgumentNullException"/>
    bool WaitFreezes(MaaSize millisecondsTime, IMaaRectBuffer recognitionBox, [StringSyntax("Json")] string waitFreezesParam = "{}");

    /// <param name="time">The wait time.</param>
    /// <param name="recognitionBox">The recognition hit box, used when target is Self to calculate ROI.</param>
    /// <param name="waitFreezesParam">The wait parameters.<para>Supports time, target, target_offset, threshold, method, rate_limit, timeout.</para></param>
    /// <remarks>
    ///     <paramref name="time"/> and <paramref name="waitFreezesParam"/>.time are mutually exclusive.
    /// </remarks>
    /// <inheritdoc cref="WaitFreezes(ulong, IMaaRectBuffer, string)"/>
    /// <exception cref="ArgumentNullException"/>
    bool WaitFreezes(TimeSpan time, IMaaRectBuffer recognitionBox, [StringSyntax("Json")] string waitFreezesParam = "{}");

    /// <summary>
    ///     Overrides a pipeline.
    /// </summary>
    /// <param name="pipelineOverride">The json used to override the pipeline.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool OverridePipeline([StringSyntax("Json")] string pipelineOverride);

    /// <summary>
    ///     Overrides the property field "next" in a node.
    /// </summary>
    /// <param name="nodeName">The node name.</param>
    /// <param name="nextList">The next list.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool OverrideNext(string nodeName, IEnumerable<string> nextList);

    /// <summary>
    ///     Overrides the image which name from the value of property field e.g. "template".
    /// </summary>
    /// <param name="imageName">The image name.</param>
    /// <param name="image">An <see cref="IMaaImageBuffer"/> used to set the image.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool OverrideImage(string imageName, IMaaImageBuffer image);

    /// <summary>
    ///     Gets the node data from the <see cref="IMaaContext"/> by node name.
    /// </summary>
    /// <param name="nodeName">The node name.</param>
    /// <param name="data">The node data.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool GetNodeData(string nodeName, [MaybeNullWhen(false)][StringSyntax("Json")] out string data);

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

    /// <summary>
    ///     Sets an anchor to a node.
    /// </summary>
    /// <param name="anchorName">The anchor name.</param>
    /// <param name="nodeName">The node name.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool SetAnchor(string anchorName, string nodeName);

    /// <summary>
    ///     Gets the node name from an anchor.
    /// </summary>
    /// <param name="anchorName">The anchor name.</param>
    /// <param name="nodeName">The node name.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool GetAnchor(string anchorName, [MaybeNullWhen(false)] out string nodeName);

    /// <summary>
    ///     Gets the hit count of a node.
    /// </summary>
    /// <param name="nodeName">The node name.</param>
    /// <param name="count">The hit count.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool GetHitCount(string nodeName, out ulong count);

    /// <summary>
    ///     Clears the hit count of a node.
    /// </summary>
    /// <param name="nodeName">The node name.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool ClearHitCount(string nodeName);
}
