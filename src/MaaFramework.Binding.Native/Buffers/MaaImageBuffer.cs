using MaaFramework.Binding.Abstractions;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Image Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaImageBuffer : MaaDisposableHandle<nint>, IMaaImageBuffer<nint>
{
    /// <inheritdoc/>
    public override string ToString() => $"{GetType().Name}: {Width}x{Height} {{ {nameof(Channels)} = {Channels}, {nameof(Type)} = {Type} }}";

    /// <inheritdoc/>
    public bool CopyTo(IMaaImageBuffer buffer) => buffer switch
    {
        MaaImageBuffer native => MaaImageBufferSetRawData(
            handle: native.Handle,
            data: MaaImageBufferGetRawData(Handle),
            width: MaaImageBufferWidth(Handle),
            height: MaaImageBufferHeight(Handle),
            type: MaaImageBufferType(Handle)
            ),
        null => false,
        _ => TrySetEncodedDataStream(buffer.EncodedDataStream),
    };

    /// <summary>
    ///     Creates a <see cref="MaaImageBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaImageBufferHandle.</param>
    public MaaImageBuffer(MaaImageBufferHandle handle)
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc cref="MaaImageBuffer(nint)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferCreate"/>.
    /// </remarks>
    public MaaImageBuffer()
            : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(MaaImageBufferCreate(), needReleased: true);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaImageBufferDestroy(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferIsEmpty"/>.
    /// </remarks>
    public bool IsEmpty => MaaImageBufferIsEmpty(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferClear"/>.
    /// </remarks>
    public bool Clear()
        => MaaImageBufferClear(Handle);

    /// <summary>
    ///     Gets the image raw data.
    /// </summary>
    /// <returns>The raw data of image(cv::Mat::data).</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferGetRawData"/>.
    /// </remarks>
    public MaaImageRawData GetRawData()
        => MaaImageBufferGetRawData(Handle);

    /// <inheritdoc/>
    public ImageInfo Info => new
        (
            Width: MaaImageBufferWidth(Handle),
            Height: MaaImageBufferHeight(Handle),
            Channels: MaaImageBufferChannels(Handle),
            Type: MaaImageBufferType(Handle)
        );

    /// <inheritdoc cref="ImageInfo.Width"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferWidth"/>.
    /// </remarks>
    public int Width => MaaImageBufferWidth(Handle);

    /// <inheritdoc cref="ImageInfo.Height"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferHeight"/>.
    /// </remarks>
    public int Height => MaaImageBufferHeight(Handle);

    /// <inheritdoc cref="ImageInfo.Channels"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferChannels"/>.
    /// </remarks>
    public int Channels => MaaImageBufferChannels(Handle);

    /// <inheritdoc cref="ImageInfo.Type"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferType"/>.
    /// </remarks>
    public int Type => MaaImageBufferType(Handle);

    /// <summary>
    ///     Sets the image raw data.
    /// </summary>
    /// <param name="data">The raw data of image.</param>
    /// <param name="width">The width of image.</param>
    /// <param name="height">The height of image.</param>
    /// <param name="type">The type of image.</param>
    /// <returns><see langword="true"/> if the image raw data was set successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferSetRawData"/>.
    /// </remarks>
    public bool SetRawData(MaaImageRawData data, int width, int height, int type)
        => MaaImageBufferSetRawData(Handle, data, width, height, type);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferGetEncoded"/> and <see cref="MaaImageBufferGetEncodedSize"/>.
    /// </remarks>
    public MaaImageEncodedData GetEncodedData(out MaaSize size)
    {
        size = MaaImageBufferGetEncodedSize(Handle);
        return MaaImageBufferGetEncoded(Handle);
    }

    /// <inheritdoc cref="GetEncodedData"/>
    public static MaaImageEncodedData Get(MaaImageBufferHandle handle, out MaaSize size)
    {
        size = MaaImageBufferGetEncodedSize(handle);
        return MaaImageBufferGetEncoded(handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferSetEncoded"/>.
    /// </remarks>
    public bool SetEncodedData(MaaImageBufferHandle data, MaaSize size)
        => MaaImageBufferSetEncoded(Handle, data, size);

    /// <inheritdoc cref="SetEncodedData"/>
    public static bool Set(MaaImageBufferHandle handle, MaaImageBufferHandle data, MaaSize size)
        => MaaImageBufferSetEncoded(handle, data, size);

    /// <inheritdoc/>
    public unsafe Stream EncodedDataStream
    {
        get
        {
            return new UnmanagedMemoryStream(
                (byte*)MaaImageBufferGetEncoded(Handle).ToPointer(),
                (long)MaaImageBufferGetEncodedSize(Handle));
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (!TrySetEncodedDataStream(value))
                throw new InvalidOperationException();
        }
    }

    private unsafe bool TrySetEncodedDataStream(Stream value)
    {
        value.Position = 0;
        var size = (MaaSize)value.Length;
        if (value is UnmanagedMemoryStream unmanagedMemoryStream)
        {
            return MaaImageBufferSetEncoded(Handle, (nint)unmanagedMemoryStream.PositionPointer, size);
        }

        byte[] data;
        if (value is MemoryStream memoryStream && memoryStream.TryGetBuffer(out var seg))
        {
            data = seg.Array!;
        }
        else
        {
            data = new byte[size];
            _ = value.Read(data, 0, data.Length);
        }

        // Pin - Pin data in preparation for calling the P/Invoke.
        fixed (void* __data_native = &global::System.Runtime.InteropServices.Marshalling.ArrayMarshaller<byte, byte>.ManagedToUnmanagedIn.GetPinnableReference(data))
        {
            return MaaImageBufferSetEncoded(Handle, (nint)__data_native, size);
        }
    }
}
