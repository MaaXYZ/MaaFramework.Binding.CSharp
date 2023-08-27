using static MaaToolKit.Extensions.Interop.MaaDef;

namespace MaaToolKit.Extensions.Interfaces;

/// <summary>
///     A interface providing a public event for <see cref="MaaApiCallback"/> and a internal invoke method.
/// </summary>
public interface IMaaNotify
{
    /// <summary>
    ///     Occurs when maa call back.
    /// </summary>
    event MaaCallback? Callback;

    /// <summary>
    ///     MAA _callback delegate
    /// </summary>
    delegate void MaaCallback(string msg, string detailsJson, IntPtr identifier);
}
