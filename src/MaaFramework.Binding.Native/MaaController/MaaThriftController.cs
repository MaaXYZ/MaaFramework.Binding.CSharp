using static MaaFramework.Binding.Native.Interop.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaThriftControllerCreate"/>.
/// </summary>
public class MaaThriftController : MaaController
{
    /// <inheritdoc cref="MaaThriftController(ThriftControllerType, string, int, string, MaaCallbackTransparentArg)"/>
    public MaaThriftController(ThriftControllerType type, string host, int port, string config)
        : this(type, host, port, config, MaaCallbackTransparentArg.Zero)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaThriftController"/> instance.
    /// </summary>
    /// <param name="type">The ThriftControllerType.</param>
    /// <param name="host">The host.</param>
    /// <param name="port">The port.</param>
    /// <param name="config">The config.</param>
    /// <param name="maaCallbackTransparentArg">The MaaCallbackTransparentArg.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaThriftControllerCreate"/>.
    /// </remarks>
    public MaaThriftController(ThriftControllerType type, string host, int port, string config, MaaCallbackTransparentArg maaCallbackTransparentArg)
    {
        var handle = MaaThriftControllerCreate((MaaThriftControllerType)type, host, port, config, MaaApiCallback, maaCallbackTransparentArg);
        SetHandle(handle, needReleased: true);
    }
}
