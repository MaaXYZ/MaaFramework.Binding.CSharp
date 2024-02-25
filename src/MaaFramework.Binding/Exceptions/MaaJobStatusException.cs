namespace MaaFramework.Binding;

/// <summary>
///     The exception is thrown when the <see cref="MaaJobStatus"/> of a <see cref="IMaaJob"/> is unexpected.
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
    ///     Creates a <see cref="MaaJobStatusException"/> instance.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="args">The key arguments.</param>
    public MaaJobStatusException(string message = "MaaJobStatus was unexpected.", params object?[] args) : base(string.Join('\n', message, args))
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaJobStatusException"/> instance.
    /// </summary>
    /// <param name="status">The MaaJobStatus.</param>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="args">The key arguments.</param>
    public MaaJobStatusException(MaaJobStatus status, string message = "", params object?[] args)
        : this(string.IsNullOrEmpty(message) ? $"MaaJobStatus cannot be {status}." : $"{message} MaaJobStatus cannot be {status}.", args)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaJobStatusException"/> instance.
    /// </summary>
    /// <param name="job">The IMaaJob.</param>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="args">The key arguments.</param>
    public MaaJobStatusException(IMaaJob job, string message = "", params object?[] args)
        : this(string.IsNullOrEmpty(message) ? $"MaaJobStatus cannot be {job?.Status}." : $"{message} MaaJobStatus cannot be {job?.Status}.", args)
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
    /// <param name="args">The key arguments.</param>
    /// <exception cref="MaaJobStatusException"></exception>
    public static void ThrowIf(this MaaJobStatus current, MaaJobStatus incorrect, string message = "", params object?[] args)
    {
        if (current == incorrect) throw new MaaJobStatusException(current, message, args);
    }

    /// <summary>
    ///     Throws a <see cref="MaaJobStatusException"/> if the <paramref name="current"/> status is not in a <paramref name="correct"/> status.
    /// </summary>
    /// <param name="current">The current status.</param>
    /// <param name="correct">The correct status.</param>
    /// <param name="message">The message.</param>
    /// <param name="args">The key arguments.</param>
    /// <exception cref="MaaJobStatusException"></exception>
    public static void ThrowIfNot(this MaaJobStatus current, MaaJobStatus correct, string message = "", params object?[] args)
    {
        if (current != correct) throw new MaaJobStatusException(current, message, args);
    }
}
