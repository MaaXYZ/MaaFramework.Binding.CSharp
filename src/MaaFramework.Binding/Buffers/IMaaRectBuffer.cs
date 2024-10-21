using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped members for MaaRectBuffer with generic handle.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public interface IMaaRectBuffer<out T> : IMaaRectBuffer, IMaaDisposableHandle<T>
{
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
    ///     Gets the width value.
    /// </summary>
    int Width { get; }

    /// <summary>
    ///     Gets the height value.
    /// </summary>
    int Height { get; }

    /// <summary>
    ///     Sets values of a <see cref="IMaaRectBuffer"/>.
    /// </summary>
    /// <param name="x">The horizontal coordinate.</param>
    /// <param name="y">The vertical coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    bool SetValues(int x, int y, int width, int height);

    /// <summary>
    ///     Gets values of a <see cref="IMaaRectBuffer"/>.
    /// </summary>
    /// <param name="x">The horizontal coordinate.</param>
    /// <param name="y">The vertical coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    void GetValues(out int x, out int y, out int width, out int height);
}

