namespace MaaFramework.Binding;

/// <summary>
///     The exception is thrown when the <see cref="MaaJobStatus"/> of a <see cref="IMaaJob"/> is incorrect.
/// </summary>
public class MaaJobStatusException : MaaException
{
    /// <summary>
    ///     Maa controller message.
    /// </summary>
    public const string MaaControllerMessage = $"{nameof(IMaaController)} failed to connect to the device.";

    /// <summary>
    ///     Maa resource message.
    /// </summary>
    public const string MaaResourceMessage = $"{nameof(IMaaResource)} failed to load resources.";

    /// <summary>
    ///     The exception is thrown when a <see cref="MaaJobStatus"/> is unexpected.
    /// </summary>
    public MaaJobStatusException(string message = "MaaJobStatus was unexpected.") : base(message)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public MaaJobStatusException(MaaJobStatus status, string message = "")
        : this(string.IsNullOrEmpty(message) ? $"MaaJobStatus cannot be {status}." : $"{message} MaaJobStatus cannot be {status}.")
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public MaaJobStatusException(IMaaJob job, string message = "")
        : this(string.IsNullOrEmpty(message) ? $"MaaJobStatus cannot be {job.Status}." : $"{message} MaaJobStatus cannot be {job.Status}.")
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
    /// <param name="message">The message.</param>
    /// <exception cref="MaaJobStatusException"></exception>
    public static void ThrowIf(this MaaJobStatus current, MaaJobStatus incorrect, string message = "")
    {
        if (current == incorrect) throw new MaaJobStatusException(current, message);
    }

    /// <summary>
    ///     Throws a <see cref="MaaJobStatusException"/> if the <paramref name="current"/> status is not in a <paramref name="correct"/> status.
    /// </summary>
    /// <param name="current">The current status.</param>
    /// <param name="correct">The correct status.</param>
    /// <param name="message">The message.</param>
    /// <exception cref="MaaJobStatusException"></exception>
    public static void ThrowIfNot(this MaaJobStatus current, MaaJobStatus correct, string message = "")
    {
        if (current != correct) throw new MaaJobStatusException(current, message);
    }
}
