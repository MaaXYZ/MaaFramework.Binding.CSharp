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
    /// <inheritdoc cref="MaaRectBuffer(nint)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCreateRectBuffer"/>.
    /// </remarks>
    public MaaRectBuffer()
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(MaaCreateRectBuffer(), needReleased: true);
    }

    /// <summary>
    ///     Creates a <see cref="MaaRectBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaRectHandle.</param>
    public MaaRectBuffer(MaaRectHandle handle)
        : base(invalidHandleValue: nint.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDestroyRectBuffer"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaDestroyRectBuffer(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetRectX"/> and <see cref="MaaSetRectX"/>.
    /// </remarks>
    public int X
    {
        get => MaaGetRectX(Handle);
        set
        {
            if (!MaaSetRectX(Handle, value).ToBoolean())
                throw new InvalidOperationException();
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetRectY"/> and <see cref="MaaSetRectY"/>.
    /// </remarks>
    public int Y
    {
        get => MaaGetRectY(Handle);
        set
        {
            if (!MaaSetRectY(Handle, value).ToBoolean())
                throw new InvalidOperationException();
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetRectW"/> and <see cref="MaaSetRectW"/>.
    /// </remarks>
    public int Width
    {
        get => MaaGetRectW(Handle);
        set
        {
            if (!MaaSetRectW(Handle, value).ToBoolean())
                throw new InvalidOperationException();
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGetRectH"/> and <see cref="MaaSetRectH"/>.
    /// </remarks>
    public int Height
    {
        get => MaaGetRectH(Handle);
        set
        {
            if (!MaaSetRectH(Handle, value).ToBoolean())
                throw new InvalidOperationException();
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetRect"/>.
    /// </remarks>
    public bool SetValues(int x, int y, int width, int height)
        => MaaSetRect(Handle, x, y, width, height).ToBoolean();

    /// <inheritdoc cref="SetValues(int, int, int, int)"/>
    public static bool Set(MaaRectHandle handle, int x, int y, int width, int height)
        => MaaSetRect(handle, x, y, width, height).ToBoolean();

    /// <inheritdoc/>
    public void GetValues(out int x, out int y, out int width, out int height)
    {
        x = MaaGetRectX(Handle);
        y = MaaGetRectY(Handle);
        width = MaaGetRectW(Handle);
        height = MaaGetRectH(Handle);
    }

    /// <inheritdoc cref="GetValues(out int, out int, out int, out int)"/>
    public static void Get(MaaRectHandle handle, out int x, out int y, out int width, out int height)
    {
        x = MaaGetRectX(handle);
        y = MaaGetRectY(handle);
        width = MaaGetRectW(handle);
        height = MaaGetRectH(handle);
    }
}
