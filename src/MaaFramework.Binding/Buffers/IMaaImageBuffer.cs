using MaaFramework.Binding.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped members for MaaImageBuffer with generic handle.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
public interface IMaaImageBuffer<THandle> : IMaaImageBuffer, IMaaBuffer<THandle, IMaaImageBuffer>, IMaaDisposableHandle<THandle>
{
    // Implement IMaaImageBufferStatic<THandle> at the same time if this interface is implemented.
}

/// <summary>
///     An interface defining wrapped members for MaaImageBuffer.
/// </summary>
public interface IMaaImageBuffer : IMaaBuffer<IMaaImageBuffer>
{
    /// <summary>
    ///     Gets a value indicates whether the image of the <see cref="IMaaImageBuffer"/> is empty.
    /// </summary>
    /// <returns><see langword="true"/> if the image is empty; otherwise, <see langword="false"/>.</returns>
    bool IsEmpty { get; }

    /// <summary>
    ///     Clears the image of the <see cref="IMaaImageBuffer"/>.
    /// </summary>
    /// <returns><see langword="true"/> if the image was cleared successfully; otherwise, <see langword="false"/>.</returns>
    bool TryClear();

    /// <summary>
    ///     Gets the image info.
    /// </summary>
    /// <returns>The info including width, height, channels, type.</returns>
    ImageInfo GetInfo();

    /// <inheritdoc cref="ImageInfo.Width"/>
    int Width { get; }

    /// <inheritdoc cref="ImageInfo.Height"/>
    int Height { get; }

    /// <inheritdoc cref="ImageInfo.Channels"/>
    int Channels { get; }

    /// <inheritdoc cref="ImageInfo.Type"/>
    int Type { get; }

    /// <inheritdoc cref="IMaaImageBufferStatic{THandle}.TryGetEncodedData(THandle, out byte[])"/>
    bool TryGetEncodedData([MaybeNullWhen(false)] out byte[] data);

    /// <inheritdoc cref="IMaaImageBufferStatic{THandle}.TryGetEncodedData(THandle, out Stream)"/>
    bool TryGetEncodedData([MaybeNullWhen(false)] out Stream data);

    /// <inheritdoc cref="IMaaImageBufferStatic{THandle}.TryGetEncodedData(THandle, out ReadOnlySpan{byte})"/>
    bool TryGetEncodedData(out ReadOnlySpan<byte> data);

    /// <inheritdoc cref="IMaaImageBufferStatic{THandle}.TrySetEncodedData(THandle, byte[])"/>
    bool TrySetEncodedData(byte[] data);

    /// <inheritdoc cref="IMaaImageBufferStatic{THandle}.TrySetEncodedData(THandle, Stream)"/>
    bool TrySetEncodedData(Stream data);

    /// <inheritdoc cref="IMaaImageBufferStatic{THandle}.TrySetEncodedData(THandle, ReadOnlySpan{byte})"/>
    bool TrySetEncodedData(ReadOnlySpan<byte> data);
}

/// <summary>
///     An interface defining wrapped static abstract members for MaaImageBuffer with generic handle.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
public interface IMaaImageBufferStatic<THandle>
{
    /// <summary>
    ///     Gets the image encoded data from a MaaImageBuffer.
    /// </summary>
    /// <param name="handle">The MaaImageBufferHandle.</param>
    /// <param name="data">The image data (PNG).</param>
    /// <returns><see langword="true"/> if the image encoded data was got successfully; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetEncodedData(THandle handle, [MaybeNullWhen(false)] out byte[] data);

    /// <remarks>
    ///     <para>Avoids disposing <see cref="IMaaImageBuffer"/> before the stream is read.</para>
    /// </remarks>
    /// <inheritdoc cref="TryGetEncodedData(THandle, out byte[])"/>
    static abstract bool TryGetEncodedData(THandle handle, [MaybeNullWhen(false)] out Stream data);

    /// <remarks>
    ///     <para>Avoids disposing <see cref="IMaaImageBuffer"/> before the span is read.</para>
    /// </remarks>
    /// <inheritdoc cref="TryGetEncodedData(THandle, out byte[])"/>
    static abstract bool TryGetEncodedData(THandle handle, out ReadOnlySpan<byte> data);

    /// <summary>
    ///     Gets the image encoded data from a function using MaaRectBuffer.
    /// </summary>
    /// <param name="data">The image data (PNG).</param>
    /// <param name="writeBuffer">The function used to write the data to the buffer.</param>
    /// <returns><see langword="true"/> if the image encoded data was got successfully; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetEncodedData([MaybeNullWhen(false)] out byte[] data, Func<THandle, bool> writeBuffer);

    /// <summary>
    ///     Sets the image encoded data to a MaaImageBuffer.
    /// </summary>
    /// <param name="handle">The MaaImageBufferHandle.</param>
    /// <param name="data">The image data (PNG).</param>
    /// <returns><see langword="true"/> if the image encoded data was set successfully; otherwise, <see langword="false"/>.</returns>
    static abstract bool TrySetEncodedData(THandle handle, byte[] data);

    /// <inheritdoc cref="TrySetEncodedData(THandle, byte[])"/>
    static abstract bool TrySetEncodedData(THandle handle, Stream data);

    /// <inheritdoc cref="TrySetEncodedData(THandle, byte[])"/>
    static abstract bool TrySetEncodedData(THandle handle, ReadOnlySpan<byte> data);

    /// <summary>
    ///     Sets the image encoded data to a function using MaaRectBuffer.
    /// </summary>
    /// <param name="data">The image data (PNG).</param>
    /// <param name="readBuffer">The function used to read the data from the buffer.</param>
    /// <returns><see langword="true"/> if the image encoded data was set successfully; otherwise, <see langword="false"/>.</returns>
    static abstract bool TrySetEncodedData(byte[] data, Func<THandle, bool> readBuffer);

    /// <inheritdoc cref="TrySetEncodedData(byte[], Func{THandle, bool})"/>
    static abstract bool TrySetEncodedData(Stream data, Func<THandle, bool> readBuffer);

    /// <inheritdoc cref="TrySetEncodedData(byte[], Func{THandle, bool})"/>
    static abstract bool TrySetEncodedData(ReadOnlySpan<byte> data, Func<THandle, bool> readBuffer);
}
