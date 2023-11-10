using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Enums;
using MaaFramework.Binding.Exceptions;
using MaaFramework.Binding.Interop;
using MaaFramework.Binding.Interop.Framework;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Framework.MaaInstance;

namespace MaaFramework.Binding;

/// <summary>
///     A class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Framework.MaaInstance"/>.
/// </summary>
public class MaaInstance : MaaCommon<InstanceOption>
{
    internal MaaInstanceHandle _handle;
    private bool disposed;
    private MaaResource _resource = new();
    private MaaController _controller = new();

    /* 为 RPC 准备的
    [SetsRequiredMembers]
    public MaaInstance(MaaInstance maaInstance)
    {
        _resource = maaInstance.Resource;
        _controller = maaInstance.Controller;

        if (Resource == null || Controller == null)
            throw new ArgumentNullException(nameof(maaInstance), "Resource and Controller cannot be null");
    }
    */

    /// <inheritdoc cref="MaaInstance(MaaCallbackTransparentArg)"/>
    public MaaInstance()
        : this(MaaCallbackTransparentArg.Zero)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaInstance"/> instance.
    /// </summary>
    /// <param name="maaCallbackTransparentArg"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreate"/>.
    /// </remarks>
    public MaaInstance(MaaCallbackTransparentArg maaCallbackTransparentArg)
    {
        _handle = MaaCreate(MaaApiCallback, maaCallbackTransparentArg);
    }

    /// <param name="resource">The resource.</param>
    /// <param name="controller">The controller.</param>
    /// <inheritdoc cref="MaaInstance(MaaCallbackTransparentArg)"/>
    [SetsRequiredMembers]
    public MaaInstance(MaaResource resource, MaaController controller)
        : this(MaaCallbackTransparentArg.Zero)
    {
        Resource = resource;
        Controller = controller;
    }

