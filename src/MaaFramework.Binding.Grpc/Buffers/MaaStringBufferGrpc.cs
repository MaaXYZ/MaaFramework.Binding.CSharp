using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa String Buffer section.
/// </summary>
public class MaaStringBufferGrpc : IMaaStringBuffer
{
    /// <summary>
    ///     Creates a <see cref="MaaStringBufferGrpc"/> instance.
    /// </summary>
    public MaaStringBufferGrpc()
    {
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected virtual void Dispose(bool disposing)
    {
        // Cleanup
    }

    /// <inheritdoc/>
    public bool IsEmpty()
        => string.IsNullOrEmpty(String);

    /// <inheritdoc/>
    public bool Clear()
    {
        String = string.Empty;
        return true;
    }

    /// <inheritdoc/>
    public string Get()
        => String;

    /// <inheritdoc/>
    public ulong Size => (ulong)String.Length;

    /// <inheritdoc/>
    public bool Set(string str, bool useEx = true)
    {
        String = str;
        return true;
    }

    /// <inheritdoc/>
    public string String { get; set; } = string.Empty;

    /// <inheritdoc cref="Get"/>
    public override string ToString()
        => String;
}
