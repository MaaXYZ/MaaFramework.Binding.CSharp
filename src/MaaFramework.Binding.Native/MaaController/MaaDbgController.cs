using static MaaFramework.Binding.Native.Interop.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaDbgControllerCreate"/>.
/// </summary>
public class MaaDbgController : MaaController
{
    /// <inheritdoc cref="MaaDbgController(string, string, DbgControllerType, string, MaaCallbackTransparentArg)"/>
    public MaaDbgController(string readPath, string writePath, DbgControllerType type, string config)
       : this(readPath, writePath, type, config, MaaCallbackTransparentArg.Zero)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaDbgController"/> instance.
    /// </summary>
    /// <param name="readPath">The read path.</param>
    /// <param name="writePath">The write path.</param>
    /// <param name="type">The DebugControllerType.</param>
    /// <param name="config">The config.</param>
    /// <param name="maaCallbackTransparentArg">The MaaCallbackTransparentArg.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDbgControllerCreate"/>.
    /// </remarks>
    public MaaDbgController(string readPath, string writePath, DbgControllerType type, string config, MaaCallbackTransparentArg maaCallbackTransparentArg)
    {
        var handle = MaaDbgControllerCreate(readPath, writePath, (MaaDbgControllerType)type, config, MaaApiCallback, maaCallbackTransparentArg);
        SetHandle(handle, needReleased: true);
    }
}
