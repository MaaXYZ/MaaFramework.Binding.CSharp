using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Image List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaImageList : MaaDisposableHandle<nint>, IMaaList<nint, MaaImageBuffer>
{
    /// <inheritdoc cref="MaaImageList(nint)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreateImageListBuffer"/>.
    /// </remarks>
    public MaaImageList()
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(MaaCreateImageListBuffer(), needReleased: true);
    }

    /// <summary>
    ///     Creates a <see cref="MaaImageList"/> instance.
    /// </summary>
    /// <param name="handle">The MaaImageListBufferHandle.</param>
    public MaaImageList(MaaImageListBufferHandle handle)
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDestroyImageListBuffer"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaDestroyImageListBuffer(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaIsImageListEmpty"/>.
    /// </remarks>
    public bool IsEmpty => MaaIsImageListEmpty(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaClearImageList"/>.
    /// </remarks>
    public void Clear()
    {
        if (!MaaClearImageList(Handle).ToBoolean())
            throw new MaaException();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageListSize"/>.
    /// </remarks>
    public int Count => (int)MaaGetImageListSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetImageListAt"/>.
    /// </remarks>
    public MaaImageBuffer this[int index]
    {
        get
        {
            if ((uint)index >= (uint)Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return new(MaaGetImageListAt(Handle, (ulong)index));
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListAppend"/>.
    /// </remarks>
    public void Add(MaaImageBuffer item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!MaaImageListAppend(Handle, item.Handle).ToBoolean())
            throw new MaaException();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListRemove"/>.
    /// </remarks>
    public void RemoveAt(int index)
    {
        if (!MaaImageListRemove(Handle, (ulong)index).ToBoolean())
            throw new ArgumentOutOfRangeException(nameof(index));
    }

    /// <inheritdoc/>
    public void CopyTo(MaaImageBuffer[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);

        var count = MaaGetImageListSize(Handle);
        for (ulong i = 0; i < count; i++)
            array[arrayIndex + (int)i] = new MaaImageBuffer(MaaGetImageListAt(Handle, i));
    }

    #region Explicit Interface Implementations
#pragma warning disable CA1033 // 接口方法应可由子类型调用

    MaaImageBuffer IList<MaaImageBuffer>.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

    bool ICollection<MaaImageBuffer>.IsReadOnly => false;

    bool ICollection<MaaImageBuffer>.Contains(MaaImageBuffer item)
    {
        var count = MaaGetImageListSize(Handle);
        for (ulong i = 0; i < count; i++)
            if (MaaGetImageListAt(Handle, i) == item.Handle)
                return true;
        return false;
    }

    IEnumerator<MaaImageBuffer> IEnumerable<MaaImageBuffer>.GetEnumerator()
        => new MaaListEnumerator<MaaImageBuffer>(
        i => new(MaaGetImageListAt(Handle, i)),
        () => MaaGetImageListSize(Handle));

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        => new MaaListEnumerator<MaaImageBuffer>(
        i => new(MaaGetImageListAt(Handle, i)),
        () => MaaGetImageListSize(Handle));

    int IList<MaaImageBuffer>.IndexOf(MaaImageBuffer item)
    {
        var count = MaaGetImageListSize(Handle);
        for (ulong i = 0; i < count; i++)
            if (MaaGetImageListAt(Handle, i) == item.Handle)
                return (int)i;
        return -1;
    }

    void IList<MaaImageBuffer>.Insert(int index, MaaImageBuffer item)
    {
        throw new NotSupportedException();
    }

    bool ICollection<MaaImageBuffer>.Remove(MaaImageBuffer item)
    {
        var count = MaaGetImageListSize(Handle);
        for (ulong i = 0; i < count; i++)
            if (MaaGetImageListAt(Handle, i) == item.Handle)
                return MaaImageListRemove(Handle, i).ToBoolean();
        return false;
    }

    #endregion
}
