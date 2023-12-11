using static MaaFramework.Binding.Native.Interop.MaaRpc;

namespace MaaFramework.Binding;

/// <summary>
///     A static wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Native.Interop.MaaRpc"/>.
/// </summary>

public static class MaaRpc
{
    /// <summary>
    ///     Starts Maa Grpc server.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRpcStart"/>.
    /// </remarks>
    public static void Start(string address)
        => MaaRpcStart(address);

    /// <summary>
    ///     Stops Maa Grpc server.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRpcStop"/>.
    /// </remarks>
    public static void Stop()
        => MaaRpcStop();

    /// <summary>
    ///     Blocks Maa Grpc server.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRpcWait"/>.
    /// </remarks>
    public static void Wait()
        => MaaRpcWait();
}
