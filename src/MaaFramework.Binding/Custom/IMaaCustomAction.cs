using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Custom;

/// <summary>
///     An interface representing implementation is maa custom action.
/// </summary>
public interface IMaaCustomAction : IMaaCustomResource
{
    /// <summary>
    ///     Run custom action.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="args">The run args.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool Run(in IMaaContext context, in RunArgs args);
}

/// <summary>
///     The arguments used for run.
/// </summary>
/// <param name="NodeName">Gets the node name.</param>
/// <param name="TaskDetail">Gets the task detail</param>
/// <param name="ActionName">Gets the action name.</param>
/// <param name="ActionParam">Gets the action param.</param>
/// <param name="RecognitionDetail">Gets the recognition detail.</param>
/// <param name="RecognitionBox">Gets the recognition box.</param>
public sealed record RunArgs(string NodeName, TaskDetail TaskDetail, string ActionName, string ActionParam, RecognitionDetail RecognitionDetail, IMaaRectBuffer RecognitionBox);
