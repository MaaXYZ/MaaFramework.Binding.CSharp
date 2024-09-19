using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Image List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaImageListBuffer : MaaListBuffer<nint, MaaImageBuffer>
{
    /// <summary>
    ///     Creates a <see cref="MaaImageListBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaImageListBufferHandle.</param>
    public MaaImageListBuffer(MaaImageListBufferHandle handle) : base(nint.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc cref="MaaImageListBuffer(MaaImageListBufferHandle)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferCreate"/>.
    /// </remarks>
    public MaaImageListBuffer() : base(nint.Zero)
    {
        SetHandle(MaaImageListBufferCreate(), needReleased: true);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaImageListBufferDestroy(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferIsEmpty"/>.
    /// </remarks>
    public override bool IsEmpty => MaaImageListBufferIsEmpty(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferSize"/>.
    /// </remarks>
    public override MaaSize MaaSizeCount => MaaImageListBufferSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferAt"/>.
    /// </remarks>
    public override MaaImageBuffer this[MaaSize index] => new(MaaImageListBufferAt(Handle, index));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferAppend"/>.
    /// </remarks>
    public override bool Add(MaaImageBuffer item)
        => item is not null
           && MaaImageListBufferAppend(Handle, item.Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferRemove"/>.
    /// </remarks>
    public override bool RemoveAt(MaaSize index)
        => MaaImageListBufferRemove(Handle, index).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferClear"/>.
    /// </remarks>
    public override bool Clear()
        => MaaImageListBufferClear(Handle).ToBoolean();

    /// <inheritdoc/>
    public override bool IsReadOnly => false;

    /// <inheritdoc/>
    public override bool TryIndexOf(MaaImageBuffer item, out ulong index)
    {
        index = 0;
        if (item is null) return false;
        var count = MaaSizeCount;
        for (; index < count; index++)
            if (MaaImageListBufferAt(Handle, index).Equals(item.Handle))
                return true;

        return false;
    }
}
