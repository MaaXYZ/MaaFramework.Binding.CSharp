using MaaFramework.Binding.Abstractions.Native;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;
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
        var handle = MaaResourceCreate(MaaNotificationCallback, nint.Zero);
        SetHandle(handle, needReleased: true);
    }

    /// <inheritdoc cref="MaaResource(CheckStatusOption, string[])"/>
    public MaaResource(params string[] paths)
        : this(CheckStatusOption.ThrowIfNotSucceeded, paths)
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
            if (check == CheckStatusOption.ThrowIfNotSucceeded)
            {
                status.ThrowIfNot(MaaJobStatus.Succeeded, MaaJobStatusException.MaaResourceMessage, path);
            }
        }
    }

    /// <inheritdoc cref="MaaResource(CheckStatusOption, string[])"/>
    public MaaResource(IEnumerable<string> paths)
        : this(CheckStatusOption.ThrowIfNotSucceeded, paths)
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
            if (check == CheckStatusOption.ThrowIfNotSucceeded)
            {
                status.ThrowIfNot(MaaJobStatus.Succeeded, MaaJobStatusException.MaaResourceMessage, path);
            }
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaResourceDestroy(Handle);

    private readonly MaaMarshaledApis<MaaCustomActionCallback> _actions = new();
    private readonly MaaMarshaledApis<MaaCustomRecognitionCallback> _recognitions = new();

    /// <inheritdoc/>
    public bool Register<T>(string name, T custom) where T : IMaaCustomResource
    {
        custom.Name = name;
        return Register(custom);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceRegisterCustomAction"/> and <see cref="MaaResourceRegisterCustomRecognition"/>.
    /// </remarks>
    public bool Register<T>(T custom) where T : IMaaCustomResource => custom switch
    {
        IMaaCustomAction res
            => MaaResourceRegisterCustomAction(Handle, res.Name, res.Convert(out var callback), nint.Zero).ToBoolean()
            && _actions.Set(res.Name, callback),
        IMaaCustomRecognition res
            => MaaResourceRegisterCustomRecognition(Handle, res.Name, res.Convert(out var callback), nint.Zero).ToBoolean()
            && _recognitions.Set(res.Name, callback),
        _
            => throw new NotImplementedException(),
    };

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceUnregisterCustomAction"/> and <see cref="MaaResourceUnregisterCustomRecognition"/>.
    /// </remarks>
    public bool Unregister<T>(string name) where T : IMaaCustomResource
    {
        var t = typeof(T);
        if (typeof(IMaaCustomAction).IsAssignableFrom(t))
            return MaaResourceUnregisterCustomAction(Handle, name).ToBoolean()
                && _actions.Remove(name);
        if (typeof(IMaaCustomRecognition).IsAssignableFrom(t))
            return MaaResourceUnregisterCustomRecognition(Handle, name).ToBoolean()
                && _recognitions.Remove(name);

        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceUnregisterCustomAction"/> and <see cref="MaaResourceUnregisterCustomRecognition"/>.
    /// </remarks>
    public bool Unregister<T>(T custom) where T : IMaaCustomResource => custom switch
    {
        IMaaCustomAction
            => MaaResourceUnregisterCustomAction(Handle, custom.Name).ToBoolean()
            && _actions.Remove(custom.Name),
        IMaaCustomRecognition
            => MaaResourceUnregisterCustomRecognition(Handle, custom.Name).ToBoolean()
            && _recognitions.Remove(custom.Name),
        _
            => throw new NotImplementedException(),
    };

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceClearCustomAction"/> and <see cref="MaaResourceClearCustomRecognition"/>.
    /// </remarks>
    public bool Clear<T>() where T : IMaaCustomResource => typeof(T).Name switch
    {
        nameof(IMaaCustomAction)
            => MaaResourceClearCustomAction(Handle).ToBoolean()
            && _actions.Clear(),
        nameof(IMaaCustomRecognition)
            => MaaResourceClearCustomRecognition(Handle).ToBoolean()
            && _recognitions.Clear(),
        _
            => throw new NotImplementedException()
    };

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceClear"/>.
    /// </remarks>
    public bool Clear(bool includeCustomResource = false)
    {
        var ret = MaaResourceClear(Handle).ToBoolean();
        if (!includeCustomResource) return ret;
        ret &= Clear<IMaaCustomAction>();
        ret &= Clear<IMaaCustomRecognition>();
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourcePostPath"/>.
    /// </remarks>
    public MaaJob AppendPath(string path)
    {
        var id = MaaResourcePostPath(Handle, path);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(MaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return (MaaJobStatus)MaaResourceStatus(Handle, job.Id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceWait"/>.
    /// </remarks>
    public MaaJobStatus Wait(MaaJob job)
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
        throw new InvalidOperationException();

        /*
        byte[] optValue = (value, opt) switch
        {
            (int vvvv, ResourceOption.Invalid) => vvvv.ToMaaOptionValues(),
            _ => throw new InvalidOperationException(),
        };

        return MaaResourceSetOption(Handle, (MaaResOption)opt, optValue, (MaaOptionValueSize)optValue.Length).ToBoolean();
        */
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
