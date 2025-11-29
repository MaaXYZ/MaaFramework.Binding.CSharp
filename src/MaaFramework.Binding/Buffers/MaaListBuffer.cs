using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An abstract class providing a reference implementation for MaaListBuffer.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
/// <typeparam name="T">The type of element.</typeparam>
/// <param name="invalidHandleValue">The invalid handle value.</param>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public abstract class MaaListBuffer<THandle, T>(THandle invalidHandleValue)
    : MaaDisposableHandle<THandle>(invalidHandleValue), IMaaListBuffer<THandle, T>
    where THandle : IEquatable<THandle>
{
    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ {nameof(Count)} = {MaaSizeCount}, {nameof(IsReadOnly)} = {IsReadOnly} }}";

    /// <inheritdoc/>
    public abstract bool IsEmpty { get; }
    /// <inheritdoc/>
    public abstract MaaSize MaaSizeCount { get; }
    /// <inheritdoc/>
    [NotNull]
    public abstract T this[MaaSize index] { get; }

    /// <remarks>
    ///     For operations like TryAdd, TryRemoveAt, TryClear, and ReleaseHandle,
    /// which may cause the underlying std::vector storage to change,
    /// it is necessary to dispose T and increment _version by 1.
    /// </remarks>
    internal int _version;
    /// <inheritdoc/>
    public abstract bool TryAdd(T item);
    /// <inheritdoc/>
    public abstract bool TryRemoveAt(MaaSize index);
    /// <inheritdoc/>
    public abstract bool TryClear();

    /// <inheritdoc/>
    public abstract bool IsReadOnly { get; }

    #region Non-Standard Methods

    /// <inheritdoc/>
    public abstract bool TryIndexOf(T item, out MaaSize index);
    /// <inheritdoc/>
    public bool Remove(T item)
        => IsReadOnly
            ? throw new NotSupportedException($"{GetType().Name} is read-only.")
            : TryIndexOf(item, out var index) && TryRemoveAt(index);
    /// <inheritdoc/>
    public bool Contains(T item)
        => TryIndexOf(item, out _);
    /// <inheritdoc/>
    public int IndexOf(T item)
        => TryIndexOf(item, out var index) && index <= int.MaxValue ? (int)index : -1;

    /// <inheritdoc/>
    public int Count => (int)MaaSizeCount;
    /// <inheritdoc/>
    public abstract bool TryCopyTo(THandle bufferHandle);
    /// <inheritdoc/>
    public bool TryCopyTo(IMaaListBuffer<T> buffer) => buffer switch
    {
        MaaListBuffer<THandle, T> bufferWithHandle when GetType().IsInstanceOfType(buffer) => TryCopyTo(bufferWithHandle.Handle),
        null => false,
        _ => MaaSizeCount <= MaaSize.MaxValue - buffer.MaaSizeCount
            && this.All(buffer.TryAdd),
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
        => new MaaListBuffer<THandle, T>.Enumerator(this);

    #endregion

#pragma warning disable CA1033 // 接口方法应可由子类型调用

    #region Explicit Interface Implementations

    IEnumerator IEnumerable.GetEnumerator()
        => new MaaListBuffer<THandle, T>.Enumerator(this);

    void ICollection<T>.Add(T item)
        => MaaInteroperationException.ThrowIfNot(
            TryAdd(item),
            $"The {nameof(item)} was invalid.");
    void ICollection<T>.Clear()
        => MaaInteroperationException.ThrowIfNot(
            TryClear(),
            "The collection was invalid.");

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
        set => throw new NotSupportedException($"{nameof(MaaListBuffer<,>)} does not support setting the element at the specified index.");
    }
    void IList<T>.RemoveAt(int index)
        => MaaInteroperationException.ThrowIfNot(
            TryRemoveAt((MaaSize)index),
            $"The {nameof(index)} was out of range or handle was zero.");
    void IList<T>.Insert(int index, T item)
        => throw new NotSupportedException($"{nameof(MaaListBuffer<,>)} does not support insert a element at the specified index.");

    #endregion

#pragma warning restore CA1033 // 接口方法应可由子类型调用

    /// <summary>
    ///     A struct providing a reference enumerator implementation for <see cref="MaaListBuffer{THandle, T}"/>.
    /// </summary>
    /// <remarks>
    ///     The internal enumerator implementation of <see cref="MaaListBuffer{THandle, T}"/>.
    /// </remarks>
    public struct Enumerator : IEnumerator<T>
    {
        private readonly MaaListBuffer<THandle, T> _buffer;
        private readonly int _version;

        // In general, the maximum value will not be used, so the maximum value is used as a flag that has not started enumeration.
        private MaaSize _index = MaaSize.MaxValue;
        private T? _current = default;

        internal Enumerator(MaaListBuffer<THandle, T> buffer)
        {
            _buffer = buffer;
            _version = buffer._version;
        }

        /// <inheritdoc/>
        public readonly void Dispose()
        {
        }

        /// <inheritdoc/>
        public bool MoveNext()
        {
            if (_version != _buffer._version)
            {
                throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
            }

            var index = _index + 1;
            var length = _buffer.MaaSizeCount;
            if (index < length)
            {
                _current = _buffer[index];
                _index = index;
                return true;
            }

            _current = default;
            _index = MaaSize.MaxValue;
            return false;
        }

        /// <inheritdoc/>
        public readonly T Current => _current!;

        readonly object? IEnumerator.Current
        {
            get
            {
                if (_index == MaaSize.MaxValue)
                {
                    throw new InvalidOperationException("Enumeration has either not started or has already finished.");
                }

                return _current;
            }
        }

        /// <inheritdoc/>
        public void Reset()
        {
            if (_version != _buffer._version)
            {
                throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
            }

            _index = MaaSize.MaxValue;
            _current = default;
        }
    }
}
