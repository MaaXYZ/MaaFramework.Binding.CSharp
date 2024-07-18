using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Image Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
[System.Diagnostics.DebuggerDisplay("{Width}x{Height}x{Type}")]
public class MaaImageBuffer : MaaDisposableHandle<nint>, IMaaImageBuffer<nint>
{
    /// <inheritdoc cref="MaaImageBuffer(nint)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreateImageBuffer"/>.
    /// </remarks>
    public MaaImageBuffer()
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(MaaCreateImageBuffer(), needReleased: true);
    }

    /// <summary>
    ///     Creates a <see cref="MaaImageBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaImageBufferHandle.</param>
    public MaaImageBuffer(MaaImageBufferHandle handle)
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDestroyImageBuffer"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaDestroyImageBuffer(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaIsImageEmpty"/>.
    /// </remarks>
    public bool IsEmpty => MaaIsImageEmpty(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaClearImage"/>.
    /// </remarks>
    public bool Clear()
        => MaaClearImage(Handle).ToBoolean();

    /// <summary>
    ///     Gets the image raw data.
    /// </summary>
    /// <returns>The raw data of image(cv::Mat::data).</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageRawData"/>.
    /// </remarks>
    public MaaImageRawData GetRawData()
        => MaaGetImageRawData(Handle);

    /// <inheritdoc/>
    public ImageInfo Info => new()
    {
        Width = MaaGetImageWidth(Handle),
        Height = MaaGetImageHeight(Handle),
        Type = MaaGetImageType(Handle),
    };

    /// <inheritdoc cref="ImageInfo.Width"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageWidth"/>.
    /// </remarks>
    public int Width => MaaGetImageWidth(Handle);

    /// <inheritdoc cref="ImageInfo.Height"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageHeight"/>.
    /// </remarks>
    public int Height => MaaGetImageHeight(Handle);

    /// <inheritdoc cref="ImageInfo.Type"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageType"/>.
    /// </remarks>
    public int Type => MaaGetImageType(Handle);

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
        => MaaSetImageRawData(Handle, data, width, height, type).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageEncoded"/> and <see cref="MaaGetImageEncodedSize"/>.
    /// </remarks>
    public MaaImageEncodedData GetEncodedData(out ulong size)
    {
        size = MaaGetImageEncodedSize(Handle);
        return MaaGetImageEncoded(Handle);
    }

    /// <inheritdoc cref="GetEncodedData"/>
    public static MaaImageEncodedData Get(MaaImageBufferHandle handle, out ulong size)
    {
        size = MaaGetImageEncodedSize(handle);
        return MaaGetImageEncoded(handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetImageEncoded"/>.
    /// </remarks>
    public bool SetEncodedData(MaaImageBufferHandle data, ulong size)
        => MaaSetImageEncoded(Handle, data, size).ToBoolean();

    /// <inheritdoc cref="SetEncodedData"/>
    public static bool Set(MaaImageBufferHandle handle, MaaImageBufferHandle data, ulong size)
        => MaaSetImageEncoded(handle, data, size).ToBoolean();
}
