using Grpc.Core;
using Grpc.Net.Client;
using MaaFramework.Binding.Abstractions.Grpc;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Interop.Grpc;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Grpc.SyncContext;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Grpc.SyncContext"/>.
/// </summary>
public class MaaSyncContextGrpc : MaaGrpcChannel, IMaaSyncContext<string>
{
    private SyncContextClient _client = default!;

    /// <inheritdoc/>
    public required string Handle { get; init; }

    /// <inheritdoc/>
    protected override void OnChannelChanged(GrpcChannel channel)
    {
        _client = new SyncContextClient(channel);
    }

    /// <inheritdoc cref="MaaSyncContextGrpc(GrpcChannel, string)"/>
    public MaaSyncContextGrpc(GrpcChannel channel)
        : base(channel)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaSyncContextGrpc"/> instance.
    /// </summary>
    /// <param name="channel">The channel to use to make remote calls.</param>
    /// <param name="syncContextHandle">The MaaSyncContextHandle.</param>
    [SetsRequiredMembers]
    public MaaSyncContextGrpc(GrpcChannel channel, string syncContextHandle)
        : base(channel)
    {
        Handle = syncContextHandle;
    }

    /// <inheritdoc/>
    public bool RunTask(string taskName, string param)
    {
        try
        {
            _client.run_task(new SyncContextRunTaskRequest { Handle = Handle, Task = taskName, Param = param, });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool RunRecognizer(IMaaImageBuffer image, string taskName, string taskParam, IMaaRectBuffer outBox, IMaaStringBuffer outDetail)
        => RunRecognizer((IMaaImageBuffer<string>)image, taskName, taskParam, (IMaaRectBuffer<string>)outBox, (IMaaStringBuffer<string>)outDetail);

    /// <inheritdoc/>
    public bool RunRecognizer(IMaaImageBuffer<string> image, string taskName, string taskParam, IMaaRectBuffer<string> outBox, IMaaStringBuffer<string> outDetail)
    {
        ArgumentNullException.ThrowIfNull(image);
        ArgumentNullException.ThrowIfNull(outBox);
        ArgumentNullException.ThrowIfNull(outDetail);

        try
        {
            var response = _client.run_recognizer(new SyncContextRunRecognizerRequest
            {
                Handle = Handle,
                ImageHandle = image.Handle,
                Task = taskName,
                Param = taskParam,
            });
            outBox.SetValues(x: response.Box.Xy.X,
                             y: response.Box.Xy.Y,
                             width: response.Box.Wh.Width,
                             height: response.Box.Wh.Height);
            outDetail.SetValue(response.Detail);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool RunAction(string taskName, string taskParam, IMaaRectBuffer curBox, string curRecDetail)
        => RunAction(taskName, taskParam, (IMaaRectBuffer<string>)curBox, curRecDetail);

    /// <inheritdoc/>
    public bool RunAction(string taskName, string taskParam, IMaaRectBuffer<string> curBox, string curRecDetail)
    {
        ArgumentNullException.ThrowIfNull(curBox);

        try
        {
            _client.run_action(new SyncContextRunActionRequest
            {
                Handle = Handle,
                Task = taskName,
                Param = taskParam,
                Detail = curRecDetail,
                Box = new Rect
                {
                    Xy = new Point { X = curBox.X, Y = curBox.Y, },
                    Wh = new Size { Width = curBox.Width, Height = curBox.Height, },
                },
            });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool Click(int x, int y)
    {
        try
        {
            _client.click(new SyncContextClickRequest
            {
                Handle = Handle,
                Param = new ClickParam
                {
                    Point = new Point { X = x, Y = y, },
                },
            });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool Swipe(int x1, int y1, int x2, int y2, int duration)
    {
        try
        {
            _client.swipe(new SyncContextSwipeRequest
            {
                Handle = Handle,
                Param = new SwipeParam
                {
                    From = new Point { X = x1, Y = y1, },
                    To = new Point { X = x1, Y = y1, },
                    Duration = duration,
                }
            });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool PressKey(int keyCode)
    {
        try
        {
            _client.key(new SyncContextKeyRequest
            {
                Handle = Handle,
                Param = new KeyParam
                {
                    Key = keyCode,
                },
            });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    bool IMaaSyncContext.InputText(string text)
        => throw new NotImplementedException();

    /// <inheritdoc/>
    public bool TouchDown(int contact, int x, int y, int pressure)
    {
        try
        {
            _client.touch_down(new SyncContextTouchRequest
            {
                Handle = Handle,
                Param = new TouchParam
                {
                    Contact = contact,
                    Pos = new Point { X = x, Y = y, },
                    Pressure = pressure,
                },
            });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool TouchMove(int contact, int x, int y, int pressure)
    {
        try
        {
            _client.touch_move(new SyncContextTouchRequest
            {
                Handle = Handle,
                Param = new TouchParam
                {
                    Contact = contact,
                    Pos = new Point { X = x, Y = y, },
                    Pressure = pressure,
                },
            });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool TouchUp(int contact)
    {
        try
        {
            _client.touch_move(new SyncContextTouchRequest
            {
                Handle = Handle,
                Param = new TouchParam
                {
                    Contact = contact,
                },
            });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool Screencap(IMaaImageBuffer outImage)
        => Screencap((IMaaImageBuffer<string>)outImage);

    /// <inheritdoc/>
    public bool Screencap(IMaaImageBuffer<string> outImage)
    {
        ArgumentNullException.ThrowIfNull(outImage);

        try
        {
            _client.screencap(new SyncContextScreencapRequest { Handle = Handle, ImageHandle = outImage.Handle, });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool GetTaskResult(string taskName, IMaaStringBuffer outTaskResult)
        => GetTaskResult(taskName, (IMaaStringBuffer<string>)outTaskResult);

    /// <inheritdoc/>
    public bool GetTaskResult(string taskName, IMaaStringBuffer<string> outTaskResult)
    {
        ArgumentNullException.ThrowIfNull(outTaskResult);

        try
        {
            _client.task_result(new HandleStringRequest { Handle = Handle, Str = outTaskResult.ToString(), });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }
}
