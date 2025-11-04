using MaaFramework.Binding.Interop.Native;
using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding.Abstractions;

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
    /// <param name="handle">
    ///     <para> - MaaTasker* for MaaTasker event.</para>
    ///     <para> - MaaResource* for MaaResource event.</para>
    ///     <para> - MaaController* for MaaController event.</para>
    ///     <para> - MaaContext* for MaaContext event.</para>
    /// </param>
    /// <param name="message">The MaaStringView.</param>
    /// <param name="detailsJson">The MaaStringView.</param>
    /// <param name="transArg">The MaaCallbackTransparentArg.</param>
    /// <remarks>
    ///     Usually invoked by MaaFramework.
    /// </remarks>
    protected virtual void OnCallback(nint handle, string message, [StringSyntax("Json")] string detailsJson, nint transArg)
        => Callback?.Invoke(this, new MaaCallbackEventArgs(message, detailsJson));

    /// <summary>
    ///     Gets the delegate to avoid garbage collection before MaaFramework calls <see cref="OnCallback"/>.
    /// </summary>
    protected MaaEventCallback MaaEventCallback { get; }

    /// <summary>
    ///     Initializes <see cref="MaaEventCallback"/>.
    /// </summary>
    protected MaaCommon()
        : base(invalidHandleValue: nint.Zero)
    {
        MaaEventCallback = OnCallback;
    }
}
