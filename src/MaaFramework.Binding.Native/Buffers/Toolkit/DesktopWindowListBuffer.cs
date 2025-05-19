using MaaFramework.Binding.Interop.Native;
using System.Diagnostics.CodeAnalysis;
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
    public override DesktopWindowInfo this[MaaSize index] => new MaaToolkitDesktopWindowInfo(
        MaaToolkitDesktopWindowListAt(Handle, index).ThrowIfEquals(MaaToolkitDesktopWindowHandle.Zero));

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
    public override bool TryIndexOf(DesktopWindowInfo item, out MaaSize index)
    {
        if (item is MaaToolkitDesktopWindowInfo info)
        {
            var count = MaaSizeCount;
            for (MaaSize tmpIndex = 0; tmpIndex < count; tmpIndex++)
            {
                if (MaaToolkitDesktopWindowListAt(Handle, tmpIndex).Equals(info.InfoHandle))
                {
                    index = tmpIndex;
                    return true;
                }
            }

        }

        index = 0;
        return false;
    }

    /// <inheritdoc/>
    public override bool TryCopyTo(MaaImageListBufferHandle bufferHandle)
        => throw new NotSupportedException($"{nameof(DesktopWindowListBuffer)} is read-only.");

    /// <inheritdoc/>
    public static bool TryGetList(MaaToolkitDesktopWindowListHandle handle, [MaybeNullWhen(false)] out IList<DesktopWindowInfo> windowList)
    {
        if (handle == default)
        {
            windowList = default;
            return false;
        }

        var size = (int)MaaToolkitDesktopWindowListSize(handle);
        if (size < 0 || size > Array.MaxLength)
        {
            windowList = Array.Empty<DesktopWindowInfo>();
            return false;
        }

        var array = size == 0 ? [] : new DesktopWindowInfo[size];
        for (var i = 0; i < size; i++)
        {
            var window = MaaToolkitDesktopWindowListAt(handle, (MaaSize)i);
            if (window == default)
            {
                windowList = Array.Empty<DesktopWindowInfo>();
                return false;
            }
            array[i] = new MaaToolkitDesktopWindowInfo(window);
        }

        windowList = array;
        return true;
    }

    /// <inheritdoc/>
    public static bool TryGetList([MaybeNullWhen(false)] out IList<DesktopWindowInfo> windowList, Func<MaaToolkitDesktopWindowListHandle, bool> writeBuffer)
    {
        ArgumentNullException.ThrowIfNull(writeBuffer);
        var handle = MaaToolkitDesktopWindowListCreate();
        try
        {
            if (!writeBuffer.Invoke(handle))
            {
                windowList = default;
                return false;
            }

            return TryGetList(handle, out windowList);
        }
        finally
        {
            MaaToolkitDesktopWindowListDestroy(handle);
        }
    }
}
