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
    /// <typeparam name="T">The implemented type of <see cref="IMaaImageBuffer"/>.</typeparam>
    /// <param name="context">The context.</param>
    /// <param name="args">The run args.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool Run<T>(in IMaaContext context, in RunArgs<T> args) where T : IMaaImageBuffer;
}

/// <summary>
///     The arguments used for run.
/// </summary>
/// <typeparam name="T">Gets the implemented type of <see cref="IMaaImageBuffer"/>.</typeparam>
/// <param name="TaskName">Gets the task name.</param>
/// <param name="TaskDetail">Gets the task detail</param>
/// <param name="ActionName">Gets the action name.</param>
/// <param name="ActionParam">Gets the action param.</param>
/// <param name="RecognitionDetail">Gets the recognition detail.</param>
/// <param name="RecognitionBox">Gets the recognition box.</param>
public sealed record RunArgs<T>(string TaskName, TaskDetail TaskDetail, string ActionName, string ActionParam, RecognitionDetail<T> RecognitionDetail, IMaaRectBuffer RecognitionBox) where T : IMaaImageBuffer;
