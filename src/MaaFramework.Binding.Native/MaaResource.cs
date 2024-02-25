using MaaFramework.Binding.Abstractions.Native;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaResource;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaResource"/>.
/// </summary>
public class MaaResource : MaaCommon, IMaaResource<nint>
{
    /// <summary>
    ///     Creates a <see cref="MaaResource"/> instance.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceCreate"/>.
    /// </remarks>
    public MaaResource()
    {
        var handle = MaaResourceCreate(MaaApiCallback, nint.Zero);
        SetHandle(handle, needReleased: true);
    }

    /// <inheritdoc cref="MaaResource(CheckStatusOption, string[])"/>
    public MaaResource(params string[] paths)
        : this(CheckStatusOption.ThrowIfNotSuccess, paths)
    {
    }

    /// <param name="check">Checks AppendPath(path).Wait() status if true; otherwise, not checks.</param>
    /// <param name="paths">The paths of maa resource.</param>
    /// <exception cref="MaaJobStatusException" />
    /// <inheritdoc cref="MaaResource()"/>
    public MaaResource(CheckStatusOption check, params string[] paths)
        : this()
    {
        ArgumentNullException.ThrowIfNull(paths);

        foreach (var path in paths)
        {
            var status = AppendPath(path).Wait();
            if (check == CheckStatusOption.ThrowIfNotSuccess)
            {
                status.ThrowIfNot(MaaJobStatus.Success, MaaJobStatusException.MaaResourceMessage);
            }
        }
    }

    /// <inheritdoc cref="MaaResource(CheckStatusOption, string[])"/>
    public MaaResource(IEnumerable<string> paths)
        : this(CheckStatusOption.ThrowIfNotSuccess, paths)
    {
    }

    /// <inheritdoc cref="MaaResource(CheckStatusOption, string[])"/>
    public MaaResource(CheckStatusOption check, IEnumerable<string> paths)
        : this()
    {
        ArgumentNullException.ThrowIfNull(paths);

        foreach (var path in paths)
        {
            var status = AppendPath(path).Wait();
            if (check == CheckStatusOption.ThrowIfNotSuccess)
            {
                status.ThrowIfNot(MaaJobStatus.Success, MaaJobStatusException.MaaResourceMessage);
            }
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaResourceDestroy(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourcePostPath"/>.
    /// </remarks>
    public IMaaJob AppendPath(string resourcePath)
    {
        var id = MaaResourcePostPath(Handle, resourcePath);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    bool Abstractions.IMaaPost.SetParam(IMaaJob job, string param)
        => throw new InvalidOperationException();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(IMaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return (MaaJobStatus)MaaResourceStatus(Handle, job.Id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceWait"/>.
    /// </remarks>
    public MaaJobStatus Wait(IMaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return (MaaJobStatus)MaaResourceWait(Handle, job.Id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceLoaded"/>.
    /// </remarks>
    public bool Loaded => MaaResourceLoaded(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceSetOption"/>.
    /// </remarks>
    public bool SetOption<T>(ResourceOption opt, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        MaaOptionValue[] bytes = (value, opt) switch
        {
            // (int vvvv, ResourceOption.Invalid) => vvvv.ToMaaOptionValues(),
            _ => throw new InvalidOperationException(),
        };

        return MaaResourceSetOption(Handle, (MaaResOption)opt, ref bytes[0], (MaaOptionValueSize)bytes.Length).ToBoolean();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceGetHash"/>.
    /// </remarks>
    public string? Hash
    {
        get
        {
            using var buffer = new MaaStringBuffer();
            var ret = MaaResourceGetHash(Handle, buffer.Handle).ToBoolean();
            return ret ? buffer.ToString() : null;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceGetTaskList"/>.
    /// </remarks>
    public string? TaskList
    {
        get
        {
            using var buffer = new MaaStringBuffer();
            var ret = MaaResourceGetTaskList(Handle, buffer.Handle).ToBoolean();
            return ret ? buffer.ToString() : null;
        }
    }
}
