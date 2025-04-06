namespace MaaFramework.Binding;

#pragma warning disable IDE0290 // 使用主构造函数

/// <summary>
///     The exception is thrown when the <see cref="MaaJobStatus"/> of a <see cref="MaaFramework.Binding.MaaJob"/> is unexpected.
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
    /// <param name="job">The MaaJob.</param>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="args">The key arguments.</param>
    public MaaJobStatusException(MaaJob job, string message = "", params object?[] args)
        : this(string.IsNullOrEmpty(message) ? $"MaaJobStatus cannot be {job?.Status}." : $"{message} MaaJobStatus cannot be {job?.Status}.", args)
    {
    }
}
