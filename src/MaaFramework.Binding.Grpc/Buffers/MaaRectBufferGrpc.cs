using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Rect Buffer section.
/// </summary>
public class MaaRectBufferGrpc : IMaaRectBuffer
{
    /// <summary>
    ///     Creates a <see cref="MaaRectBufferGrpc"/> instance.
    /// </summary>
    public MaaRectBufferGrpc()
    {
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected virtual void Dispose(bool disposing)
    {
        // Cleanup
    }

    /// <inheritdoc/>
    public int X { get; set; }

    /// <inheritdoc/>
    public int Y { get; set; }

    /// <inheritdoc/>
    public int Width { get; set; }

    /// <inheritdoc/>
    public int Height { get; set; }

    /// <inheritdoc/>
    public void SetValues(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <inheritdoc/>
    public void GetValues(out int x, out int y, out int width, out int height)
    {
        x = X;
        y = Y;
        width = Width;
        height = Height;
    }
}
