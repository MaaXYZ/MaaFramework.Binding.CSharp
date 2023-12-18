using Grpc.Core;
using Grpc.Net.Client;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Grpc.Abstractions;
using MaaFramework.Binding.Grpc.Interop;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Grpc.Interop.SyncContext;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Grpc.Interop.SyncContext"/>.
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
    public bool RunTask(string task, string param)
    {
        try
        {
            _client.run_task(new SyncContextRunTaskRequest { Handle = Handle, Task = task, Param = param, });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool RunRecognizer(IMaaImageBuffer image, string task, string taskParam, IMaaRectBuffer outBox, IMaaStringBuffer detailBuff)
        => RunRecognizer((IMaaImageBuffer<string>)image, task, taskParam, (IMaaRectBuffer<string>)outBox, (IMaaStringBuffer<string>)detailBuff);

    /// <inheritdoc/>
    public bool RunRecognizer(IMaaImageBuffer<string> image, string task, string taskParam, IMaaRectBuffer<string> outBox, IMaaStringBuffer<string> detailBuff)
    {
        ArgumentNullException.ThrowIfNull(image);
        ArgumentNullException.ThrowIfNull(outBox);
        ArgumentNullException.ThrowIfNull(detailBuff);

        try
        {
            var response = _client.run_recognizer(new SyncContextRunRecognizerRequest
            {
                Handle = Handle,
                ImageHandle = image.Handle,
                Task = task,
                Param = taskParam,
            });
            outBox.SetValues(x: response.Box.Xy.X,
                             y: response.Box.Xy.Y,
                             width: response.Box.Wh.Width,
                             height: response.Box.Wh.Height);
            detailBuff.SetValue(response.Detail);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool RunAction(string task, string taskParam, IMaaRectBuffer curBox, string curRecDetail)
        => RunAction(task, taskParam, (IMaaRectBuffer<string>)curBox, curRecDetail);

    /// <inheritdoc/>
    public bool RunAction(string task, string taskParam, IMaaRectBuffer<string> curBox, string curRecDetail)
    {
        ArgumentNullException.ThrowIfNull(curBox);

        try
        {
            _client.run_action(new SyncContextRunActionRequest
            {
                Handle = Handle,
                Task = task,
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
    public bool Screencap(IMaaImageBuffer buffer)
        => Screencap((IMaaImageBuffer<string>)buffer);

    /// <inheritdoc/>
    public bool Screencap(IMaaImageBuffer<string> buffer)
    {
        ArgumentNullException.ThrowIfNull(buffer);

        try
        {
            _client.screencap(new SyncContextScreencapRequest { Handle = Handle, ImageHandle = buffer.Handle, });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool GetTaskResult(string task, IMaaStringBuffer buffer)
        => GetTaskResult(task, (IMaaStringBuffer<string>)buffer);

    /// <inheritdoc/>
    public bool GetTaskResult(string task, IMaaStringBuffer<string> buffer)
    {
        ArgumentNullException.ThrowIfNull(buffer);

        try
        {
            _client.task_result(new HandleStringRequest { Handle = Handle, Str = buffer.ToString(), });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }
}
