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
    /// <inheritdoc/>
    protected override void OnChannelChanged(GrpcChannel channel)
    {
        var configGrpcChannel = Config as MaaGrpcChannel;
        var deviceGrpcChannel = Device as MaaGrpcChannel;

        if (configGrpcChannel is not null)
            configGrpcChannel.Channel = channel;
        if (deviceGrpcChannel is not null)
            deviceGrpcChannel.Channel = channel;
    }

    /// <summary>
    ///     Creates a <see cref="MaaToolkitGrpc"/> instance.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="init">Whether invokes the <see cref="IMaaToolkitConfig.Init"/>.</param>
    public MaaToolkitGrpc(GrpcChannel channel, bool init = false)
        : base(channel)
    {
        Config = new ConfigClass(channel);
        Device = new DeviceClass(channel);

        if (init)
        {
            Config.Init();
        }
    }

    /// <inheritdoc/>
    public IMaaToolkitConfig Config { get; set; }

    /// <inheritdoc/>
    public IMaaToolkitDevice Device { get; set; }

    /// <inheritdoc/>
    public IMaaToolkitExecAgent ExecAgent { get => _execAgent is null ? throw new NotSupportedException() : _execAgent; set => _execAgent = value; }

    /// <inheritdoc/>
    public IMaaToolkitWin32 Win32 { get => _win32 is null ? throw new NotSupportedException() : _win32; set => _win32 = value; }

    /// <inheritdoc cref="MaaToolkitGrpc"/>
    /// <inheritdoc cref="MaaToolkitGrpc.MaaToolkitGrpc"/>
    protected class ConfigClass(GrpcChannel channel) : MaaGrpcChannel(channel), IMaaToolkitConfig
    {
        private ConfigClient _configClient = default!;

        /// <inheritdoc/>
        protected override void OnChannelChanged(GrpcChannel channel)
        {
            _configClient = new ConfigClient(channel);
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
    }

    /// <inheritdoc cref="MaaToolkitGrpc"/>
    /// <inheritdoc cref="MaaToolkitGrpc.MaaToolkitGrpc"/>
    protected class DeviceClass(GrpcChannel channel) : MaaGrpcChannel(channel), IMaaToolkitDevice
    {
        private DeviceClient _deviceClient = default!;

        /// <inheritdoc/>
        protected override void OnChannelChanged(GrpcChannel channel)
        {
            _deviceClient = new DeviceClient(channel);
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

    private IMaaToolkitExecAgent _execAgent = default!;
    private IMaaToolkitWin32 _win32 = default!;
}
