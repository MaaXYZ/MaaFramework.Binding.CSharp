using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaUtility.
/// </summary>
public interface IMaaUtility : IMaaOption<GlobalOption>
{
    /// <summary>
    ///     Gets version of MaaFramework.
    /// </summary>
    string Version { get; }
}
