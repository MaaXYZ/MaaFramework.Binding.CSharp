using MaaFramework.Binding.Exceptions;

namespace MaaFramework.Binding.Enums;

/// <summary>
///     Maa job status.
/// </summary>
public enum MaaJobStatus
{
    /// <summary>
    /// 
    /// </summary>
    Invalid = 0,
    /// <summary>
    /// 
    /// </summary>
    Pending = 1000,
    /// <summary>
    /// 
    /// </summary>
    Running = 2000,
    /// <summary>
    /// 
    /// </summary>
    Success = 3000,
    /// <summary>
    /// 
    /// </summary>
    Failed = 4000,

    // Timeout = 5000,
};
