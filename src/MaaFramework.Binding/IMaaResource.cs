using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;
using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaResource with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaResource<T> : IMaaResource, IMaaDisposableHandle<T>;

/// <summary>
///     An interface defining wrapped members for MaaResource.
/// </summary>
public interface IMaaResource : IMaaCommon, IMaaOption<ResourceOption>, IMaaDisposable
{
    /// <summary>
    ///     Gets the last valid posted job.
    /// </summary>
    /// <returns>A <see cref="MaaJob"/> if any valid job has been posted; otherwise, <see langword="null"/>..</returns>
    MaaJob? LastJob { get; }

    /// <summary>
    ///     Registers a <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/> in the <see cref="IMaaResource"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</typeparam>
    /// <param name="name">The new name that will be used to reference the custom resource.</param>
    /// <param name="custom">The custom resource instance to register.</param>
    /// <returns><see langword="true"/> if the registration was successful; otherwise, <see langword="false"/>.</returns>
    bool Register<T>(string name, T custom) where T : IMaaCustom;

    /// <inheritdoc cref="Register{T}(string, T)"/>
    bool Register<T>(string? name = null) where T : IMaaCustom, new();

    /// <inheritdoc cref="Register{T}(string, T)"/>
    bool Register<T>(T custom) where T : IMaaCustom;

    /// <summary>
    ///     Unregisters a <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/> in the <see cref="IMaaResource"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</typeparam>
    /// <param name="name">The name of the instance when it was registered.</param>
    /// <returns><see langword="true"/> if the unregistration was successful; otherwise, <see langword="false"/>.</returns>
    bool Unregister<T>(string name) where T : IMaaCustom;

    /// <inheritdoc cref="Unregister{T}(string)"/>
    /// <param name="custom">The custom resource instance to unregister.</param>
    bool Unregister<T>(T custom) where T : IMaaCustom;

    /// <summary>
    ///     Clears all <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/> registered in the <see cref="IMaaResource"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</typeparam>
    /// <returns><see langword="true"/> if custom resource instances were cleared successfully; otherwise, <see langword="false"/>.</returns>
    bool Clear<T>() where T : IMaaCustom;

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
    ///     Appends a job for loading OCR model from <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The OCR model path.</param>
    /// <returns>A load OCR model <see cref="MaaJob"/>.</returns>
    MaaJob AppendOcrModel(string path);

    /// <summary>
    ///     Appends a job for loading pipeline from <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The pipeline path.</param>
    /// <returns>A load pipeline <see cref="MaaJob"/>.</returns>
    MaaJob AppendPipeline(string path);

    /// <summary>
    ///     Appends a job for loading image from <paramref name="path"/>.
    /// </summary>
    /// <param name="path">The image path.</param>
    /// <returns>A load image <see cref="MaaJob"/>.</returns>
    MaaJob AppendImage(string path);

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
    ///     Override the image which name from the value of property field e.g. "template".
    /// </summary>
    /// <param name="imageName">The image name.</param>
    /// <param name="image">An <see cref="IMaaImageBuffer"/> used to set the image.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool OverrideImage(string imageName, IMaaImageBuffer image);

    /// <summary>
    ///     Gets the node data from the <see cref="IMaaResource"/> by node name.
    /// </summary>
    /// <param name="nodeName">The node name.</param>
    /// <param name="data">The node data.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool GetNodeData(string nodeName, [MaybeNullWhen(false)][StringSyntax("Json")] out string data);

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

    /// <summary>
    ///     Gets the list of registered custom recognitions.
    /// </summary>
    IList<string> CustomRecognitionList { get; }

    /// <summary>
    ///     Gets the list of registered custom actions.
    /// </summary>
    IList<string> CustomActionList { get; }

    /// <summary>
    ///     Gets the default recognition parameters for the specified type.
    /// </summary>
    /// <param name="type">The recognition type.<para>(e.g., "OCR", "TemplateMatch")</para></param>
    /// <param name="param">The default recognition parameters.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool GetDefaultRecognitionParam(string type, [MaybeNullWhen(false)][StringSyntax("Json")] out string param);

    /// <summary>
    ///     Gets the default action parameters for the specified type.
    /// </summary>
    /// <param name="type">The action type.<para>(e.g., "Click", "Swipe")</para></param>
    /// <param name="param">The default action parameters.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool GetDefaultActionParam(string type, [MaybeNullWhen(false)][StringSyntax("Json")] out string param);
}
