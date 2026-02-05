using MaaFramework.Binding.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding.Custom;

/// <summary>
///     An interface representing which implementation is maa custom recognition.
/// </summary>
public interface IMaaCustomRecognition : IMaaCustom
{
    /// <summary>
    ///     Analyze with <paramref name="args"/>, and write the recognition <paramref name="results"/>.
    /// </summary>
    /// <typeparam name="T">The type of context.</typeparam>
    /// <param name="context">The context.</param>
    /// <param name="args">The args.</param>
    /// <param name="results">The recognition results.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool Analyze<T>(T context, in AnalyzeArgs args, in AnalyzeResults results) where T : IMaaContext;
}

/// <summary>
///     A readonly record <see langword="struct"/> that packages <c>arguments</c> in <see cref="IMaaCustomRecognition.Analyze"/>.
/// </summary>
/// <param name="NodeName">Gets the node name.</param>
/// <param name="TaskDetail">Gets the task detail.</param>
/// <param name="RecognitionName">Gets the recognition name.</param>
/// <param name="RecognitionParam">Gets the recognition param.</param>
/// <param name="Image">Gets the image.</param>
/// <param name="Roi">Gets the roi.</param>
public readonly record struct AnalyzeArgs(string NodeName, TaskDetail TaskDetail, string RecognitionName, [StringSyntax("Json")] string RecognitionParam, IMaaImageBuffer Image, IMaaRectBuffer Roi);

/// <summary>
///     A readonly record <see langword="struct"/> that packages <c>results</c> in <see cref="IMaaCustomRecognition.Analyze"/>.
/// </summary>
/// <param name="Box">Gets a <see cref="IMaaRectBuffer"/> for writing the recognition box.</param>
/// <param name="Detail">Gets a <see cref="IMaaStringBuffer"/> for writing the recognition detail.</param>
public readonly record struct AnalyzeResults(IMaaRectBuffer Box, IMaaStringBuffer Detail);
