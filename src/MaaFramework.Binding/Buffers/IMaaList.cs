using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped members for MaaStringListBuffer with generic handle.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
/// <typeparam name="TBuffer">The type of buffer.</typeparam>
public interface IMaaList<out THandle, TBuffer> : IMaaList<TBuffer>, IMaaDisposableHandle<THandle>
{
}

/// <summary>
///     An interface defining wrapped members for MaaStringListBuffer.
/// </summary>
/// <typeparam name="T">The type of buffer.</typeparam>
public interface IMaaList<T> : IDisposable, IList<T>
{
    /// <summary>
    ///     Gets a value indicates whether the string of the MaaStringListBuffer is empty.
    /// </summary>
    /// <returns>true if the string is empty; otherwise, false.</returns>
    bool IsEmpty { get; }

    /// <summary>
    ///     Gets the buffer at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the buffer to get.</param>
    /// <returns>The buffer at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Index is not a valid index.</exception>
    new T this[int index] { get; }
}
