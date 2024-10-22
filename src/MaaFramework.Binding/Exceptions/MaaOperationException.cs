using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MaaFramework.Binding;

/// <summary>
///     The exception is thrown when a maa interoperation failed.
/// </summary>
public class MaaInteroperationException : MaaException
{
    /// <summary>
    ///     Resource binding failed message.
    /// </summary>
    public const string ResourceBindingFailedMessage = "MaaTasker failed to bind MaaResource.";

    /// <summary>
    ///     Controller binding failed message.
    /// </summary>
    public const string ControllerBindingFailedMessage = "MaaTasker failed to bind MaaController.";

    /// <summary>
    ///     Resource modified message.
    /// </summary>
    public const string ResourceModifiedMessage = "Binding MaaResource was modified.";

    /// <summary>
    ///     Controller modified message.
    /// </summary>
    public const string ControllerModifiedMessage = "Binding MaaController was modified.";

    /// <summary>
    ///     Initializes a new instance of the <see cref="MaaInteroperationException"/> class with a specified error message.
    /// </summary>
    public MaaInteroperationException(string message) : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MaaInteroperationException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    public MaaInteroperationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    ///     Throws a <see cref="MaaInteroperationException"/> when the <paramref name="condition"/> is <see langword="true"/>.
    /// </summary>
    /// <param name="condition">The condition.</param>
    /// <param name="message">The message of <see cref="MaaInteroperationException"/>.</param>
    /// <param name="operationExpression">The operation expression of condition.</param>
    /// <exception cref="MaaInteroperationException"/>
    public static void ThrowIf(bool condition, string message = "", [CallerArgumentExpression(nameof(condition))] string? operationExpression = null)
    {
        if (condition) { throw new MaaInteroperationException($"'{operationExpression}' evaluated as a failure. {message}"); }
    }

    /// <summary>
    ///     Throws a <see cref="MaaInteroperationException"/> when the <paramref name="condition"/> is <see langword="false"/>.
    /// </summary>
    /// <param name="condition">The condition.</param>
    /// <param name="message">The message of <see cref="MaaInteroperationException"/>.</param>
    /// <param name="operationExpression">The operation expression of result.</param>
    /// <exception cref="MaaInteroperationException"/>
    public static void ThrowIfNot(bool condition, string message = "", [CallerArgumentExpression(nameof(condition))] string? operationExpression = null)
    {
        if (!condition) { throw new MaaInteroperationException($"'{operationExpression}' evaluated as a failure. {message}"); }
    }

    /// <summary>
    ///     Throws a <see cref="MaaInteroperationException"/> when the <paramref name="operation"/> is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">Any type.</typeparam>
    /// <param name="operation">The operation.</param>
    /// <param name="message">The message of <see cref="MaaInteroperationException"/>.</param>
    /// <param name="operationExpression">The operation expression of result.</param>
    /// <exception cref="MaaInteroperationException"/>
    public static void ThrowIfNull<T>([NotNull] T? operation, string message = "", [CallerArgumentExpression(nameof(operation))] string? operationExpression = null)
    {
        if (operation is null) { throw new MaaInteroperationException($"Value cannot be null. (Operation '{operationExpression}') {message}"); }
    }
}