    /// <summary>
    ///     Disposes the <see cref="MaaInstance"/> instance.
    /// </summary>
    /// <param name="disposing"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDestroy"/>.
    /// </remarks>
    protected override void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Controller.Dispose();
                Resource.Dispose();
            }

            MaaDestroy(_handle);
            _handle = MaaInstanceHandle.Zero;
            disposed = true;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetOption"/>.
    /// </remarks>
    internal override bool SetOption(InstanceOption option, MaaOptionValue[] value)
     => MaaSetOption(_handle, (MaaInstOption)option, ref value[0], (MaaOptionValueSize)value.Length).ToBoolean();

    /// <summary>
    ///     Gets the resource or inits to bind a <see cref="MaaResource"/>.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaBindResource"/> and <see cref="MaaGetResource"/>.
    /// </remarks>
    /// <exception cref="MaaBindException"/>
    public required MaaResource Resource
    {
        get
        {
            MaaBindException.ThrowIfFalse(
                MaaGetResource(_handle) != _resource!._handle,
                MaaBindException.ResourceModifiedMessage);
            return _resource;
        }
        init
        {
            MaaBindException.ThrowIfFalse(
                MaaBindResource(_handle, value._handle).ToBoolean(),
                MaaBindException.ResourceMessage);
            _resource = value;
        }
    }

    /// <summary>
    ///     Gets the controller or inits to bind a <see cref="MaaController"/>.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaBindController"/> and <see cref="MaaGetController"/>.
    /// </remarks>
    /// <exception cref="MaaBindException"/>
    public required MaaController Controller
    {
        get
        {
            MaaBindException.ThrowIfFalse(
                MaaGetController(_handle) != _controller!._handle,
                MaaBindException.ControllerModifiedMessage);
            return _controller;
        }
        init
        {
            MaaBindException.ThrowIfFalse(
                MaaBindController(_handle, value._handle).ToBoolean(),
                MaaBindException.ControllerMessage);
            _controller = value;
        }
    }

    /// <summary>
    ///     Gets whether the <see cref="MaaInstance"/> is fully initialized.
    /// </summary>
    /// <value>
    ///     true if the <see cref="MaaInstance"/> was fully initialized; otherwise, false.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaInited"/>.
    /// </remarks>
    public bool Initialized => MaaInited(_handle).ToBoolean();

    private static readonly Dictionary<string, MaaCustomRecognizerApi> _recognizers = new();
    private static readonly Dictionary<string, MaaCustomActionApi> _actions = new();

    /// <summary>
    ///     Registers a <see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/> named <paramref name="name"/> in the <see cref="MaaInstance"/>.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="custom">The MaaCustomRecognizerApi or MaaCustomActionApi.</param>
    /// <param name="arg"></param>
    /// <typeparam name="T"><see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/>.</typeparam>
    /// <returns>
    ///     true if the <see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/> was registered successfully; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRegisterCustomRecognizer"/> and <see cref="MaaRegisterCustomAction"/>.
    /// </remarks>
    public bool Register<T>(string name, T custom, nint arg) where T : IMaaDefStruct
    {
        var ret = false;
        switch (custom)
        {
            case MaaCustomRecognizerApi recognizer:
                ret = MaaRegisterCustomRecognizer(_handle, name, ref recognizer, arg).ToBoolean();
                if (ret) _recognizers[name] = recognizer;
                return ret;
            case MaaCustomActionApi action:
                ret = MaaRegisterCustomAction(_handle, name, ref action, arg).ToBoolean();
                if (ret) _actions[name] = action;
                return ret;
            default:
                return ret;
        }
    }

    /// <summary>
    ///     Unregisters a <see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/> named <paramref name="name"/> in the <see cref="MaaInstance"/>.
    /// </summary>
    /// <param name="name">The name of recognizer.</param>
    /// <typeparam name="T"><see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/>.</typeparam>
    /// <returns>
    ///     true if the <see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/> was unregistered successfully; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaUnregisterCustomRecognizer"/> and <see cref="MaaUnregisterCustomAction"/>.
    /// </remarks>
    public bool Unregister<T>(string name) where T : IMaaDefStruct
    {
        var ret = false;
        switch (typeof(T).Name)
        {
            case nameof(MaaCustomRecognizerApi):
                ret = MaaUnregisterCustomRecognizer(_handle, name).ToBoolean();
                if (ret) _recognizers.Remove(name);
                return ret;
            case nameof(MaaCustomActionApi):
                ret = MaaUnregisterCustomAction(_handle, name).ToBoolean();
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
    /// <returns>
    ///     true if <see cref="MaaCustomRecognizerApi"/>s or <see cref="MaaCustomActionApi"/>s were cleared successfully; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaClearCustomRecognizer"/> and <see cref="MaaClearCustomAction"/>.
    /// </remarks>
    public bool Clear<T>() where T : IMaaDefStruct
    {
        var ret = false;
        switch (typeof(T).Name)
        {
            case nameof(MaaCustomRecognizerApi):
                ret = MaaClearCustomRecognizer(_handle).ToBoolean();
                if (ret) _recognizers.Clear();
                return ret;
            case nameof(MaaCustomActionApi):
                ret = MaaClearCustomAction(_handle).ToBoolean();
                if (ret) _actions.Clear();
                return ret;
            default:
                return ret;
        }
    }

    /// <summary>
    ///     Appends a async job of executing a maa task, could be called multiple times.
    /// </summary>
    /// <param name="taskEntryName">The name of task entry.</param>
    /// <param name="taskParam">The param of task, which could be parsed to a JSON.</param>
    /// <returns></returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaPostTask"/>.
    /// </remarks>
    public MaaJob AppendTask(string taskEntryName, string taskParam = MaaDef.EmptyMaaTaskParam)
    {
        var id = MaaPostTask(_handle, taskEntryName, taskParam);
        return new(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetTaskParam"/>.
    /// </remarks>
    public override bool SetParam(MaaJob job, string param)
        => MaaSetTaskParam(_handle, job, param).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskStatus"/>.
    /// </remarks>
    public override MaaJobStatus GetStatus(MaaJob job)
        => (MaaJobStatus)MaaTaskStatus(_handle, job);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaWaitTask"/>.
    /// </remarks>
    public override MaaJobStatus Wait(MaaJob job)
        => (MaaJobStatus)MaaWaitTask(_handle, job);

    /// <summary>
    ///     Gets whether the all maa tasks finished.
    /// </summary>
    /// <value>
    ///     true if all tasks finished; otherwise, false.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskAllFinished"/>.
    /// </remarks>
    public bool AllTasksFinished => MaaTaskAllFinished(_handle).ToBoolean();

    /// <summary>
    ///     Stops the binded <see cref="MaaResource"/>, the binded <see cref="MaaController"/>, all appended tasks. 
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStop"/>.
    /// </remarks>
    public bool Stop()
        => MaaStop(_handle).ToBoolean();
}
