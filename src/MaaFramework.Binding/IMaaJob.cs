using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding;

/// <summary>
///     An interface representing a MaaJob that wrapping a MaaId and methods to invoke MaaId.
/// </summary>
public interface IMaaJob
{
    /// <summary>
    ///     Gets a MaaId.
    /// </summary>
    MaaId Id { get; }

    /// <summary>
    ///     Gets the status of a <see cref="IMaaJob"/>.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="IMaaPost.GetStatus"/>.
    /// </remarks>
    MaaJobStatus Status { get; }

    /// <summary>
    ///     Waits for a <see cref="IMaaJob"/> to complete.
    /// </summary>
    /// <returns>The job status.</returns>
    /// <remarks>
    ///     Calls <see cref="IMaaPost.Wait"/>.
    /// </remarks>
    MaaJobStatus Wait();

    /// <summary>
    ///     Sets the parameters of a <see cref="IMaaJob"/>.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>true if the parameters was setted successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Calls <see cref="IMaaPost.SetParam"/>.
    /// </remarks>
    bool SetParam(string parameters);
}
