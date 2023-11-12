using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Native.Interop;
using static MaaFramework.Binding.Native.Interop.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Image Buffer section of <see cref="MaaFramework.Binding.Native.Interop.MaaBuffer"/>.
/// </summary>
public class MaaImageBuffer : MaaDisposableHandle, IMaaImageBuffer
{
    /// <inheritdoc cref="MaaImageBuffer(nint)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreateImageBuffer"/>.
    /// </remarks>
    public MaaImageBuffer()
        : this(MaaCreateImageBuffer())
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaImageBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaImageBufferHandle.</param>
    public MaaImageBuffer(MaaImageBufferHandle handle)
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(handle);
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
    public bool IsEmpty()
        => MaaIsImageEmpty(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaClearImage"/>.
    /// </remarks>
    public bool Clear()
        => MaaClearImage(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageRawData"/>.
    /// </remarks>
    public MaaImageRawData GetRawData()
        => MaaGetImageRawData(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageWidth"/>.
    /// </remarks>
    public int Width => MaaGetImageWidth(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageHeight"/>.
    /// </remarks>
    public int Height => MaaGetImageHeight(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageType"/>.
    /// </remarks>
    public int Type => MaaGetImageType(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetImageRawData"/>.
    /// </remarks>
    public bool SetRawData(MaaImageRawData data, int width, int height, int type)
        => MaaSetImageRawData(Handle, data, width, height, type).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageEncoded"/>.
    /// </remarks>
    public MaaImageEncodedData GetEncodedData()
        => MaaGetImageEncoded(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageEncodedSize"/>.
    /// </remarks>
    public ulong Size => MaaGetImageEncodedSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetImageEncoded"/>.
    /// </remarks>
    public bool SetEncodedData(MaaImageBufferHandle data, ulong size)
        => MaaSetImageEncoded(Handle, data, size).ToBoolean();
}
