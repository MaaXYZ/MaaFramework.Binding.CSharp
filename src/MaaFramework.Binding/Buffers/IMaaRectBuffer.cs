using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped members for MaaImageBuffer.
/// </summary>
public interface IMaaRectBuffer : IMaaDisposableHandle
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
    /// <exception cref="InvalidOperationException" />
    void Set(int x, int y, int width, int height);

    /// <summary>
    ///     Gets values of a MaaRectBuffer.
    /// </summary>
    /// <param name="x">The horizontal coordinate.</param>
    /// <param name="y">The vertical coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    void Get(out int x, out int y, out int width, out int height);
}

