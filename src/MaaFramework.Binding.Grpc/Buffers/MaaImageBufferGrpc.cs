using Grpc.Net.Client;
using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Grpc.Interop;
using System.Buffers;
using System.Runtime.InteropServices;
using static MaaFramework.Binding.Grpc.Interop.Image;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Image Buffer section of <see cref="MaaFramework.Binding.Grpc.Interop.Image"/>.
/// </summary>
public class MaaImageBufferGrpc : MaaDisposableHandle<string>, IMaaImageBuffer<string>
{
    private readonly ImageClient _client;
    private readonly List<MemoryHandle> _memoryHandles = new();

    /// <inheritdoc cref="MaaImageBufferGrpc(string, GrpcChannel)"/>
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
    public MaaImageBufferGrpc(string handle, GrpcChannel channel)
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
    public bool IsEmpty()
        => _client.is_empty(new HandleRequest { Handle = Handle, }).Bool;

    /// <inheritdoc/>
    public bool Clear()
    {
        _client.clear(new HandleRequest { Handle = Handle, });
        return true;
    }

    /// <inheritdoc/>
    public nint GetRawData()
        => throw new NotImplementedException();

    /// <inheritdoc/>
    public int Width => _client.info(new HandleRequest { Handle = Handle, }).Size.Width;

    /// <inheritdoc/>
    public int Height => _client.info(new HandleRequest { Handle = Handle, }).Size.Height;

    /// <inheritdoc/>
    public int Type => _client.info(new HandleRequest { Handle = Handle, }).Type;

    /// <inheritdoc/>
    public bool SetRawData(nint data, int width, int height, int type)
         => throw new NotImplementedException();

    /// <inheritdoc/>
    public unsafe nint GetEncodedData(out ulong size)
    {
        var memory = _client.encoded(new HandleRequest { Handle = Handle, }).Buf.Memory;
        var handle = memory.Pin();
        _memoryHandles.Add(handle);
        size = (ulong)memory.Length;
        return (nint)handle.Pointer;
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
