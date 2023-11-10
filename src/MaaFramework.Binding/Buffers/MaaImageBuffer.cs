using MaaFramework.Binding.Interop;
using static MaaFramework.Binding.Interop.Framework.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Image Buffer section of <see cref="MaaFramework.Binding.Interop.Framework.MaaBuffer"/>.
/// </summary>
public class MaaImageBuffer : IDisposable
{
    internal MaaImageBufferHandle _handle;
    private bool disposed;

    /// <summary>
    ///     Creates a <see cref="MaaImageBuffer"/> instance.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreateImageBuffer"/>.
    /// </remarks>
    public MaaImageBuffer()
    {
        _handle = MaaCreateImageBuffer();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Disposes the <see cref="MaaImageBuffer"/> instance.
    /// </summary>
    /// <param name="disposing"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDestroyImageBuffer"/>.
    /// </remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            // if (disposing) Dispose managed resources.
            MaaDestroyImageBuffer(_handle);
            disposed = true;
        }
    }

    /// <summary>
    ///     Indicates whether the image of the <see cref="MaaImageBuffer" /> is empty.
    /// </summary>
    /// <returns>true if the image is empty; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaIsImageEmpty"/>.
    /// </remarks>
    public bool IsEmpty()
        => MaaIsImageEmpty(_handle).ToBoolean();

    /// <summary>
    ///     Clears the image of the <see cref="MaaImageBuffer" />.
    /// </summary>
    /// <returns>true if the image was cleared successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaClearImage"/>.
    /// </remarks>
    public bool Clear()
        => MaaClearImage(_handle).ToBoolean();

    /// <summary>
    ///     Gets the image raw data.
    /// </summary>
    /// <returns>The raw data of image.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageRawData"/>.
    /// </remarks>
    public MaaImageRawData GetRawData()
        => MaaGetImageRawData(_handle);

    /// <summary>
    ///     Gets the image width.
    /// </summary>
    /// <value>
    ///     The width of image.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageWidth"/>.
    /// </remarks>
    public int Width => MaaGetImageWidth(_handle);

    /// <summary>
    ///     Gets the image height.
    /// </summary>
    /// <value>
    ///     The height of image.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageHeight"/>.
    /// </remarks>
    public int Height => MaaGetImageHeight(_handle);

    /// <summary>
    ///     Gets the image type.
    /// </summary>
    /// <value>
    ///     The type of image.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageType"/>.
    /// </remarks>
    public int Type => MaaGetImageType(_handle);

    /// <summary>
    ///     Sets the image raw data.
    /// </summary>
    /// <param name="data">The raw data of image.</param>
    /// <param name="width">The width of image.</param>
    /// <param name="height">The height of image.</param>
    /// <param name="type">The type of image.</param>
    /// <returns>true if the image raw data was setted successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetImageRawData"/>.
    /// </remarks>
    public bool SetRawData(MaaImageRawData data, int width, int height, int type)
        => MaaSetImageRawData(_handle, data, width, height, type).ToBoolean();

    /// <summary>
    ///     Gets the image encoded data.
    /// </summary>
    /// <returns>The encoded data of image.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageEncoded"/>.
    /// </remarks>
    public MaaImageEncodedData GetEncodedData()
        => MaaGetImageEncoded(_handle);

    /// <summary>
    ///     Gets the image encoded size.
    /// </summary>
    /// <value>
    ///     The encoded size of image.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageEncodedSize"/>.
    /// </remarks>
    public ulong Size => MaaGetImageEncodedSize(_handle);

    /// <summary>
    ///     Sets the image encoded data.
    /// </summary>
    /// <param name="data">The encoded data of image.</param>
    /// <param name="size">The encoded size of image.</param>
    /// <returns>true if the image encoded data was setted successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetImageEncoded"/>.
    /// </remarks>
    public bool SetEncodedData(MaaImageBufferHandle data, ulong size)
        => MaaSetImageEncoded(_handle, data, size).ToBoolean();
}
