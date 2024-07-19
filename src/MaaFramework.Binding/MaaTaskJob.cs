namespace MaaFramework.Binding;

/// <inheritdoc/>
public class MaaTaskJob(MaaId id, IMaaInstance maa) : MaaJob(id, maa)
{
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
