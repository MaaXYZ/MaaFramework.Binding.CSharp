using MaaFramework.Binding.Abstractions;
using System.Security.Principal;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for return value of Maa Post method.
/// </summary>
public class MaaJob : IMaaJob
{
    private readonly IMaaPost _maa;

    /// <summary>
    ///     Creats a <see cref="MaaJob"/> instance.
    /// </summary>
    /// <param name="id">The MaaId.</param>
    /// <param name="maa">The IMaaPost.</param>
    public MaaJob(MaaId id, IMaaPost maa)
    {
        Id = id;
        _maa = maa;
    }

    /// <summary>
    ///     Creats a <see cref="MaaJob"/> instance.
    /// </summary>
    /// <param name="id">The MaaId.</param>
    /// <param name="maa">The IMaaPost.</param>
    public MaaJob(ulong id, IMaaPost maa)
    {
        IId = id;
        _maa = maa;
    }

    /// <inheritdoc/>
    public MaaId Id { get; }

    /// <inheritdoc/>
    public ulong IId { get; }

    /// <inheritdoc/>
    public MaaJobStatus Status => _maa.GetStatus(this);

    /// <inheritdoc/>
    public MaaJobStatus Wait() => _maa.Wait(this);

    /// <inheritdoc/>
    public bool SetParam(string parameters) => _maa.SetParam(this, parameters);
}
