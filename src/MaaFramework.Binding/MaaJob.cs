﻿using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding;

#pragma warning disable S1121

/// <summary>
///     A wrapper class providing a reference implementation for return value of Maa Post method.
/// </summary>
/// <param name="id">The MaaId.</param>
/// <param name="maa">The <see cref="IMaaPost"/>.</param>
public class MaaJob(MaaId id, IMaaPost maa)
{
    /// <inheritdoc/>
    public override string ToString() => $"{GetType().Name} {{ {nameof(Status)} = {Status} }}";

    private MaaJobStatus _finalStatus = MaaJobStatus.Invalid;

    /// <summary>
    ///     Gets a MaaId.
    /// </summary>
    public MaaId Id => id;

    /// <summary>
    ///     Gets the status of a <see cref="MaaJob"/>.
    /// </summary>
    /// <remarks>
    ///     Calls <see cref="IMaaPost.GetStatus"/>.
    /// </remarks>
    public MaaJobStatus Status => _finalStatus.IsDone()
        ? _finalStatus
        : _finalStatus = maa.GetStatus(this);

    /// <summary>
    ///     Waits for a <see cref="MaaJob"/> to complete.
    /// </summary>
    /// <returns>The job status.</returns>
    /// <remarks>
    ///     Calls <see cref="IMaaPost.Wait"/>.
    /// </remarks>
    public MaaJobStatus Wait() => _finalStatus = maa.Wait(this);
}
