namespace MaaFramework.Binding.Buffers;


/// <summary>
///     A sealed class providing a reference enumerator implementation for <see cref="IMaaListBuffer{T}"/>.
/// </summary>
/// <typeparam name="T">The type of buffer.</typeparam>
/// <remarks>
///     The internal enumerator implementation of <see cref="IMaaListBuffer{T}"/>.
/// </remarks>
/// <param name="getAt">Use MaaGetListAt().</param>
/// <param name="getSize">Use MaaGetListSize().</param>
public sealed class MaaListEnumerator<T>(Func<MaaSize, T> getAt, Func<MaaSize> getSize) : IEnumerator<T>
{
    private MaaSize _index = MaaSize.MaxValue;

    /// <inheritdoc/>
    public T Current => _index < getSize.Invoke()
        ? getAt.Invoke(_index)
        : throw new InvalidOperationException($"_index({_index}) should be less than _size{getSize.Invoke()}.");

    object System.Collections.IEnumerator.Current => Current!;

    /// <inheritdoc/>
    public bool MoveNext()
    {
        var index = _index + 1;
        var length = getSize.Invoke();
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
    public void Dispose()
    {
        // No resources to dispose of
    }
}
