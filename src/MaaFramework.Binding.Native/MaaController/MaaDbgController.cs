using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaDbgControllerCreate"/>.
/// </summary>
public class MaaDbgController : MaaController
{
    /// <summary>
    ///     Creates a <see cref="MaaDbgController"/> instance.
    /// </summary>
    /// <param name="readPath">The read path.</param>
    /// <param name="writePath">The write path.</param>
    /// <param name="type">The DebugControllerType.</param>
    /// <param name="config">The config.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDbgControllerCreate"/>.
    /// </remarks>
    public MaaDbgController(string readPath, string writePath, DbgControllerType type, string config)
    {
        var handle = MaaDbgControllerCreate(readPath, writePath, (MaaDbgControllerType)type, config, MaaNotificationCallback, nint.Zero);
        SetHandle(handle, needReleased: true);
    }
}
