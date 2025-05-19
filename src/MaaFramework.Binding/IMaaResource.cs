using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Custom;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaResource with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaResource<T> : IMaaResource, IMaaDisposableHandle<T>;

/// <summary>
///     An interface defining wrapped members for MaaResource.
/// </summary>
public interface IMaaResource : IMaaCommon, IMaaOption<ResourceOption>, IMaaPost, IMaaDisposable
{
    /// <summary>
    ///     Registers a <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/> in the <see cref="IMaaResource"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</typeparam>
    /// <param name="name">The new name that will be used to reference the custom resource.</param>
    /// <param name="custom">The custom resource instance to register.</param>
    /// <returns><see langword="true"/> if the registration was successful; otherwise, <see langword="false"/>.</returns>
    bool Register<T>(string name, T custom) where T : IMaaCustomResource;

    /// <inheritdoc cref="Register{T}(string, T)"/>
    bool Register<T>(T custom) where T : IMaaCustomResource;

    /// <summary>
    ///     Unregisters a <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/> in the <see cref="IMaaResource"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</typeparam>
    /// <param name="name">The name of the instance when it was registered.</param>
    /// <returns><see langword="true"/> if the unregistration was successful; otherwise, <see langword="false"/>.</returns>
    bool Unregister<T>(string name) where T : IMaaCustomResource;

    /// <inheritdoc cref="Unregister{T}(string)"/>
    /// <param name="custom">The custom resource instance to unregister.</param>
    bool Unregister<T>(T custom) where T : IMaaCustomResource;

    /// <summary>
    ///     Clears all <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/> registered in the <see cref="IMaaResource"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</typeparam>
    /// <returns><see langword="true"/> if custom resource instances were cleared successfully; otherwise, <see langword="false"/>.</returns>
    bool Clear<T>() where T : IMaaCustomResource;

    /// <summary>
    ///     Clears the loaded resource.
    /// </summary>
    /// <param name="includeCustomResource">Whether to include custom resources.</param>    
    /// <returns><see langword="true"/> if the <see cref="IMaaResource"/> is cleared successfully; otherwise, <see langword="false"/>.</returns>
    bool Clear(bool includeCustomResource = false);

    /// <summary>
    ///     Appends a job for loading bundle from <paramref name="path"/> , could be called multiple times.
    /// </summary>
    /// <param name="path">The bundle path.</param>
    /// <returns>A load bundle <see cref="MaaJob"/>.</returns>
    MaaJob AppendBundle(string path);

    /// <summary>
    ///     Override a pipeline.
    /// </summary>
    /// <param name="pipelineOverride">The json used to override the pipeline.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool OverridePipeline(string pipelineOverride);

    /// <summary>
    ///     Override the property field "next" in a node.
    /// </summary>
    /// <param name="nodeName">The node name.</param>
    /// <param name="nextList">The next list.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool OverrideNext(string nodeName, IEnumerable<string> nextList);

    /// <summary>
    ///     Gets whether the <see cref="IMaaResource"/> is fully loaded.
    /// </summary>
    /// <returns><see langword="true"/> if the <see cref="IMaaResource"/> is fully loaded; otherwise, <see langword="false"/>.</returns>
    bool IsLoaded { get; }

    /// <summary>
    ///     Gets the hash string of the <see cref="IMaaResource"/>.
    /// </summary>
    /// <returns>A <see cref="string"/> if the hash was got successfully; otherwise, <see langword="null"/>.</returns>
    string? Hash { get; }

    /// <summary>
    ///     Gets the string of current node list.
    /// </summary>
    /// <returns>A <see cref="string"/> if the node list was got successfully; otherwise, <see langword="null"/>.</returns>
    IList<string> NodeList { get; }
}
