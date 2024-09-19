using MaaFramework.Binding.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped members for MaaListBuffer with generic handle.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
/// <typeparam name="T">The type of element.</typeparam>
public interface IMaaListBuffer<out THandle, T> : IMaaListBuffer<T>, IMaaDisposableHandle<THandle>
{
}

/// <summary>
///     An interface defining wrapped members for MaaListBuffer.
/// </summary>
/// <typeparam name="T">The type of element.</typeparam>
public interface IMaaListBuffer<T> : IMaaBuffer<IMaaListBuffer<T>>, IList<T>
{
    /// <summary>
    ///     Gets a value indicates whether the <see cref="IMaaListBuffer{T}"/> is empty.
    /// </summary>
    /// <returns><see langword="true"/> if the <see cref="IMaaListBuffer{T}"/> is empty; otherwise, <see langword="false"/>.</returns>
    bool IsEmpty { get; }

    /// <summary>
    ///     Gets the number of elements contained in the <see cref="IMaaListBuffer{T}"/>.
    /// </summary>
    /// <returns>The number of elements contained in the <see cref="IMaaListBuffer{T}"/>.</returns>
    MaaSize MaaSizeCount { get; }

    /// <summary>
    ///     Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is not a valid index in the <see cref="IMaaListBuffer{T}" />.</exception>
    [NotNull]
    T this[MaaSize index] { get; }

    /// <summary>
    ///     Adds an element to the <see cref="IMaaListBuffer{T}"/>.
    /// </summary>
    /// <param name="item">The object to add to the <see cref="IMaaListBuffer{T}"/>.</param>
    /// <returns><see langword="true"/> if the element is added successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="NotSupportedException">The <see cref="IMaaListBuffer{T}"/> is read-only.</exception>
    new bool Add(T item);

    /// <summary>
    ///     Removes the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    /// <returns><see langword="true"/> if the element is removed successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="NotSupportedException">The <see cref="IMaaListBuffer{T}"/> is read-only.</exception>
    bool RemoveAt(MaaSize index);

    /// <summary>
    ///     Removes all elements from the <see cref="IMaaListBuffer{T}"/>.
    /// </summary>
    /// <returns><see langword="true"/> if all elements are removed successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="NotSupportedException">The <see cref="IMaaListBuffer{T}"/> is read-only.</exception>
    new bool Clear();

    /// <summary>
    ///     Attempts to get index of the specified element.
    /// </summary>
    /// <param name="item">The element.</param>
    /// <param name="index">The zero-based index of the <paramref name="item"/>.</param>
    /// <returns><see langword="true"/> if the index is got successfully; otherwise, <see langword="false"/>.</returns>
    bool TryIndexOf(T item, out MaaSize index);
}
