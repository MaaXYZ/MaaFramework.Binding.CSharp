using static MaaFramework.Binding.Native.Interop.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaThriftControllerCreate"/>.
/// </summary>
public class MaaThriftController : MaaController
{
    /// <summary>
    ///     Creates a <see cref="MaaThriftController"/> instance.
    /// </summary>
    /// <param name="type">The ThriftControllerType.</param>
    /// <param name="host">The host.</param>
    /// <param name="port">The port.</param>
    /// <param name="config">The config.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaThriftControllerCreate"/>.
    /// </remarks>
    public MaaThriftController(ThriftControllerType type, string host, int port, string config)
    {
        var handle = MaaThriftControllerCreate((MaaThriftControllerType)type, host, port, config, MaaApiCallback, nint.Zero);
        SetHandle(handle, needReleased: true);
    }
}
