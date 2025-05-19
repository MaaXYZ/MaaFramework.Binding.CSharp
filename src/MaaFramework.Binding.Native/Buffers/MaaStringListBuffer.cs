using MaaFramework.Binding.Interop.Native;
using System.Diagnostics.CodeAnalysis;
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
    public override bool TryIndexOf(MaaStringBuffer item, out MaaSize index)
    {
        if (MaaStringBuffer.TryGetValue(item?.Handle ?? nint.Zero, out var stringInItem))
        {
            var count = MaaSizeCount;
            for (MaaSize tmpIndex = 0; tmpIndex < count; tmpIndex++)
            {
                if (MaaStringBuffer.TryGetValue(MaaStringListBufferAt(Handle, tmpIndex), out var stringInList)
                    && stringInList.Equals(stringInItem))
                {
                    index = tmpIndex;
                    return true;
                }
            }
        }

        index = 0;
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
    public static bool TryGetList(MaaStringListBufferHandle handle, [MaybeNullWhen(false)] out IList<string> stringList)
    {
        if (handle == default)
        {
            stringList = default;
            return false;
        }

        var size = (int)MaaStringListBufferSize(handle);
        if (size < 0 || size > Array.MaxLength)
        {
            stringList = Array.Empty<string>();
            return false;
        }

        var array = size == 0 ? [] : new string[size];
        for (var i = 0; i < size; i++)
        {
            var buffer = MaaStringListBufferAt(handle, (MaaSize)i);
            if (!MaaStringBuffer.TryGetValue(buffer, out var str))
            {
                stringList = Array.Empty<string>();
                return false;
            }
            array[i] = str;
        }

        stringList = array;
        return true;
    }

    /// <inheritdoc/>
    public static bool TryGetList([MaybeNullWhen(false)] out IList<string> stringList, Func<MaaStringListBufferHandle, bool> writeBuffer)
    {
        ArgumentNullException.ThrowIfNull(writeBuffer);
        var handle = MaaStringListBufferCreate();
        try
        {
            if (!writeBuffer.Invoke(handle))
            {
                stringList = default;
                return false;
            }

            return TryGetList(handle, out stringList);
        }
        finally
        {
            MaaStringListBufferDestroy(handle);
        }
    }

    /// <inheritdoc/>
    public static bool TrySetList(MaaStringListBufferHandle handle, IEnumerable<string> stringList)
    {
        var buffer = MaaStringBufferCreate();
        try
        {
            if (stringList is string[] array)
            {
                foreach (var str in array)
                {
                    if (!MaaStringBuffer.TrySetValue(buffer, str) || !MaaStringListBufferAppend(handle, buffer))
                        return false;
                }
                return true;
            }

            return stringList.All(str => MaaStringBuffer.TrySetValue(buffer, str) && MaaStringListBufferAppend(handle, buffer));
        }
        finally
        {
            MaaStringBufferDestroy(buffer);
        }
    }

    /// <inheritdoc/>
    public static bool TrySetList(IEnumerable<string> stringList, Func<MaaStringListBufferHandle, bool> readBuffer)
    {
        ArgumentNullException.ThrowIfNull(readBuffer);
        var handle = MaaStringListBufferCreate();
        try
        {
            return TrySetList(handle, stringList) && readBuffer.Invoke(handle);
        }
        finally
        {
            MaaStringListBufferDestroy(handle);
        }
    }
}
