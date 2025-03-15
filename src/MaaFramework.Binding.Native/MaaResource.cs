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
    private readonly HashSet<string> _postedPaths = [];

    /// <inheritdoc/>
    public override string ToString() => $"{GetType().Name} {{ Paths = {string.Join(" & ", _postedPaths)}, CustomActions = {string.Join(" & ", _actions.Names)}, CustomRecognitions = {string.Join(" & ", _recognitions.Names)} }}";

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
    public MaaResource(params string[] bundlePaths)
        : this(CheckStatusOption.ThrowIfNotSucceeded, bundlePaths)
    {
    }

    /// <param name="check">Checks AppendBundle(bundlePath).Wait() status if is <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not checks.</param>
    /// <param name="bundlePaths">The paths of maa bundle.</param>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="MaaJobStatusException"/>
    /// <inheritdoc cref="MaaResource()"/>
    public MaaResource(CheckStatusOption check, params string[] bundlePaths)
        : this()
    {
        ArgumentNullException.ThrowIfNull(bundlePaths);

        foreach (var bundlePath in bundlePaths)
        {
            var status = AppendBundle(bundlePath).Wait();
            if (check == CheckStatusOption.ThrowIfNotSucceeded)
            {
                status.ThrowIfNot(MaaJobStatus.Succeeded, MaaJobStatusException.MaaResourceMessage, bundlePath);
            }
        }
    }

    /// <inheritdoc cref="MaaResource(CheckStatusOption, string[])"/>
    public MaaResource(IEnumerable<string> bundlePaths)
        : this(CheckStatusOption.ThrowIfNotSucceeded, bundlePaths)
    {
    }

    /// <inheritdoc cref="MaaResource(CheckStatusOption, string[])"/>
    public MaaResource(CheckStatusOption check, IEnumerable<string> bundlePaths)
        : this()
    {
        ArgumentNullException.ThrowIfNull(bundlePaths);

        foreach (var bundlePath in bundlePaths)
        {
            var status = AppendBundle(bundlePath).Wait();
            if (check == CheckStatusOption.ThrowIfNotSucceeded)
            {
                status.ThrowIfNot(MaaJobStatus.Succeeded, MaaJobStatusException.MaaResourceMessage, bundlePath);
            }
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaResourceDestroy(Handle);

    private readonly MaaMarshaledApiRegistry<MaaCustomActionCallback> _actions = new();
    private readonly MaaMarshaledApiRegistry<MaaCustomRecognitionCallback> _recognitions = new();

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
            => MaaResourceRegisterCustomAction(Handle, res.Name, res.Convert(out var callback), nint.Zero)
            && _actions.Register(res.Name, callback),
        IMaaCustomRecognition res
            => MaaResourceRegisterCustomRecognition(Handle, res.Name, res.Convert(out var callback), nint.Zero)
            && _recognitions.Register(res.Name, callback),
        _
            => throw new NotImplementedException($"Type '{typeof(T)}' is not implemented."),
    };

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceUnregisterCustomAction"/> and <see cref="MaaResourceUnregisterCustomRecognition"/>.
    /// </remarks>
    public bool Unregister<T>(string name) where T : IMaaCustomResource
    {
        var t = typeof(T);
        if (typeof(IMaaCustomAction).IsAssignableFrom(t))
            return MaaResourceUnregisterCustomAction(Handle, name)
                && _actions.Remove(name);
        if (typeof(IMaaCustomRecognition).IsAssignableFrom(t))
            return MaaResourceUnregisterCustomRecognition(Handle, name)
                && _recognitions.Remove(name);

        throw new NotImplementedException($"Type '{typeof(T)}' is not implemented.");
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceUnregisterCustomAction"/> and <see cref="MaaResourceUnregisterCustomRecognition"/>.
    /// </remarks>
    public bool Unregister<T>(T custom) where T : IMaaCustomResource => custom switch
    {
        IMaaCustomAction
            => MaaResourceUnregisterCustomAction(Handle, custom.Name)
            && _actions.Remove(custom.Name),
        IMaaCustomRecognition
            => MaaResourceUnregisterCustomRecognition(Handle, custom.Name)
            && _recognitions.Remove(custom.Name),
        _
            => throw new NotImplementedException($"Type '{typeof(T)}' is not implemented."),
    };

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceClearCustomAction"/> and <see cref="MaaResourceClearCustomRecognition"/>.
    /// </remarks>
    public bool Clear<T>() where T : IMaaCustomResource => typeof(T).Name switch
    {
        nameof(IMaaCustomAction)
            => MaaResourceClearCustomAction(Handle)
            && _actions.Clear(),
        nameof(IMaaCustomRecognition)
            => MaaResourceClearCustomRecognition(Handle)
            && _recognitions.Clear(),
        _
            => throw new NotImplementedException($"Type '{typeof(T)}' is not implemented."),
    };

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceClear"/>.
    /// </remarks>
    public bool Clear(bool includeCustomResource = false)
    {
        _postedPaths.Clear();
        var ret = MaaResourceClear(Handle);
        if (!includeCustomResource) return ret;
        ret &= Clear<IMaaCustomAction>();
        ret &= Clear<IMaaCustomRecognition>();
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourcePostBundle"/>.
    /// </remarks>
    public MaaJob AppendBundle(string path)
    {
        _postedPaths.Add(path);
        var id = MaaResourcePostBundle(Handle, path);
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
    public bool IsLoaded => MaaResourceLoaded(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceSetOption"/>.
    /// </remarks>
    public bool SetOption<T>(ResourceOption opt, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var optValue = (value, opt) switch
        {
            (int vvvv, ResourceOption.InferenceDevice
                    or ResourceOption.InferenceExecutionProvider) => vvvv.ToMaaOptionValue(),

            (InferenceDevice v, ResourceOption.InferenceDevice) => ((int)v).ToMaaOptionValue(),
            (InferenceExecutionProvider v, ResourceOption.InferenceExecutionProvider) => ((int)v).ToMaaOptionValue(),

            _ => throw new NotSupportedException($"'{nameof(ResourceOption)}.{opt}' or type '{typeof(T)}' is not supported."),
        };

        return MaaResourceSetOption(Handle, (MaaResOption)opt, optValue, (MaaOptionValueSize)optValue.Length);
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
            var ret = MaaResourceGetHash(Handle, buffer.Handle);
            return ret ? buffer.ToString() : null;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceGetNodeList"/>.
    /// </remarks>
    public IList<string> NodeList
    {
        get
        {
            MaaStringListBuffer.Get(out var list, h => MaaResourceGetNodeList(Handle, h)).ThrowIfFalse();
            return list;
        }
    }
}
