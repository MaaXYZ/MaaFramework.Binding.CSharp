using Grpc.Net.Client;

namespace MaaFramework.Binding;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

/// <summary>
///     A static class providing extension methods for the creation of MaaController.
/// </summary>
public static class DeviceInfoExtension
{
    public static MaaAdbControllerGrpc ToAdbControllerGrpc(this DeviceInfo info,
        GrpcChannel grpcChannel,
        string agentPath,
        string? adbPath = null,
        string? address = null,
        AdbControllerTypes? type = null,
        string? adbConfig = null,
        CheckStatusOption check = CheckStatusOption.ThrowIfNotSuccess,
        LinkOption link = LinkOption.Start)
    {
        ArgumentNullException.ThrowIfNull(info);

        return new(grpcChannel,
                   adbPath ?? info.AdbPath,
                   address ?? info.AdbSerial,
                   type ?? info.AdbTypes,
                   adbConfig ?? info.AdbConfig,
                   agentPath,
                   check,
                   link);
    }
}
