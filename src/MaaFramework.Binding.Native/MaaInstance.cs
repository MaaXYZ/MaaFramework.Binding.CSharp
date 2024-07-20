using MaaFramework.Binding.Abstractions.Native;
using MaaFramework.Binding.Custom;
using MaaFramework.Binding.Interop.Native;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaInstance;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaInstance"/>.
/// </summary>
public class MaaInstance : MaaCommon, IMaaInstance<nint>
{
#pragma warning disable CA2213
    private readonly IMaaResource<nint> _resource = default!;
    private readonly IMaaController<nint> _controller = default!;
#pragma warning restore CA2213

    /// <summary>
    ///     Creates a <see cref="MaaInstance"/> instance.
    /// </summary>
    /// <param name="toolkitInit">Whether initializes the <see cref="Toolkit"/>.</param>
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
    /// <param name="toolkitInit">Whether initializes the <see cref="Toolkit"/>.</param>
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

        MaaDestroy(Handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetOption"/>.
    /// </remarks>
    public bool SetOption<T>(InstanceOption opt, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        byte[] optValue = (value, opt) switch
        {
            // (int vvvv, InstanceOption.Invalid) => vvvv.ToMaaOptionValues(),
            _ => throw new InvalidOperationException(),
        };

        return MaaSetOption(Handle, (MaaInstOption)opt, optValue, (MaaOptionValueSize)optValue.Length).ToBoolean();
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

    private readonly MaaMarshaledApis<MaaActionApiTuple> _action = new();
    private readonly MaaMarshaledApis<MaaRecognizerApiTuple> _recognizer = new();

    /// <inheritdoc/>
    public bool Register<T>(string name, T custom) where T : IMaaCustomTask
    {
        custom.Name = name;
        return Register(custom);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRegisterCustomAction"/> and <see cref="MaaRegisterCustomRecognizer"/>.
    /// </remarks>
    public bool Register<T>(T custom) where T : IMaaCustomTask => custom switch
    {
        IMaaCustomAction task
            => MaaRegisterCustomAction(Handle, task.Name, task.Convert(out var t), nint.Zero).ToBoolean() && _action.Set(t.Managed.Name, t),
        IMaaCustomRecognizer task
            => MaaRegisterCustomRecognizer(Handle, task.Name, task.Convert(out var t), nint.Zero).ToBoolean() && _recognizer.Set(t.Managed.Name, t),
        _ => throw new NotImplementedException(),
    };

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaUnregisterCustomAction"/> and <see cref="MaaUnregisterCustomRecognizer"/>.
    /// </remarks>
    public bool Unregister<T>(string name) where T : IMaaCustomTask
    {
        var t = typeof(T);
        if (typeof(IMaaCustomAction).IsAssignableFrom(t))
            return MaaUnregisterCustomAction(Handle, name).ToBoolean() && _action.Remove(name);
        if (typeof(IMaaCustomRecognizer).IsAssignableFrom(t))
            return MaaUnregisterCustomRecognizer(Handle, name).ToBoolean() && _recognizer.Remove(name);
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaUnregisterCustomAction"/> and <see cref="MaaUnregisterCustomRecognizer"/>.
    /// </remarks>
    public bool Unregister<T>(T custom) where T : IMaaCustomTask => custom switch
    {
        IMaaCustomAction
            => MaaUnregisterCustomAction(Handle, custom.Name).ToBoolean() && _action.Remove(custom.Name),
        IMaaCustomRecognizer
            => MaaUnregisterCustomRecognizer(Handle, custom.Name).ToBoolean() && _recognizer.Remove(custom.Name),
        _ => throw new NotImplementedException(),
    };

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaClearCustomAction"/> and <see cref="MaaClearCustomRecognizer"/>.
    /// </remarks>
    public bool Clear<T>() where T : IMaaCustomTask => typeof(T).Name switch
    {
        nameof(IMaaCustomAction)
            => MaaClearCustomAction(Handle).ToBoolean() && _action.Clear(),
        nameof(IMaaCustomRecognizer)
            => MaaClearCustomRecognizer(Handle).ToBoolean() && _recognizer.Clear(),
        _ => throw new NotImplementedException()
    };

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaPostTask"/>.
    /// </remarks>
    public MaaTaskJob AppendTask(string entry, string param = "{}")
    {
        var id = MaaPostTask(Handle, entry, param);
        return new MaaTaskJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaPostRecognition"/>.
    /// </remarks>
    public MaaTaskJob AppendRecognition(string entry, string param = "{}")
    {
        var id = MaaPostRecognition(Handle, entry, param);
        return new MaaTaskJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaPostAction"/>.
    /// </remarks>
    public MaaTaskJob AppendAction(string entry, string param = "{}")
    {
        var id = MaaPostAction(Handle, entry, param);
        return new MaaTaskJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetTaskParam"/>.
    /// </remarks>
    public bool SetTaskParam(MaaTaskJob job, string param)
    {
        ArgumentNullException.ThrowIfNull(job);

        return MaaSetTaskParam(Handle, job.Id, param).ToBoolean();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(MaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return (MaaJobStatus)MaaTaskStatus(Handle, job.Id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaWaitTask"/>.
    /// </remarks>
    public MaaJobStatus Wait(MaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        return (MaaJobStatus)MaaWaitTask(Handle, job.Id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskAllFinished"/>.
    /// </remarks>
    [Obsolete("Use !Running instead.")]
    public bool AllTasksFinished => MaaTaskAllFinished(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRunning"/>.
    /// </remarks>
    public bool Running => MaaRunning(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaPostStop"/>.
    /// </remarks>
    public bool Abort()
        => MaaPostStop(Handle).ToBoolean();
}
