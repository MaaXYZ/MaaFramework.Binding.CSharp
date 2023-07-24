using MaaToolKit.Extensions.Exceptions;

namespace MaaToolKit.Extensions.ComponentModel;

/// <summary>
/// 
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

/// <summary>
///     The class for throwing a <see cref="MaaJobStatusException"/>.
/// </summary>
public static class MaaJobStatusThrow
{
    /// <summary>
    ///     Throws a <see cref="MaaJobStatusException"/> if the <paramref name="current"/> status is in a <paramref name="incorrect"/> status.
    /// </summary>
    /// <param name="current">The current status.</param>
    /// <param name="incorrect">The incorrect status.</param>
    /// <exception cref="MaaJobStatusException"></exception>
    public static void ThrowIf(this MaaJobStatus current, MaaJobStatus incorrect)
    {
        if (current == incorrect) { throw new MaaJobStatusException(current); }
    }

    /// <summary>
    ///     Throws a <see cref="MaaJobStatusException"/> if the <paramref name="current"/> status is not in a <paramref name="correct"/> status.
    /// </summary>
    /// <param name="current">The current status.</param>
    /// <param name="correct">The correct status.</param>
    /// <exception cref="MaaJobStatusException"></exception>
    public static void ThrowIfNot(this MaaJobStatus current, MaaJobStatus correct)
    {
        if (current != correct) { throw new MaaJobStatusException(current); }
    }
}
