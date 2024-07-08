using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Custom;

/// <summary>
///     An interface representing implementation is maa custom action.
/// </summary>
public interface IMaaCustomAction : IMaaCustomTask
{
    /// <summary/>
    bool Run(in IMaaSyncContext syncContext, string taskName, string customActionParam, IMaaRectBuffer curBox, string curRecDetail);

    /// <summary/>
    void Abort();
}
