namespace MaaFramework.Binding.Exceptions;

/// <summary>
///     The exception that is thrown when a <see cref="MaaInstance"/> fails to bind a <see cref="MaaResource"/> or a <see cref="MaaController"/>.
/// </summary>
public class MaaBindException : Exception
{
    /// <summary>
    ///     The exception that is thrown when a <see cref="MaaInstance"/> fails to bind a <see cref="MaaResource"/> or a <see cref="MaaController"/>.
    /// </summary>
    public MaaBindException(string message =
        "MaaInstance failed to bind MaaResource or MaaController in constructor of MaaObject."
        ) : base(message)
    {
    }

    internal static void ThrowIfFalse(bool condition)
    {
        if (!condition) { throw new MaaBindException(); }
    }
}
