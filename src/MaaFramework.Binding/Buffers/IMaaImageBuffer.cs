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
    /// <returns>true if the image encoded data was set successfully; otherwise, false.</returns>
    bool SetEncodedData(T data, MaaSize size);
}

/// <summary>
///     An interface defining wrapped members for MaaImageBuffer.
/// </summary>
public interface IMaaImageBuffer : IDisposable
{
    /// <summary>
    ///     Gets a value indicates whether the image of the MaaImageBuffer is empty.
    /// </summary>
    /// <value>
    ///     true if the image is empty; otherwise, false.
    /// </value>
    bool IsEmpty { get; }

    /// <summary>
    ///     Clears the image of the MaaImageBuffer.
    /// </summary>
    /// <returns>true if the image was cleared successfully; otherwise, false.</returns>
    bool Clear();

    /// <summary>
    ///     Gets the image info.
    /// </summary>
    /// <value>
    ///     The info includes width, height, type.
    /// </value>
    ImageInfo Info { get; }

    /// <summary>
    ///     Gets or sets the image encoded data stream.
    /// </summary>
    /// <returns>The stream of image(PNG).</returns>
    /// <remarks>
    ///     1. Avoids disposing <see cref="IMaaImageBuffer"/> before the stream is read.
    ///     2. Sets a png image into the <see cref="IMaaImageBuffer"/> if a stream is set, then the stream will be closed.
    /// </remarks>
    Stream EncodedDataStream { get; set; }
}
