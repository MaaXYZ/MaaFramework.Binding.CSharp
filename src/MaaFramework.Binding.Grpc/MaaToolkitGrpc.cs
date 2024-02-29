using Grpc.Core;
using Grpc.Net.Client;
using MaaFramework.Binding.Abstractions.Grpc;
using MaaFramework.Binding.Interop.Grpc;
using static MaaFramework.Binding.Interop.Grpc.Config;
using static MaaFramework.Binding.Interop.Grpc.Device;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Grpc.Config"/> and <see cref="MaaFramework.Binding.Interop.Grpc.Device"/>.
/// </summary>
public class MaaToolkitGrpc : MaaGrpcChannel, IMaaToolkit
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
    ///     Creates a <see cref="MaaToolkitGrpc"/> instance.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="init">Whether invokes the <see cref="IMaaToolkit.Init"/>.</param>
    public MaaToolkitGrpc(GrpcChannel channel, bool init = false)
        : base(channel)
    {
        if (init)
        {
            Init();
        }
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

    /// <inheritdoc/>
    public async Task<DeviceInfo[]> FindAsync(string adbPath = "")
    {
        var response = await _deviceClient.findAsync(new EmptyRequest());
        return response.Info
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
}
