using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Native.Interop;
using static MaaFramework.Binding.Native.Interop.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa String Buffer section of <see cref="MaaFramework.Binding.Native.Interop.MaaBuffer"/>.
/// </summary>
public class MaaStringBuffer : MaaDisposableHandle<nint>, IMaaStringBuffer<nint>
{
    /// <inheritdoc cref="MaaStringBuffer(nint)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreateStringBuffer"/>.
    /// </remarks>
    public MaaStringBuffer()
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(MaaCreateStringBuffer(), needReleased: true);
    }

    /// <summary>
    ///     Creates a <see cref="MaaStringBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaStringBufferHandle.</param>
    public MaaStringBuffer(MaaStringBufferHandle handle)
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDestroyStringBuffer"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaDestroyStringBuffer(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaIsStringEmpty"/>.
    /// </remarks>
    public bool IsEmpty => MaaIsStringEmpty(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaClearString"/>.
    /// </remarks>
    public bool Clear()
        => MaaClearString(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetString"/>.
    /// </remarks>
    public string GetValue()
        => IsEmpty ? string.Empty : MaaGetString(Handle).ToStringUTF8(Size);

    /// <inheritdoc cref="GetValue"/>
    public static string Get(MaaStringBufferHandle handle)
        => MaaIsStringEmpty(handle).ToBoolean() ? string.Empty : MaaGetString(handle).ToStringUTF8(MaaGetStringSize(handle));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetStringSize"/>.
    /// </remarks>
    public ulong Size => MaaGetStringSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetString"/> and <see cref="MaaSetStringEx"/>.
    /// </remarks>
    public bool SetValue(string str, bool useEx = true)
    {
        if (useEx)
        {
            var bytes = str.ToBytes();
            return MaaSetStringEx(Handle, ref bytes[0], (MaaSize)bytes.LongLength).ToBoolean();
        }

        return MaaSetString(Handle, str).ToBoolean();
    }

    /// <inheritdoc cref="SetValue"/>
    public static bool Set(MaaStringBufferHandle handle, string str, bool useEx = true)
    {
        if (useEx)
        {
            var bytes = str.ToBytes();
            return MaaSetStringEx(handle, ref bytes[0], (MaaSize)bytes.LongLength).ToBoolean();
        }

        return MaaSetString(handle, str).ToBoolean();
    }

    /// <inheritdoc cref="GetValue"/>
    public override string ToString()
        => GetValue();
}
