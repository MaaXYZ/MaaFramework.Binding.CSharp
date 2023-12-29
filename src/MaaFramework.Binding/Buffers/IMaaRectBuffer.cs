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
///     An interface defining wrapped members for MaaRectBuffer.
/// </summary>
public interface IMaaRectBuffer : IDisposable
{
    /// <summary>
    ///     Gets or Sets the horizontal coordinate.
    /// </summary>
    /// <exception cref="InvalidOperationException" />
    int X { get; set; }

    /// <summary>
    ///     Gets or Sets the vertical coordinate.
    /// </summary>
    /// <exception cref="InvalidOperationException" />
    int Y { get; set; }

    /// <summary>
    ///     Gets or Sets the Width value.
    /// </summary>
    /// <exception cref="InvalidOperationException" />
    int Width { get; set; }

    /// <summary>
    ///     Gets or Sets the Height value.
    /// </summary>
    /// <exception cref="InvalidOperationException" />
    int Height { get; set; }

    /// <summary>
    ///     Sets values of a MaaRectBuffer.
    /// </summary>
    /// <param name="x">The horizontal coordinate.</param>
    /// <param name="y">The vertical coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    bool SetValues(int x, int y, int width, int height);

    /// <summary>
    ///     Gets values of a MaaRectBuffer.
    /// </summary>
    /// <param name="x">The horizontal coordinate.</param>
    /// <param name="y">The vertical coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns>true if the operation was executed successfully; otherwise, false.</returns>
    void GetValues(out int x, out int y, out int width, out int height);
}

