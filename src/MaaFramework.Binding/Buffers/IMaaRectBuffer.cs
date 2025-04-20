using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped members for MaaRectBuffer with generic handle.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
public interface IMaaRectBuffer<THandle> : IMaaRectBuffer, IMaaBuffer<THandle, IMaaRectBuffer>, IMaaDisposableHandle<THandle>
{
    // Implement IMaaRectBufferStatic<THandle> at the same time if this interface is implemented.
}

/// <summary>
///     An interface defining wrapped members for <see cref="IMaaRectBuffer"/>.
/// </summary>
public interface IMaaRectBuffer : IMaaBuffer<IMaaRectBuffer>
{
    /// <summary>
    ///     Gets the horizontal coordinate.
    /// </summary>
    int X { get; }

    /// <summary>
    ///     Gets the vertical coordinate.
    /// </summary>
    int Y { get; }

    /// <summary>
    ///     Gets the width.
    /// </summary>
    int Width { get; }

    /// <summary>
    ///     Gets the height.
    /// </summary>
    int Height { get; }

    /// <inheritdoc cref="IMaaRectBufferStatic{THandle}.TrySetValues(THandle, int, int, int, int)"/>
    bool TrySetValues(int x, int y, int width, int height);

    /// <inheritdoc cref="IMaaRectBufferStatic{THandle}.TryGetValues(THandle, out int, out int, out int, out int)"/>
    bool TryGetValues(out int x, out int y, out int width, out int height);

    /// <summary>
    ///     Deconstructs the current <see cref="IMaaRectBuffer"/>.
    /// </summary>
    /// <inheritdoc cref="IMaaRectBufferStatic{THandle}.TryGetValues(THandle, out int, out int, out int, out int)"/>
    void Deconstruct(out int x, out int y, out int width, out int height);

    /// <inheritdoc cref="IMaaRectBufferStatic{THandle}.GetValues(THandle)"/>
    RectInfo GetValues();
}

/// <summary>
///     An interface defining wrapped static abstract members for MaaRectBuffer with generic handle.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
public interface IMaaRectBufferStatic<THandle>
{
    /// <summary>
    ///     Sets values to a MaaRectBuffer.
    /// </summary>
    /// <param name="handle">The MaaRectBufferHandle.</param>
    /// <param name="x">The horizontal coordinate.</param>
    /// <param name="y">The vertical coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    static abstract bool TrySetValues(THandle handle, int x, int y, int width, int height);

    /// <summary>
    ///     Sets values to a function using MaaRectBuffer.
    /// </summary>
    /// <param name="x">The horizontal coordinate.</param>
    /// <param name="y">The vertical coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="readBuffer">The function used to read the values from the buffer.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    static abstract bool TrySetValues(int x, int y, int width, int height, Func<THandle, bool> readBuffer);

    /// <summary>
    ///     Gets values from a MaaRectBuffer.
    /// </summary>
    /// <param name="handle">The MaaRectBufferHandle.</param>
    /// <param name="x">The horizontal coordinate.</param>
    /// <param name="y">The vertical coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetValues(THandle handle, out int x, out int y, out int width, out int height);

    /// <summary>
    ///     Gets values from a function using MaaRectBuffer.
    /// </summary>
    /// <param name="x">The horizontal coordinate.</param>
    /// <param name="y">The vertical coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="writeBuffer">The function used to write the values to the buffer.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetValues(out int x, out int y, out int width, out int height, Func<THandle, bool> writeBuffer);

    /// <summary>
    ///     Gets the rect info from a MaaRectBuffer.
    /// </summary>
    /// <param name="handle">The MaaRectBufferHandle.</param>
    /// <returns>The info including x, y, width, height.</returns>
    static abstract RectInfo GetValues(THandle handle);

    /// <summary>
    ///     Gets the rect info from a function using MaaRectBuffer.
    /// </summary>
    /// <param name="writeBuffer">The function used to write the rect info to the buffer.</param>
    /// <returns>The info including x, y, width, height.</returns>
    static abstract RectInfo GetValues(Func<THandle, bool> writeBuffer);
}
