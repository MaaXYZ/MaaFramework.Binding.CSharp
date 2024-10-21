namespace MaaFramework.Binding;

/// <summary>
///     The exception that is the base class for custom exceptions in <see cref="MaaFramework.Binding"/>.
/// </summary>
public class MaaException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MaaException"/> class.
    /// </summary>
    public MaaException()
        : base("MaaFramework.Binding threw an undefined exception.")
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MaaException"/> class with a specified error message.
    /// </summary>
    public MaaException(string? message)
        : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MaaException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    public MaaException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
