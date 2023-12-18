using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Native.Interop;
using System.Reflection.Metadata;
using static MaaFramework.Binding.Native.Interop.MaaBuffer;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Rect Buffer section of <see cref="MaaFramework.Binding.Native.Interop.MaaBuffer"/>.
/// </summary>
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
            if (!MaaSetRectX(Handle, value).ToBoolean()) throw new InvalidOperationException();
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
            if (!MaaSetRectY(Handle, value).ToBoolean()) throw new InvalidOperationException();
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
            if (!MaaSetRectW(Handle, value).ToBoolean()) throw new InvalidOperationException();
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
            if (!MaaSetRectH(Handle, value).ToBoolean()) throw new InvalidOperationException();
        }
    }

    /// <inheritdoc/>
    public void SetValues(int x, int y, int width, int height)
        => Set(Handle, x, y, width, height);

    /// <inheritdoc/>
    public void GetValues(out int x, out int y, out int width, out int height)
        => Get(Handle, out x, out y, out width, out height);

    /// <inheritdoc cref="SetValues(int, int, int, int)"/>
    public static void Set(MaaRectHandle handle, int x, int y, int width, int height)
    {
        if (!MaaSetRectX(handle, x).ToBoolean()) throw new InvalidOperationException();
        if (!MaaSetRectY(handle, y).ToBoolean()) throw new InvalidOperationException();
        if (!MaaSetRectW(handle, width).ToBoolean()) throw new InvalidOperationException();
        if (!MaaSetRectH(handle, height).ToBoolean()) throw new InvalidOperationException();
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
