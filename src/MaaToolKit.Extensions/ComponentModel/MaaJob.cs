using MaaToolKit.Extensions.Interfaces;

namespace MaaToolKit.Extensions.ComponentModel;

/// <summary>
/// A class providing a reference implementation for return value of Maa Post method.
/// </summary>
public class MaaJob
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="job"></param>
    public static implicit operator long(MaaJob job) => job._id;

    private readonly long _id;
    private readonly IMaaPost _maaPost;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="maaPost"></param>
    public MaaJob(long id, IMaaPost maaPost)
    {
        _id = id;
        _maaPost = maaPost;
    }

    /// <summary>
    ///     Gets the status of the <see cref="MaaJob"/>.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="IMaaPost.GetStatus"/>.
    /// </remarks>
    public MaaJobStatus Status => _maaPost.GetStatus(this);

    /// <summary>
    ///     Calls <see cref="IMaaPost.Wait"/>.
    /// </summary>
    /// <returns></returns>
    public MaaJobStatus Wait() => _maaPost.Wait(this);

    /// <summary>
    ///     Calls <see cref="IMaaPost.SetParam"/>.
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public bool SetParam(string param) => _maaPost.SetParam(this, param);
}
