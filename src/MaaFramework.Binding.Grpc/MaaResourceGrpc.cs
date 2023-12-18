using Grpc.Core;
using Grpc.Net.Client;
using MaaFramework.Binding.Grpc.Abstractions;
using MaaFramework.Binding.Grpc.Interop;
using static MaaFramework.Binding.Grpc.Interop.Resource;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Grpc.Interop.Resource"/>.
/// </summary>
public class MaaResourceGrpc : MaaCommonGrpc, IMaaResource<string>
{
    private ResourceClient _client = default!;

    /// <inheritdoc/>
    protected override void OnChannelChanged(GrpcChannel channel)
    {
        _client = new ResourceClient(channel);
    }

    /// <summary>
    ///     Converts a <see cref="IMaaResource{String}"/> instance to a <see cref="MaaResourceGrpc"/>.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="maaResource">The <see cref="IMaaResource{String}"/> instance.</param>
    public MaaResourceGrpc(GrpcChannel channel, IMaaResource<string> maaResource)
    {
        ArgumentNullException.ThrowIfNull(maaResource);
        Channel = channel;

        SetHandle(maaResource.Handle, needReleased: true);
    }

    /// <summary>
    ///     Creates a <see cref="MaaResourceGrpc"/> instance.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    public MaaResourceGrpc(GrpcChannel channel)
    {
        Channel = channel;

        var handle = _client.create(new IdRequest { Id = CallbackId }).Handle;
        SetHandle(handle, needReleased: true);
    }

    /// <inheritdoc cref="MaaResourceGrpc(GrpcChannel, CheckStatusOption, string[])"/>
    public MaaResourceGrpc(GrpcChannel channel, params string[] paths)
        : this(channel, CheckStatusOption.ThrowIfNotSuccess, paths)
    {
    }

    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="check">Checks AppendPath(path).Wait() status if true; otherwise, not checks.</param>
    /// <param name="paths">The paths of maa resource.</param>
    /// <exception cref="MaaJobStatusException" />
    /// <inheritdoc cref="MaaResourceGrpc(GrpcChannel)"/>
    public MaaResourceGrpc(GrpcChannel channel, CheckStatusOption check, params string[] paths)
        : this(channel)
    {
        ArgumentNullException.ThrowIfNull(paths);

        foreach (var path in paths)
        {
            var status = AppendPath(path).Wait();
            if (check == CheckStatusOption.ThrowIfNotSuccess)
            {
                status.ThrowIfNot(MaaJobStatus.Success, MaaJobStatusException.MaaResourceMessage);
            }
        }
    }

    /// <inheritdoc/>
    protected override void ReleaseHandle()
        => _client.destroy(new HandleRequest { Handle = Handle, });

    /// <inheritdoc/>
    public IMaaJob AppendPath(string resourcePath)
    {
        var id = _client.post_path(new HandleStringRequest { Handle = Handle, Str = resourcePath }).Id;
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Always return false.
    /// </remarks>
    public bool SetParam(IMaaJob job, string param)
        => false;

    /// <inheritdoc/>
    public MaaJobStatus GetStatus(IMaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);
        return (MaaJobStatus)_client.status(new HandleIIdRequest { Handle = Handle, Id = job.IId, }).Status;
    }

    /// <inheritdoc/>
    public MaaJobStatus Wait(IMaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);
        return (MaaJobStatus)_client.wait(new HandleIIdRequest { Handle = Handle, Id = job.IId, }).Status;
    }

    /// <inheritdoc/>
    public bool Loaded => _client.loaded(new HandleRequest { Handle = Handle, }).Bool;

    /// <inheritdoc/>
    public bool SetOption(ResourceOption opt, int value)
        => false;

    /// <inheritdoc/>
    public bool SetOption(ResourceOption opt, bool value)
        => false;

    /// <inheritdoc/>
    public bool SetOption(ResourceOption opt, string value)
        => false;

    /*
    /// <inheritdoc cref="SetOption(ResourceOption, int)"/>
    protected bool SetOption(SetResourceOptionRequest request)
    {
        try
        {
            _client.set_option(request);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }
    */

    /// <inheritdoc/>
    public string? Hash
    {
        get
        {
            try
            {
                return _client.hash(new HandleRequest { Handle = Handle, }).Str;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
            {
                return null;
            }
        }
    }
}
