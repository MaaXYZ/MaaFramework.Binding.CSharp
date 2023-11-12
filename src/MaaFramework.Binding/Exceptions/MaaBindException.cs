namespace MaaFramework.Binding;

/// <summary>
///     The exception is thrown when a <see cref="IMaaInstance"/> fails to bind a <see cref="IMaaResource"/> or a <see cref="IMaaController"/>.
/// </summary>
public class MaaBindException : MaaException
{
    /// <summary>
    ///     Default message.
    /// </summary>
    public const string DefaultMessage = "MaaInstance failed to bind MaaResource or MaaController.";

    /// <summary>
    ///     Resource binding failed message.
    /// </summary>
    public const string ResourceMessage = "MaaInstance failed to bind MaaResource.";

    /// <summary>
    ///     Controller binding failed message.
    /// </summary>
    public const string ControllerMessage = "MaaInstance failed to bind MaaController.";

    /// <summary>
    ///     Resource modified message.
    /// </summary>
    public const string ResourceModifiedMessage = "Binding MaaResource was modified.";

    /// <summary>
    ///     Controller modified message.
    /// </summary>
    public const string ControllerModifiedMessage = "Binding MaaController was modified.";

    /// <summary>
    ///     The exception is thrown when a <see cref="IMaaInstance"/> fails to bind a <see cref="IMaaResource"/> or a <see cref="IMaaController"/>.
    /// </summary>
    public MaaBindException(string message = DefaultMessage) : base(message)
    {
    }

    /// <summary>
    ///     Throws a <see cref="MaaBindException"/> when a condition is true.
    /// </summary>
    /// <param name="condition">The condition.</param>
    /// <param name="message">The message of <see cref="MaaBindException"/>.</param>
    /// <exception cref="MaaBindException"/>
    public static void ThrowIf(bool condition, string message = DefaultMessage)
    {
        if (condition) { throw new MaaBindException(message); }
    }
}
