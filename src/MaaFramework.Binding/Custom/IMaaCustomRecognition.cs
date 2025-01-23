using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Custom;

/// <summary>
///     An interface representing implementation is maa custom recognition.
/// </summary>
public interface IMaaCustomRecognition : IMaaCustomResource
{
    /// <summary>
    ///     Analyze <paramref name="args"/>, and write the recognition <paramref name="results"/>.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="args">The args.</param>
    /// <param name="results">The recognition results.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool Analyze(in IMaaContext context, in AnalyzeArgs args, in AnalyzeResults results);
}

/// <summary>
///     The arguments used for analysis.
/// </summary>
/// <param name="NodeName">Gets the node name.</param>
/// <param name="TaskDetail">Gets the task detail.</param>
/// <param name="RecognitionName">Gets the recognition name.</param>
/// <param name="RecognitionParam">Gets the recognition param.</param>
/// <param name="Image">Gets the image.</param>
/// <param name="Roi">Gets the roi.</param>
public sealed record AnalyzeArgs(string NodeName, TaskDetail TaskDetail, string RecognitionName, string RecognitionParam, IMaaImageBuffer Image, IMaaRectBuffer Roi);

/// <summary>
///     The recognition analysis results.
/// </summary>
/// <param name="Box">Gets a <see cref="IMaaRectBuffer"/> for writing the recognition box.</param>
/// <param name="Detail">Gets a <see cref="IMaaStringBuffer"/> for writing the recognition detail.</param>
public sealed record AnalyzeResults(IMaaRectBuffer Box, IMaaStringBuffer Detail);
