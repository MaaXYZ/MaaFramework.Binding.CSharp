using MaaFramework.Binding.Enums;

namespace MaaFramework.Binding.Exceptions;

/// <summary>
///     The exception that is thrown when the <see cref="MaaJobStatus"/> of a <see cref="MaaJob"/> is incorrect.
/// </summary>
public class MaaJobStatusException : MaaException
{
    /// <summary>
    /// 
    /// </summary>
    public MaaJobStatusException()
        : base("MaaJobStatus was unexpected.")
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public MaaJobStatusException(string? message) : base(message)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public MaaJobStatusException(MaaJobStatus status)
        : this($"MaaJobStatus cannot be {status}.")
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public MaaJobStatusException(MaaJob job)
        : this($"MaaJobStatus cannot be {job.Status}.")
    {
    }
}

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
        if (current == incorrect) throw new MaaJobStatusException(current);
    }

    /// <summary>
    ///     Throws a <see cref="MaaJobStatusException"/> if the <paramref name="current"/> status is not in a <paramref name="correct"/> status.
    /// </summary>
    /// <param name="current">The current status.</param>
    /// <param name="correct">The correct status.</param>
    /// <exception cref="MaaJobStatusException"></exception>
    public static void ThrowIfNot(this MaaJobStatus current, MaaJobStatus correct)
    {
        if (current != correct) throw new MaaJobStatusException(current);
    }

    internal static void ThrowIfMaaControllerNotSuccess(this MaaJobStatus current)
    {
        if (current != MaaJobStatus.Success) throw new MaaJobStatusException($"{nameof(MaaController)} failed to connect to the device. Connection status: {current}.");
    }

    internal static void ThrowIfMaaResourceNotSuccess(this MaaJobStatus current)
    {
        if (current != MaaJobStatus.Success) throw new MaaJobStatusException($"{nameof(MaaResource)} failed to connect to load resources. Loading status: {current}.");
    }
}
