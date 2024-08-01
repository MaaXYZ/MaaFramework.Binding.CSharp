namespace MaaFramework.Binding;

/// <inheritdoc/>
public class MaaTaskJob(MaaId id, IMaaInstance maa) : MaaJob(id, maa)
{
    /// <summary>
    ///     Gets the maa instance.
    /// </summary>
    /// <remarks>
    ///     A property used to simplify design of <see cref="TaskDetail.Query"/>.
    /// </remarks>
    public IMaaInstance Maa => maa;

    /// <summary>
    ///     Sets the parameters of a <see cref="MaaTaskJob"/>.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>true if the parameters was set successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Calls <see cref="IMaaInstance.SetTaskParam"/>.
    /// </remarks>
    public bool SetParam(string parameters) => maa.SetTaskParam(this, parameters);
}
