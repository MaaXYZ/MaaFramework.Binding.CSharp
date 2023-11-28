using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaInstance with generic handle.
/// </summary>
public interface IMaaInstance<T> : IMaaInstance, IMaaDisposableHandle<T>
{
    /// <summary>
    ///     Gets the resource or inits to bind a <see cref="IMaaResource"/>.
    /// </summary>
    /// <exception cref="MaaBindException"/>
    new IMaaResource<T> Resource { get; init; }

    /// <summary>
    ///     Gets the controller or inits to bind a <see cref="IMaaController"/>.
    /// </summary>
    /// <exception cref="MaaBindException"/>
    new IMaaController<T> Controller { get; init; }
}

/// <summary>
///     An interface defining wrapped members for MaaInstance.
/// </summary>
public interface IMaaInstance : IMaaCommon, IMaaOption<InstanceOption>, IMaaPost, IDisposable
{
    /// <summary>
    ///     Gets the resource.
    /// </summary>
    /// <exception cref="MaaBindException"/>
    IMaaResource Resource { get; }

    /// <summary>
    ///     Gets the controller.
    /// </summary>
    /// <exception cref="MaaBindException"/>
    IMaaController Controller { get; }

    /// <summary>
    ///     Gets whether the <see cref="IMaaInstance"/> is fully initialized.
    /// </summary>
    /// <value>
    ///     true if the <see cref="IMaaInstance"/> was fully initialized; otherwise, false.
    /// </value>
    bool Initialized { get; }

    /// <summary>
    ///     Registers a custom recognizer or action named <paramref name="name"/> in the <see cref="IMaaInstance"/>.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="custom">The MaaCustomRecognizerApi or MaaCustomActionApi.</param>
    /// <param name="arg">The MaaTransparentArg.</param>
    /// <returns>
    ///     true if the custom recognizer or action was registered successfully; otherwise, false.
    /// </returns>
    bool Register<T>(string name, T custom, nint arg) where T : IMaaDefStruct;

    /// <summary>
    ///     Unregisters a custom recognizer or action named <paramref name="name"/> in the <see cref="IMaaInstance"/>.
    /// </summary>
    /// <param name="name">The name of recognizer.</param>
    /// <returns>
    ///     true if the custom recognizer or action was unregistered successfully; otherwise, false.
    /// </returns>
    bool Unregister<T>(string name) where T : IMaaDefStruct;

    /// <summary>
    ///     Clears custom recognizers or actions in the <see cref="IMaaInstance"/>.
    /// </summary>
    /// <returns>
    ///     true if custom recognizers or actions were cleared successfully; otherwise, false.
    /// </returns>
    bool Clear<T>() where T : IMaaDefStruct;

    /// <summary>
    ///     Appends a async job of executing a maa task, could be called multiple times.
    /// </summary>
    /// <param name="taskEntryName">The name of task entry.</param>
    /// <param name="taskParam">The param of task, which could be parsed to a JSON.</param>
    /// <returns>A task job.</returns>
    IMaaJob AppendTask(string taskEntryName, string taskParam = "{}");

    /// <summary>
    ///     Gets whether the all maa tasks finished.
    /// </summary>
    /// <value>
    ///     true if all tasks finished; otherwise, false.
    /// </value>
    bool AllTasksFinished { get; }

    /// <summary>
    ///     Stops the binded <see cref="IMaaResource"/>, the binded <see cref="IMaaController"/>, all appended tasks. 
    /// </summary>
    bool Stop();
}
