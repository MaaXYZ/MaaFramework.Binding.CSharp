namespace MaaFramework.Binding.Exceptions;

/// <summary>
///     The exception that is thrown when a <see cref="MaaInstance"/> fails to bind a <see cref="MaaResource"/> or a <see cref="MaaController"/>.
/// </summary>
public class MaaBindException : MaaException
{
    internal const string DefaultMessage = "MaaInstance failed to bind MaaResource or MaaController.";
    internal const string ResourceMessage = "MaaInstance failed to bind MaaResource.";
    internal const string ControllerMessage = "MaaInstance failed to bind MaaController.";
    internal const string ResourceModifiedMessage = "Binding MaaResource was modified.";
    internal const string ControllerModifiedMessage = "Binding MaaController was modified.";

    /// <summary>
    ///     The exception that is thrown when a <see cref="MaaInstance"/> fails to bind a <see cref="MaaResource"/> or a <see cref="MaaController"/>.
    /// </summary>
    public MaaBindException(string? message = DefaultMessage) : base(message)
    {
    }

    internal static void ThrowIfFalse(bool condition, string message = DefaultMessage)
    {
        if (!condition) { throw new MaaBindException(message); }
    }
}
