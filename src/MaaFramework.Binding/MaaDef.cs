// Some necessary maa definitions for abstractions in MaaFramework.Binding.
global using MaaId = System.Int64;

namespace MaaFramework.Binding;

/// <summary>
///     A class provides data for the <see cref="MaaFramework.Binding.Abstractions.IMaaCommon.Callback"/> event.
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
    ///     MaaCallbackTransparentArg.
    /// </summary>
    public nint Arg { get; }

    /// <summary>
    ///      Creates a <see cref="MaaCallbackEventArgs"/> instance.
    /// </summary>
    /// <param name="message">The callback message.</param>
    /// <param name="details">The callback details json.</param>
    /// <param name="arg">The MaaCallbackTransparentArg.</param>
    public MaaCallbackEventArgs(string message, string details, nint arg)
    {
        Message = message;
        Details = details;
        Arg = arg;
    }
}
