using Grpc.Core;
using Grpc.Net.Client;
using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Grpc.Abstractions;

/// <summary>
///     An abstract class providing common members for <see cref="MaaControllerGrpc"/>, <see cref="MaaInstanceGrpc"/> and <see cref="MaaResourceGrpc"/>.
/// </summary>
public abstract class MaaCommonGrpc : MaaDisposableHandle<string>, IMaaCommon
{
    /// <inheritdoc/>
    public event EventHandler<MaaCallbackEventArgs>? Callback;

    /// <summary>
    ///     Raises the Callback event.
    /// </summary>
    /// <param name="msg">The MaaStringView.</param>
    /// <param name="detail">The MaaStringView.</param>
    /// <param name="arg">The MaaCallbackTransparentArg.</param>
    /// <remarks>
    ///     Usually invoked by MaaFramework.
    /// </remarks>
    protected virtual void OnCallback(string msg, string detail, nint arg)
        => Callback?.Invoke(this, new MaaCallbackEventArgs(msg, detail, arg));

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (IsValidCallbackId)
        {
            var id = CallbackId;
            CallbackId = string.Empty;
            MaaUtilityGrpc.UnregisterCallback(Channel, id);
        }
    }

    /// <summary>
    ///     Initializes MaaCommonGrpc.
    /// </summary>
    protected MaaCommonGrpc(GrpcChannel channel)
        : base(invalidHandleValue: "")
    {
        Channel = channel;
        CallbackId = RegisterCallback();
    }

    /// <summary>
    ///     Gets the callback id.
    /// </summary>
    protected string CallbackId { get; private set; }
    private bool IsValidCallbackId => !string.IsNullOrEmpty(CallbackId);

    private string RegisterCallback()
    {
        if (!MaaUtilityGrpc.RegisterCallback(Channel, out var id, out var streamingCall))
            throw new InvalidOperationException("Failed to register callback.");

        Task.Run(async () =>
        {
            await foreach (var response in streamingCall.ResponseStream.ReadAllAsync())
            {
                OnCallback(response.Msg, response.Detail, nint.Zero);
            }

            streamingCall.Dispose();
            if (IsValidCallbackId)
                throw new TaskCanceledException();
        });

        CallbackId = id;
        return id;
    }

#pragma warning disable CA2213 // 应释放可释放的字段
    private GrpcChannel _channel = default!;
#pragma warning restore CA2213 // 应释放可释放的字段

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
