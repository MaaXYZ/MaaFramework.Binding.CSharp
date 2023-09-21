using MaaToolKit.Extensions.Interop;
using static MaaToolKit.Extensions.Interop.MaaApi;

namespace MaaToolKit.Extensions;

/// <summary>
///     A class providing a reference implementation for Maa String Buffer section of <see cref="MaaApi"/>.
/// </summary>
public class MaaStringBuffer : IDisposable
{
    internal MaaStringBufferHandle _handle;
    private bool disposed;

    /// <summary>
    ///     Creates a <see cref="MaaStringBuffer"/> instance.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreateStringBuffer"/>.
    /// </remarks>
    public MaaStringBuffer()
    {
        _handle = MaaCreateStringBuffer();
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
    ///     Indicates whether the string of <see cref="MaaStringBuffer" /> is empty.
    /// </summary>
    /// <returns>
    ///     true if the string is empty; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaIsStringEmpty"/>.
    /// </remarks>
    public bool IsEmpty()
        => MaaIsStringEmpty(_handle).ToBoolean();

    /// <summary>
    ///     Clears the string of <see cref="MaaStringBuffer" />.
    /// </summary>
    /// <returns>
    ///     true if the string was cleared successfully; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaClearString"/>.
    /// </remarks>
    public bool Clear()
        => MaaClearString(_handle).ToBoolean();

    /// <summary>
    ///     Gets the pointer of the string.
    /// </summary>
    /// <returns>
    ///     The string pointer.
    /// </returns>
    /// <remarks>
    ///     Using <see cref="ToString"/> instead.
    ///     Wrapper of <see cref="MaaGetString"/>.
    /// </remarks>
    public nint GetStringPointer()
        => MaaGetString(_handle);

    /// <summary>
    ///     Gets the size of the string.
    /// </summary>
    /// <returns>
    ///     The size.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetStringSize"/>.
    /// </remarks>
    public ulong Size => MaaGetStringSize(_handle);

    /// <summary>
    ///     Sets a string into the <see cref="MaaStringBuffer" />.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <returns></returns>
    public bool SetString(string str)
        => MaaSetString(_handle, str).ToBoolean();

    /// <summary>
    ///     Sets a string into the <see cref="MaaStringBuffer" /> with size.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="size">The size of the string.</param>
    /// <returns></returns>
    public bool SetString(string str, ulong size)
        => MaaSetStringEx(_handle, str, size).ToBoolean();

    /// <summary>
    ///     Gets the string of the <see cref="MaaStringBuffer" />.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
        => IsEmpty() ? string.Empty : GetStringPointer().ToStringUTF8();
}
