using MaaFramework.Binding.Interop;
using static MaaFramework.Binding.Interop.Framework.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa String Buffer section of <see cref="MaaFramework.Binding.Interop.Framework.MaaBuffer"/>.
/// </summary>
public class MaaStringBuffer : IDisposable
{
    internal MaaStringBufferHandle _handle;
    private bool disposed;

    /// <inheritdoc cref="MaaRectBuffer(nint)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreateStringBuffer"/>.
    /// </remarks>
    public MaaStringBuffer()
        : this(MaaCreateStringBuffer())
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaStringBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaStringBufferHandle.</param>
    public MaaStringBuffer(MaaStringBufferHandle handle)
    {
        _handle = handle;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Disposes the <see cref="MaaStringBuffer"/> instance.
    /// </summary>
    /// <param name="disposing"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDestroyStringBuffer"/>.
    /// </remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            // if (disposing) Dispose managed resources.
            MaaDestroyStringBuffer(_handle);
            disposed = true;
        }
    }

    /// <summary>
    ///     Indicates whether the string of the <see cref="MaaStringBuffer" /> is empty.
    /// </summary>
    /// <returns>true if the string is empty; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaIsStringEmpty"/>.
    /// </remarks>
    public bool IsEmpty()
        => MaaIsStringEmpty(_handle).ToBoolean();

    /// <summary>
    ///     Clears the string of the <see cref="MaaStringBuffer" />.
    /// </summary>
    /// <returns>true if the string was cleared successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaClearString"/>.
    /// </remarks>
    public bool Clear()
        => MaaClearString(_handle).ToBoolean();

    /// <summary>
    ///     Gets the string from the <see cref="MaaStringBuffer" />.
    /// </summary>
    /// <returns>The string.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetString"/>.
    /// </remarks>
    public string Get()
        => IsEmpty() ? string.Empty : MaaGetString(_handle).ToStringUTF8(Size);

    /// <summary>
    ///     Gets the size of the string.
    /// </summary>
    /// <returns>The size.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetStringSize"/>.
    /// </remarks>
    public ulong Size => MaaGetStringSize(_handle);

    /// <summary>
    ///     Sets a string into the <see cref="MaaStringBuffer" />.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="useEx">Uses <see cref="MaaSetStringEx"/> if true; otherwise, Uses <see cref="MaaSetString"/>.</param>
    /// <returns>true if the string was setted successfully; otherwise, false.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetString"/> and <see cref="MaaSetStringEx"/>.
    /// </remarks>
    public bool Set(string str, bool useEx = true)
    {
        if (useEx)
        {
            var bytes = str.ToBytes();
            return MaaSetStringEx(_handle, ref bytes[0], (MaaSize)bytes.LongLength).ToBoolean();
        }

        return MaaSetString(_handle, str).ToBoolean();
    }

    /// <summary>
    ///     Gets or Sets the string of the <see cref="MaaStringBuffer" />.
    /// </summary>
    public string String
    {
        get => Get();
        set
        {
            if (!Set(value)) throw new InvalidOperationException();
        }
    }

    /// <inheritdoc cref="Get"/>
    public override string ToString()
        => Get();
}
