namespace MaaFramework.Binding;

/// <inheritdoc/>
public class MaaTaskJob(MaaId id, IMaaTasker tasker) : MaaJob(id, tasker)
{
    /// <inheritdoc/>
    public override string ToString() => $"{GetType().Name} {{ {nameof(Status)} = {Status}, {nameof(TaskDetail.Entry)} = {this.QueryTaskDetail()?.Entry} }}";

    /// <summary>
    ///     Gets the maa tasker.
    /// </summary>
    /// <remarks>
    ///     A property used to simplify design of <see cref="TaskDetail.Query"/>.
    /// </remarks>
    public IMaaTasker Tasker => tasker;

    /// <summary>
    ///     Waits for a <see cref="MaaTaskJob"/> to complete with an expected <paramref name="status"/>.
    /// </summary>
    /// <param name="status">The expected status.</param>
    /// <returns><see langword="this"/> if it completes with the <paramref name="status"/>; otherwise, <see langword="null"/>.</returns>
    public MaaTaskJob? WaitFor(MaaJobStatus status)
        => Wait() == status ? this : null;
}
