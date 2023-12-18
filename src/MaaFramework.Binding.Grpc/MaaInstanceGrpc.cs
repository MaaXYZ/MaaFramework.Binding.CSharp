using Grpc.Core;
using Grpc.Net.Client;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Grpc.Abstractions;
using MaaFramework.Binding.Grpc.Interop;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Grpc.Interop.Instance;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Grpc.Interop.Instance"/>.
/// </summary>
public class MaaInstanceGrpc : MaaCommonGrpc, IMaaInstance<string>
{
    private IMaaResource<string> _resource = default!;
    private IMaaController<string> _controller = default!;
    private InstanceClient _client = default!;

    /// <inheritdoc/>
    protected override void OnChannelChanged(GrpcChannel channel)
    {
        _client = new InstanceClient(channel);
    }

    /// <summary>
    ///     Converts a <see cref="IMaaInstance{String}"/> instance to a <see cref="MaaInstanceGrpc"/>.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="maaInstance">The <see cref="IMaaInstance{String}"/> instance.</param>
    /// <exception cref="ArgumentNullException"/>
    [SetsRequiredMembers]
    public MaaInstanceGrpc(GrpcChannel channel, IMaaInstance<string> maaInstance)
    {
        ArgumentNullException.ThrowIfNull(maaInstance);
        Channel = channel;

        _resource ??= new MaaResourceGrpc(channel, maaInstance.Resource);
        _controller ??= new MaaControllerGrpc(channel, maaInstance.Controller);

        if (Resource == null || Controller == null)
            throw new ArgumentNullException(nameof(maaInstance), "Resource and Controller cannot be null");
    }

    /// <summary>
    ///     Creates a <see cref="MaaInstanceGrpc"/> instance.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    public MaaInstanceGrpc(GrpcChannel channel)
    {
        Channel = channel;

        var handle = _client.create(new IdRequest { Id = CallbackId }).Handle;
        SetHandle(handle, needReleased: true);
    }

    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="controller">The controller.</param>
    /// <param name="disposeOptions">The dispose options.</param>
    /// <inheritdoc cref="MaaInstanceGrpc(GrpcChannel)"/>
    [SetsRequiredMembers]
    public MaaInstanceGrpc(GrpcChannel channel, IMaaResource<string> resource, IMaaController<string> controller, DisposeOptions disposeOptions)
        : this(channel)
    {
        // Channel = channel;
        Resource = resource;
        Controller = controller;
        DisposeOptions = disposeOptions;
    }

    /// <summary>
    ///     Whether to dispose the <see cref="Resource"/> and the <see cref="Controller"/> when <see cref="IDisposable.Dispose"/> was invoked.
    /// </summary>
    public required DisposeOptions DisposeOptions { get; set; }

    /// <inheritdoc/>
    protected override void ReleaseHandle()
    {
        _client.destroy(new HandleRequest { Handle = Handle, });

        if (DisposeOptions.HasFlag(DisposeOptions.Controller))
        {
            Controller.Dispose();
        }

        if (DisposeOptions.HasFlag(DisposeOptions.Resource))
        {
            Resource.Dispose();
        }
    }

    /// <inheritdoc/>
    public bool SetOption(InstanceOption opt, int value)
        => false;

    /// <inheritdoc/>
    public bool SetOption(InstanceOption opt, bool value)
        => false;

    /// <inheritdoc/>
    public bool SetOption(InstanceOption opt, string value)
        => false;

