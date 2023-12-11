using MaaFramework.Binding.Native.Abstractions;
using MaaFramework.Binding.Native.Interop;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Native.Interop.MaaInstance;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Native.Interop.MaaInstance"/>.
/// </summary>
public class MaaInstance : MaaCommon<InstanceOption>, IMaaInstance<nint>
{
    private IMaaResource<nint> _resource = default!;
    private IMaaController<nint> _controller = default!;

    /// <summary>
    ///     Converts a <see cref="IMaaInstance{nint}"/> instance to a <see cref="MaaInstance"/>.
    /// </summary>
    /// <param name="maaInstance">The <see cref="IMaaInstance{nint}"/> instance.</param>
    /// <exception cref="ArgumentNullException"/>
    [SetsRequiredMembers]
    public MaaInstance(IMaaInstance<nint> maaInstance)
    {
        _resource ??= new MaaResource(maaInstance.Resource);
        _controller ??= new MaaController(maaInstance.Controller);

        if (Resource == null || Controller == null)
            throw new ArgumentNullException(nameof(maaInstance), "Resource and Controller cannot be null");
    }

    /// <inheritdoc cref="MaaInstance(MaaCallbackTransparentArg)"/>
    public MaaInstance()
        : this(MaaCallbackTransparentArg.Zero)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaInstance"/> instance.
    /// </summary>
    /// <param name="maaCallbackTransparentArg">The MaaCallbackTransparentArg.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreate"/>.
    /// </remarks>
    public MaaInstance(MaaCallbackTransparentArg maaCallbackTransparentArg)
    {
        var handle = MaaCreate(maaApiCallback, maaCallbackTransparentArg);
        SetHandle(handle, needReleased: true);
    }

    /// <param name="resource">The resource.</param>
    /// <param name="controller">The controller.</param>
    /// <param name="disposeOptions">The dispose options.</param>
    /// <inheritdoc cref="MaaInstance(MaaCallbackTransparentArg)"/>
    [SetsRequiredMembers]
    public MaaInstance(IMaaResource<nint> resource, IMaaController<nint> controller, DisposeOptions disposeOptions)
        : this(MaaCallbackTransparentArg.Zero)
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
        MaaDestroy(Handle);

        if (DisposeOptions.HasFlag(DisposeOptions.Controller))
        {
            Controller.Dispose();
        }

        if (DisposeOptions.HasFlag(DisposeOptions.Resource))
        {
            Resource.Dispose();
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetOption"/>.
    /// </remarks>
    sealed protected override bool SetOption(InstanceOption option, MaaOptionValue[] value)
     => MaaSetOption(Handle, (MaaInstOption)option, ref value[0], (MaaOptionValueSize)value.Length).ToBoolean();

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
            MaaBindException.ThrowIf(
                MaaBindResource(Handle, value.Handle).ToBoolean(),
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
            MaaBindException.ThrowIf(
                MaaBindController(Handle, value.Handle).ToBoolean(),
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

    /// <summary>
    ///     Registers a <see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/> named <paramref name="name"/> in the <see cref="MaaInstance"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/>.</typeparam>
    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRegisterCustomRecognizer"/> and <see cref="MaaRegisterCustomAction"/>.
    /// </remarks>
    public bool Register<T>(string name, T custom, nint arg) where T : IMaaDefStruct
    {
        var ret = false;
        switch (custom)
        {
            case MaaCustomRecognizerApi recognizer:
                ret = MaaRegisterCustomRecognizer(Handle, name, ref recognizer, arg).ToBoolean();
                if (ret) _recognizers[name] = recognizer;
                return ret;
            case MaaCustomActionApi action:
                ret = MaaRegisterCustomAction(Handle, name, ref action, arg).ToBoolean();
                if (ret) _actions[name] = action;
                return ret;
            default:
                return ret;
        }
    }

    /// <summary>
    ///     Unregisters a <see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/> named <paramref name="name"/> in the <see cref="MaaInstance"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/>.</typeparam>
    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaUnregisterCustomRecognizer"/> and <see cref="MaaUnregisterCustomAction"/>.
    /// </remarks>
    public bool Unregister<T>(string name) where T : IMaaDefStruct
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

    /// <summary>
    ///     Clears <see cref="MaaCustomRecognizerApi"/>s or <see cref="MaaCustomActionApi"/>s in the <see cref="MaaInstance"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/>.</typeparam>
    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaClearCustomRecognizer"/> and <see cref="MaaClearCustomAction"/>.
    /// </remarks>
    public bool Clear<T>() where T : IMaaDefStruct
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
        => MaaSetTaskParam(Handle, job.Id, param).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(IMaaJob job)
        => (MaaJobStatus)MaaTaskStatus(Handle, job.Id);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaWaitTask"/>.
    /// </remarks>
    public MaaJobStatus Wait(IMaaJob job)
        => (MaaJobStatus)MaaWaitTask(Handle, job.Id);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskAllFinished"/>.
    /// </remarks>
    public bool AllTasksFinished => MaaTaskAllFinished(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStop"/>.
    /// </remarks>
    public bool Stop()
        => MaaStop(Handle).ToBoolean();
}
