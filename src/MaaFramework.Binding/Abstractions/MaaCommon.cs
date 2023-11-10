using MaaFramework.Binding.Enums;
using MaaFramework.Binding.Interop.Framework;
using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     A abstract class providing common members for <see cref="MaaController"/>, <see cref="MaaInstance"/> and <see cref="MaaResource"/>.
/// </summary>
public abstract class MaaCommon<T> : MaaOption<T>, IMaaPost, IDisposable where T : Enum
{
    /// <inheritdoc/>
    ~MaaCommon()
    {
        Dispose(false);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected abstract void Dispose(bool disposing);

    /// <inheritdoc/>
    public abstract bool SetParam(MaaJob job, string param);

    /// <inheritdoc/>
    public abstract MaaJobStatus GetStatus(MaaJob job);

    /// <inheritdoc/>
    public abstract MaaJobStatus Wait(MaaJob job);

    /// <summary>
    ///     Maa Callback delegate.
    /// </summary>
    public delegate void MaaCallback(string msg, string detailsJson, MaaCallbackTransparentArg identifier);

    /// <summary>
    ///     Occurs when MaaFramework calls back.
    /// </summary>
    public event MaaCallback? Callback;

    /// <summary>
    ///     Raises the Startup event.
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="detail"></param>
    /// <param name="arg"></param>
    /// <remarks>
    ///     Usually invoked by MaaFramework.
    /// </remarks>
    protected virtual void OnCallback(MaaStringView msg, MaaStringView detail, MaaCallbackTransparentArg arg)
    {
        Callback?.Invoke(
            Marshal.PtrToStringUTF8(msg) ?? string.Empty,
            Marshal.PtrToStringUTF8(detail) ?? "{}",
            arg);
    }

    /// <summary>
    ///     A delegate to avoid garbage collection before MaaFramework calls <see cref="OnCallback"/>.
    /// </summary>
    internal readonly MaaApiCallback MaaApiCallback;

    /// <summary>
    ///     Initialize <see cref="MaaApiCallback"/>
    /// </summary>
    protected MaaCommon()
    {
        MaaApiCallback = OnCallback;
    }
}
