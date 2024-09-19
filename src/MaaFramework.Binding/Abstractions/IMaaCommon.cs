namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An interface defining common members for <see cref="IMaaController"/>, <see cref="IMaaTasker"/> and <see cref="IMaaResource"/>.
/// </summary>
public interface IMaaCommon
{
    /// <summary>
    ///     Occurs when MaaFramework calls back.
    /// </summary>
    event EventHandler<MaaCallbackEventArgs>? Callback;
}
