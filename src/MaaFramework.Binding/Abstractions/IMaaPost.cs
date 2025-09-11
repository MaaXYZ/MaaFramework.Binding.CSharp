namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An interface defining methods that use the <see cref="MaaJob"/> returned by the post methods.
/// </summary>
public interface IMaaPost
{
    /// <summary>
    ///     Gets the current status of a <see cref="MaaJob"/>.
    /// </summary>
    /// <param name="job">The MaaJob.</param>
    /// <returns>The status of <paramref name="job"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    MaaJobStatus GetStatus(MaaJob job);

    /// <summary>
    ///     Waits a <see cref="MaaJob"/>.
    /// </summary>
    /// <param name="job">The MaaJob.</param>
    /// <returns>The status at the end of the <paramref name="job"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    MaaJobStatus Wait(MaaJob job);
}
