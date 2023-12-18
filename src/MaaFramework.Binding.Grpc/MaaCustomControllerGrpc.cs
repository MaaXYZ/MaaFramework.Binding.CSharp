using Grpc.Core;
using Grpc.Net.Client;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Grpc.Interop;
using static MaaFramework.Binding.Grpc.Interop.Controller;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for MaaCustomControllerGrpc.
/// </summary>
public class MaaCustomControllerGrpc : MaaControllerGrpc
{
    /// <summary>
    ///     Creates a <see cref="MaaCustomControllerGrpc"/> instance.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="customController">The MaaCustomControllerApi.</param>
    /// <param name="arg">The MaaTransparentArg.</param>
    public MaaCustomControllerGrpc(GrpcChannel channel, MaaCustomControllerApi customController, nint arg)
        : base(channel)
    {
        var client = new ControllerClient(channel);
        var streamingCall = client.create_custom();
        Task.Run(() => CallCustomController(arg, customController, streamingCall));
        RegisterCustomController(CallbackId, streamingCall).Wait();
    }

    private static async Task RegisterCustomController(
        string id,
        AsyncDuplexStreamingCall<CustomControllerRequest, CustomControllerResponse> streamingCall)
    {
        await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
        {
            Ok = true,
            Init = id,
        });
        await streamingCall.RequestStream.CompleteAsync();
    }

    private async Task CallCustomController(
        nint arg,
        MaaCustomControllerApi controller,
        AsyncDuplexStreamingCall<CustomControllerRequest, CustomControllerResponse> streamingCall)
    {
        await foreach (var response in streamingCall.ResponseStream.ReadAllAsync())
        {
            switch (response.CommandCase)
            {
                case CustomControllerResponse.CommandOneofCase.Init:
                    SetHandle(response.Init, needReleased: true);
                    break;
                case CustomControllerResponse.CommandOneofCase.Connect:
                    await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
                    {
                        Ok = controller.Connect.Invoke(
                                arg),
                    });
                    break;
                case CustomControllerResponse.CommandOneofCase.Click:
                    await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
                    {
                        Ok = controller.Click.Invoke(
                                response.Click.Point.X,
                                response.Click.Point.Y,
                                arg),
                    });
                    break;
                case CustomControllerResponse.CommandOneofCase.Swipe:
                    await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
                    {
                        Ok = controller.Swipe.Invoke(
                                response.Swipe.From.X,
                                response.Swipe.From.Y,
                                response.Swipe.To.X,
                                response.Swipe.To.Y,
                                response.Swipe.Duration,
                                arg),
                    });
                    break;
                case CustomControllerResponse.CommandOneofCase.Key:
                    await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
                    {
                        Ok = controller.PressKey.Invoke(
                                response.Key.Key,
                                arg),
                    });
                    break;
                case CustomControllerResponse.CommandOneofCase.TouchDown:
                    await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
                    {
                        Ok = controller.TouchDown.Invoke(
                                response.TouchDown.Contact,
                                response.TouchDown.Pos.X,
                                response.TouchDown.Pos.Y,
                                response.TouchDown.Pressure,
                                arg),
                    });
                    break;
                case CustomControllerResponse.CommandOneofCase.TouchMove:
                    await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
                    {
                        Ok = controller.TouchMove.Invoke(
                                response.TouchMove.Contact,
                                response.TouchMove.Pos.X,
                                response.TouchMove.Pos.Y,
                                response.TouchMove.Pressure,
                                arg),
                    });
                    break;
                case CustomControllerResponse.CommandOneofCase.TouchUp:
                    await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
                    {
                        Ok = controller.TouchUp.Invoke(
                                response.TouchUp.Contact,
                                arg),
                    });
                    break;
                case CustomControllerResponse.CommandOneofCase.Start:
                    await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
                    {
                        Ok = controller.StartApp.Invoke(
                                response.Start,
                                arg),
                    });
                    break;
                case CustomControllerResponse.CommandOneofCase.Stop:
                    await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
                    {
                        Ok = controller.StopApp.Invoke(
                                response.Stop,
                                arg),
                    });
                    break;
                case CustomControllerResponse.CommandOneofCase.Resolution:
                    var width = 0;
                    var height = 0;
                    await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
                    {
                        Ok = controller.GetResolution.Invoke(
                                arg,
                                ref width,
                                ref height),
                        Resolution = new Size { Width = width, Height = height },
                    });
                    break;
                case CustomControllerResponse.CommandOneofCase.Image:
                    using (var image = new MaaImageBufferGrpc(response.Image, Channel))
                    {
                        await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
                        {
                            Ok = controller.GetImage.Invoke(
                                arg,
                                image),
                        });
                    }
                    break;
                case CustomControllerResponse.CommandOneofCase.Uuid:
                    using (var uuid = new MaaStringBufferGrpc())
                    {
                        await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
                        {
                            Ok = controller.GetUuid.Invoke(
                                    arg,
                                    uuid),
                            Uuid = uuid.ToString(),
                        });
                    }
                    break;
                case CustomControllerResponse.CommandOneofCase.SetOption:
                    await streamingCall.RequestStream.WriteAsync(new CustomControllerRequest
                    {
                        Ok = controller.SetOption.Invoke(
                                (ControllerOption)response.SetOption.Key,
                                response.SetOption.Value,
                                arg),
                    });
                    break;
                default:
                    break;
            }
        }

        streamingCall.Dispose();
    }
}
