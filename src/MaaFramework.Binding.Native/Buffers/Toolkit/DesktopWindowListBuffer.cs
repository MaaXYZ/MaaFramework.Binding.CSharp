using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaToolkit;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Toolkit Desktop Window List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaToolkit"/>.
/// </summary>
public class DesktopWindowListBuffer : MaaListBuffer<MaaToolkitDesktopWindowListHandle, DesktopWindowInfo>
    , IDesktopWindowListBufferStatic<MaaToolkitDesktopWindowListHandle>
{
    /// <summary>
    ///     A sealed record providing a reference implementation for <see cref="DesktopWindowInfo"/>.
    /// </summary>
    /// <param name="InfoHandle">The MaaToolkitDesktopWindowHandle in the <see cref="AdbDeviceListBuffer"/>.</param>
    protected internal sealed record MaaToolkitDesktopWindowInfo(MaaToolkitDesktopWindowHandle InfoHandle) : DesktopWindowInfo(
        Handle: MaaToolkitDesktopWindowGetHandle(InfoHandle),
        ClassName: MaaToolkitDesktopWindowGetClassName(InfoHandle),
        Name: MaaToolkitDesktopWindowGetWindowName(InfoHandle)
    );

    /// <summary>
    ///     Creates a <see cref="DesktopWindowListBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaToolkitDesktopWindowListHandle.</param>
    public DesktopWindowListBuffer(MaaToolkitDesktopWindowListHandle handle) : base(MaaToolkitDesktopWindowListHandle.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc cref="DesktopWindowListBuffer(MaaToolkitDesktopWindowListHandle)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitDesktopWindowListCreate"/>.
    /// </remarks>
    public DesktopWindowListBuffer() : base(MaaToolkitDesktopWindowListHandle.Zero)
    {
        SetHandle(MaaToolkitDesktopWindowListCreate(), needReleased: true);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitDesktopWindowListDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaToolkitDesktopWindowListDestroy(Handle);

    /// <inheritdoc/>
    public override bool IsEmpty => MaaToolkitDesktopWindowListSize(Handle) == 0;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitDesktopWindowListSize"/>.
    /// </remarks>
    public override MaaSize MaaSizeCount => MaaToolkitDesktopWindowListSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitDesktopWindowListAt"/>.
    /// </remarks>
    public override DesktopWindowInfo this[MaaSize index] => new MaaToolkitDesktopWindowInfo(MaaToolkitDesktopWindowListAt(Handle, index).ThrowIfEquals(MaaToolkitDesktopWindowHandle.Zero));

    /// <inheritdoc/>
    public override bool TryAdd(DesktopWindowInfo item)
        => throw new NotSupportedException($"{nameof(DesktopWindowListBuffer)} is read-only.");

    /// <inheritdoc/>
    public override bool TryRemoveAt(MaaSize index)
        => throw new NotSupportedException($"{nameof(DesktopWindowListBuffer)} is read-only.");

    /// <inheritdoc/>
    public override bool TryClear()
        => throw new NotSupportedException($"{nameof(DesktopWindowListBuffer)} is read-only.");

    /// <inheritdoc/>
    public override bool IsReadOnly => true;

    /// <inheritdoc/>
    public override bool TryIndexOf(DesktopWindowInfo item, out ulong index)
    {
        if (item is not MaaToolkitDesktopWindowInfo info)
            throw new NotSupportedException($"{nameof(item)} must be the type: {typeof(MaaToolkitDesktopWindowInfo)}.");

        var count = MaaSizeCount;
        for (index = 0; index < count; index++)
            if (MaaToolkitDesktopWindowListAt(Handle, index).Equals(info.InfoHandle))
                return true;

        return false;
    }

    /// <inheritdoc/>
    public override bool TryCopyTo(MaaImageListBufferHandle bufferHandle)
        => throw new NotSupportedException($"{nameof(DesktopWindowListBuffer)} is read-only.");

    /// <inheritdoc/>
    public static bool TryGetList(MaaToolkitDesktopWindowListHandle handle, out IList<DesktopWindowInfo> windowList)
    {
        var ret = false;
        var count = (int)MaaToolkitDesktopWindowListSize(handle);
        var array = count <= 0 || count > Array.MaxLength ? [] : new DesktopWindowInfo[count];

        count = array.Length;
        for (var i = 0; i < count; i++)
        {
            var window = MaaToolkitDesktopWindowListAt(handle, (MaaSize)i);
            ret |= window != default;
            array[i] = new MaaToolkitDesktopWindowInfo(window);
        }

        windowList = array;
        return ret;
    }

    /// <inheritdoc/>
    public static bool TryGetList(out IList<DesktopWindowInfo> windowList, Func<MaaToolkitDesktopWindowListHandle, bool> writeBuffer)
    {
        ArgumentNullException.ThrowIfNull(writeBuffer);
        var handle = MaaToolkitDesktopWindowListCreate();
        if (!writeBuffer.Invoke(handle))
        {
            windowList = Array.Empty<DesktopWindowInfo>();
            MaaToolkitDesktopWindowListDestroy(handle);
            return false;
        }

        var ret = TryGetList(handle, out windowList);
        MaaToolkitDesktopWindowListDestroy(handle);
        return ret;
    }
}
