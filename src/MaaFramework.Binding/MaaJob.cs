using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Enums;

namespace MaaFramework.Binding;

/// <summary>
/// A class providing a reference implementation for return value of Maa Post method.
/// </summary>
public class MaaJob
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="job"></param>
    public static implicit operator MaaId(MaaJob job) => job._id;

    private readonly MaaId _id;
    private readonly IMaaPost _maa;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="maa"></param>
    public MaaJob(MaaId id, IMaaPost maa)
    {
        _id = id;
        _maa = maa;
    }

    /// <summary>
    ///     Gets the status of the <see cref="MaaJob"/>.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="IMaaPost.GetStatus"/>.
    /// </remarks>
    public MaaJobStatus Status => _maa.GetStatus(this);

    /// <summary>
    ///     Calls <see cref="IMaaPost.Wait"/>.
    /// </summary>
    /// <returns></returns>
    public MaaJobStatus Wait() => _maa.Wait(this);

    /// <summary>
    ///     Calls <see cref="IMaaPost.SetParam"/>.
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public bool SetParam(string param) => _maa.SetParam(this, param);
}
