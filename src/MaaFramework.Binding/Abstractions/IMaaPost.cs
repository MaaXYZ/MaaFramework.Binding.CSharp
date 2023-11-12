namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An interface defining methods that use the <see cref="IMaaJob"/> returned by the post methods.
/// </summary>
public interface IMaaPost
{
    /// <summary>
    ///     Sets <paramref name="param"/> of a <see cref="IMaaJob"/>.
    /// </summary>
    /// <param name="job">The MaaJob.</param>
    /// <param name="param">The param, which could be parsed to a JSON.</param>
    /// <returns>true if the <paramref name="param"/> were setted successfully in the <paramref name="job"/>; otherwise, false.</returns>
    bool SetParam(IMaaJob job, string param);

    /// <summary>
    ///     Gets the current status of a <see cref="IMaaJob"/>.
    /// </summary>
    /// <param name="job">The MaaJob.</param>
    /// <returns>The status of <paramref name="job"/>.</returns>
    MaaJobStatus GetStatus(IMaaJob job);

    /// <summary>
    ///     Waits a <see cref="IMaaJob"/>.
    /// </summary>
    /// <param name="job">The MaaJob.</param>
    /// <returns>The status at the end of the <paramref name="job"/>.</returns>
    MaaJobStatus Wait(IMaaJob job);
}
