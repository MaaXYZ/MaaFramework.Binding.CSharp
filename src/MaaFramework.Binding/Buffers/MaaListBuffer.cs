using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An abstract class providing a reference implementation for MaaListBuffer.
/// </summary>
/// <param name="invalidHandleValue">The invalid handle value.</param>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class MaaListBuffer<THandle, T>(THandle invalidHandleValue)
    : MaaDisposableHandle<THandle>(invalidHandleValue), IMaaListBuffer<THandle, T> where THandle : IEquatable<THandle>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)] private string DebuggerDisplay => $"{{{GetType().Name} {{ Count = {MaaSizeCount}, Disposed = {IsInvalid} }}}}";

    /// <inheritdoc/>
    public abstract bool IsEmpty { get; }
    /// <inheritdoc/>
    public abstract MaaSize MaaSizeCount { get; }
    /// <inheritdoc/>
    [NotNull]
    public abstract T this[MaaSize index] { get; }
    /// <inheritdoc/>
    public abstract bool Add(T item);
    /// <inheritdoc/>
    public abstract bool RemoveAt(MaaSize index);
    /// <inheritdoc/>
    public abstract bool Clear();
    /// <inheritdoc/>
    public abstract bool IsReadOnly { get; }

    #region Non-Standard Methods

    /// <inheritdoc/>
    public abstract bool TryIndexOf(T item, out MaaSize index);
    /// <inheritdoc/>
    public bool Remove(T item)
        => TryIndexOf(item, out var index) && RemoveAt(index);
    /// <inheritdoc/>
    public bool Contains(T item)
        => TryIndexOf(item, out _);
    /// <inheritdoc/>
    public int IndexOf(T item)
    {
        if (!TryIndexOf(item, out var index)) return -1;
        if (index > int.MaxValue) return -2;
        return (int)index;
    }

    /// <inheritdoc/>
    public int Count => (int)MaaSizeCount;
    /// <inheritdoc/>
    public abstract bool CopyTo(THandle bufferHandle);
    /// <inheritdoc/>
    public bool CopyTo(IMaaListBuffer<THandle, T> buffer)
        => buffer is not null && CopyTo(buffer.Handle);
    /// <inheritdoc/>
    public bool CopyTo(IMaaListBuffer<T> buffer) => buffer switch
    {
        IMaaListBuffer<THandle, T> bufferWithHandle => CopyTo(bufferWithHandle),
        null => false,
        _ => MaaSizeCount <= MaaSize.MaxValue - buffer.MaaSizeCount
            && this.All(buffer.Add),
    };
    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        ArgumentNullException.ThrowIfNull(array);

        var arrayIndexMaaSize = (MaaSize)arrayIndex;
        var count = MaaSizeCount;
        if (count > (MaaSize)array.LongLength - arrayIndexMaaSize)
            throw new ArgumentException("Destination array was not long enough. Check the destination index, length, and the array's lower bounds.", nameof(array));

        for (MaaSize i = 0; i < count; i++)
            array[arrayIndexMaaSize + i] = this[i];
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
        => new MaaListEnumerator<T>(
            getAt: i => this[i],
            getSize: () => MaaSizeCount);

    #endregion

    #region Explicit Interface Implementations

    IEnumerator IEnumerable.GetEnumerator()
        => new MaaListEnumerator<T>(
            getAt: i => this[i],
            getSize: () => MaaSizeCount);

    void ICollection<T>.Add(T item)
        => Add(item);
    void ICollection<T>.Clear()
        => Clear();

    T IList<T>.this[int index]
    {
        get
        {
            try
            {
                return this[(MaaSize)index];
            }
            catch (MaaInteroperationException)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
        set => throw new NotSupportedException($"{nameof(MaaListBuffer<THandle, T>)} does not support setting the element at the specified index.");
    }
    void IList<T>.RemoveAt(int index)
        => RemoveAt((MaaSize)index);
    void IList<T>.Insert(int index, T item)
        => throw new NotSupportedException($"{nameof(MaaListBuffer<THandle, T>)} does not support insert a element at the specified index.");

    #endregion
}
