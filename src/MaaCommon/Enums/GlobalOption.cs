using System.Diagnostics.CodeAnalysis;

namespace MaaCommon.Enums;

/// <summary>
///     Global options
/// </summary>
[SuppressMessage("Design", "CA1008:Enums should have zero value")]
public enum GlobalOption
{
    /// <summary>
    ///     Whether to enable logging
    /// </summary>
    Logging = 1
}
