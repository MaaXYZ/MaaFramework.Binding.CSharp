using Grpc.Net.Client;

namespace MaaFramework.Binding;

/// <summary>
///     A static class providing extension methods for the creation of MaaAdbControllerGrpc.
/// </summary>
public static class DeviceInfoExtension
{
    /// <summary>
    ///     Converts a <see cref="DeviceInfo"/> to a <see cref="MaaAdbControllerGrpc"/>.
    /// </summary>
    /// <param name="info">The DeviceInfo.</param>
    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="agentPath">The new agent path.</param>
    /// <param name="adbPath">The new adb path.</param>
    /// <param name="address">The new address.</param>
    /// <param name="types">The new AdbControllerTypes.</param>
    /// <param name="adbConfig">The new adb config.</param>
    /// <param name="link">Executes <see cref="MaaControllerGrpc.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSuccess"/>; otherwise, not check.</param>
    /// <returns>A MaaAdbController.</returns>
    public static MaaAdbControllerGrpc ToAdbControllerGrpc(this DeviceInfo info,
        GrpcChannel channel,
        string agentPath,
        string? adbPath = null,
        string? address = null,
        AdbControllerTypes? types = null,
        string? adbConfig = null,
        LinkOption link = LinkOption.Start,
        CheckStatusOption check = CheckStatusOption.ThrowIfNotSuccess)
    {
        ArgumentNullException.ThrowIfNull(info);

        return new(channel,
                   adbPath ?? info.AdbPath,
                   address ?? info.AdbSerial,
                   types ?? info.AdbTypes,
                   adbConfig ?? info.AdbConfig,
                   agentPath,
                   link,
                   check);
    }
}
