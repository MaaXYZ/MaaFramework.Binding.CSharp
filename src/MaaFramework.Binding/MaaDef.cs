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
/// <typeparam name="T">The handle type.</typeparam>
/// <param name="handle">
///     <para> - MaaTasker* for MaaTasker event.</para>
///     <para> - MaaResource* for MaaResource event.</para>
///     <para> - MaaController* for MaaController event.</para>
///     <para> - MaaContext* for MaaContext event.</para>
/// </param>
/// <param name="message">The callback message.</param>
/// <param name="details">The callback details json.</param>
/// <param name="handleType">The callback handle type.</param>
public class MaaCallbackEventArgs<T>(T handle, string message, [StringSyntax("Json")] string details, MaaHandleType handleType)
    : MaaCallbackEventArgs(message, details, handleType)
{
    /// <summary>
    ///     The MaaEventCallback sender handle.
    /// </summary>
    public T Handle { get; } = handle;
}

/// <inheritdoc cref="MaaCallbackEventArgs{T}"/>
public class MaaCallbackEventArgs(string message, [StringSyntax("Json")] string details, MaaHandleType handleType) : EventArgs
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
    ///     The MaaEventCallback sender handle type.
    /// </summary>
    public MaaHandleType HandleType { get; } = handleType;
}
