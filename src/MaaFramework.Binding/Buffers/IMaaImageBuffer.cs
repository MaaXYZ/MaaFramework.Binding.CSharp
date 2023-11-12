using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped members for MaaImageBuffer.
/// </summary>
public interface IMaaImageBuffer : IMaaDisposableHandle
{
    /// <summary>
    ///     Indicates whether the image of the MaaImageBuffer is empty.
    /// </summary>
    /// <returns>true if the image is empty; otherwise, false.</returns>
    bool IsEmpty();

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
    ///     Gets the image width.
    /// </summary>
    /// <value>
    ///     The width of image.
    /// </value>
    int Width { get; }

    /// <summary>
    ///     Gets the image height.
    /// </summary>
    /// <value>
    ///     The height of image.
    /// </value>
    int Height { get; }

    /// <summary>
    ///     Gets the image type.
    /// </summary>
    /// <value>
    ///     The type of image.
    /// </value>
    int Type { get; }

    /// <summary>
    ///     Sets the image raw data.
    /// </summary>
    /// <param name="data">The raw data of image.</param>
    /// <param name="width">The width of image.</param>
    /// <param name="height">The height of image.</param>
    /// <param name="type">The type of image.</param>
    /// <returns>true if the image raw data was setted successfully; otherwise, false.</returns>
    bool SetRawData(nint data, int width, int height, int type);

    /// <summary>
    ///     Gets the image encoded data.
    /// </summary>
    /// <returns>The encoded data of image.</returns>
    nint GetEncodedData();

    /// <summary>
    ///     Gets the image encoded size.
    /// </summary>
    /// <value>
    ///     The encoded size of image.
    /// </value>
    ulong Size { get; }

    /// <summary>
    ///     Sets the image encoded data.
    /// </summary>
    /// <param name="data">The encoded data of image.</param>
    /// <param name="size">The encoded size of image.</param>
    /// <returns>true if the image encoded data was setted successfully; otherwise, false.</returns>
    bool SetEncodedData(nint data, ulong size);
}
