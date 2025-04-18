﻿// Some necessary maa definitions for abstractions in MaaFramework.Binding.
global using MaaId = System.Int64;
global using MaaTaskId = System.Int64;
global using MaaNodeId = System.Int64;
global using MaaRecoId = System.Int64;
global using MaaSize = System.UInt64;

using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding;

/// <summary>
///     A class providing data for the <see cref="MaaFramework.Binding.Abstractions.IMaaCommon.Callback"/> event.
/// </summary>
/// <remarks>
///      Creates a <see cref="MaaCallbackEventArgs"/> instance.
/// </remarks>
/// <param name="message">The callback message.</param>
/// <param name="details">The callback details json.</param>
public class MaaCallbackEventArgs(string message, [StringSyntax("Json")] string details) : EventArgs
{
    /// <summary>
    ///     Maa callback message.
    /// </summary>
    public string Message { get; } = message;

    /// <summary>
    ///     Maa callback details json.
    /// </summary>
    public string Details { get; } = details;
}
