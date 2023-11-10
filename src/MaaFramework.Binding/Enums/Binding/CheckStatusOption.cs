namespace MaaFramework.Binding.Enums;

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
    ///     Throw a <see cref="Exceptions.MaaJobStatusException"/> if the Status is not <see cref="MaaJobStatus.Success"/> .
    /// </summary>
    ThrowIfNotSuccess,
}
