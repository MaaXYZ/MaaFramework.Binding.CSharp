using Grpc.Net.Client;
using MaaFramework.Binding.Interop.Grpc;
using static MaaFramework.Binding.Interop.Grpc.Controller;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for MaaAdbControllerGrpc.
/// </summary>
public class MaaAdbControllerGrpc : MaaControllerGrpc
{
    /// <inheritdoc cref="MaaAdbControllerGrpc(GrpcChannel, string, string, AdbControllerTypes, string, string, LinkOption)"/>
    public MaaAdbControllerGrpc(GrpcChannel channel, string adbPath, string address, AdbControllerTypes type, string adbConfig, string agentPath)
        : this(channel, adbPath, address, type, adbConfig, agentPath, LinkOption.Start)
    {
    }

    /// <inheritdoc cref="MaaAdbControllerGrpc(GrpcChannel, string, string, AdbControllerTypes, string, string, CheckStatusOption, LinkOption)"/>
    public MaaAdbControllerGrpc(GrpcChannel channel, string adbPath, string address, AdbControllerTypes type, string adbConfig, string agentPath, LinkOption link)
        : this(channel, adbPath, address, type, adbConfig, agentPath, CheckStatusOption.ThrowIfNotSuccess, link)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaAdbControllerGrpc"/> instance.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="adbPath">The path of adb executable file.</param>
    /// <param name="address">The device address.</param>
    /// <param name="type">The AdbControllerTypes including touch type, key type and screencap type.</param>
    /// <param name="adbConfig">The path of adb config file.</param>
    /// <param name="agentPath">The path of agent directory.</param>
    /// <param name="check">Checks LinkStart().Wait() status if true; otherwise, not check.</param>
    /// <param name="link">Executes <see cref="MaaControllerGrpc.LinkStart"/> if true; otherwise, not link.</param>
    /// <exception cref="ArgumentException" />
    /// <exception cref="MaaJobStatusException" />
    public MaaAdbControllerGrpc(GrpcChannel channel, string adbPath, string address, AdbControllerTypes type, string adbConfig, string agentPath, CheckStatusOption check, LinkOption link)
        : base(channel)
    {
        ArgumentException.ThrowIfNullOrEmpty(adbPath);
        ArgumentException.ThrowIfNullOrEmpty(address);
        type.Check();
        ArgumentException.ThrowIfNullOrEmpty(adbConfig);
        ArgumentException.ThrowIfNullOrEmpty(agentPath);

        var client = new ControllerClient(channel);
        var handle = client.create_adb(new AdbControllerRequest
        {
            Id = CallbackId,
            AdbPath = adbPath,
            AdbSerial = address,
            AdbType = (uint)type,
            AdbConfig = adbConfig,
            AgentPath = agentPath,
        }).Handle;
        SetHandle(handle, needReleased: true);

        if (link == LinkOption.Start)
        {
            var status = LinkStart().Wait();
            if (check == CheckStatusOption.ThrowIfNotSuccess)
            {
                status.ThrowIfNot(MaaJobStatus.Success, MaaJobStatusException.MaaControllerMessage);
            }
        }
    }
}
