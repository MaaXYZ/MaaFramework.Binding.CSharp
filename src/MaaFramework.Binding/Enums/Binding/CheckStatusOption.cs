namespace MaaFramework.Binding;

/// <summary>
///     Checking <see cref="MaaJobStatus"/> option.
/// </summary>
public enum CheckStatusOption
{
    /// <summary>
    ///     Not Check.
    /// </summary>
    None,

    /// <summary>
    ///     Throw a <see cref="MaaJobStatusException"/> if the Status is not <see cref="MaaJobStatus.Succeeded"/> .
    /// </summary>
    ThrowIfNotSucceeded,
}
