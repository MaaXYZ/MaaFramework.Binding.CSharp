using MaaFramework.Binding.Native.Interop;
using static MaaFramework.Binding.Native.Interop.Framework.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Rect Buffer section of <see cref="MaaFramework.Binding.Native.Interop.Framework.MaaBuffer"/>.
/// </summary>
public class MaaRectBuffer : IDisposable
{
    internal MaaRectHandle _handle;
    private bool disposed;

    /// <inheritdoc cref="MaaRectBuffer(nint)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreateRectBuffer"/>.
    /// </remarks>
    public MaaRectBuffer()
        : this(MaaCreateRectBuffer())
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaRectBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaRectHandle.</param>
    public MaaRectBuffer(MaaRectHandle handle)
    {
        _handle = handle;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Disposes the <see cref="MaaRectBuffer"/> instance.
    /// </summary>
    /// <param name="disposing"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDestroyRectBuffer"/>.
    /// </remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            // if (disposing) Dispose managed resources.
            MaaDestroyRectBuffer(_handle);
            disposed = true;
        }
    }

    /// <summary>
    ///     Gets or Sets the horizontal coordinate.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetRectX"/> and <see cref="MaaSetRectX"/>.
    /// </remarks>
    /// <exception cref="InvalidOperationException" />
    public int X
    {
        get => MaaGetRectX(_handle);
        set
        {
            if (!MaaSetRectX(_handle, value).ToBoolean()) throw new InvalidOperationException();
        }
    }

    /// <summary>
    ///     Gets or Sets the vertical coordinate.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetRectY"/> and <see cref="MaaSetRectY"/>.
    /// </remarks>
    /// <exception cref="InvalidOperationException" />
    public int Y
    {
        get => MaaGetRectY(_handle);
        set
        {
            if (!MaaSetRectY(_handle, value).ToBoolean()) throw new InvalidOperationException();
        }
    }

    /// <summary>
    ///     Gets or Sets the Width value.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetRectW"/> and <see cref="MaaSetRectW"/>.
    /// </remarks>
    /// <exception cref="InvalidOperationException" />
    public int Width
    {
        get => MaaGetRectW(_handle);
        set
        {
            if (!MaaSetRectW(_handle, value).ToBoolean()) throw new InvalidOperationException();
        }
    }

    /// <summary>
    ///     Gets or Sets the Height value.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetRectH"/> and <see cref="MaaSetRectH"/>.
    /// </remarks>
    /// <exception cref="InvalidOperationException" />
    public int Height
    {
        get => MaaGetRectH(_handle);
        set
        {
            if (!MaaSetRectH(_handle, value).ToBoolean()) throw new InvalidOperationException();
        }
    }

    /// <summary>
    ///     Sets values of a <see cref="MaaRectBuffer"/>.
    /// </summary>
    /// <inheritdoc cref="Set(nint, int, int, int, int)"/>
    public void Set(int x, int y, int width, int height)
        => Set(_handle, x, y, width, height);

    /// <summary>
    ///     Gets values of a <see cref="MaaRectBuffer"/>.
    /// </summary>
    /// <inheritdoc cref="Get(nint, out int, out int, out int, out int)"/>
    public void Get(out int x, out int y, out int width, out int height)
        => Get(_handle, out x, out y, out width, out height);

    /// <summary>
    ///     Sets values from a MaaRectHandle.
    /// </summary>
    /// <param name="handle">The MaaRectHandle.</param>
    /// <param name="x">The horizontal coordinate.</param>
    /// <param name="y">The vertical coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <exception cref="InvalidOperationException" />
    public static void Set(MaaRectHandle handle, int x, int y, int width, int height)
    {
        if (!MaaSetRectX(handle, x).ToBoolean()) throw new InvalidOperationException();
        if (!MaaSetRectY(handle, y).ToBoolean()) throw new InvalidOperationException();
        if (!MaaSetRectW(handle, width).ToBoolean()) throw new InvalidOperationException();
        if (!MaaSetRectH(handle, height).ToBoolean()) throw new InvalidOperationException();
    }

    /// <summary>
    ///     Gets values from a MaaRectHandle.
    /// </summary>
    /// <param name="handle">The MaaRectHandle.</param>
    /// <param name="x">The horizontal coordinate.</param>
    /// <param name="y">The vertical coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public static void Get(MaaRectHandle handle, out int x, out int y, out int width, out int height)
    {
        x = MaaGetRectX(handle);
        y = MaaGetRectY(handle);
        width = MaaGetRectW(handle);
        height = MaaGetRectH(handle);
    }
}
