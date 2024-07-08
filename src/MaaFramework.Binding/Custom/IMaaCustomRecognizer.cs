using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Custom;

/// <summary>
///     An interface representing implementation is maa custom recognizer.
/// </summary>
public interface IMaaCustomRecognizer : IMaaCustomTask
{
    /// <summary>
    ///     Write the recognition result to the out_box and return true if the recognition is
    /// successful. If the recognition fails, return false. You can also write details to the
    /// out_detail buffer.
    /// </summary>
    bool Analyze(in IMaaSyncContext syncContext, IMaaImageBuffer image, string taskName, string customRecognitionParam, in IMaaRectBuffer outBox, in IMaaStringBuffer outDetail);
}

