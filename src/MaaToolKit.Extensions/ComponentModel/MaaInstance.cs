using MaaToolKit.Enums;
using MaaToolKit.Extensions.Interfaces;
using MaaToolKit.Interop;
using static MaaToolKit.Interop.MaaApiWrapper;

namespace MaaToolKit.Extensions.ComponentModel;

/// <summary>
///     A class providing a reference implementation for Maa Controller section of <see cref="MaaApiWrapper"/>.
/// </summary>
public class MaaInstance : IMaaNotify, IMaaPost, IDisposable
{
    internal IntPtr _handle;

    private static readonly HashSet<MaaInstance> _instances = new HashSet<MaaInstance>();
    internal static MaaInstance GetMaaInstance(IntPtr handle) => _instances.First(x => x._handle == handle);

    /// <inheritdoc/>
    public event MaaCallback? Notify;

    /// <inheritdoc/>
    public void OnNotify(string msg, string detailsJson, IntPtr identifier)
    {
        Notify?.Invoke(msg, detailsJson, identifier);
    }

    /// <summary>
    ///     Creates a <see cref="MaaInstance"/> instance.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaInstanceCreate"/>.
    /// </remarks>
    public MaaInstance()
    {
        _handle = MaaInstanceCreate(OnNotify, IntPtr.Zero);
        _instances.Add(this);
    }

    /// <summary>
    ///     Creates a <see cref="MaaInstance"/> instance.
    /// </summary>
    /// <param name="identifier"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaInstanceCreate"/>.
    /// </remarks>
    public MaaInstance(IntPtr identifier)
    {
        _handle = MaaInstanceCreate(OnNotify, identifier);
        _instances.Add(this);
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
    ///     Wrapper of <see cref="MaaInstanceDestroy"/>.
    /// </remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            MaaInstanceDestroy(_handle);
            _handle = IntPtr.Zero;
        }
    }

    /// <summary>
    ///     Binds a <see cref="MaaResource"/> to the <see cref="MaaInstance"/>.
    /// </summary>
    /// <param name="resource">The <see cref="MaaResource"/>.</param>
    /// <returns>
    ///     true if the <see cref="MaaResource"/> was successfully binded; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaBindResource"/>.
    /// </remarks>
    public bool BindResource(MaaResource resource)
        => MaaBindResource(_handle, resource._handle);

    /// <summary>
    ///     Binds a <see cref="MaaController"/> to the <see cref="MaaInstance"/>.
    /// </summary>
    /// <param name="controller">The <see cref="MaaController"/>.</param>
    /// <returns>
    ///     true if the <see cref="MaaController"/> was successfully binded; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaBindController"/>.
    /// </remarks>
    public bool BindController(MaaController controller)
        => MaaBindController(_handle, controller._handle);

    /// <summary>
    ///     Gets whether the <see cref="MaaInstance"/> is fully initialized.
    /// </summary>
    /// <value>
    ///     true if the <see cref="MaaInstance"/> is fully initialized; otherwise, false.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaInitialized"/>.
    /// </remarks>
    public bool Initialized => MaaInitialized(_handle);

    /// <summary>
    ///     Appends a async job of executing a maa task, could be called multiple times.
    /// </summary>
    /// <param name="taskName">The name of task.</param>
    /// <param name="taskParam">The param of task, which could be parsed to a JSON.</param>
    /// <returns></returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaInstancePostTask"/>.
    /// </remarks>
    public MaaJob AppendTask(string taskName, string taskParam = "{}")
    {
        var id = MaaInstancePostTask(_handle, taskName, taskParam);
        return new(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetTaskParam"/>.
    /// </remarks>
    public bool SetParam(MaaJob job, string param)
        => MaaSetTaskParam(_handle, job, param);

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
    public bool AllTasksFinished => MaaTaskAllFinished(_handle);

    /// <summary>
    ///     Stops the binded <see cref="MaaResource"/>, the binded <see cref="MaaController"/>, all appended tasks. 
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStop"/>.
    /// </remarks>
    public void Stop()
        => MaaStop(_handle);

    /// <summary>
    ///     Gets the <see cref="MaaResource"/> binded to this <see cref="MaaInstance"/>.
    /// </summary>
    /// <returns>The <see cref="MaaResource"/>.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetResource"/>.
    /// </remarks>
    public MaaResource GetBindedResource()
        => MaaResource.GetMaaResource(MaaGetResource(_handle));

    /// <summary>
    ///     Gets the <see cref="MaaController"/> binded to this <see cref="MaaInstance"/>.
    /// </summary>
    /// <returns>The <see cref="MaaController"/>.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetController"/>.
    /// </remarks>
    public MaaController GetBindedController()
        => MaaController.GetMaaController(MaaGetController(_handle));
}
