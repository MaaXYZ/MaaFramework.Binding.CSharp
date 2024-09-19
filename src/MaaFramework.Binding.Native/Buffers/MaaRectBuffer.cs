using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Rect Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaBuffer"/>.
/// </summary>
[System.Diagnostics.DebuggerDisplay("x:{X}, y:{Y}, w:{Width}, h:{Height}")]
public class MaaRectBuffer : MaaDisposableHandle<nint>, IMaaRectBuffer<nint>
{
    /// <inheritdoc/>
    public bool CopyTo(IMaaRectBuffer buffer) => buffer switch
    {
        // MaaRectBuffer native => native method is same to wrapped method
        null => false,
        _ => buffer.SetValues(
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
    public int X
    {
        get => MaaRectGetX(Handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRectGetY"/>.
    /// </remarks>
    public int Y
    {
        get => MaaRectGetY(Handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRectGetW"/>.
    /// </remarks>
    public int Width
    {
        get => MaaRectGetW(Handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRectGetH"/>.
    /// </remarks>
    public int Height
    {
        get => MaaRectGetH(Handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRectSet"/>.
    /// </remarks>
    public bool SetValues(int x, int y, int width, int height)
        => MaaRectSet(Handle, x, y, width, height).ToBoolean();

    /// <inheritdoc cref="SetValues(int, int, int, int)"/>
    public static bool Set(MaaRectHandle handle, int x, int y, int width, int height)
        => MaaRectSet(handle, x, y, width, height).ToBoolean();

    /// <inheritdoc/>
    public void GetValues(out int x, out int y, out int width, out int height)
    {
        x = MaaRectGetX(Handle);
        y = MaaRectGetY(Handle);
        width = MaaRectGetW(Handle);
        height = MaaRectGetH(Handle);
    }

    /// <inheritdoc cref="GetValues(out int, out int, out int, out int)"/>
    public static void Get(MaaRectHandle handle, out int x, out int y, out int width, out int height)
    {
        x = MaaRectGetX(handle);
        y = MaaRectGetY(handle);
        width = MaaRectGetW(handle);
        height = MaaRectGetH(handle);
    }
}
