using Grpc.Net.Client;
using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Grpc.Interop;
using System.Buffers;
using static MaaFramework.Binding.Grpc.Interop.Image;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Image Buffer section of <see cref="MaaFramework.Binding.Grpc.Interop.Image"/>.
/// </summary>
public class MaaImageBufferGrpc : MaaDisposableHandle<string>, IMaaImageBuffer<string>
{
    private readonly ImageClient _client;
    private readonly List<MemoryHandle> _memoryHandles = [];

    /// <inheritdoc cref="MaaImageBufferGrpc(GrpcChannel, string)"/>
    public MaaImageBufferGrpc(GrpcChannel channel)
        : base(invalidHandleValue: string.Empty)
    {
        _client = new ImageClient(channel);
        SetHandle(_client.create(new EmptyRequest()).Handle, needReleased: true);
    }

    /// <summary>
    ///     Creates a <see cref="MaaImageBufferGrpc"/> instance.
    /// </summary>
    /// <param name="handle">The MaaImageBufferHandle.</param>
    /// <param name="channel">The channel to use to make remote calls.</param>
    public MaaImageBufferGrpc(GrpcChannel channel, string handle)
        : base(invalidHandleValue: string.Empty)
    {
        _client = new ImageClient(channel);
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc/>
    protected override void ReleaseHandle()
        => _client.destroy(new HandleRequest { Handle = Handle, });

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _memoryHandles.ForEach(h => h.Dispose());
    }

    /// <inheritdoc/>
    public bool IsEmpty => _client.is_empty(new HandleRequest { Handle = Handle, }).Bool;

    /// <inheritdoc/>
    public bool Clear()
    {
        _client.clear(new HandleRequest { Handle = Handle, });
        return true;
    }

    /// <inheritdoc/>
    public ImageInfo Info
    {
        get
        {
            var info = _client.info(new HandleRequest { Handle = Handle, });
            return new()
            {
                Width = info.Size.Width,
                Height = info.Size.Height,
                Type = info.Type,
            };
        }
    }

    /// <inheritdoc/>
    public unsafe nint GetEncodedData(out ulong size)
    {
        var memory = _client.encoded(new HandleRequest { Handle = Handle, }).Buf.Memory;
        var memoryHandle = memory.Pin();
        _memoryHandles.Add(memoryHandle);
        size = (ulong)memory.Length;
        return (nint)memoryHandle.Pointer;
    }

    /// <inheritdoc/>
    public unsafe bool SetEncodedData(nint data, ulong size)
    {
        var bytes = new ReadOnlySpan<byte>(data.ToPointer(), (int)size);
        return _client.set_encoded(new HandleBufferRequest
        {
            Handle = Handle,
            Buffer = Google.Protobuf.ByteString.CopyFrom(bytes),
        }).Bool;
    }
}
