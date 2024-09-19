using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining common members for MaaBuffer.
/// </summary>
/// <typeparam name="T">The buffer.</typeparam>
public interface IMaaBuffer<in T> : IMaaDisposable
{
    /// <summary>
    ///     Copies all values of the current buffer to the specified buffer.
    /// </summary>
    /// <param name="buffer">The buffer that is the destination of values copied from the current buffer.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="NotSupportedException">The <see cref="IMaaBuffer{T}"/> is read-only.</exception>
    bool CopyTo(T buffer);
}
