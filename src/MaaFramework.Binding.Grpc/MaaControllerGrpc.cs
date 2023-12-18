using Grpc.Core;
using Grpc.Net.Client;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Grpc.Abstractions;
using MaaFramework.Binding.Grpc.Interop;
using static MaaFramework.Binding.Grpc.Interop.Controller;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Grpc.Interop.Controller"/>.
/// </summary>
public class MaaControllerGrpc : MaaCommonGrpc, IMaaController<string>
{
    private ControllerClient _client = default!;

    /// <inheritdoc/>
    protected override void OnChannelChanged(GrpcChannel channel)
    {
        _client = new ControllerClient(channel);
    }

    /// <summary>
    ///     Creates a <see cref="MaaControllerGrpc"/> instance.
    /// </summary>
    protected MaaControllerGrpc(GrpcChannel channel)
        : base(channel)
    {
    }

    /// <inheritdoc/>
    protected override void ReleaseHandle()
        => _client.destroy(new HandleRequest { Handle = Handle, });

    /// <inheritdoc/>
    public bool SetOption(ControllerOption opt, int value)
    {
        var request = new ControllerSetOptionRequest { Handle = Handle, };
        switch (opt)
        {
            case ControllerOption.ScreenshotTargetLongSide:
                request.LongSide = value;
                break;
            case ControllerOption.ScreenshotTargetShortSide:
                request.ShortSide = value;
                break;
            default:
                return false;
        }

        return SetOption(request);
    }

    /// <inheritdoc/>
    public bool SetOption(ControllerOption opt, bool value)
        => false;

    /// <inheritdoc/>
    public bool SetOption(ControllerOption opt, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);

        var request = new ControllerSetOptionRequest { Handle = Handle, };
        switch (opt)
        {
            case ControllerOption.DefaultAppPackageEntry:
                request.DefPackageEntry = value;
                break;
            case ControllerOption.DefaultAppPackage:
                request.DefPackage = value;
                break;
            default:
                return false;
        }

        return SetOption(request);
    }

    /// <inheritdoc cref="SetOption(ControllerOption, int)"/>
    protected bool SetOption(ControllerSetOptionRequest request)
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

    /// <inheritdoc/>
    public IMaaJob LinkStart()
    {
        var id = _client.post_connection(new HandleRequest { Handle = Handle, }).Id;
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    public IMaaJob Click(int x, int y)
    {
        var id = _client.post_click(new ControllerPostClickRequest
        {
            Handle = Handle,
            Param = new ClickParam
            {
                Point = new Point { X = x, Y = y },
            },
        }).Id;
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    public IMaaJob Swipe(int x1, int y1, int x2, int y2, int duration)
    {
        var id = _client.post_swipe(new ControllerPostSwipeRequest
        {
            Handle = Handle,
            Param = new SwipeParam
            {
                From = new Point { X = x1, Y = y1, },
                To = new Point { X = x2, Y = y2 },
                Duration = duration,
            },
        }).Id;
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    public IMaaJob PressKey(int keyCode)
    {
        var id = _client.post_press_key(new ControllerPostKeyRequest
        {
            Handle = Handle,
            Param = new KeyParam { Key = keyCode, },
        }).Id;
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    public IMaaJob TouchDown(int contact, int x, int y, int pressure)
    {
        var id = _client.post_touch_down(new ControllerPostTouchRequest
        {
            Handle = Handle,
            Param = new TouchParam
            {
                Contact = contact,
                Pos = new Point { X = x, Y = y, },
                Pressure = pressure,
            },
        }).Id;
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    public IMaaJob TouchMove(int contact, int x, int y, int pressure)
    {
        var id = _client.post_touch_move(new ControllerPostTouchRequest
        {
            Handle = Handle,
            Param = new TouchParam
            {
                Contact = contact,
                Pos = new Point { X = x, Y = y, },
                Pressure = pressure,
            },
        }).Id;
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    public IMaaJob TouchUp(int contact)
    {
        var id = _client.post_touch_up(new ControllerPostTouchRequest
        {
            Handle = Handle,
            Param = new TouchParam
            {
                Contact = contact,
            },
        }).Id;
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    public IMaaJob Screencap()
    {
        var id = _client.post_screencap(new HandleRequest { Handle = Handle, }).Id;
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    public bool SetParam(IMaaJob job, string param)
        => false;

    /// <inheritdoc/>
    public MaaJobStatus GetStatus(IMaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);
        return (MaaJobStatus)_client.status(new HandleIIdRequest { Handle = Handle, Id = job.IId }).Status;
    }

    /// <inheritdoc/>
    public MaaJobStatus Wait(IMaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);
        return (MaaJobStatus)_client.wait(new HandleIIdRequest { Handle = Handle, Id = job.IId }).Status;
    }

    /// <inheritdoc/>
    public bool LinkStop()
        => _client.connected(new HandleRequest { Handle = Handle, }).Bool;

    /// <inheritdoc/>
    public bool GetImage(IMaaImageBuffer maaImage)
        => GetImage((IMaaImageBuffer<string>)maaImage);

    /// <inheritdoc/>
    public bool GetImage(IMaaImageBuffer<string> maaImage)
    {
        ArgumentNullException.ThrowIfNull(maaImage);
        try
        {
            _client.image(new HandleHandleRequest { Handle = Handle, AnotherHandle = maaImage.Handle, });
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public string? Uuid
    {
        get
        {
            try
            {
                return _client.uuid(new HandleRequest { Handle = Handle, }).Str;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unknown)
            {
                return null;
            }
        }
    }
}
