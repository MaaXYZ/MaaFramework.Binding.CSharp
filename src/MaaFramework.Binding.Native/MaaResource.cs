using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;
using MaaFramework.Binding.Interop.Native;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static MaaFramework.Binding.Interop.Native.MaaResource;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaResource"/>.
/// </summary>
public class MaaResource : MaaCommon, IMaaResource<MaaResourceHandle>, IMaaPost
{
    private readonly HashSet<string> _postedPaths = [];

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    public override string ToString() => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ Paths = [{string.Join(", ", _postedPaths)}], CustomActions = [{string.Join(", ", _actions.Names)}] , CustomRecognitions = [{string.Join(" & ", _recognitions.Names)}] }}";

    [ExcludeFromCodeCoverage(Justification = "Test for stateful mode.")]
    internal MaaResource(MaaResourceHandle handle)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <summary>
    ///     Creates a <see cref="MaaResource"/> instance.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceCreate"/>.
    /// </remarks>
    public MaaResource()
    {
        var handle = MaaResourceCreate();
        _ = MaaResourceAddSink(handle, MaaEventCallback, 2);
        SetHandle(handle, needReleased: true);
    }

    /// <inheritdoc cref="Binding.MaaResource(CheckStatusOption, string[])"/>
    public MaaResource(params string[] bundlePaths)
        : this(CheckStatusOption.ThrowIfNotSucceeded, bundlePaths)
    {
    }

    /// <param name="check">Checks AppendBundle(bundlePath).Wait() status if is <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not checks.</param>
    /// <param name="bundlePaths">The paths of maa bundle.</param>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="MaaJobStatusException"/>
    /// <inheritdoc cref="Binding.MaaResource()"/>
    public MaaResource(CheckStatusOption check, params string[] bundlePaths)
        : this()
    {
        ArgumentNullException.ThrowIfNull(bundlePaths);

        foreach (var bundlePath in bundlePaths)
        {
            var status = AppendBundle(bundlePath).Wait();
            if (check == CheckStatusOption.ThrowIfNotSucceeded)
            {
                _ = status.ThrowIfNot(MaaJobStatus.Succeeded, MaaJobStatusException.MaaResourceMessage, bundlePath);
            }
        }
    }

    /// <inheritdoc cref="Binding.MaaResource(CheckStatusOption, string[])"/>
    public MaaResource(IEnumerable<string> bundlePaths)
        : this(CheckStatusOption.ThrowIfNotSucceeded, bundlePaths)
    {
    }

    /// <inheritdoc cref="Binding.MaaResource(CheckStatusOption, string[])"/>
    public MaaResource(CheckStatusOption check, IEnumerable<string> bundlePaths)
        : this()
    {
        ArgumentNullException.ThrowIfNull(bundlePaths);

        foreach (var bundlePath in bundlePaths)
        {
            var status = AppendBundle(bundlePath).Wait();
            if (check == CheckStatusOption.ThrowIfNotSucceeded)
            {
                _ = status.ThrowIfNot(MaaJobStatus.Succeeded, MaaJobStatusException.MaaResourceMessage, bundlePath);
            }
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle(MaaResourceHandle handle)
    {
        try
        {
            if (LastJob != null)
                _ = MaaResourceWait(handle, LastJob.Id);
        }
        finally
        {
            MaaResourceDestroy(handle);
        }
    }

    private readonly MaaMarshaledApiRegistry<MaaCustomActionCallback> _actions = new();
    private readonly MaaMarshaledApiRegistry<MaaCustomRecognitionCallback> _recognitions = new();

    /// <inheritdoc/>
    public bool Register<T>(string name, T custom) where T : IMaaCustomResource
    {
        custom.Name = name;
        return Register(custom);
    }

    /// <inheritdoc/>
    public bool Register<T>(string? name = null) where T : IMaaCustomResource, new()
    {
        var custom = new T();
        if (name != null)
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
            return MaaResourceUnregisterCustomAction(Handle, name) && _actions.Remove(name);
        if (typeof(IMaaCustomRecognition).IsAssignableFrom(t))
            return MaaResourceUnregisterCustomRecognition(Handle, name) && _recognitions.Remove(name);

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
        _ = _postedPaths.Add(path);
        var id = MaaResourcePostBundle(Handle, path);
        return CreateJob(id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceOverridePipeline"/>.
    /// </remarks>
    public bool OverridePipeline([StringSyntax("Json")] string pipelineOverride)
        => MaaResourceOverridePipeline(Handle, pipelineOverride);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceOverrideNext"/>.
    /// </remarks>
    public bool OverrideNext(string nodeName, IEnumerable<string> nextList)
        => MaaStringListBuffer.TrySetList(nextList, listBuffer
            => MaaResourceOverrideNext(Handle, nodeName, listBuffer));

    /// <inheritdoc/>
    public bool OverrideImage(string imageName, IMaaImageBuffer image)
        => OverrideImage(imageName, (MaaImageBuffer)image);

    /// <inheritdoc cref="IMaaResource.OverrideImage"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceOverrideImage"/>.
    /// </remarks>
    public bool OverrideImage(string imageName, MaaImageBuffer image)
    {
        ArgumentNullException.ThrowIfNull(image);

        return MaaResourceOverrideImage(Handle, imageName, image.Handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceGetNodeData"/>.
    /// </remarks>
    public bool GetNodeData(string nodeName, [MaybeNullWhen(false)][StringSyntax("Json")] out string data)
        => MaaStringBuffer.TryGetValue(out data, buffer
            => MaaResourceGetNodeData(Handle, nodeName, buffer));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceStatus"/>.
    /// </remarks>
    [Obsolete("Deprecated from v4.5.0.")]
    public MaaJobStatus GetStatus(MaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        var id = job.Id;
        var handle = Handle;
        return IsInvalid ? MaaJobStatus.Invalid : (MaaJobStatus)MaaResourceStatus(handle, id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceWait"/>.
    /// </remarks>
    [Obsolete("Deprecated from v4.5.0.")]
    public MaaJobStatus Wait(MaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        var id = job.Id;
        var handle = Handle;
        return IsInvalid ? MaaJobStatus.Invalid : (MaaJobStatus)MaaResourceWait(handle, id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private MaaJob CreateJob(MaaResId id)
    {
        var job = new MaaJob(id, this);
        if (id != MaaDef.MaaInvalidId)
            LastJob = job;
        return job;
    }

    /// <inheritdoc/>
    public MaaJob? LastJob { get; private set; }

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
            (InferenceCoreMLFlags v, ResourceOption.InferenceDevice) => ((int)v).ToMaaOptionValue(),
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
            _ = MaaStringBuffer.TryGetValue(out var str, buffer => MaaResourceGetHash(Handle, buffer));
            return str;
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
            _ = MaaStringListBuffer.TryGetList(out var list, h => MaaResourceGetNodeList(Handle, h)).ThrowIfFalse();
            return list!;
        }
    }
}
