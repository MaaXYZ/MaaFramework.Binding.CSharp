using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Custom;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaResource with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaResource<out T> : IMaaResource, IMaaDisposableHandle<T>
{
}

/// <summary>
///     An interface defining wrapped members for MaaResource.
/// </summary>
public interface IMaaResource : IMaaCommon, IMaaOption<ResourceOption>, IMaaPost, IMaaDisposable
{
    /// <summary>
    ///     Registers a <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/> in the <see cref="IMaaResource"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</typeparam>
    /// <param name="name">The new name that will be used to reference it.</param>
    /// <param name="custom">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</param>
    /// <returns><see langword="true"/> if the custom action or recognition was registered successfully; otherwise, <see langword="false"/>.</returns>
    bool Register<T>(string name, T custom) where T : IMaaCustomResource;

    /// <inheritdoc cref="Register{T}(string, T)"/>
    bool Register<T>(T custom) where T : IMaaCustomResource;

    /// <summary>
    ///     Unregisters a <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/> in the <see cref="IMaaResource"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</typeparam>
    /// <param name="name">The name of <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/> when it was registered.</param>
    /// <returns><see langword="true"/> if the custom action or recognition was unregistered successfully; otherwise, <see langword="false"/>.</returns>
    bool Unregister<T>(string name) where T : IMaaCustomResource;

    /// <inheritdoc cref="Unregister{T}(string)"/>
    /// <param name="custom">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</param>
    bool Unregister<T>(T custom) where T : IMaaCustomResource;

    /// <summary>
    ///     Clears all <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/> registered in the <see cref="IMaaResource"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</typeparam>
    /// <returns><see langword="true"/> if custom actions or recognitions were cleared successfully; otherwise, <see langword="false"/>.</returns>
    bool Clear<T>() where T : IMaaCustomResource;

    /// <summary>
    ///     Clear the loaded resource paths.
    /// </summary>
    /// <returns><see langword="true"/> if the <see cref="IMaaResource"/> is cleared successfully; otherwise, <see langword="false"/>.</returns>
    bool Clear(bool includeCustomResource = false);

    /// <summary>
    ///     Appends a job for loading resource from <paramref name="path"/> , could be called multiple times.
    /// </summary>
    /// <param name="path">The resource path.</param>
    /// <returns>A load resource <see cref="MaaJob"/>.</returns>
    MaaJob AppendBundle(string path);

    /// <summary>
    ///     Gets whether the <see cref="IMaaResource"/> is fully loaded.
    /// </summary>
    /// <returns><see langword="true"/> if the <see cref="IMaaResource"/> is fully loaded; otherwise, <see langword="false"/>.</returns>
    bool Loaded { get; }

    /// <summary>
    ///     Gets the hash string of the <see cref="IMaaResource"/>.
    /// </summary>
    /// <returns>A <see cref="string"/> if the hash was got successfully; otherwise, <see langword="null"/>.</returns>
    string? Hash { get; }

    /// <summary>
    ///     Gets the string of current task list.
    /// </summary>
    /// <returns>A <see cref="string"/> if the task list was got successfully; otherwise, <see langword="null"/>.</returns>
    IList<string> NodeList { get; }
}
