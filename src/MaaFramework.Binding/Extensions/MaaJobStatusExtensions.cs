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

    /// <summary>
    ///     Gets a value indicates whether current job status is pending.
    /// </summary>
    /// <param name="current">The current job status.</param>
    /// <returns><see langword="true"/> if <paramref name="current"/> is pending; otherwise, <see langword="false"/>.</returns>
    public static bool IsPending(this MaaJobStatus current)
        => current is MaaJobStatus.Pending;

    /// <summary>
    ///     Gets a value indicates whether current job status is running.
    /// </summary>
    /// <param name="current">The current job status.</param>
    /// <returns><see langword="true"/> if <paramref name="current"/> is running; otherwise, <see langword="false"/>.</returns>
    public static bool IsRunning(this MaaJobStatus current)
        => current is MaaJobStatus.Running;

    /// <summary>
    ///     Gets a value indicates whether current job status is succeeded.
    /// </summary>
    /// <param name="current">The current job status.</param>
    /// <returns><see langword="true"/> if <paramref name="current"/> is succeeded; otherwise, <see langword="false"/>.</returns>
    public static bool IsSucceeded(this MaaJobStatus current)
        => current is MaaJobStatus.Succeeded;

    /// <summary>
    ///     Gets a value indicates whether current job status is failed.
    /// </summary>
    /// <param name="current">The current job status.</param>
    /// <returns><see langword="true"/> if <paramref name="current"/> is failed; otherwise, <see langword="false"/>.</returns>
    public static bool IsFailed(this MaaJobStatus current)
        => current is MaaJobStatus.Failed;

    /// <summary>
    ///     Gets a value indicates whether current job status is succeeded or failed.
    /// </summary>
    /// <param name="current">The current job status.</param>
    /// <returns><see langword="true"/> if <paramref name="current"/> is succeeded or failed; otherwise, <see langword="false"/>.</returns>
    public static bool IsDone(this MaaJobStatus current)
        => current is MaaJobStatus.Succeeded or MaaJobStatus.Failed;
}
