using Grpc.Core;
using Grpc.Net.Client;
using MaaFramework.Binding.Abstractions.Grpc;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;
using MaaFramework.Binding.Interop.Grpc;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Grpc.Instance;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Grpc.Instance"/>.
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
    ///     Creates a <see cref="MaaInstanceGrpc"/> instance.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="toolkitInit">Whether inits the <see cref="Toolkit"/>.</param>
    public MaaInstanceGrpc(GrpcChannel channel, bool toolkitInit = false)
        : base(channel)
    {
        var handle = _client.create(new IdRequest { Id = CallbackId }).Handle;
        SetHandle(handle, needReleased: true);

        Toolkit = new MaaToolkitGrpc(Channel, toolkitInit);
        Utility = new MaaUtilityGrpc(Channel);
    }

    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="controller">The controller.</param>
    /// <param name="resource">The resource.</param>
    /// <param name="disposeOptions">The dispose options.</param>
    /// <param name="toolkitInit">Whether inits the <see cref="Toolkit"/>.</param>
    /// <inheritdoc cref="MaaInstanceGrpc(GrpcChannel, bool)"/>
    [SetsRequiredMembers]
    public MaaInstanceGrpc(GrpcChannel channel, IMaaController<string> controller, IMaaResource<string> resource, DisposeOptions disposeOptions, bool toolkitInit = false)
        : this(channel, toolkitInit)
    {
        Resource = resource;
        Controller = controller;
        DisposeOptions = disposeOptions;
    }

    /// <inheritdoc/>
    public required DisposeOptions DisposeOptions { get; set; }

    /// <inheritdoc/>
    protected override void ReleaseHandle()
    {
        // Cannot destroy Instance before disposing Controller and Resource.

        if (DisposeOptions.HasFlag(DisposeOptions.Controller))
        {
            Controller.Dispose();
        }

        if (DisposeOptions.HasFlag(DisposeOptions.Resource))
        {
            Resource.Dispose();
        }

        if (DisposeOptions.HasFlag(DisposeOptions.Toolkit))
        {
            Toolkit.Uninit();
        }

        _client.destroy(new HandleRequest { Handle = Handle, });
    }

    /// <inheritdoc/>
    public bool SetOption<T>(InstanceOption opt, T value) => (value, opt) switch
    {
        _ => throw new InvalidOperationException(),
    };

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
    public IMaaToolkit Toolkit { get; set; }

    /// <inheritdoc/>
    public IMaaUtility Utility { get; set; }

    /// <inheritdoc/>
    public bool Initialized => _client.inited(new HandleRequest { Handle = Handle, }).Bool;

    /// <inheritdoc/>
    public bool Register<T>(string name, T custom) where T : IMaaCustomTask
    {
        switch (custom)
        {
            case MaaCustomRecognizerApi recognizer:
                var streamingCallRecognizer = _client.register_custom_recognizer();
                Task.Run(() => CallCustomRecognizer(recognizer, streamingCallRecognizer));
                RegisterCustomRecognizer(name, streamingCallRecognizer).Wait();
                return true;
            case MaaCustomActionApi action:
                var streamingCallAction = _client.register_custom_action();
                Task.Run(() => CallCustomAction(action, streamingCallAction));
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

    private async Task CallCustomRecognizer(
        MaaCustomRecognizerApi recognizer,
        AsyncDuplexStreamingCall<CustomRecognizerRequest, CustomRecognizerResponse> streamingCall)
    {
        await foreach (var response in streamingCall.ResponseStream.ReadAllAsync())
        {
            switch (response.CommandCase)
            {
                case CustomRecognizerResponse.CommandOneofCase.Analyze:
                    using (var box = new MaaRectBufferGrpc())
                    {
                        using var str = new MaaStringBufferGrpc();
                        using var image = new MaaImageBufferGrpc(Channel, response.Analyze.ImageHandle);

                        var ret = recognizer.Analyze.Invoke(
                            new MaaSyncContextGrpc(Channel, response.Analyze.Context),
                            image,
                            response.Analyze.Task,
                            response.Analyze.Param,
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
                    throw new NotImplementedException();
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

    private async Task CallCustomAction(
        //string name,
        MaaCustomActionApi action,
        AsyncDuplexStreamingCall<CustomActionRequest, CustomActionResponse> streamingCall)
    {
        await foreach (var response in streamingCall.ResponseStream.ReadAllAsync())
        {
            switch (response.CommandCase)
            {
                case CustomActionResponse.CommandOneofCase.Run:
                    using (var box = new MaaRectBufferGrpc())
                    {
                        box.X = response.Run.Box.Xy.X;
                        box.Y = response.Run.Box.Xy.Y;
                        box.Width = response.Run.Box.Wh.Width;
                        box.Height = response.Run.Box.Wh.Height;

                        var ret = action.Run.Invoke(
                            new MaaSyncContextGrpc(Channel, response.Run.Context),
                            response.Run.Task,
                            response.Run.Param,
                            box,
                            response.Run.Detail);

                        await streamingCall.RequestStream.WriteAsync(new CustomActionRequest
                        {
                            Ok = ret,
                        });
                    }
                    break;
                case CustomActionResponse.CommandOneofCase.Stop:
                    action.Abort.Invoke();
                    await streamingCall.RequestStream.WriteAsync(new CustomActionRequest
                    {
                        Ok = true,
                    });
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        streamingCall.Dispose();
    }

    /// <inheritdoc/>
    public bool Unregister<T>(string name) where T : IMaaCustomTask
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

    /// <inheritdoc/>
    public bool Clear<T>() where T : IMaaCustomTask
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
