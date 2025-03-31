using MaaFramework.Binding.Abstractions;
using System.Buffers;
using System.Runtime.InteropServices;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Image Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaImageBuffer : MaaDisposableHandle<MaaImageBufferHandle>, IMaaImageBuffer<MaaImageBufferHandle>, IMaaImageBufferStatic<MaaImageBufferHandle>
{
    /// <inheritdoc/>
    public override string ToString() => $"{GetType().Name}: {Width}x{Height} {{ {nameof(Channels)} = {Channels}, {nameof(Type)} = {Type} }}";

    /// <inheritdoc/>
    public bool TryCopyTo(MaaImageBufferHandle bufferHandle) => MaaImageBufferSetRawData(
            handle: bufferHandle,
            data: MaaImageBufferGetRawData(Handle),
            width: MaaImageBufferWidth(Handle),
            height: MaaImageBufferHeight(Handle),
            type: MaaImageBufferType(Handle));

    /// <inheritdoc/>
    public bool TryCopyTo(IMaaImageBuffer buffer) => buffer switch
    {
        MaaImageBuffer native => MaaImageBufferSetRawData(
            handle: native.Handle,
            data: MaaImageBufferGetRawData(Handle),
            width: MaaImageBufferWidth(Handle),
            height: MaaImageBufferHeight(Handle),
            type: MaaImageBufferType(Handle)),
        null => false,
        _ => buffer.TryGetEncodedData(out Stream data) && TrySetEncodedData(Handle, data),
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
    public bool TryClear()
        => MaaImageBufferClear(Handle);

    /// <inheritdoc/>
    public ImageInfo GetInfo() => new
    (
        Width: MaaImageBufferWidth(Handle),
        Height: MaaImageBufferHeight(Handle),
        Channels: MaaImageBufferChannels(Handle),
        Type: MaaImageBufferType(Handle)
    );

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferWidth"/>.
    /// </remarks>
    public int Width => MaaImageBufferWidth(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferHeight"/>.
    /// </remarks>
    public int Height => MaaImageBufferHeight(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferChannels"/>.
    /// </remarks>
    public int Channels => MaaImageBufferChannels(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferType"/>.
    /// </remarks>
    public int Type => MaaImageBufferType(Handle);

    #region EncodedData

    /// <inheritdoc/>
    public unsafe bool TryGetEncodedData(out byte[] data)
        => TryGetEncodedData(Handle, out data);

    /// <inheritdoc/>
    public bool TryGetEncodedData(out Stream data)
        => TryGetEncodedData(Handle, out data);

    /// <inheritdoc/>
    public bool TryGetEncodedData(out ReadOnlySpan<byte> data)
        => TryGetEncodedData(Handle, out data);

    /// <inheritdoc/>
    public static unsafe bool TryGetEncodedData(MaaImageBufferHandle handle, out byte[] data)
    {
        if (!TryGetEncodedData(handle, out ReadOnlySpan<byte> span) || span.Length > Array.MaxLength)
        {
            data = [];
            return false;
        }

        data = span.ToArray();
        return true;
    }

    /// <inheritdoc/>
    public static unsafe bool TryGetEncodedData(MaaImageBufferHandle handle, out Stream data)
    {
        var dataHandle = MaaImageBufferGetEncoded(handle);
        if (dataHandle == default)
        {
            data = Stream.Null;
            return false;
        }

        data = new UnmanagedMemoryStream(
            (byte*)dataHandle,
            (long)MaaImageBufferGetEncodedSize(handle));
        return true;
    }

    /// <inheritdoc/>
    public static unsafe bool TryGetEncodedData(MaaImageBufferHandle handle, out ReadOnlySpan<byte> data)
    {
        var size = (int)MaaImageBufferGetEncodedSize(handle);
        if (size <= 0)
        {
            data = [];
            return false;
        }

        data = new ReadOnlySpan<byte>((void*)MaaImageBufferGetEncoded(handle), size);
        return true;
    }

    /// <param name="handle">The MaaImageBufferHandle.</param>
    /// <param name="data">The image data (PNG).</param>
    /// <param name="size">The encoded size.</param>
    /// <inheritdoc cref="TryGetEncodedData(MaaImageBufferHandle, out byte[])"/>
    public static bool TryGetEncodedData(MaaImageBufferHandle handle, out MaaImageEncodedData data, out MaaSize size)
    {
        data = MaaImageBufferGetEncoded(handle);
        size = MaaImageBufferGetEncodedSize(handle);
        return data != default;
    }

    /// <inheritdoc/>
    public static bool TryGetEncodedData(out byte[] data, Func<MaaImageBufferHandle, bool> writeBuffer)
    {
        ArgumentNullException.ThrowIfNull(writeBuffer);
        var handle = MaaImageBufferCreate();
        if (!writeBuffer.Invoke(handle))
        {
            data = [];
            MaaImageBufferDestroy(handle);
            return false;
        }

        var ret = TryGetEncodedData(handle, out data);
        MaaImageBufferDestroy(handle);
        return ret;
    }

    /// <inheritdoc/>
    public bool TrySetEncodedData(byte[] data)
        => TrySetEncodedData(Handle, data);

    /// <inheritdoc/>
    public bool TrySetEncodedData(Stream data)
        => TrySetEncodedData(Handle, data);

    /// <inheritdoc/>
    public bool TrySetEncodedData(ReadOnlySpan<byte> data)
        => TrySetEncodedData(Handle, data);

    /// <inheritdoc/>
    public static unsafe bool TrySetEncodedData(MaaImageBufferHandle handle, byte[] data)
    {
        fixed (byte* __array_native = &global::System.Runtime.InteropServices.Marshalling.ArrayMarshaller<byte, byte>.ManagedToUnmanagedIn.GetPinnableReference(data))
        {
            return MaaImageBufferSetEncoded(handle, (nint)__array_native, (MaaSize)(data?.Length ?? 0));
        }
    }

    /// <inheritdoc/>
    public static unsafe bool TrySetEncodedData(MaaImageBufferHandle handle, Stream data)
    {
        if (data is null || !data.CanRead)
            return false;

        if (data.CanSeek)
            data.Position = 0;

        var size = (MaaSize)(data.Length - data.Position);
        if (data is UnmanagedMemoryStream unmanagedMemoryStream)
        {
            return MaaImageBufferSetEncoded(handle, (nint)unmanagedMemoryStream.PositionPointer, size);
        }

        var intSize = (int)size;
        if (data is MemoryStream memoryStream && memoryStream.TryGetBuffer(out var seg))
        {
            var span = new ReadOnlySpan<byte>(seg.Array, seg.Offset + (int)memoryStream.Position, intSize);
            return TrySetEncodedData(handle, span);
        }

        var array = ArrayPool<byte>.Shared.Rent(intSize); // using NativeMemory.Alloc if data is too big and necessary
        try
        {
            if (data.Read(array, 0, intSize) != 0)
                return false;

            fixed (byte* __array_native = &global::System.Runtime.InteropServices.Marshalling.ArrayMarshaller<byte, byte>.ManagedToUnmanagedIn.GetPinnableReference(array))
            {
                return MaaImageBufferSetEncoded(handle, (nint)__array_native, size);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(array);
        }
    }

    /// <inheritdoc/>
    public static unsafe bool TrySetEncodedData(MaaImageBufferHandle handle, ReadOnlySpan<byte> data)
    {
        fixed (byte* __array_native = &MemoryMarshal.GetReference(data))
        {
            return MaaImageBufferSetEncoded(handle, (nint)__array_native, (MaaSize)data.Length);
        }
    }

    /// <param name="handle">The MaaImageBufferHandle.</param>
    /// <param name="data">The image data (PNG).</param>
    /// <param name="size">The encoded size.</param>
    /// <inheritdoc cref="TrySetEncodedData(MaaImageBufferHandle, ReadOnlySpan{byte})"/>
    public static bool TrySetEncodedData(MaaImageBufferHandle handle, MaaImageEncodedData data, MaaSize size)
        => MaaImageBufferSetEncoded(handle, data, size);

    /// <inheritdoc/>
    public static bool TrySetEncodedData(byte[] data, Func<MaaImageBufferHandle, bool> readBuffer)
    {
        ArgumentNullException.ThrowIfNull(readBuffer);
        var handle = MaaImageBufferCreate();
        if (!TrySetEncodedData(handle, data))
        {
            MaaImageBufferDestroy(handle);
            return false;
        }

        var ret = readBuffer.Invoke(handle);
        MaaImageBufferDestroy(handle);
        return ret;
    }

    /// <inheritdoc/>
    public static bool TrySetEncodedData(Stream data, Func<MaaImageBufferHandle, bool> readBuffer)
    {
        ArgumentNullException.ThrowIfNull(readBuffer);
        var handle = MaaImageBufferCreate();
        if (!TrySetEncodedData(handle, data))
        {
            MaaImageBufferDestroy(handle);
            return false;
        }

        var ret = readBuffer.Invoke(handle);
        MaaImageBufferDestroy(handle);
        return ret;
    }

    /// <inheritdoc/>
    public static bool TrySetEncodedData(ReadOnlySpan<byte> data, Func<MaaImageBufferHandle, bool> readBuffer)
    {
        ArgumentNullException.ThrowIfNull(readBuffer);
        var handle = MaaImageBufferCreate();
        if (!TrySetEncodedData(handle, data))
        {
            MaaImageBufferDestroy(handle);
            return false;
        }

        var ret = readBuffer.Invoke(handle);
        MaaImageBufferDestroy(handle);
        return ret;
    }

    #endregion

    #region RawData

    /// <inheritdoc cref="TryGetRawData(nint, out nint, out int, out int, out int)"/>
    public bool TryGetRawData(out MaaImageRawData data)
    {
        data = MaaImageBufferGetRawData(Handle);
        return data != default;
    }

    /// <summary>
    ///     Gets the image raw data.
    /// </summary>
    /// <param name="handle">The MaaImageBufferHandle.</param>
    /// <param name="data">The image data (cv::Mat::data).</param>
    /// <param name="width">The image width.</param>
    /// <param name="height">The image height.</param>
    /// <param name="type">The image type.</param>
    /// <returns><see langword="true"/> if the image raw data was got successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferGetRawData"/>.
    /// </remarks>
    public static bool TryGetRawData(MaaImageBufferHandle handle, out MaaImageRawData data, out int width, out int height, out int type)
    {
        data = MaaImageBufferGetRawData(handle);
        width = MaaImageBufferWidth(handle);
        height = MaaImageBufferHeight(handle);
        type = MaaImageBufferType(handle);
        return data != default;
    }

    /// <inheritdoc cref="TrySetRawData(nint, nint, int, int, int)"/>
    public bool TrySetRawData(MaaImageRawData data, int width, int height, int type)
        => MaaImageBufferSetRawData(Handle, data, width, height, type);

    /// <summary>
    ///     Sets the image raw data.
    /// </summary>
    /// <param name="handle">The MaaImageBufferHandle.</param>
    /// <param name="data">The image(cv::Mat::data) data.</param>
    /// <param name="width">The image width.</param>
    /// <param name="height">The image height.</param>
    /// <param name="type">The image type.</param>
    /// <returns><see langword="true"/> if the image raw data was set successfully; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageBufferSetRawData"/>.
    /// </remarks>
    public static bool TrySetRawData(MaaImageBufferHandle handle, MaaImageRawData data, int width, int height, int type)
        => MaaImageBufferSetRawData(handle, data, width, height, type);

    #endregion
}
