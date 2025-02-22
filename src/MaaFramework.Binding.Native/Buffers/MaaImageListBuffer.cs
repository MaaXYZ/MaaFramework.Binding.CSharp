using MaaFramework.Binding.Interop.Native;
using System.Collections.Concurrent;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Image List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaImageListBuffer : MaaListBuffer<nint, MaaImageBuffer>
{
    private readonly ConcurrentDictionary<MaaSize, MaaImageBuffer> _cache = [];

    /// <inheritdoc/>
    // 涉及到 Clear、Remove、Dispose 均需要 Dispose _cache.Values。
    // 由于 ReleaseHandle 有先决条件，故重写 Dispose()方法。
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        foreach (var value in _cache.Values)
        {
            value.Dispose();
        }
        _cache.Clear();
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
    public override bool Add(MaaImageBuffer item)
        => item is not null
           && MaaImageListBufferAppend(Handle, item.Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferRemove"/>.
    /// </remarks>
    public override bool RemoveAt(MaaSize index)
    {
        if (MaaSizeCount <= index || !MaaImageListBufferRemove(Handle, index))
            return false;

        foreach (var key in _cache.Keys)
        {
            if (key < index)
                continue;
            if (_cache.TryRemove(key, out var buffer))
                buffer?.Dispose();
        }
        return true;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaImageListBufferClear"/>.
    /// </remarks>
    public override bool Clear()
    {
        if (!MaaImageListBufferClear(Handle))
            return false;

        foreach (var value in _cache.Values)
        {
            value.Dispose();
        }

        _cache.Clear();
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
        if (imageInItem.Equals(nint.Zero))
            return false;

        var count = MaaSizeCount;
        while (index < count)
        {
            var imageInList = MaaImageBufferGetRawData(
                MaaImageListBufferAt(Handle, index));
            if (imageInList.Equals(imageInItem))
                return true;
            else
                index++;
        }

        return false;
    }

    /// <inheritdoc/>
    public override bool CopyTo(MaaImageListBufferHandle bufferHandle)
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

    /// <summary>
    ///     Gets a MaaImageBuffer list from a MaaImageListBufferHandle.
    /// </summary>
    /// <param name="handle">The MaaImageListBufferHandle.</param>
    /// <returns>The list of UnmanagedMemoryStream from MaaImageBuffer.</returns>
    public static IList<UnmanagedMemoryStream> Get(MaaImageListBufferHandle handle)
    {
        var count = MaaImageListBufferSize(handle);
        if (count <= int.MaxValue)
        {
            return Enumerable.Range(0, (int)count)
                .Select(index => MaaImageBuffer.Get(MaaImageListBufferAt(handle, (MaaSize)index)))
                .ToList();
        }

        var list = new List<UnmanagedMemoryStream>();
        for (MaaSize index = 0; index < count; index++)
            list.Add(MaaImageBuffer.Get(MaaImageListBufferAt(handle, index)));
        return list;
    }

    /// <summary>
    ///     Gets a MaaImageBuffer list from a MaaImageListBufferHandle.
    /// </summary>
    /// <param name="list">The list of UnmanagedMemoryStream from MaaImageBuffer.</param>
    /// <param name="func">A function that takes a MaaImageListBufferHandle and returns a boolean indicating success or failure.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool Get(out IList<UnmanagedMemoryStream> list, Func<MaaImageListBufferHandle, bool> func)
    {
        var h = MaaImageListBufferCreate();
        var ret = func?.Invoke(h) ?? false;
        list = Get(h);
        MaaImageListBufferDestroy(h);
        return ret;
    }

    /// <summary>
    ///     Sets a image encoded data stream <paramref name="list"/> to a MaaImageListBufferHandle.
    /// </summary>
    /// <param name="handle">The MaaImageListBufferHandle.</param>
    /// <param name="list">The image encoded data stream list.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool Set(MaaImageListBufferHandle handle, IEnumerable<Stream> list)
    {
        var h = MaaImageBufferCreate();
        var ret = list.All(s => MaaImageBuffer.Set(h, s) && MaaImageListBufferAppend(handle, h));
        MaaImageBufferDestroy(h);
        return ret;
    }

    /// <summary>
    ///     Sets a image encoded data stream <paramref name="list"/> to a MaaImageListBufferHandle,
    /// then calls a <paramref name="func"/> to use the MaaImageListBufferHandle.
    /// </summary>
    /// <param name="list">The image encoded data stream list.</param>
    /// <param name="func">A function that takes a MaaImageListBufferHandle and returns a boolean indicating success or failure.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool Set(IEnumerable<Stream> list, Func<MaaImageListBufferHandle, bool> func)
    {
        if (func is null) return false;
        var h = MaaImageListBufferCreate();
        var ret = Set(h, list) && func.Invoke(h);
        MaaImageListBufferDestroy(h);
        return ret;
    }
}
