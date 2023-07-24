using static MaaToolKit.Interop.MaaApiWrapper;

namespace MaaToolKit.Extensions.Interfaces;

/// <summary>
///     A interface providing a public event for <see cref="MaaCallback"/> and a internal invoke method.
/// </summary>
public interface IMaaNotify
{
    /// <summary>
    ///     Occurs when maa call back.
    /// </summary>
    event MaaCallback? Notify;
}
