using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa String List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaStringListBuffer : MaaListBuffer<MaaStringListBufferHandle, MaaStringBuffer>
    , IMaaStringListBufferStatic<MaaStringListBufferHandle>
{
    /// <summary>
    ///     Creates a <see cref="MaaStringListBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaStringListBufferHandle.</param>
    public MaaStringListBuffer(MaaStringListBufferHandle handle) : base(nint.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc cref="MaaStringListBuffer(MaaStringListBufferHandle)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListBufferCreate"/>.
    /// </remarks>
    public MaaStringListBuffer() : base(nint.Zero)
    {
        SetHandle(MaaStringListBufferCreate(), needReleased: true);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListBufferDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaStringListBufferDestroy(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListBufferIsEmpty"/>.
    /// </remarks>
    public override bool IsEmpty => MaaStringListBufferIsEmpty(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListBufferSize"/>.
    /// </remarks>
    public override MaaSize MaaSizeCount => MaaStringListBufferSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListBufferAt"/>.
    /// </remarks>
    public override MaaStringBuffer this[MaaSize index] => new(MaaStringListBufferAt(Handle, index).ThrowIfEquals(nint.Zero));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListBufferAppend"/>.
    /// </remarks>
    public override bool TryAdd(MaaStringBuffer item)
        => item is not null
           && MaaStringListBufferAppend(Handle, item.Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListBufferRemove"/>.
    /// </remarks>
    public override bool TryRemoveAt(MaaSize index)
        => MaaStringListBufferRemove(Handle, index);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListBufferClear"/>.
    /// </remarks>
    public override bool TryClear()
        => MaaStringListBufferClear(Handle);

    /// <inheritdoc/>
    public override bool IsReadOnly => false;

    /// <inheritdoc/>
    public override bool TryIndexOf(MaaStringBuffer item, out ulong index)
    {
        index = 0;
        if (item is null) return false;
        var count = MaaSizeCount;
        for (index = 0; index < count; index++)
            if (MaaStringListBufferAt(Handle, index).Equals(item.Handle))
                return true;

        return false;
    }

    /// <inheritdoc/>
    public override bool TryCopyTo(MaaStringListBufferHandle bufferHandle)
    {
        var count = MaaStringListBufferSize(Handle);
        if (count > MaaSize.MaxValue - MaaStringListBufferSize(bufferHandle))
            return false;

        for (MaaSize index = 0; index < count; index++)
        {
            var item = MaaStringListBufferAt(Handle, index);
            if (!MaaStringListBufferAppend(bufferHandle, item))
                return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public static bool TryGetList(MaaStringListBufferHandle handle, out IList<string> stringList)
    {
        var ret = false;
        var count = (int)MaaStringListBufferSize(handle);
        var array = (count > 0 && count < Array.MaxLength) ? new string[count] : [];
        for (var i = 0; i < count; i++)
        {
            ret |= MaaStringBuffer.TryGetValue(MaaStringListBufferAt(handle, (MaaSize)i), out var str);
            array[i] = str;
        }

        stringList = array;
        return ret;
    }

    /// <inheritdoc/>
    public static bool TryGetList(out IList<string> stringList, Func<MaaStringListBufferHandle, bool> writeBuffer)
    {
        ArgumentNullException.ThrowIfNull(writeBuffer);
        var handle = MaaStringListBufferCreate();
        if (!writeBuffer.Invoke(handle))
        {
            stringList = Array.Empty<string>();
            MaaStringListBufferDestroy(handle);
            return false;
        }

        var ret = TryGetList(handle, out stringList);
        MaaStringListBufferDestroy(handle);
        return ret;
    }

    /// <inheritdoc/>
    public static bool TrySetList(MaaStringListBufferHandle handle, IEnumerable<string> stringList)
    {
        var ret = true;
        var buffer = MaaStringBufferCreate();
        if (stringList is string[] array)
        {
            foreach (var str in array)
            {
                ret &= MaaStringBuffer.TrySetValue(buffer, str) && MaaStringListBufferAppend(handle, buffer);
            }
            MaaStringBufferDestroy(buffer);
            return ret;
        }

        ret = stringList.All(
            str => MaaStringBuffer.TrySetValue(buffer, str) && MaaStringListBufferAppend(handle, buffer));
        MaaStringBufferDestroy(buffer);
        return ret;
    }

    /// <inheritdoc/>
    public static bool TrySetList(IEnumerable<string> stringList, Func<MaaStringListBufferHandle, bool> readBuffer)
    {
        ArgumentNullException.ThrowIfNull(readBuffer);
        var handle = MaaStringListBufferCreate();
        if (!TrySetList(handle, stringList))
        {
            MaaStringListBufferDestroy(handle);
            return false;
        }

        var ret = readBuffer.Invoke(handle);
        MaaStringListBufferDestroy(handle);
        return ret;
    }
}
