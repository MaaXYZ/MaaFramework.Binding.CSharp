using MaaFramework.Binding.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding.Custom;

/// <summary>
///     An interface representing which implementation is maa custom action.
/// </summary>
public interface IMaaCustomAction : IMaaCustomResource
{
    /// <summary>
    ///     Run with <paramref name="args"/>, and write the run <paramref name="results"/>.
    /// </summary>
    /// <typeparam name="T">The type of context.</typeparam>
    /// <param name="context">The context.</param>
    /// <param name="args">The run args.</param>
    /// <param name="results">The run results.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool Run<T>(T context, in RunArgs args, in RunResults results) where T : IMaaContext;
}

/// <summary>
///     A readonly record <see langword="struct"/> that packages <c>arguments</c> in <see cref="IMaaCustomAction.Run"/>.
/// </summary>
/// <param name="NodeName">Gets the node name.</param>
/// <param name="TaskDetail">Gets the task detail</param>
/// <param name="ActionName">Gets the action name.</param>
/// <param name="ActionParam">Gets the action param.</param>
/// <param name="RecognitionDetail">Gets the recognition detail.</param>
/// <param name="RecognitionBox">Gets the recognition box.</param>
public readonly record struct RunArgs(string NodeName, TaskDetail TaskDetail, string ActionName, [StringSyntax("Json")] string ActionParam, RecognitionDetail RecognitionDetail, IMaaRectBuffer RecognitionBox);

/// <summary>
///     A readonly record <see langword="struct"/> that packages <c>results</c> in <see cref="IMaaCustomAction.Run"/>.
/// </summary>
/// <remarks>
///     This a empty record.
/// </remarks>
public readonly record struct RunResults;
