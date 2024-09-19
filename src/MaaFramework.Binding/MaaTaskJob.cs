namespace MaaFramework.Binding;

/// <inheritdoc/>
public class MaaTaskJob(MaaId id, IMaaTasker tasker) : MaaJob(id, tasker)
{
    /// <summary>
    ///     Gets the maa tasker.
    /// </summary>
    /// <remarks>
    ///     A property used to simplify design of <see cref="TaskDetail.Query"/>.
    /// </remarks>
    public IMaaTasker Tasker => tasker;
}
