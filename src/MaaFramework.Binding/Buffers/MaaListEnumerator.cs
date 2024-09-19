namespace MaaFramework.Binding.Buffers;


/// <summary>
///     A sealed class providing a reference enumerator implementation for <see cref="IMaaListBuffer{T}"/>.
/// </summary>
/// <typeparam name="T">The type of buffer.</typeparam>
public sealed class MaaListEnumerator<T> : IEnumerator<T>
{
    private readonly Func<MaaSize, T> _getAt;
    private readonly Func<MaaSize> _getSize;
    private MaaSize _index;

    /// <summary>
    ///     The internal enumerator implementation of IMaaListBuffer.
    /// </summary>
    /// <param name="getAt">Use MaaGetListAt().</param>
    /// <param name="getSize">Use MaaGetListSize().</param>
    public MaaListEnumerator(Func<MaaSize, T> getAt, Func<MaaSize> getSize)
    {
        _getAt = getAt;
        _getSize = getSize;
        _index = MaaSize.MaxValue;
    }

    /// <inheritdoc/>
    public T Current
    {
        get
        {
            if (_index >= _getSize())
                throw new InvalidOperationException($"_index({_index}) should be less than _size{_getSize()}");
            return _getAt(_index);
        }
    }

    object System.Collections.IEnumerator.Current => Current!;

    /// <inheritdoc/>
    public bool MoveNext()
    {
        var index = _index + 1;
        var length = _getSize();
        if (index >= length)
        {
            _index = length;
            return false;
        }
        _index = index;
        return true;
    }

    /// <inheritdoc/>
    public void Reset() => _index = MaaSize.MaxValue;

    /// <inheritdoc/>
    public void Dispose() { }
}
