using MaaFramework.Binding.Abstractions.Native;
using MaaFramework.Binding.Interop.Native;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaInstance;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaInstance"/>.
/// </summary>
public class MaaInstance : MaaCommon, IMaaInstance<nint>
{
    private IMaaResource<nint> _resource = default!;
    private IMaaController<nint> _controller = default!;

    /// <summary>
    ///     Creates a <see cref="MaaInstance"/> instance.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreate"/>.
    /// </remarks>
    public MaaInstance()
    {
        var handle = MaaCreate(MaaApiCallback, nint.Zero);
        SetHandle(handle, needReleased: true);
    }

    /// <param name="resource">The resource.</param>
    /// <param name="controller">The controller.</param>
    /// <param name="disposeOptions">The dispose options.</param>
    /// <inheritdoc cref="MaaInstance()"/>
    [SetsRequiredMembers]
    public MaaInstance(IMaaResource<nint> resource, IMaaController<nint> controller, DisposeOptions disposeOptions)
        : this()
    {
        Resource = resource;
        Controller = controller;
        DisposeOptions = disposeOptions;
    }

    /// <summary>
    ///     Whether to dispose the <see cref="Resource"/> and the <see cref="Controller"/> when <see cref="IDisposable.Dispose"/> was invoked.
    /// </summary>
    public required DisposeOptions DisposeOptions { get; set; }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
    {
        // Cannot destroy Instance before disposing Controller and Resource.

        if (DisposeOptions.HasFlag(DisposeOptions.Controller))
        {
            Controller.Dispose();
        }

        if (DisposeOptions.HasFlag(DisposeOptions.Resource))
        {
            Resource.Dispose();
        }

        MaaDestroy(Handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetOption"/>.
    /// </remarks>
    public bool SetOption<T>(InstanceOption opt, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        MaaOptionValue[] bytes = opt switch
        {
            InstanceOption.Invalid => throw new InvalidOperationException(),
            _ => throw new NotImplementedException(),
        };

        return MaaSetOption(Handle, (MaaInstOption)opt, ref bytes[0], (MaaOptionValueSize)bytes.Length).ToBoolean();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetResource"/>.
    /// </remarks>
    IMaaResource IMaaInstance.Resource => Resource;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetController"/>.
    /// </remarks>
    IMaaController IMaaInstance.Controller => Controller;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaBindResource"/> and <see cref="MaaGetResource"/>.
    /// </remarks>
    public required IMaaResource<nint> Resource
    {
        get
        {
            MaaBindException.ThrowIf(
                MaaGetResource(Handle) != _resource.Handle,
                MaaBindException.ResourceModifiedMessage);
            return _resource;
        }
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            MaaBindException.ThrowIf(
                !MaaBindResource(Handle, value.Handle).ToBoolean(),
                MaaBindException.ResourceMessage);
            _resource = value;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaBindController"/> and <see cref="MaaGetController"/>.
    /// </remarks>
    public required IMaaController<nint> Controller
    {
        get
        {
            MaaBindException.ThrowIf(
                MaaGetController(Handle) != _controller.Handle,
                MaaBindException.ControllerModifiedMessage);
            return _controller;
        }
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            MaaBindException.ThrowIf(
                !MaaBindController(Handle, value.Handle).ToBoolean(),
                MaaBindException.ControllerMessage);
            _controller = value;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaInited"/>.
    /// </remarks>
    public bool Initialized => MaaInited(Handle).ToBoolean();

    private static readonly Dictionary<string, MaaCustomRecognizerApi> _recognizers = new();
    private static readonly Dictionary<string, MaaCustomActionApi> _actions = new();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRegisterCustomRecognizer"/> and <see cref="MaaRegisterCustomAction"/>.
    /// </remarks>
    public bool Register<T>(string name, T custom) where T : Custom.IMaaCustomTask
    {
        var ret = false;
        switch (custom)
        {
            case Custom.MaaCustomRecognizerApi api:
                var recognizer = MaaCustomRecognizerApi.Convert(api);
                ret = MaaRegisterCustomRecognizer(Handle, name, ref recognizer, nint.Zero).ToBoolean();
                if (ret) _recognizers[name] = recognizer;
                return ret;
            case Custom.MaaCustomActionApi api:
                var action = MaaCustomActionApi.Convert(api);
                ret = MaaRegisterCustomAction(Handle, name, ref action, nint.Zero).ToBoolean();
                if (ret) _actions[name] = action;
                return ret;
            default:
                return ret;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaUnregisterCustomRecognizer"/> and <see cref="MaaUnregisterCustomAction"/>.
    /// </remarks>
    public bool Unregister<T>(string name) where T : Custom.IMaaCustomTask
    {
        var ret = false;
        switch (typeof(T).Name)
        {
            case nameof(MaaCustomRecognizerApi):
                ret = MaaUnregisterCustomRecognizer(Handle, name).ToBoolean();
                if (ret) _recognizers.Remove(name);
                return ret;
            case nameof(MaaCustomActionApi):
                ret = MaaUnregisterCustomAction(Handle, name).ToBoolean();
                if (ret) _actions.Remove(name);
                return ret;
            default:
                return ret;
        }
    }

    /// <remarks>
    ///     Wrapper of <see cref="MaaClearCustomRecognizer"/> and <see cref="MaaClearCustomAction"/>.
    /// </remarks>
    public bool Clear<T>() where T : Custom.IMaaCustomTask
    {
        var ret = false;
        switch (typeof(T).Name)
        {
            case nameof(MaaCustomRecognizerApi):
                ret = MaaClearCustomRecognizer(Handle).ToBoolean();
                if (ret) _recognizers.Clear();
                return ret;
            case nameof(MaaCustomActionApi):
                ret = MaaClearCustomAction(Handle).ToBoolean();
                if (ret) _actions.Clear();
                return ret;
            default:
                return ret;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaPostTask"/>.
    /// </remarks>
    public IMaaJob AppendTask(string taskEntryName, string taskParam = "{}")
    {
        var id = MaaPostTask(Handle, taskEntryName, taskParam);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetTaskParam"/>.
    /// </remarks>
    public bool SetParam(IMaaJob job, string param)
    {
        ArgumentNullException.ThrowIfNull(job);

        return MaaSetTaskParam(Handle, job.Id, param).ToBoolean();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(IMaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return (MaaJobStatus)MaaTaskStatus(Handle, job.Id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaWaitTask"/>.
    /// </remarks>
    public MaaJobStatus Wait(IMaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return (MaaJobStatus)MaaWaitTask(Handle, job.Id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskAllFinished"/>.
    /// </remarks>
    public bool AllTasksFinished => MaaTaskAllFinished(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStop"/>.
    /// </remarks>
    public bool Abort()
        => MaaStop(Handle).ToBoolean();
}
