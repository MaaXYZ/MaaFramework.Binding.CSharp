using Grpc.Core;
using Grpc.Net.Client;
using MaaFramework.Binding.Grpc.Abstractions;
using MaaFramework.Binding.Grpc.Interop;
using static MaaFramework.Binding.Grpc.Interop.Config;
using static MaaFramework.Binding.Grpc.Interop.Device;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Grpc.Interop.Config"/> and <see cref="MaaFramework.Binding.Grpc.Interop.Device"/>.
/// </summary>
public class MaaToolKitGrpc : MaaGrpcChannel, IMaaToolkit
{
    private ConfigClient _configClient = default!;
    private DeviceClient _deviceClient = default!;

    /// <inheritdoc/>
    protected override void OnChannelChanged(GrpcChannel channel)
    {
        _configClient = new ConfigClient(channel);
        _deviceClient = new DeviceClient(channel);
    }

    /// <summary>
    ///     Creates a <see cref="MaaToolKitGrpc"/> instance.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    public MaaToolKitGrpc(GrpcChannel channel)
    {
        Channel = channel;
    }

    /// <inheritdoc/>
    public bool Init()
    {
        try
        {
            _configClient.init(new EmptyRequest());
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool Uninit()
    {
        try
        {
            _configClient.uninit(new EmptyRequest());
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public DeviceInfo[] Find(string adbPath = "")
        => _deviceClient.find(new EmptyRequest()).Info
        .Select(device => new DeviceInfo
        {
            Name = device.Name,
            AdbConfig = device.AdbConfig,
            AdbPath = device.AdbPath,
            AdbSerial = device.AdbSerial,
            AdbTypes = (AdbControllerTypes)device.AdbType,
        })
        .ToArray();
}
