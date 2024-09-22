using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa String List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaStringListBuffer : MaaListBuffer<nint, MaaStringBuffer>
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
    public override bool IsEmpty => MaaStringListBufferIsEmpty(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListBufferSize"/>.
    /// </remarks>
    public override MaaSize MaaSizeCount => MaaStringListBufferSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListBufferAt"/>.
    /// </remarks>
    public override MaaStringBuffer this[MaaSize index] => new(MaaStringListBufferAt(Handle, index));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListBufferAppend"/>.
    /// </remarks>
    public override bool Add(MaaStringBuffer item)
        => item is not null
           && MaaStringListBufferAppend(Handle, item.Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListBufferRemove"/>.
    /// </remarks>
    public override bool RemoveAt(MaaSize index)
        => MaaStringListBufferRemove(Handle, index).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringListBufferClear"/>.
    /// </remarks>
    public override bool Clear()
        => MaaStringListBufferClear(Handle).ToBoolean();

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

    /// <summary>
    ///     Gets a string list from a MaaStringListBufferHandle.
    /// </summary>
    /// <param name="handle">The MaaStringListBufferHandle.</param>
    /// <returns>The string list.</returns>
    public static IList<string> Get(MaaStringListBufferHandle handle)
    {
        var count = MaaStringListBufferSize(handle);
        if (count <= int.MaxValue)
        {
            return Enumerable.Range(0, (int)count)
                .Select(index => MaaStringBuffer.Get(MaaStringListBufferAt(handle, (MaaSize)index)))
                .ToList();
        }

        var list = new List<string>();
        for (MaaSize index = 0; index < count; index++)
            list.Add(MaaStringBuffer.Get(MaaStringListBufferAt(handle, index)));
        return list;
    }

    /// <summary>
    ///     Gets a string list from a MaaStringListBufferHandle.
    /// </summary>
    /// <param name="list">The string list.</param>
    /// <param name="func">A function that takes a MaaStringListBufferHandle and returns a boolean indicating success or failure.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool Get(out IList<string> list, Func<MaaStringListBufferHandle, bool> func)
    {
        var h = MaaStringListBufferCreate();
        var ret = func?.Invoke(h) ?? false;
        list = Get(h);
        MaaStringListBufferDestroy(h);
        return ret;
    }

    /// <summary>
    ///     Sets a string <paramref name="list"/> to a MaaStringListBufferHandle.
    /// </summary>
    /// <param name="handle">The MaaStringListBufferHandle.</param>
    /// <param name="list">The string list.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool Set(MaaStringListBufferHandle handle, IEnumerable<string> list)
        => list.All(s =>
        {
            var h = MaaStringBufferCreate();
            var ret = MaaStringBuffer.Set(h, s) && MaaStringListBufferAppend(handle, h).ToBoolean();
            MaaStringBufferDestroy(h);
            return ret;
        });

    /// <summary>
    ///     Sets a string <paramref name="list"/> to a MaaStringListBufferHandle,
    /// then calls a <paramref name="func"/> to use the MaaStringListBufferHandle.
    /// </summary>
    /// <param name="list">The string list.</param>
    /// <param name="func">A function that takes a MaaStringListBufferHandle and returns a boolean indicating success or failure.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool Set(IEnumerable<string> list, Func<MaaStringListBufferHandle, bool> func)
    {
        if (func is null) return false;
        var h = MaaStringListBufferCreate();
        var ret = Set(h, list) && func.Invoke(h);
        MaaStringListBufferDestroy(h);
        return ret;
    }
}
