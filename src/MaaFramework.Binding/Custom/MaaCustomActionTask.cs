using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Custom;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

/// <summary>
///     A static class provides the delegates of <see cref="MaaCustomActionTask" />.
/// </summary>
public static class MaaActionApi
{
    public delegate bool Run(IMaaSyncContext syncContext, string taskName, string customActionParam, IMaaRectBuffer curBox, string curRecDetail);

    public delegate void Abort();
}

/// <summary>
///     MaaCustomActionTask
/// </summary>
public class MaaCustomActionTask : IMaaCustomTask
{
    /// <inheritdoc/>
    public string Name { get; set; } = string.Empty;

    public required MaaActionApi.Run Run { get; init; }
    public required MaaActionApi.Abort Abort { get; init; }
}
