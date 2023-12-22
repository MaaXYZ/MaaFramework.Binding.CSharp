using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped members for MaaImageBuffer with generic handle.
/// </summary>
public interface IMaaImageBuffer<out T> : IMaaImageBuffer, IMaaDisposableHandle<T>
{
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
    ///     Gets the image raw data.
    /// </summary>
    /// <returns>The raw data of image.</returns>
    nint GetRawData();

    /// <summary>
    ///     Gets the image info.
    /// </summary>
    /// <value>
    ///     The info includes width, height, type.
    /// </value>
    ImageInfo Info { get; }

    /// <summary>
    ///     Sets the image raw data.
    /// </summary>
    /// <param name="data">The raw data of image.</param>
    /// <param name="width">The width of image.</param>
    /// <param name="height">The height of image.</param>
    /// <param name="type">The type of image.</param>
    /// <returns>true if the image raw data was setted successfully; otherwise, false.</returns>
    /// <remarks>
    ///     The data is cv::Mat::data.
    /// </remarks>
    bool SetRawData(nint data, int width, int height, int type);

    /// <summary>
    ///     Gets the image encoded data.
    /// </summary>
    /// <param name="size">The image encoded size.</param>
    /// <returns>The encoded data of image.</returns>
    /// <remarks>
    ///     The data is a PNG image.
    /// </remarks>
    nint GetEncodedData(out ulong size);

    /// <summary>
    ///     Sets the image encoded data.
    /// </summary>
    /// <param name="data">The encoded data of image.</param>
    /// <param name="size">The encoded size of image.</param>
    /// <returns>true if the image encoded data was setted successfully; otherwise, false.</returns>
    bool SetEncodedData(nint data, ulong size);
}
