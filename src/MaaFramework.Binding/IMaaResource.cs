using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaResource with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaResource<T> : IMaaResource, IMaaDisposableHandle<T>
{
}

/// <summary>
///     An interface defining wrapped members for MaaInstance.
/// </summary>
public interface IMaaResource : IMaaCommon, IMaaOption<ResourceOption>, IMaaPost, IMaaDisposable
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
    string? Hash { get; }

    /// <summary>
    ///     Gets the string of current task list.
    /// </summary>
    /// <value>
    ///     A string if the task list was got successfully; otherwise, null.
    /// </value>
    string? TaskList { get; }
}
