using System.Runtime.Serialization;

namespace MaaFramework.Binding.Exceptions;

/// <summary>
///     The exception that is the base class for custom exceptions in <see cref="MaaFramework.Binding"/>.
/// </summary>
public class MaaException : Exception
{
    /// <inheritdoc cref="Exception()"/>
    public MaaException()
        : base("MaaFramework.Binding threw an undefined exception.")
    {
    }

    /// <inheritdoc cref="Exception(string)"/>
    public MaaException(string? message)
        : base(message)
    {
    }

    /// <inheritdoc cref="Exception(string, Exception)"/>
    public MaaException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <inheritdoc cref="Exception(SerializationInfo, StreamingContext)"/>
    public MaaException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
