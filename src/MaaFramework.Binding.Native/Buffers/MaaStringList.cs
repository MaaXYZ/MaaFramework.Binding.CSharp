using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa String List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaStringList : MaaDisposableHandle<nint>, IMaaList<nint, MaaStringBuffer>
{
    /// <inheritdoc cref="MaaStringList(nint)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreateStringListBuffer"/>.
    /// </remarks>
    public MaaStringList()
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(MaaCreateStringListBuffer(), needReleased: true);
    }

    /// <summary>
    ///     Creates a <see cref="MaaStringList"/> instance.
    /// </summary>
    /// <param name="handle">The MaaStringListBufferHandle.</param>
    public MaaStringList(MaaStringListBufferHandle handle)
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDestroyStringListBuffer"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaDestroyStringListBuffer(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaIsStringListEmpty"/>.
    /// </remarks>
    public bool IsEmpty => MaaIsStringListEmpty(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaClearStringList"/>.
    /// </remarks>
    public void Clear()
    {
        if (!MaaClearStringList(Handle).ToBoolean())
            throw new MaaException();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetStringListSize"/>.
    /// </remarks>
    public int Count => (int)MaaGetStringListSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetStringListAt"/>.
    /// </remarks>
    public MaaStringBuffer this[int index]
    {
        get
        {
            if ((uint)index >= (uint)Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return new MaaStringBuffer(MaaGetStringListAt(Handle, (ulong)index));
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListAppend"/>.
    /// </remarks>
    public void Add(MaaStringBuffer item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!MaaStringListAppend(Handle, item.Handle).ToBoolean())
            throw new MaaException();
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListRemove"/>.
    /// </remarks>
    public void RemoveAt(int index)
    {
        if (!MaaStringListRemove(Handle, (ulong)index).ToBoolean())
            throw new ArgumentOutOfRangeException(nameof(index));
    }

    /// <inheritdoc/>
    public void CopyTo(MaaStringBuffer[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);

        var count = MaaGetStringListSize(Handle);
        for (ulong i = 0; i < count; i++)
            array[arrayIndex + (int)i] = new MaaStringBuffer(MaaGetStringListAt(Handle, i));
    }

    #region Explicit Interface Implementations
#pragma warning disable CA1033 // 接口方法应可由子类型调用

    MaaStringBuffer IList<MaaStringBuffer>.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

    bool ICollection<MaaStringBuffer>.IsReadOnly => false;

    bool ICollection<MaaStringBuffer>.Contains(MaaStringBuffer item)
    {
        var count = MaaGetStringListSize(Handle);
        for (ulong i = 0; i < count; i++)
            if (MaaGetStringListAt(Handle, i) == item.Handle)
                return true;
        return false;
    }

    IEnumerator<MaaStringBuffer> IEnumerable<MaaStringBuffer>.GetEnumerator()
        => new MaaListEnumerator<MaaStringBuffer>(
        i => new MaaStringBuffer(MaaGetStringListAt(Handle, i)),
        () => MaaGetStringListSize(Handle));

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        => new MaaListEnumerator<MaaStringBuffer>(
        i => new MaaStringBuffer(MaaGetStringListAt(Handle, i)),
        () => MaaGetStringListSize(Handle));

    int IList<MaaStringBuffer>.IndexOf(MaaStringBuffer item)
    {
        var count = MaaGetStringListSize(Handle);
        for (ulong i = 0; i < count; i++)
            if (MaaGetStringListAt(Handle, i) == item.Handle)
                return (int)i;
        return -1;
    }

    void IList<MaaStringBuffer>.Insert(int index, MaaStringBuffer item)
    {
        throw new NotSupportedException();
    }

    bool ICollection<MaaStringBuffer>.Remove(MaaStringBuffer item)
    {
        var count = MaaGetStringListSize(Handle);
        for (ulong i = 0; i < count; i++)
            if (MaaGetStringListAt(Handle, i) == item.Handle)
                return MaaStringListRemove(Handle, i).ToBoolean();
        return false;
    }

    #endregion
}
