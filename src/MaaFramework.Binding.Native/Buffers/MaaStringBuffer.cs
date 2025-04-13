using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Interop.Native;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa String Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaStringBuffer : MaaDisposableHandle<MaaStringBufferHandle>, IMaaStringBuffer<MaaStringBufferHandle>, IMaaStringBufferStatic<MaaStringBufferHandle>
{
    /// <inheritdoc/>
    public override string ToString() => TryGetValue(out var str) ? str : string.Empty;

    /// <inheritdoc/>
    public bool TryCopyTo(MaaStringBufferHandle bufferHandle) => MaaStringBufferSetExFromNint(
            handle: bufferHandle,
            str: MaaStringBufferGetToNint(Handle),
            size: MaaStringBufferSize(Handle));

    /// <inheritdoc/>
    public bool TryCopyTo(IMaaStringBuffer buffer) => buffer switch
    {
        MaaStringBuffer native => TryCopyTo(native.Handle),
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
    public bool TryGetValue([MaybeNullWhen(false)] out string str)
        => TryGetValue(Handle, out str);

    /// <inheritdoc/>
    public static bool TryGetValue(MaaStringBufferHandle handle, [MaybeNullWhen(false)] out string str)
    {
        if (handle == default)
        {
            str = default;
            return false;
        }

        var size = (int)MaaStringBufferSize(handle);
        if (size < 0)
        {
            str = string.Empty;
            return false;
        }

        str = size == 0 ? string.Empty : MaaStringBufferGetToNint(handle).ToStringUtf8(size);
        return true;
    }

    /// <inheritdoc/>
    public static bool TryGetValue([MaybeNullWhen(false)] out string str, Func<MaaStringBufferHandle, bool> writeBuffer)
    {
        ArgumentNullException.ThrowIfNull(writeBuffer);
        var handle = MaaStringBufferCreate();
        try
        {
            if (!writeBuffer.Invoke(handle))
            {
                str = default;
                return false;
            }

            return TryGetValue(handle, out str);
        }
        finally
        {
            MaaStringBufferDestroy(handle);
        }
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
        try
        {
            return TrySetValue(handle, str, useEx) && readBuffer.Invoke(handle);
        }
        finally
        {
            MaaStringBufferDestroy(handle);
        }
    }
}
