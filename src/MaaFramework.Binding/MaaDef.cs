// Some necessary maa definitions for abstractions in MaaFramework.Binding.
global using MaaId = System.Int64;

namespace MaaFramework.Binding;

/// <summary>
///     A class providing data for the <see cref="MaaFramework.Binding.Abstractions.IMaaCommon.Callback"/> event.
/// </summary>
public class MaaCallbackEventArgs : EventArgs
{
    /// <summary>
    ///     Maa callback message.
    /// </summary>
    public string Message { get; }


    /// <summary>
    ///     Maa callback details json.
    /// </summary>
    public string Details { get; }

    /// <summary>
    ///      Creates a <see cref="MaaCallbackEventArgs"/> instance.
    /// </summary>
    /// <param name="message">The callback message.</param>
    /// <param name="details">The callback details json.</param>
    public MaaCallbackEventArgs(string message, string details)
    {
        Message = message;
        Details = details;
    }
}
