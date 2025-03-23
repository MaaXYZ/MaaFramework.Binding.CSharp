using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaToolkit;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Toolkit Desktop Window List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaToolkit"/>.
/// </summary>
public class DesktopWindowListBuffer : MaaListBuffer<MaaToolkitDesktopWindowListHandle, DesktopWindowInfo>
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
    public override bool Add(DesktopWindowInfo item)
        => throw new NotSupportedException($"{nameof(DesktopWindowListBuffer)} is read-only.");

    /// <inheritdoc/>
    public override bool RemoveAt(MaaSize index)
        => throw new NotSupportedException($"{nameof(DesktopWindowListBuffer)} is read-only.");

    /// <inheritdoc/>
    public override bool Clear()
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

    /// <summary>
    ///     Gets a <see cref="DesktopWindowInfo"/> list from a MaaToolkitDesktopWindowListHandle.
    /// </summary>
    /// <param name="handle">The MaaToolkitDesktopWindowListHandle.</param>
    /// <returns>The <see cref="DesktopWindowInfo"/> list.</returns>
    public static IList<DesktopWindowInfo> Get(MaaToolkitDesktopWindowListHandle handle)
    {
        var count = MaaToolkitDesktopWindowListSize(handle);
        return Enumerable.Range(0, (int)count)
            .Select(index => new MaaToolkitDesktopWindowInfo(MaaToolkitDesktopWindowListAt(handle, (MaaSize)index)) as DesktopWindowInfo)
            .ToList();
    }

    /// <summary>
    ///     Gets a <see cref="DesktopWindowInfo"/> list from a MaaToolkitDesktopWindowListHandle.
    /// </summary>
    /// <param name="list">The <see cref="DesktopWindowInfo"/> list.</param>
    /// <param name="func">A function that takes a MaaToolkitDesktopWindowListHandle and returns a boolean indicating success or failure.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool Get(out IList<DesktopWindowInfo> list, Func<MaaToolkitDesktopWindowListHandle, bool> func)
    {
        var h = MaaToolkitDesktopWindowListCreate();
        var ret = func?.Invoke(h) ?? false;
        list = Get(h);
        MaaToolkitDesktopWindowListDestroy(h);
        return ret;
    }
}
