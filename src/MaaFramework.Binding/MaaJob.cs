using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for return value of Maa Post method.
/// </summary>
/// <param name="id">The MaaId.</param>
/// <param name="maa">The IMaaPost.</param>
public class MaaJob(MaaId id, IMaaPost maa)
{
    /// <summary>
    ///     Gets a MaaId.
    /// </summary>
    public MaaId Id => id;

    /// <summary>
    ///     Gets the status of a <see cref="MaaJob"/>.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="IMaaPost.GetStatus"/>.
    /// </remarks>
    public MaaJobStatus Status => maa.GetStatus(this);

    /// <summary>
    ///     Waits for a <see cref="MaaJob"/> to complete.
    /// </summary>
    /// <returns>The job status.</returns>
    /// <remarks>
    ///     Calls <see cref="IMaaPost.Wait"/>.
    /// </remarks>
    public MaaJobStatus Wait() => maa.Wait(this);
}
