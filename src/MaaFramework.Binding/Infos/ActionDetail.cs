using MaaFramework.Binding.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding;

/// <summary>
///     A sealed record providing properties of action detail.
/// </summary>
/// <param name="Id">Gets the action id.</param>
/// <param name="NodeName">Gets the node name.</param>
/// <param name="Action">Gets the action.</param>
/// <param name="Box">Gets the hit box.</param>
/// <param name="IsSucceeded">Gets a value indicating whether the action is succeeded.</param>
/// <param name="Detail">Gets the action detail.</param>
public sealed record ActionDetail(
    MaaActId Id,
    string NodeName,
    string Action,
    IMaaRectBuffer Box,
    bool IsSucceeded,
    [StringSyntax("Json")] string Detail
) : IDisposable
{
    private bool _disposed;

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;

        Box.Dispose();
    }

    /// <summary>
    ///     Queries the action detail.
    /// </summary>
    /// <param name="actionId">The action id.</param>
    /// <param name="tasker">The maa tasker.</param>
    /// <returns>A <see cref="ActionDetail"/> if query was successful; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    public static ActionDetail? Query<TRect>(MaaActId actionId, IMaaTasker tasker)
        where TRect : IMaaRectBuffer, new()
    {
        ArgumentNullException.ThrowIfNull(tasker);

        var box = new TRect();
        if (!tasker.GetActionDetail(actionId, out var nodeName, out var action, box, out var isSucceeded, out var detailJson))
        {
            box.Dispose();
            return null;
        }

        return new ActionDetail(
                Id: actionId,
                NodeName: nodeName,
                Action: action,
                Box: box,
                IsSucceeded: isSucceeded,
                Detail: detailJson);
    }
}
