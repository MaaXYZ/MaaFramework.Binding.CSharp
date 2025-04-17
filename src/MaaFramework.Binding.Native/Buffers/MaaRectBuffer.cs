using MaaFramework.Binding.Abstractions;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Rect Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
public class MaaRectBuffer : MaaDisposableHandle<MaaRectHandle>, IMaaRectBuffer<MaaRectHandle>, IMaaRectBufferStatic<MaaRectHandle>
{
    /// <inheritdoc/>
    public override string ToString() => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ {nameof(X)} = {X}, {nameof(Y)} = {Y}, {nameof(Width)} = {Width}, {nameof(Height)} = {Height} }}";

    /// <inheritdoc/>
    public bool TryCopyTo(MaaRectHandle bufferHandle) => TrySetValues(
            handle: bufferHandle,
            x: MaaRectGetX(Handle),
            y: MaaRectGetY(Handle),
            width: MaaRectGetW(Handle),
            height: MaaRectGetH(Handle));

    /// <inheritdoc/>
    public bool TryCopyTo(IMaaRectBuffer buffer) => buffer switch
    {
        MaaRectBuffer native => TryCopyTo(native.Handle),
        null => false,
        _ => buffer.TrySetValues(
            x: MaaRectGetX(Handle),
            y: MaaRectGetY(Handle),
            width: MaaRectGetW(Handle),
            height: MaaRectGetH(Handle)),
    };

    /// <summary>
    ///     Creates a <see cref="MaaRectBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaRectHandle.</param>
    public MaaRectBuffer(MaaRectHandle handle)
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc cref="MaaRectBuffer(nint)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRectCreate"/>.
    /// </remarks>
    public MaaRectBuffer()
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(MaaRectCreate(), needReleased: true);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRectDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaRectDestroy(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRectGetX"/>.
    /// </remarks>
    public int X => MaaRectGetX(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRectGetY"/>.
    /// </remarks>
    public int Y => MaaRectGetY(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRectGetW"/>.
    /// </remarks>
    public int Width => MaaRectGetW(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRectGetH"/>.
    /// </remarks>
    public int Height => MaaRectGetH(Handle);

    /// <inheritdoc/>
    public bool TrySetValues(int x, int y, int width, int height)
        => MaaRectSet(Handle, x, y, width, height);

    /// <inheritdoc/>
    public static bool TrySetValues(MaaRectHandle handle, int x, int y, int width, int height)
        => MaaRectSet(handle, x, y, width, height);

    /// <inheritdoc/>
    public static bool TrySetValues(int x, int y, int width, int height, Func<MaaRectHandle, bool> readBuffer)
    {
        ArgumentNullException.ThrowIfNull(readBuffer);
        var handle = MaaRectCreate();
        try
        {
            return TrySetValues(handle, x, y, width, height) && readBuffer.Invoke(handle);
        }
        finally
        {
            MaaRectDestroy(handle);
        }
    }

    /// <inheritdoc/>
    public bool TryGetValues(out int x, out int y, out int width, out int height)
        => TryGetValues(Handle, out x, out y, out width, out height);

    /// <inheritdoc/>
    public static bool TryGetValues(MaaRectHandle handle, out int x, out int y, out int width, out int height)
    {
        if (handle == default)
        {
            x = y = width = height = 0;
            return false;
        }

        x = MaaRectGetX(handle);
        y = MaaRectGetY(handle);
        width = MaaRectGetW(handle);
        height = MaaRectGetH(handle);
        return true;
    }

    /// <inheritdoc/>
    public static bool TryGetValues(out int x, out int y, out int width, out int height, Func<MaaRectHandle, bool> writeBuffer)
    {
        ArgumentNullException.ThrowIfNull(writeBuffer);
        var handle = MaaRectCreate();
        try
        {
            x = y = width = height = 0;
            return writeBuffer.Invoke(handle) && TryGetValues(handle, out x, out y, out width, out height);
        }
        finally
        {
            MaaRectDestroy(handle);
        }
    }

    /// <inheritdoc/>
    public RectInfo GetValues()
        => GetValues(Handle);

    /// <inheritdoc/>
    public static RectInfo GetValues(MaaRectHandle handle) => new
    (
        X: MaaRectGetX(handle),
        Y: MaaRectGetY(handle),
        Width: MaaRectGetW(handle),
        Height: MaaRectGetH(handle)
    );

    /// <inheritdoc/>
    public static RectInfo GetValues(Func<MaaRectHandle, bool> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        var handle = MaaRectCreate();
        try
        {
            return func.Invoke(handle) && TryGetValues(handle, out var x, out var y, out var width, out var height)
                ? new(X: x, Y: y, Width: width, Height: height)
                : new(0, 0, 0, 0);
        }
        finally
        {
            MaaRectDestroy(handle);
        }
    }
}
