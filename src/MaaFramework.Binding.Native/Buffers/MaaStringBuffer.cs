using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa String Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaStringBuffer : MaaDisposableHandle<MaaStringBufferHandle>, IMaaStringBuffer<MaaStringBufferHandle>, IMaaStringBufferStatic<MaaStringBufferHandle>
{
    /// <inheritdoc/>
    public override string ToString()
    {
        _ = TryGetValue(out var str);
        return str;
    }

    /// <inheritdoc/>
    public bool TryCopyTo(MaaStringBufferHandle bufferHandle) => MaaStringBufferSetExFromNint(
            handle: bufferHandle,
            str: MaaStringBufferGetToNint(Handle),
            size: MaaStringBufferSize(Handle));

    /// <inheritdoc/>
    public bool TryCopyTo(IMaaStringBuffer buffer) => buffer switch
    {
        MaaStringBuffer native => MaaStringBufferSetExFromNint(
            handle: native.Handle,
            str: MaaStringBufferGetToNint(Handle),
            size: MaaStringBufferSize(Handle)),
        null => false,
        _ => buffer.TryGetValue(out var str) && buffer.TrySetValue(str),
    };

    /// <summary>
    ///     Creates a <see cref="MaaStringBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaStringBufferHandle.</param>
    public MaaStringBuffer(MaaStringBufferHandle handle)
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc cref="MaaStringBuffer(nint)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringBufferCreate"/>.
    /// </remarks>
    public MaaStringBuffer()
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(MaaStringBufferCreate(), needReleased: true);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringBufferDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaStringBufferDestroy(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringBufferIsEmpty"/>.
    /// </remarks>
    public bool IsEmpty => MaaStringBufferIsEmpty(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringBufferSize"/>.
    /// </remarks>
    public ulong Size => MaaStringBufferSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringBufferClear"/>.
    /// </remarks>
    public bool TryClear()
        => MaaStringBufferClear(Handle);

    /// <inheritdoc/>
    public bool TryGetValue(out string str)
        => TryGetValue(Handle, out str);

    /// <inheritdoc/>
    public static bool TryGetValue(MaaStringBufferHandle handle, out string str)
    {
        if (handle == default)
        {
            str = string.Empty;
            return false;
        }

        str = MaaStringBufferIsEmpty(handle) ? string.Empty : MaaStringBufferGetToNint(handle).ToStringUtf8(MaaStringBufferSize(handle));
        return true;
    }

    /// <inheritdoc/>
    public static bool TryGetValue(out string str, Func<MaaStringBufferHandle, bool> writeBuffer)
    {
        ArgumentNullException.ThrowIfNull(writeBuffer);
        var handle = MaaStringBufferCreate();
        if (!writeBuffer.Invoke(handle))
        {
            str = string.Empty;
            MaaStringBufferDestroy(handle);
            return false;
        }

        var ret = TryGetValue(handle, out str);
        MaaStringBufferDestroy(handle);
        return ret;
    }

    /// <inheritdoc/>
    public bool TrySetValue(string str, bool useEx = true)
        => useEx ? MaaStringBufferSetEx(Handle, str) : MaaStringBufferSet(Handle, str);

    /// <inheritdoc/>
    public static bool TrySetValue(MaaStringBufferHandle handle, string str, bool useEx = true)
        => useEx ? MaaStringBufferSetEx(handle, str) : MaaStringBufferSet(handle, str);

    /// <inheritdoc/>
    public static bool TrySetValue(string str, bool useEx, Func<MaaStringBufferHandle, bool> readBuffer)
    {
        ArgumentNullException.ThrowIfNull(readBuffer);
        var handle = MaaStringBufferCreate();
        if (!TrySetValue(handle, str, useEx))
        {
            MaaStringBufferDestroy(handle);
            return false;
        }

        var ret = readBuffer.Invoke(handle);
        MaaStringBufferDestroy(handle);
        return ret;
    }
}
