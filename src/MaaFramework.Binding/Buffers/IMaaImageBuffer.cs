using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped members for MaaImageBuffer with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaImageBuffer<T> : IMaaImageBuffer, IMaaDisposableHandle<T>
{
    /// <summary>
    ///     Gets the image encoded data.
    /// </summary>
    /// <param name="size">The image encoded size.</param>
    /// <returns>The encoded data of image(PNG).</returns>
    T GetEncodedData(out MaaSize size);

    /// <summary>
    ///     Sets the image encoded data.
    /// </summary>
    /// <param name="data">The encoded data of image.</param>
    /// <param name="size">The encoded size of image.</param>
    /// <returns><see langword="true"/> if the image encoded data was set successfully; otherwise, <see langword="false"/>.</returns>
    bool SetEncodedData(T data, MaaSize size);
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
    bool Clear();

    /// <summary>
    ///     Gets the image info.
    /// </summary>
    /// <returns>The info includes width, height, channels, type.</returns>
    ImageInfo Info { get; }

    /// <summary>
    ///     Gets or sets the image encoded data stream.
    /// </summary>
    /// <returns>The stream of image(PNG).</returns>
    /// <remarks>
    ///     <para>1. Avoids disposing <see cref="IMaaImageBuffer"/> before the stream is read.</para>
    ///     <para>2. Sets a png image into the <see cref="IMaaImageBuffer"/> if a stream is set.</para>
    /// </remarks>
    /// <exception cref="ArgumentNullException"/>
    Stream EncodedDataStream { get; set; }
}
