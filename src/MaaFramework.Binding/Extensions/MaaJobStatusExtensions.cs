namespace MaaFramework.Binding;

/// <summary>
///     A static class providing extension methods for <see cref="MaaJobStatus"/>.
/// </summary>
public static class MaaJobStatusExtensions
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
