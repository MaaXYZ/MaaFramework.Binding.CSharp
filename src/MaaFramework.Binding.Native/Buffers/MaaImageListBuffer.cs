using MaaFramework.Binding.Interop.Native;
using System.Collections.Concurrent;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Image List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaImageListBuffer : MaaListBuffer<MaaImageListBufferHandle, MaaImageBuffer>
    , IMaaImageListBufferStatic<MaaImageListBufferHandle>
{
    private readonly ConcurrentDictionary<MaaSize, MaaImageBuffer> _cache = [];
    private void ClearCache()
    {
        foreach (var buffer in _cache.Values)
        {
            buffer.ThrowOnInvalid = true;
            buffer.Dispose();
        }
        _cache.Clear();
    }
    private void RemoveCache(MaaSize index)
    {
        foreach (var key in _cache.Keys.Where(x => x >= index))
        {
            if (_cache.TryRemove(key, out var buffer))
            {
                buffer.ThrowOnInvalid = true;
                buffer.Dispose();
            }
        }
    }

    /// <inheritdoc/>
    // 涉及到 TrAdd、TryClear、Remove、Dispose 均需要 Dispose _cache.Values。
    // 由于 ReleaseHandle 有先决条件，故重写 Dispose()方法。
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        ClearCache();
    }

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
    public override MaaImageBuffer this[MaaSize index] => _cache.GetOrAdd(index, i
        => new(MaaImageListBufferAt(Handle, i).ThrowIfEquals(nint.Zero)));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferAppend"/>.
    /// </remarks>
    public override bool TryAdd(MaaImageBuffer item)
    {
        if (IsEmpty)
        {
            return item is not null && MaaImageListBufferAppend(Handle, item.Handle);
        }

        var first = MaaImageListBufferAt(Handle, 0);
        var ret = item is not null && MaaImageListBufferAppend(Handle, item.Handle);
        // if std::vector<T> expanded
        if (ret && first != MaaImageListBufferAt(Handle, 0))
            ClearCache();
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferRemove"/>.
    /// </remarks>
    public override bool TryRemoveAt(MaaSize index)
    {
        if (MaaSizeCount <= index || !MaaImageListBufferRemove(Handle, index))
            return false;

        RemoveCache(index);
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
        return true;
    }

    /// <inheritdoc/>
    public override bool IsReadOnly => false;

    /// <inheritdoc/>
    public override bool TryIndexOf(MaaImageBuffer item, out MaaSize index)
    {
        index = 0;
        if (item is null)
            return false;

        var imageInItem = MaaImageBufferGetRawData(item.Handle);
        if (imageInItem == nint.Zero)
            return false;

        var count = MaaSizeCount;
        while (index < count)
        {
            var imageInList = MaaImageBufferGetRawData(MaaImageListBufferAt(Handle, index));
            if (imageInList == imageInItem)
                return true;

            index++;
        }

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
    public static bool TryGetEncodedDataList(MaaImageListBufferHandle handle, out IList<byte[]> dataList)
    {
        var ret = false;
        var count = (int)MaaImageListBufferSize(handle);
        var array = (count > 0 && count < Array.MaxLength) ? new byte[count][] : [];
        for (var i = 0; i < count; i++)
        {
            ret |= MaaImageBuffer.TryGetEncodedData(MaaImageListBufferAt(handle, (MaaSize)i), out byte[] data);
            array[i] = data;
        }

        dataList = array;
        return ret;
    }

    /// <inheritdoc/>
    public static bool TryGetEncodedDataList(out IList<byte[]> dataList, Func<MaaImageListBufferHandle, bool> writeBuffer)
    {
        ArgumentNullException.ThrowIfNull(writeBuffer);
        var handle = MaaImageListBufferCreate();
        if (!writeBuffer.Invoke(handle))
        {
            dataList = Array.Empty<byte[]>();
            MaaImageListBufferDestroy(handle);
            return false;
        }

        var ret = TryGetEncodedDataList(handle, out dataList);
        MaaImageListBufferDestroy(handle);
        return ret;
    }

    /// <inheritdoc/>
    public static bool TrySetEncodedDataList(MaaImageListBufferHandle handle, IEnumerable<byte[]> dataList)
    {
        var ret = true;
        var buffer = MaaImageBufferCreate();
        if (dataList is byte[][] array)
        {
            foreach (var data in array)
            {
                ret &= MaaImageBuffer.TrySetEncodedData(buffer, data) && MaaImageListBufferAppend(handle, buffer);
            }
            MaaImageBufferDestroy(buffer);
            return ret;
        }

        ret = dataList.All(
            data => MaaImageBuffer.TrySetEncodedData(buffer, data) && MaaImageListBufferAppend(handle, buffer));
        MaaImageBufferDestroy(buffer);
        return ret;
    }

    /// <inheritdoc/>
    public static bool TrySetEncodedDataList(MaaImageListBufferHandle handle, IEnumerable<Stream> dataList)
    {
        var ret = true;
        var buffer = MaaImageBufferCreate();
        if (dataList is Stream[] array)
        {
            foreach (var data in array)
            {
                ret &= MaaImageBuffer.TrySetEncodedData(buffer, data) && MaaImageListBufferAppend(handle, buffer);
            }
            MaaImageBufferDestroy(buffer);
            return ret;
        }

        ret = dataList.All(
            data => MaaImageBuffer.TrySetEncodedData(buffer, data) && MaaImageListBufferAppend(handle, buffer));
        MaaImageBufferDestroy(buffer);
        return ret;
    }

    /// <inheritdoc/>
    public static bool TrySetEncodedDataList(IEnumerable<byte[]> dataList, Func<MaaImageListBufferHandle, bool> readBuffer)
    {
        ArgumentNullException.ThrowIfNull(readBuffer);
        var handle = MaaImageListBufferCreate();
        if (!TrySetEncodedDataList(handle, dataList))
        {
            MaaImageListBufferDestroy(handle);
            return false;
        }

        var ret = readBuffer.Invoke(handle);
        MaaImageListBufferDestroy(handle);
        return ret;
    }

    /// <inheritdoc/>
    public static bool TrySetEncodedDataList(IEnumerable<Stream> dataList, Func<MaaImageListBufferHandle, bool> readBuffer)
    {
        ArgumentNullException.ThrowIfNull(readBuffer);
        var handle = MaaImageListBufferCreate();
        if (!TrySetEncodedDataList(handle, dataList))
        {
            MaaImageListBufferDestroy(handle);
            return false;
        }

        var ret = readBuffer.Invoke(handle);
        MaaImageListBufferDestroy(handle);
        return ret;
    }
}
