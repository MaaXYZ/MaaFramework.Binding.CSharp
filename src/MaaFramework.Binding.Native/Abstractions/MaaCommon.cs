using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Native.Interop;

namespace MaaFramework.Binding.Native.Abstractions;

/// <summary>
///     An abstract class providing common members for <see cref="MaaController"/>, <see cref="MaaInstance"/> and <see cref="MaaResource"/>.
/// </summary>
public abstract class MaaCommon<TEnum> : MaaDisposableHandle<nint>, IMaaCommon, IMaaOption<TEnum> where TEnum : Enum
{
    /// <inheritdoc/>
    public bool SetOption(TEnum option, int value)
        => SetOption(option, value.ToMaaOptionValues());

    /// <inheritdoc/>
    public bool SetOption(TEnum option, bool value)
        => SetOption(option, value.ToMaaOptionValues());

    /// <inheritdoc/>
    public bool SetOption(TEnum option, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        return SetOption(option, value.ToMaaOptionValues());
    }

    /// <inheritdoc cref="SetOption(TEnum, int)"/>
    protected abstract bool SetOption(TEnum option, MaaOptionValue[] value);

    /// <inheritdoc/>
    public event EventHandler<MaaCallbackEventArgs>? Callback;

    /// <summary>
    ///     Raises the Callback event.
    /// </summary>
    /// <param name="msg">The MaaStringView.</param>
    /// <param name="detail">The MaaStringView.</param>
    /// <param name="arg">The MaaCallbackTransparentArg.</param>
    /// <remarks>
    ///     Usually invoked by MaaFramework.
    /// </remarks>
    protected virtual void OnCallback(MaaStringView msg, MaaStringView detail, MaaCallbackTransparentArg arg)
    {
        Callback?.Invoke(this, new MaaCallbackEventArgs(
            msg.ToStringUTF8() ?? string.Empty,
            detail.ToStringUTF8() ?? "{}",
            arg));
    }

    /// <summary>
    ///     A delegate to avoid garbage collection before MaaFramework calls <see cref="OnCallback"/>.
    /// </summary>
    protected readonly MaaApiCallback maaApiCallback;

    /// <summary>
    ///     Initialize <see cref="maaApiCallback"/>.
    /// </summary>
    protected MaaCommon()
        : base(invalidHandleValue: nint.Zero)
    {
        maaApiCallback = OnCallback;
    }
}
