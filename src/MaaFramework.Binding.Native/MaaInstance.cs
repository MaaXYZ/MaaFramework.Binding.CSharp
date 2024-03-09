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
    /// <param name="toolkitInit">Whether inits the <see cref="Toolkit"/>.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreate"/>.
    /// </remarks>
    public MaaInstance(bool toolkitInit = false)
    {
        var handle = MaaCreate(MaaApiCallback, nint.Zero);
        SetHandle(handle, needReleased: true);

        Toolkit = new MaaToolkit(toolkitInit);
        Utility = new MaaUtility();
    }

    /// <param name="controller">The controller.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="disposeOptions">The dispose options.</param>
    /// <param name="toolkitInit">Whether inits the <see cref="Toolkit"/>.</param>
    /// <inheritdoc cref="MaaInstance(bool)"/>
    [SetsRequiredMembers]
    public MaaInstance(IMaaController<nint> controller, IMaaResource<nint> resource, DisposeOptions disposeOptions, bool toolkitInit = false)
        : this(toolkitInit)
    {
        Resource = resource;
        Controller = controller;
        DisposeOptions = disposeOptions;
    }

    /// <inheritdoc/>
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

        if (DisposeOptions.HasFlag(DisposeOptions.Toolkit))
        {
            Toolkit.Config.Uninit();
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

        MaaOptionValue[] bytes = (value, opt) switch
        {
            // (int vvvv, InstanceOption.Invalid) => vvvv.ToMaaOptionValues(),
            _ => throw new InvalidOperationException(),
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
    public IMaaToolkit Toolkit { get; set; }

    /// <inheritdoc/>
    public IMaaUtility Utility { get; set; }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaInited"/>.
    /// </remarks>
    public bool Initialized => MaaInited(Handle).ToBoolean();

    private static readonly Dictionary<string, MaaCustomActionApi> _actions = [];
    private static readonly Dictionary<string, MaaCustomRecognizerApi> _recognizers = [];

    /// <inheritdoc/>
    public bool Register<T>(string name, T custom) where T : Custom.IMaaCustomTask
    {
        ((Custom.IMaaCustom)custom).Name = name;
        return Register(custom);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRegisterCustomAction"/> and <see cref="MaaRegisterCustomRecognizer"/>.
    /// </remarks>
    public bool Register<T>(T custom) where T : Custom.IMaaCustomTask
    {
        var ret = false;
        switch (custom)
        {
            case Custom.MaaCustomActionTask task:
                var action = MaaCustomActionApi.Convert(task);
                ret = MaaRegisterCustomAction(Handle, task.Name, ref action, nint.Zero).ToBoolean();
                if (ret) _actions[task.Name] = action;
                return ret;
            case Custom.MaaCustomRecognizerTask task:
                var recognizer = MaaCustomRecognizerApi.Convert(task);
                ret = MaaRegisterCustomRecognizer(Handle, task.Name, ref recognizer, nint.Zero).ToBoolean();
                if (ret) _recognizers[task.Name] = recognizer;
                return ret;
            default:
                return ret;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaUnregisterCustomAction"/> and <see cref="MaaUnregisterCustomRecognizer"/>.
    /// </remarks>
    public bool Unregister<T>(string name) where T : Custom.IMaaCustomTask
    {
        var ret = false;
        if (typeof(T) == typeof(MaaCustomActionApi))
        {
            ret = MaaUnregisterCustomAction(Handle, name).ToBoolean();
            if (ret) _actions.Remove(name);
            return ret;
        }
        else if (typeof(T) == typeof(MaaCustomRecognizerApi))
        {
            ret = MaaUnregisterCustomRecognizer(Handle, name).ToBoolean();
            if (ret) _recognizers.Remove(name);
            return ret;
        }
        else
        {
            return ret;
        }
    }

    /// <inheritdoc/>
    public bool Unregister<T>(T custom) where T : Custom.IMaaCustomTask
    {
        return Unregister<T>(((Custom.IMaaCustom)custom).Name);
    }

    /// <remarks>
    ///     Wrapper of <see cref="MaaClearCustomAction"/> and <see cref="MaaClearCustomRecognizer"/>.
    /// </remarks>
    public bool Clear<T>() where T : Custom.IMaaCustomTask
    {
        var ret = false;
        if (typeof(T) == typeof(MaaCustomActionApi))
        {
            ret = MaaClearCustomAction(Handle).ToBoolean();
            if (ret) _actions.Clear();
            return ret;
        }
        else if (typeof(T) == typeof(MaaCustomRecognizerApi))
        {
            ret = MaaClearCustomRecognizer(Handle).ToBoolean();
            if (ret) _recognizers.Clear();
            return ret;
        }
        else
        {
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
        => MaaPostStop(Handle).ToBoolean();
}