    /*
    /// <inheritdoc cref="SetOption(InstanceOption, int)"/>
    protected bool SetOption(SetInstanceOptionRequest request)
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
    IMaaResource IMaaInstance.Resource => Resource;

    /// <inheritdoc/>
    IMaaController IMaaInstance.Controller => Controller;

    /// <inheritdoc/>
    public required IMaaResource<string> Resource
    {
        get
        {
            MaaBindException.ThrowIf(
                _client.resource(new HandleRequest { Handle = Handle, }).Handle != _resource.Handle,
                MaaBindException.ResourceModifiedMessage);
            return _resource;
        }
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            try
            {
                _client.bind_resource(new HandleHandleRequest { Handle = Handle, AnotherHandle = value.Handle, });
                _resource = value;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
            {
                throw new MaaBindException(MaaBindException.ResourceMessage, ex);
            }
        }
    }

    /// <inheritdoc/>
    public required IMaaController<string> Controller
    {
        get
        {
            MaaBindException.ThrowIf(
                _client.controller(new HandleRequest { Handle = Handle, }).Handle != _controller.Handle,
                MaaBindException.ControllerModifiedMessage);
            return _controller;
        }
        init
        {
            ArgumentNullException.ThrowIfNull(value);

            try
            {
                _client.bind_controller(new HandleHandleRequest { Handle = Handle, AnotherHandle = value.Handle, });
                _controller = value;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
            {
                throw new MaaBindException(MaaBindException.ControllerMessage, ex);
            }
        }
    }

    /// <inheritdoc/>
    public bool Initialized => _client.inited(new HandleRequest { Handle = Handle, }).Bool;

    /// <summary>
    ///     Registers a <see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/> named <paramref name="name"/> in the <see cref="MaaInstanceGrpc"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/>.</typeparam>
    /// <inheritdoc/>
    public bool Register<T>(string name, T custom, nint arg) where T : IMaaDef
    {
        switch (custom)
        {
            case MaaCustomRecognizerApi recognizer:
                var streamingCallRecognizer = _client.register_custom_recognizer();
                Task.Run(() => CallCustomRecognizer(recognizer, streamingCallRecognizer, arg));
                RegisterCustomRecognizer(name, streamingCallRecognizer).Wait();
                return true;
            case MaaCustomActionApi action:
                var streamingCallAction = _client.register_custom_action();
                Task.Run(() => CallCustomAction(action, streamingCallAction, arg));
                RegisterCustomAction(name, streamingCallAction).Wait();
                return true;
            default:
                return false;
        }
    }

    private async Task RegisterCustomRecognizer(
        string name,
        AsyncDuplexStreamingCall<CustomRecognizerRequest, CustomRecognizerResponse> streamingCall)
    {
        await streamingCall.RequestStream.WriteAsync(new CustomRecognizerRequest
        {
            Ok = true,
            Init = new CustomRecognizerInit { Handle = Handle, Name = name, }
        });
        await streamingCall.RequestStream.CompleteAsync();
    }

    private static async Task CallCustomRecognizer(
        MaaCustomRecognizerApi recognizer,
        AsyncDuplexStreamingCall<CustomRecognizerRequest, CustomRecognizerResponse> streamingCall,
        nint arg)
    {
        await foreach (var response in streamingCall.ResponseStream.ReadAllAsync())
        {
            switch (response.CommandCase)
            {
                case CustomRecognizerResponse.CommandOneofCase.Analyze:
                    using (var box = new MaaRectBufferGrpc())
                    {
                        using var str = new MaaStringBufferGrpc();

                        var ret = recognizer.Analyze.Invoke(
                            response.Analyze.Context,
                            response.Analyze.ImageHandle,
                            response.Analyze.Task,
                            response.Analyze.Param,
                            arg,
                            box,
                            str);

                        await streamingCall.RequestStream.WriteAsync(new CustomRecognizerRequest
                        {
                            Ok = ret,
                            Analyze = new CustomRecognizerAnalyzeResult
                            {
                                Match = ret,
                                Box = new Rect
                                {
                                    Xy = new Point { X = box.X, Y = box.Y, },
                                    Wh = new Size { Width = box.Width, Height = box.Height, },
                                },
                                Detail = str.GetValue(),
                            },
                        });
                    }
                    break;
                default:
                    break;
            }
        }

        streamingCall.Dispose();
    }

    private async Task RegisterCustomAction(
        string name,
        AsyncDuplexStreamingCall<CustomActionRequest, CustomActionResponse> streamingCall)
    {
        await streamingCall.RequestStream.WriteAsync(new CustomActionRequest
        {
            Ok = true,
            Init = new CustomActionInit { Handle = Handle, Name = name, }
        });
        await streamingCall.RequestStream.CompleteAsync();
    }

    private static async Task CallCustomAction(
        //string name,
        MaaCustomActionApi action,
        AsyncDuplexStreamingCall<CustomActionRequest, CustomActionResponse> streamingCall,
        nint arg)
    {
        await foreach (var response in streamingCall.ResponseStream.ReadAllAsync())
        {
            switch (response.CommandCase)
            {
                case CustomActionResponse.CommandOneofCase.Run:
                    using (var box = new MaaRectBufferGrpc())
                    {
                        using var str = new MaaStringBufferGrpc();

                        var ret = action.Run.Invoke(
                            response.Run.Context,
                            response.Run.Task,
                            response.Run.Param,
                            box,
                            str,
                            arg);

                        await streamingCall.RequestStream.WriteAsync(new CustomActionRequest
                        {
                            Ok = ret,
                            //Init = new CustomActionInit { Handle = Handle, Name = name },
                        });
                    }
                    break;
                case CustomActionResponse.CommandOneofCase.Stop:
                    action.Abort.Invoke(arg);
                    await streamingCall.RequestStream.WriteAsync(new CustomActionRequest
                    {
                        // Ok = ret,
                        //Init = new CustomActionInit { Handle = Handle, Name = name },
                    });
                    break;
                default:
                    break;
            }
        }

        streamingCall.Dispose();
    }

    /// <summary>
    ///     Unregisters a <see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/> named <paramref name="name"/> in the <see cref="MaaInstanceGrpc"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/>.</typeparam>
    /// <inheritdoc/>
    public bool Unregister<T>(string name) where T : IMaaDef
    {
        switch (typeof(T).Name)
        {
            case nameof(MaaCustomRecognizerApi):
                _client.unregister_custom_recognizer(new HandleStringRequest { Handle = Handle, Str = name });
                return true;
            case nameof(MaaCustomActionApi):
                _client.unregister_custom_action(new HandleStringRequest { Handle = Handle, Str = name });
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    ///     Clears <see cref="MaaCustomRecognizerApi"/>s or <see cref="MaaCustomActionApi"/>s in the <see cref="MaaInstanceGrpc"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="MaaCustomRecognizerApi"/> or <see cref="MaaCustomActionApi"/>.</typeparam>
    /// <inheritdoc/>
    public bool Clear<T>() where T : IMaaDef
    {
        switch (typeof(T).Name)
        {
            case nameof(MaaCustomRecognizerApi):
                _client.clear_custom_recognizer(new HandleRequest { Handle = Handle, });
                return true;
            case nameof(MaaCustomActionApi):
                _client.clear_custom_action(new HandleRequest { Handle = Handle, });
                return true;
            default:
                return false;
        }
    }

    /// <inheritdoc/>
    public IMaaJob AppendTask(string taskEntryName, string taskParam = "{}")
    {
        var id = _client.post_task(new InstancePostTaskRequest { Handle = Handle, Task = taskEntryName, Param = taskParam, }).Id;
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    public bool SetParam(IMaaJob job, string param)
    {
        ArgumentNullException.ThrowIfNull(job);

        try
        {
            _client.set_task_param(new InstanceSetTaskParamRequest { Handle = Handle, Id = job.IId, Param = param, });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

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
    public bool AllTasksFinished => _client.all_finished(new HandleRequest { Handle = Handle, }).Bool;

    /// <inheritdoc/>
    public bool Abort()
    {
        try
        {
            _client.stop(new HandleRequest { Handle = Handle, });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }
}
