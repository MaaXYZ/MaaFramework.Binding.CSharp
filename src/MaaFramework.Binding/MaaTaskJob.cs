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
}
