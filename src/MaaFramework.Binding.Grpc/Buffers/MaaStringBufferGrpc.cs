using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa String Buffer section.
/// </summary>
public class MaaStringBufferGrpc : IMaaStringBuffer
{
    private string _str = string.Empty;

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
    public bool IsEmpty => string.IsNullOrEmpty(_str);

    /// <inheritdoc/>
    public bool Clear()
    {
        _str = string.Empty;
        return true;
    }

    /// <inheritdoc/>
    public string GetValue()
        => _str;

    /// <inheritdoc/>
    public ulong Size => (ulong)_str.Length;

    /// <inheritdoc/>
    public bool SetValue(string str, bool useEx = true)
    {
        _str = str;
        return true;
    }

    /// <inheritdoc cref="GetValue"/>
    public override string ToString()
        => _str;
}
