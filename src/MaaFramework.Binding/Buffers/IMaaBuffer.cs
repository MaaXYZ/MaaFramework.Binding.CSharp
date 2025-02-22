using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining common members for MaaBuffer.
/// </summary>
/// <typeparam name="THandle">The type of buffer handle.</typeparam>
/// <typeparam name="TBuffer">The type of buffer.</typeparam>
public interface IMaaBuffer<in THandle, in TBuffer> : IMaaBuffer<TBuffer>
{
    /// <summary>
    ///     Copies all values of the current buffer to the specified buffer.
    /// </summary>
    /// <param name="bufferHandle">The hadle of the buffer that is the destination of values copied from the current buffer.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="NotSupportedException">The <see cref="IMaaBuffer{THandle, TBuffer}"/> is read-only.</exception>
    bool CopyTo(THandle bufferHandle);
}

/// <summary>
///     An interface defining common members for MaaBuffer.
/// </summary>
/// <typeparam name="TBuffer">The type of buffer.</typeparam>
public interface IMaaBuffer<in TBuffer> : IMaaDisposable
{
    /// <summary>
    ///     Copies all values of the current buffer to the specified buffer.
    /// </summary>
    /// <param name="buffer">The buffer that is the destination of values copied from the current buffer.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="NotSupportedException">The <see cref="IMaaBuffer{TBuffer}"/> is read-only.</exception>
    bool CopyTo(TBuffer buffer);
}
