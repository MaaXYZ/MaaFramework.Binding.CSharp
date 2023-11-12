using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaInstance.
/// </summary>
public interface IMaaResource : IMaaCommon, IMaaDisposableHandle, IMaaOption<ResourceOption>, IMaaPost
{
    /// <summary>
    ///     Appends a async job of loading resource from <paramref name="resourcePath"/> , could be called multiple times.
    /// </summary>
    /// <param name="resourcePath">The resource path.</param>
    /// <returns>A load resource job.</returns>
    IMaaJob AppendPath(string resourcePath);

    /// <summary>
    ///     Gets whether the <see cref="IMaaResource"/> is fully loaded.
    /// </summary>
    /// <value>
    ///     true if the <see cref="IMaaResource"/> is fully loaded; otherwise, false.
    /// </value>
    bool Loaded { get; }

    /// <summary>
    ///     Gets the hash string of the <see cref="IMaaResource"/>.
    /// </summary>
    /// <value>
    ///     A string if the hash was got successfully; otherwise, null.
    /// </value>
    public string? Hash { get; }
}
