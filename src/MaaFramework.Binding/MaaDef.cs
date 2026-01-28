// Some necessary maa definitions for abstractions in MaaFramework.Binding.
global using MaaId = System.Int64;
global using MaaTaskId = System.Int64;
global using MaaNodeId = System.Int64;
global using MaaRecoId = System.Int64;
global using MaaActId = System.Int64;
global using MaaSize = System.UInt64;

using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding;

/// <summary>
///     A class providing data for the <see cref="MaaFramework.Binding.Abstractions.IMaaCommon.Callback"/> event.
/// </summary>
/// <remarks>
///      Creates a <see cref="MaaCallbackEventArgs"/> instance.
/// </remarks>
/// <param name="handle">
///     <para> - MaaTasker* for MaaTasker event.</para>
///     <para> - MaaResource* for MaaResource event.</para>
///     <para> - MaaController* for MaaController event.</para>
///     <para> - MaaContext* for MaaContext event.</para>
/// </param>
/// <param name="message">The callback message.</param>
/// <param name="details">The callback details json.</param>
/// <param name="transArg">The MaaCallbackTransparentArg which value is <see cref="MaaHandleType"/> in <see cref="Binding"/>.</param>
public class MaaCallbackEventArgs(nint handle, string message, [StringSyntax("Json")] string details, nint transArg) : EventArgs
{
    /// <summary>
    ///     Maa callback message.
    /// </summary>
    public string Message { get; } = message;

    /// <summary>
    ///     Maa callback details json.
    /// </summary>
    public string Details { get; } = details;

    /// <summary>
    ///     The MaaEventCallback sender handle.
    /// </summary>
    public nint Handle { get; } = handle;

    /// <summary>
    ///     The MaaEventCallback sender handle type.
    /// </summary>
    public MaaHandleType HandleType { get; } = (MaaHandleType)transArg;
}
