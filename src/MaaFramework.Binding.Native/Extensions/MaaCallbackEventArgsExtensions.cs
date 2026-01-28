using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaContext;
using static MaaFramework.Binding.Notification.MaaMsg;

namespace MaaFramework.Binding;

/// <summary>
///     A static class providing extension methods for <see cref="MaaCallbackEventArgs"/>.
/// </summary>
public static class MaaCallbackEventArgsExtensions
{
    /// <summary>
    ///     Gets a <paramref name="tasker"/> with the specified <paramref name="sender"/> and <see cref="MaaCallbackEventArgs"/>.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs"/> from <see cref="MaaFramework"/> callback.</param>
    /// <param name="sender">The sender from <see cref="Binding"/>.</param>
    /// <param name="tasker">The tasker, if the original one is discovered, or a new one can be constructed; otherwise, <see langword="default"/>.</param>
    /// <returns><see langword="true"/> if the original <paramref name="tasker"/> is discovered, or a new <paramref name="tasker"/> can be constructed; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    public static bool TryGetTasker(this MaaCallbackEventArgs e, object? sender, [MaybeNullWhen(false)] out MaaTasker tasker)
    {
        tasker = sender as MaaTasker;
        if (tasker is not null) return true;
        ArgumentNullException.ThrowIfNull(e);

        if (e.HandleType is MaaHandleType.Tasker)
        {
            if (!MaaTasker.Instances.TryGetValue(e.Handle, out tasker))
                tasker = new MaaTasker(e.Handle);
            return true;
        }

        if (e.HandleType is MaaHandleType.Context)
        {
            var handle = MaaContextGetTasker(e.Handle);
            if (!MaaTasker.Instances.TryGetValue(handle, out tasker))
                tasker = new MaaTasker(handle);
            return true;
        }

        tasker = default;
        return false;
    }

    /// <summary>
    ///     Gets a <paramref name="resource"/> with the specified <paramref name="sender"/> and <see cref="MaaCallbackEventArgs"/>.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs"/> from <see cref="MaaFramework"/> callback.</param>
    /// <param name="sender">The sender from <see cref="Binding"/>.</param>
    /// <param name="resource">The resource, if the original one is discovered, or a new one can be constructed; otherwise, <see langword="default"/>.</param>
    /// <returns><see langword="true"/> if the original <paramref name="resource"/> is discovered, or a new <paramref name="resource"/> can be constructed; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    public static bool TryGetResource(this MaaCallbackEventArgs e, object? sender, [MaybeNullWhen(false)] out MaaResource resource)
    {
        resource = sender as MaaResource;
        if (resource is not null) return true;
        ArgumentNullException.ThrowIfNull(e);

        if (e.HandleType is MaaHandleType.Resource)
        {
            resource = new MaaResource(e.Handle);
            return true;
        }

        resource = default;
        return false;
    }

    /// <summary>
    ///     Gets a <paramref name="controller"/> with the specified <paramref name="sender"/> and <see cref="MaaCallbackEventArgs"/>.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs"/> from <see cref="MaaFramework"/> callback.</param>
    /// <param name="sender">The sender from <see cref="Binding"/>.</param>
    /// <param name="controller">The controller, if the original one is discovered, or a new one can be constructed; otherwise, <see langword="default"/>.</param>
    /// <returns><see langword="true"/> if the original <paramref name="controller"/> is discovered, or a new <paramref name="controller"/> can be constructed; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetController(this MaaCallbackEventArgs e, object? sender, [MaybeNullWhen(false)] out MaaController controller)
    {
        controller = sender as MaaController;
        if (controller is not null) return true;
        ArgumentNullException.ThrowIfNull(e);

        if (e.HandleType is MaaHandleType.Controller)
        {
            controller = new MaaController(e.Handle);
            return true;
        }

        controller = default;
        return false;
    }

    /// <summary>
    ///     Gets a <paramref name="context"/> with the specified <paramref name="sender"/> and <see cref="MaaCallbackEventArgs"/>.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs"/> from <see cref="MaaFramework"/> callback.</param>
    /// <param name="sender">The sender from <see cref="Binding"/>.</param>
    /// <param name="context">The context, if the original one is discovered, or a new one can be constructed; otherwise, <see langword="default"/>.</param>
    /// <returns><see langword="true"/> if the original <paramref name="context"/> is discovered, or a new <paramref name="context"/> can be constructed; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetContext(this MaaCallbackEventArgs e, object? sender, [MaybeNullWhen(false)] out MaaContext context)
    {
        ArgumentNullException.ThrowIfNull(e);

        if (e.HandleType is MaaHandleType.Context)
        {
            context = sender switch
            {
                MaaTasker tasker => new MaaContext(e.Handle, tasker),
                MaaContext c => c,
                _ => new MaaContext(e.Handle),
            };
            return true;
        }

        context = default;
        return false;
    }
}
