using MaaToolKit.Extensions.Interop;
using static MaaToolKit.Extensions.Interop.MaaApi;

namespace MaaToolKit.Extensions.ComponentModel;

/// <summary>
///     A class providing a reference implementation for Maa Image section of <see cref="MaaApi"/>.
/// </summary>
public class MaaImage : IDisposable
{
    internal MaaImageBufferHandle _handle;
    private bool disposed;

    /// <summary>
    ///     Creates a <see cref="MaaImage"/> instance.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreateImageBuffer"/>.
    /// </remarks>
    public MaaImage()
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
    ///     Disposes the <see cref="MaaImage"/> instance.
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
    ///     Gets the image raw data of <see cref="MaaImage"/>.
    /// </summary>
    /// <returns>
    ///     The raw data of image.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageRawData"/>.
    /// </remarks>
    public MaaImageRawData GetRawData()
        => MaaGetImageRawData(_handle);

    /// <summary>
    ///     Gets the image width of <see cref="MaaImage"/>.
    /// </summary>
    /// <value>
    ///     The width of image.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageWidth"/>.
    /// </remarks>
    public int Width => MaaGetImageWidth(_handle);

    /// <summary>
    ///     Gets the image height of <see cref="MaaImage"/>.
    /// </summary>
    /// <value>
    ///     The height of image.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageHeight"/>.
    /// </remarks>
    public int Height => MaaGetImageHeight(_handle);

    /// <summary>
    ///     Gets the image type of <see cref="MaaImage"/>.
    /// </summary>
    /// <value>
    ///     The type of image.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageType"/>.
    /// </remarks>
    public int Type => MaaGetImageType(_handle);

    /// <summary>
    ///     Sets the image raw data of <see cref="MaaImage"/>.
    /// </summary>
    /// <param name="data">The raw data of image.</param>
    /// <param name="width">The width of image.</param>
    /// <param name="height">The height of image.</param>
    /// <param name="type">The type of image.</param>
    /// <returns>
    ///     true if the image raw data was successfully setted; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetImageRawData"/>.
    /// </remarks>
    public bool SetRawData(MaaImageRawData data, int width, int height, int type)
        => MaaSetImageRawData(_handle, data, width, height, type).ToBoolean();

    /// <summary>
    ///     Gets the image encoded data of <see cref="MaaImage"/>.
    /// </summary>
    /// <returns>The encoded data of image.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageEncoded"/>.
    /// </remarks>
    public MaaImageEncodedData GetEncodedData()
        => MaaGetImageEncoded(_handle);

    /// <summary>
    ///     Gets the image encoded size of <see cref="MaaImage"/>.
    /// </summary>
    /// <value>
    ///     The encoded size of image.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageEncodedSize"/>.
    /// </remarks>
    public ulong Size => MaaGetImageEncodedSize(_handle);

    /// <summary>
    ///     Sets the image encoded data of <see cref="MaaImage"/>.
    /// </summary>
    /// <param name="data">The encoded data of image.</param>
    /// <param name="size">The encoded size of image.</param>
    /// <returns>
    ///     true if the image encoded data was successfully setted; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetImageEncoded"/>.
    /// </remarks>
    public bool SetEncodedData(MaaImageBufferHandle data, ulong size)
        => MaaSetImageEncoded(_handle, data, size).ToBoolean();
}
