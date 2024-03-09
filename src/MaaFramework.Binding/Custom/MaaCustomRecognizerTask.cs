using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Custom;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

/// <summary>
///     A static class provides the delegates of <see cref="MaaCustomRecognizerTask" />.
/// </summary>
public static class MaaRecognizerApi
{
    /// <param name="syncContext">in</param>
    /// <param name="image">in</param>
    /// <param name="taskName">in</param>
    /// <param name="customRecognitionParam">in</param>
    /// <param name="outBox">out</param>
    /// <param name="outDetail">out</param>
    public delegate bool Analyze(IMaaSyncContext syncContext, IMaaImageBuffer image, string taskName, string customRecognitionParam, /*out*/ IMaaRectBuffer outBox, /*out*/ IMaaStringBuffer outDetail);
}

/// <summary>
///     MaaCustomRecognizerTask
/// </summary>
public class MaaCustomRecognizerTask : IMaaCustomTask
{
    /// <inheritdoc/>
    public string Name { get; set; } = string.Empty;

    public required MaaRecognizerApi.Analyze Analyze { get; init; }
}

