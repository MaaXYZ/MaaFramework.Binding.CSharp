using static MaaFramework.Binding.Interop.Native.MaaToolkit;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Toolkit Desktop Window List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaToolkit"/>.
/// </summary>
public class DesktopWindowListBuffer : MaaListBuffer<nint, DesktopWindowInfo>
{
    /// <summary>
    ///     Creates a <see cref="DesktopWindowListBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaToolkitDesktopWindowListHandle.</param>
    public DesktopWindowListBuffer(MaaToolkitDesktopWindowListHandle handle) : base(nint.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc cref="DesktopWindowListBuffer(MaaToolkitDesktopWindowListHandle)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitDesktopWindowListCreate"/>.
    /// </remarks>
    public DesktopWindowListBuffer() : base(nint.Zero)
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
    public override bool IsEmpty => MaaToolkitDesktopWindowListSize(Handle) != 0;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitDesktopWindowListSize"/>.
    /// </remarks>
    public override MaaSize MaaSizeCount => MaaToolkitDesktopWindowListSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitDesktopWindowListAt"/>.
    /// </remarks>
    public override DesktopWindowInfo this[MaaSize index] => new MaaToolkitDesktopWindow(MaaToolkitDesktopWindowListAt(Handle, index));

    /// <inheritdoc/>
    public override bool Add(DesktopWindowInfo item)
        => throw new NotSupportedException();

    /// <inheritdoc/>
    public override bool RemoveAt(MaaSize index)
        => throw new NotSupportedException();

    /// <inheritdoc/>
    public override bool Clear()
        => throw new NotSupportedException();

    /// <inheritdoc/>
    public override bool IsReadOnly => true;

    /// <inheritdoc/>
    public override bool TryIndexOf(DesktopWindowInfo item, out ulong index)
    {
        if (item is not MaaToolkitDesktopWindow info)
            throw new NotSupportedException($"{nameof(item)} must be the type: {typeof(MaaToolkitDesktopWindow)}");

        var count = MaaSizeCount;
        for (index = 0; index < count; index++)
            if (MaaToolkitDesktopWindowListAt(Handle, index).Equals(info.InfoHandle))
                return true;

        return false;
    }

    /// <summary>
    ///     A sealed record providing a reference implementation for <see cref="DesktopWindowInfo"/>.
    /// </summary>
    /// <param name="InfoHandle">The <see cref="MaaToolkitDesktopWindow"/> handle in the <see cref="AdbDeviceListBuffer"/>.</param>
    protected internal sealed record MaaToolkitDesktopWindow(nint InfoHandle) : DesktopWindowInfo(
        Handle: MaaToolkitDesktopWindowGetHandle(InfoHandle),
        ClassName: MaaToolkitDesktopWindowGetClassName(InfoHandle),
        Name: MaaToolkitDesktopWindowGetWindowName(InfoHandle)
    );
}
