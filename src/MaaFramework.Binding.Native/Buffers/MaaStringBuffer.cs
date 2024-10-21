using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa String Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaStringBuffer : MaaDisposableHandle<nint>, IMaaStringBuffer<nint>
{
    /// <inheritdoc/>
    public bool CopyTo(IMaaStringBuffer buffer) => buffer switch
    {
        MaaStringBuffer native => MaaStringBufferSetExFromNint(
            handle: native.Handle,
            str: MaaStringBufferGetToNint(Handle),
            size: MaaStringBufferSize(Handle)),
        null => false,
        _ => buffer.SetValue(GetValue()),
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
    ///     Wrapper of <see cref="MaaStringBufferClear"/>.
    /// </remarks>
    public bool Clear()
        => MaaStringBufferClear(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringBufferGet"/>.
    /// </remarks>
    public string GetValue()
        => IsEmpty ? string.Empty : MaaStringBufferGetToNint(Handle).ToStringUtf8(Size);

    /// <inheritdoc cref="GetValue"/>
    public static string Get(MaaStringBufferHandle handle)
        => MaaStringBufferIsEmpty(handle) ? string.Empty : MaaStringBufferGetToNint(handle).ToStringUtf8(MaaStringBufferSize(handle));

    /// <inheritdoc cref="GetValue"/>
    public static string Get(Action<MaaStringBufferHandle> action)
    {
        var handle = MaaStringBufferCreate();
        action?.Invoke(handle);
        var ret = Get(handle);
        MaaStringBufferDestroy(handle);
        return ret;
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringBufferSize"/>.
    /// </remarks>
    public ulong Size => MaaStringBufferSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaStringBufferSet"/> and <see cref="MaaStringBufferSetEx(nint,byte[],ulong)"/>.
    /// </remarks>
    public bool SetValue(string str, bool useEx = true)
        => useEx ? MaaStringBufferSetEx(Handle, str) : MaaStringBufferSet(Handle, str);

    /// <inheritdoc cref="SetValue"/>
    public static bool Set(MaaStringBufferHandle handle, string str, bool useEx = true)
        => useEx ? MaaStringBufferSetEx(handle, str) : MaaStringBufferSet(handle, str);

    /// <inheritdoc cref="GetValue"/>
    public override string ToString()
        => GetValue();
}
