namespace MaaFramework.Binding;

/// <summary>
///     Maa job status.
/// </summary>
public enum MaaJobStatus // MaaStatus is used to refer to System.Int32.
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
