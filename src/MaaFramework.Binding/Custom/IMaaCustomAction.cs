using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Custom;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

/// <summary>
///     An interface representing implementation is maa custom action.
/// </summary>
public interface IMaaCustomAction : IMaaCustomTask
{
    bool Run(in IMaaSyncContext syncContext, string taskName, string customActionParam, IMaaRectBuffer curBox, string curRecDetail);
    void Abort();
}
