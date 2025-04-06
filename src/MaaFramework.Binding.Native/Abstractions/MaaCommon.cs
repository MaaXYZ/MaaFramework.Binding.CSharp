using MaaFramework.Binding.Interop.Native;
using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding.Abstractions.Native;

/// <summary>
///     An abstract class providing common members for <see cref="MaaController"/>, <see cref="MaaTasker"/> and <see cref="MaaResource"/>.
/// </summary>
public abstract class MaaCommon : MaaDisposableHandle<nint>, IMaaCommon
{
    /// <inheritdoc/>
    public event EventHandler<MaaCallbackEventArgs>? Callback;

    /// <summary>
    ///     Raises the Callback event.
    /// </summary>
    /// <param name="message">The MaaStringView.</param>
    /// <param name="detailsJson">The MaaStringView.</param>
    /// <param name="callbackArg">The MaaCallbackTransparentArg.</param>
    /// <remarks>
    ///     Usually invoked by MaaFramework.
    /// </remarks>
    protected virtual void OnCallback(string message, [StringSyntax("Json")] string detailsJson, nint callbackArg)
        => Callback?.Invoke(this, new MaaCallbackEventArgs(message, detailsJson));

    /// <summary>
    ///     Gets the delegate to avoid garbage collection before MaaFramework calls <see cref="OnCallback"/>.
    /// </summary>
    protected MaaNotificationCallback MaaNotificationCallback { get; }

    /// <summary>
    ///     Initializes <see cref="MaaNotificationCallback"/>.
    /// </summary>
    protected MaaCommon()
        : base(invalidHandleValue: nint.Zero)
    {
        MaaNotificationCallback = OnCallback;
    }
}
