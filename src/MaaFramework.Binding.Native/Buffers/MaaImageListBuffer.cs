using MaaFramework.Binding.Interop.Native;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Image List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaImageListBuffer : MaaListBuffer<MaaImageListBufferHandle, MaaImageBuffer>
    , IMaaImageListBufferStatic<MaaImageListBufferHandle>
{
    internal sealed class NullImageListBuffer : MaaImageListBuffer { internal NullImageListBuffer() : base(MaaImageListBufferHandle.Zero) { } }

    /// <summary>
    ///     Represents a null instance of the <see cref="MaaImageListBuffer"/> type.
    /// </summary>
    public static MaaImageListBuffer Null { get; } = new NullImageListBuffer();

    private readonly ConcurrentDictionary<MaaImageBuffer, MaaSize> _cache = [];
    private void ClearCache()
    {
        foreach (var buffer in _cache.Keys)
        {
            buffer.ThrowOnInvalid = true;
            buffer.Dispose();
        }
        _cache.Clear();
    }
    private void RemoveCache(MaaSize index)
    {
        foreach (var pair in _cache.Where(x => x.Value >= index))
        {
            _ = _cache.TryRemove(pair);
            pair.Key.ThrowOnInvalid = true;
            pair.Key.Dispose();
        }
    }

    /// <summary>
    ///     Creates a <see cref="MaaImageListBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaImageListBufferHandle.</param>
    public MaaImageListBuffer(MaaImageListBufferHandle handle) : base(MaaImageListBufferHandle.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc cref="MaaImageListBuffer(MaaImageListBufferHandle)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferCreate"/>.
    /// </remarks>
    public MaaImageListBuffer() : base(MaaImageListBufferHandle.Zero)
    {
        SetHandle(MaaImageListBufferCreate(), needReleased: true);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle(MaaImageListBufferHandle handle)
    {
        try
        {
            ClearCache();
        }
        finally
        {
            MaaImageListBufferDestroy(handle);
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferIsEmpty"/>.
    /// </remarks>
    public override bool IsEmpty => MaaImageListBufferIsEmpty(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferSize"/>.
    /// </remarks>
    public override MaaSize MaaSizeCount => MaaImageListBufferSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferAt"/>.
    /// </remarks>
    // Prohibit internal use of this method unless it is returned as a return value.
    public override MaaImageBuffer this[MaaSize index]
    {
        get
        {
            var handle = MaaImageListBufferAt(Handle, index).ThrowIfEquals(MaaImageBufferHandle.Zero);
            var buffer = new MaaImageBuffer(handle);
            _cache[buffer] = index;
            return buffer;
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferAppend"/>.
    /// </remarks>
    public override bool TryAdd(MaaImageBuffer item)
    {
        if (item is null)
            return false;
        if (IsEmpty)
            return MaaImageListBufferAppend(Handle, item.Handle);

        var first = MaaImageListBufferAt(Handle, 0);
        var ret = MaaImageListBufferAppend(Handle, item.Handle);
        // if std::vector<T> expanded
        if (ret && first != MaaImageListBufferAt(Handle, 0))
            ClearCache();

        _version++;
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferRemove"/>.
    /// </remarks>
    public override bool TryRemoveAt(MaaSize index)
    {
        if (!MaaImageListBufferRemove(Handle, index))
            return false;

        RemoveCache(index);
        _version++;
        return true;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferClear"/>.
    /// </remarks>
    public override bool TryClear()
    {
        if (!MaaImageListBufferClear(Handle))
            return false;

        ClearCache();
        _version++;
        return true;
    }

    /// <inheritdoc/>
    public override bool IsReadOnly => false;

    /// <inheritdoc/>
    public override bool TryIndexOf(MaaImageBuffer item, out MaaSize index)
    {
        if (MaaImageBuffer.TryGetRawData(item?.Handle ?? MaaImageBufferHandle.Zero, out var imageInItem))
        {
            var count = MaaSizeCount;
            for (MaaSize tmpIndex = 0; tmpIndex < count; tmpIndex++)
            {
                if (MaaImageBuffer.TryGetRawData(MaaImageListBufferAt(Handle, tmpIndex), out var imageInList)
                    && imageInList.Equals(imageInItem))
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
    public override bool TryCopyTo(MaaImageListBufferHandle bufferHandle)
    {
        var count = MaaImageListBufferSize(Handle);
        if (count > MaaSize.MaxValue - MaaImageListBufferSize(bufferHandle))
            return false;

        for (MaaSize index = 0; index < count; index++)
        {
            var item = MaaImageListBufferAt(Handle, index);
            if (!MaaImageListBufferAppend(bufferHandle, item))
                return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public static bool TryGetEncodedDataList(MaaImageListBufferHandle handle, [MaybeNullWhen(false)] out IList<byte[]> dataList)
    {
        if (handle == default)
        {
            dataList = default;
            return false;
        }

        var size = (int)MaaImageListBufferSize(handle);
        if (size < 0 || size > Array.MaxLength)
        {
            dataList = Array.Empty<byte[]>();
            return false;
        }

        var array = size == 0 ? [] : new byte[size][];
        for (var i = 0; i < size; i++)
        {
            var buffer = MaaImageListBufferAt(handle, (MaaSize)i);
            if (!MaaImageBuffer.TryGetEncodedData(buffer, out byte[]? data))
            {
                dataList = Array.Empty<byte[]>();
                return false;
            }
            array[i] = data;
        }

        dataList = array;
        return true;
    }

    /// <inheritdoc/>
    public static bool TryGetEncodedDataList([MaybeNullWhen(false)] out IList<byte[]> dataList, Func<MaaImageListBufferHandle, bool> writeBuffer)
    {
        ArgumentNullException.ThrowIfNull(writeBuffer);
        var handle = MaaImageListBufferCreate();
        try
        {
            if (!writeBuffer.Invoke(handle))
            {
                dataList = default;
                return false;
            }

            return TryGetEncodedDataList(handle, out dataList);
        }
        finally
        {
            MaaImageListBufferDestroy(handle);
        }
    }

    /// <inheritdoc/>
    public static bool TrySetEncodedDataList(MaaImageListBufferHandle handle, IEnumerable<byte[]> dataList)
    {
        var buffer = MaaImageBufferCreate();
        try
        {
            if (dataList is byte[][] array)
            {
                foreach (var data in array)
                {
                    if (!MaaImageBuffer.TrySetEncodedData(buffer, data) || !MaaImageListBufferAppend(handle, buffer))
                        return false;
                }
                return true;
            }

            return dataList.All(data => MaaImageBuffer.TrySetEncodedData(buffer, data) && MaaImageListBufferAppend(handle, buffer));
        }
        finally
        {
            MaaImageBufferDestroy(buffer);
        }
    }

    /// <inheritdoc/>
    public static bool TrySetEncodedDataList(MaaImageListBufferHandle handle, IEnumerable<Stream> dataList)
    {
        var buffer = MaaImageBufferCreate();
        try
        {
            if (dataList is Stream[] array)
            {
                var ret = true;
                foreach (var data in array)
                {
                    if (!MaaImageBuffer.TrySetEncodedData(buffer, data) || !MaaImageListBufferAppend(handle, buffer))
                        return false;
                }
                return ret;
            }

            return dataList.All(data => MaaImageBuffer.TrySetEncodedData(buffer, data) && MaaImageListBufferAppend(handle, buffer));
        }
        finally
        {
            MaaImageBufferDestroy(buffer);
        }
    }

    /// <inheritdoc/>
    public static bool TrySetEncodedDataList(IEnumerable<byte[]> dataList, Func<MaaImageListBufferHandle, bool> readBuffer)
    {
        ArgumentNullException.ThrowIfNull(readBuffer);
        var handle = MaaImageListBufferCreate();
        try
        {
            return TrySetEncodedDataList(handle, dataList) && readBuffer.Invoke(handle);
        }
        finally
        {
            MaaImageListBufferDestroy(handle);
        }
    }

    /// <inheritdoc/>
    public static bool TrySetEncodedDataList(IEnumerable<Stream> dataList, Func<MaaImageListBufferHandle, bool> readBuffer)
    {
        ArgumentNullException.ThrowIfNull(readBuffer);
        var handle = MaaImageListBufferCreate();
        try
        {
            return TrySetEncodedDataList(handle, dataList) && readBuffer.Invoke(handle);
        }
        finally
        {
            MaaImageListBufferDestroy(handle);
        }
    }
}
