using Grpc.Net.Client;

namespace MaaFramework.Binding.Grpc.Abstractions;

/// <summary>
///     An abstract class providing common members for GrpcChannel.
/// </summary>
public abstract class MaaGrpcChannel
{
    private GrpcChannel _channel = default!;

    /// <summary>
    ///     Gets or sets a channel to use to make remote calls.
    /// </summary>
    /// <remarks>
    ///     MaaRpc does not have a reconnect mechanism for active calls.
    /// </remarks>
    protected GrpcChannel Channel
    {
        get => _channel;
        set
        {
            if (_channel == value) return;
            _channel = value;
            OnChannelChanged(value);
        }
    }

    /// <summary>
    ///     Occurs when channel changed.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    protected abstract void OnChannelChanged(GrpcChannel channel);
}
