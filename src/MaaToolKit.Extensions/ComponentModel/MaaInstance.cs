using MaaToolKit.Extensions.Enums;
using MaaToolKit.Extensions.Interfaces;
using MaaToolKit.Extensions.Interop;
using System.Runtime.InteropServices;
using static MaaToolKit.Extensions.Interop.MaaApi;

namespace MaaToolKit.Extensions.ComponentModel;

/// <summary>
///     A class providing a reference implementation for Maa Controller section of <see cref="MaaDefConverter"/>.
/// </summary>
public class MaaInstance : IMaaNotify, IMaaPost, IDisposable
{
    internal MaaInstanceHandle _handle;
    private bool disposed;

    /// <inheritdoc/>
    public event IMaaNotify.MaaCallback? Callback;
#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables
    private readonly MaaInstanceCallback _callback;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables

    /// <summary>
    ///     Creates a <see cref="MaaInstance"/> instance.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreate"/>.
    /// </remarks>
    public MaaInstance() : this(MaaCallbackTransparentArg.Zero)
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
        _callback = (msg, detail, arg) => Callback?.Invoke(
            Marshal.PtrToStringUTF8(msg) ?? string.Empty,
            Marshal.PtrToStringUTF8(detail) ?? "{}",
            arg);
        _handle = MaaCreate(_callback, maaCallbackTransparentArg);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Disposes the <see cref="MaaInstance"/> instance.
    /// </summary>
    /// <param name="disposing"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDestroy"/>.
    /// </remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            // if (disposing) Dispose managed resources.
            MaaDestroy(_handle);
            _handle = MaaInstanceHandle.Zero;
            disposed = true;
        }
    }

#pragma warning disable S1144 // Unused private types or members should be removed
    private bool SetOption(InstanceOption option, MaaOptionValue[] values)
     => MaaControllerSetOption(_handle, (MaaInstOption)option, ref values[0], (MaaOptionValueSize)values.Length).ToBoolean();
#pragma warning restore S1144 // Unused private types or members should be removed

    /// <summary>
    ///     Binds a <see cref="MaaResource"/> to the <see cref="MaaInstance"/>.
    /// </summary>
    /// <param name="resource">The <see cref="MaaResource"/>.</param>
    /// <returns>
    ///     true if the <see cref="MaaResource"/> was binded successfully; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaBindResource"/>.
    /// </remarks>
    public bool BindResource(MaaResource resource)
        => MaaBindResource(_handle, resource._handle).ToBoolean();

    /// <summary>
    ///     Binds a <see cref="MaaController"/> to the <see cref="MaaInstance"/>.
    /// </summary>
    /// <param name="controller">The <see cref="MaaController"/>.</param>
    /// <returns>
    ///     true if the <see cref="MaaController"/> was binded successfully; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaBindController"/>.
    /// </remarks>
    public bool BindController(MaaController controller)
        => MaaBindController(_handle, controller._handle).ToBoolean();

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
    /// <typeparam name="T"><see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/>.</typeparam>
    /// <returns>
    ///     true if the <see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/> was registered successfully; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRegisterCustomRecognizer"/> and <see cref="MaaRegisterCustomAction"/>.
    /// </remarks>
    public bool Register<T>(string name, T custom) where T : IMaaDefStruct
    {
        var ret = false;
        if (custom is MaaCustomRecognizerApi recognizer)
        {
            ret = MaaRegisterCustomRecognizer(_handle, name, ref recognizer).ToBoolean();
            if (ret)
                _recognizers[name] = recognizer;
        }
        else if (custom is MaaCustomActionApi action)
        {
            ret = MaaRegisterCustomAction(_handle, name, ref action).ToBoolean();
            if (ret)
                _actions[name] = action;
        }

        return ret;
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
        var t = typeof(T);
        if (t == typeof(MaaCustomRecognizerApi))
        {
            ret = MaaUnregisterCustomRecognizer(_handle, name).ToBoolean();
            if (ret)
                _recognizers.Remove(name);
        }
        else if (t == typeof(MaaCustomActionApi))
        {
            ret = MaaUnregisterCustomAction(_handle, name).ToBoolean();
            if (ret)
                _actions.Remove(name);
        }

        return ret;
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
        var t = typeof(T);
        if (t == typeof(MaaCustomRecognizerApi))
        {
            ret = MaaClearCustomRecognizer(_handle).ToBoolean();
            if (ret)
                _recognizers.Clear();
        }
        else if (t == typeof(MaaCustomActionApi))
        {
            ret = MaaClearCustomAction(_handle).ToBoolean();
            if (ret)
                _actions.Clear();
        }

        return ret;
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
    public bool SetParam(MaaJob job, string param)
        => MaaSetTaskParam(_handle, job, param).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaTaskStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(MaaJob job)
        => (MaaJobStatus)MaaTaskStatus(_handle, job);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaWaitTask"/>.
    /// </remarks>
    public MaaJobStatus Wait(MaaJob job)
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

    /// <summary>
    ///     Gets the <see cref="MaaResource"/> binded to this <see cref="MaaInstance"/>.
    /// </summary>
    /// <returns>The <see cref="MaaResource"/>.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetResource"/>.
    /// </remarks>
    public MaaResource GetBindedResource()
        => MaaResource.Get(MaaGetResource(_handle));

    /// <summary>
    ///     Gets the <see cref="MaaController"/> binded to this <see cref="MaaInstance"/>.
    /// </summary>
    /// <returns>The <see cref="MaaController"/>.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetController"/>.
    /// </remarks>
    public MaaController GetBindedController()
        => MaaController.Get(MaaGetController(_handle));
}
