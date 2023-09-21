namespace MaaToolKit.Extensions.Exceptions;

/// <summary>
///     The exception that is thrown when the <see cref="MaaJobStatus"/> of a <see cref="MaaJob"/> is incorrect.
/// </summary>
public class MaaJobStatusException : Exception
{
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
