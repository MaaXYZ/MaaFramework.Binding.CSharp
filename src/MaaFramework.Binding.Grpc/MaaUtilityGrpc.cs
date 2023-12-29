using Grpc.Core;
using Grpc.Net.Client;
using MaaFramework.Binding.Grpc.Abstractions;
using MaaFramework.Binding.Grpc.Interop;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Grpc.Interop.Utility;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Grpc.Interop.Utility"/>.
/// </summary>
public class MaaUtilityGrpc : MaaGrpcChannel, IMaaUtility
{
    private UtilityClient _client = default!;

    /// <inheritdoc/>
    protected override void OnChannelChanged(GrpcChannel channel)
    {
        _client = new UtilityClient(channel);
    }

    /// <summary>
    ///     Creates a <see cref="MaaUtilityGrpc"/> instance.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    public MaaUtilityGrpc(GrpcChannel channel)
        : base(channel)
    {
    }

    /// <inheritdoc/>
    public string Version => _client.version(new EmptyRequest()).Str;

    /// <inheritdoc/>
    public bool SetOption<T>(GlobalOption opt, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        SetGlobalOptionRequest request = opt switch
        {
            GlobalOption.Invalid => throw new InvalidOperationException(),
            GlobalOption.LogDir => value switch { string v => new() { LogDir = v }, _ => throw new InvalidOperationException(), },
            GlobalOption.SaveDraw => value switch { bool v => new() { SaveDraw = v }, _ => throw new InvalidOperationException(), },
            GlobalOption.Recording => value switch { bool v => new() { SaveDraw = v }, _ => throw new InvalidOperationException(), },
            GlobalOption.ShowHitDraw => value switch { bool v => new() { SaveDraw = v }, _ => throw new InvalidOperationException(), },
            GlobalOption.StdoutLevel => value switch
            {
                LoggingLevel v => new() { StdoutLevel = (int)v },
                int v => new() { StdoutLevel = v },
                _ => throw new InvalidOperationException(),
            },
            _ => throw new NotImplementedException(),
        };

        try
        {
            _client.set_global_option(request);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Registers a callback.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="callbackId">The acquired id.</param>
    /// <param name="streamingCall">The callback streaming.</param>
    /// <returns>true if the callback was registered successfully; otherwise, false.</returns>
    public static bool RegisterCallback(GrpcChannel channel, out string callbackId, [NotNullWhen(true)] out AsyncDuplexStreamingCall<CallbackRequest, Callback>? streamingCall)
    {
        var client = new UtilityClient(channel);
        callbackId = client.acquire_id(new EmptyRequest()).Id;
        try
        {
            streamingCall = client.register_callback();
            return true;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
        {
            streamingCall = null;
            return false;
        }
    }

    /// <inheritdoc cref="RegisterCallback(GrpcChannel, out string, out AsyncDuplexStreamingCall{CallbackRequest, Callback}?)"/>
    public bool RegisterCallback(out string callbackId, [NotNullWhen(true)] out AsyncDuplexStreamingCall<CallbackRequest, Callback>? streamingCall)
    {
        callbackId = _client.acquire_id(new EmptyRequest()).Id;
        try
        {
            streamingCall = _client.register_callback();
            return true;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
        {
            streamingCall = null;
            return false;
        }
    }

    /// <summary>
    ///     Unregisters a callback.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="id">The id registered by callback.</param>
    /// <returns>true if the callback was unregistered successfully; otherwise, false.</returns>
    public static bool UnregisterCallback(GrpcChannel channel, string id)
    {
        try
        {
            new UtilityClient(channel).unregister_callback(new IdRequest { Id = id });
            return true;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return false;
        }
    }

    /// <inheritdoc cref="UnregisterCallback(GrpcChannel, string)"/>
    public bool UnregisterCallback(string id)
    {
        try
        {
            _client.unregister_callback(new IdRequest { Id = id });
            return true;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return false;
        }
    }
}
