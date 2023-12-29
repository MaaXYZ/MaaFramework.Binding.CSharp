using MaaFramework.Binding.Native.Interop;
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
    /// <param name="address">The listening address. (ip:port)</param>
    /// <returns>true if the maa rpc server started successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRpcStart"/>.
    /// </remarks>
    public static bool Start(string address)
        => MaaRpcStart(address).ToBoolean();

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
