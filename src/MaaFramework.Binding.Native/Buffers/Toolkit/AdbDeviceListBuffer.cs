using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaToolkit;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Toolkit Adb Device List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaToolkit"/>.
/// </summary>
public class AdbDeviceListBuffer : MaaListBuffer<nint, AdbDeviceInfo>
{
    /// <summary>
    ///     Creates a <see cref="AdbDeviceListBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaToolkitAdbDeviceListHandle.</param>
    public AdbDeviceListBuffer(MaaToolkitAdbDeviceListHandle handle) : base(nint.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc cref="AdbDeviceListBuffer(MaaToolkitAdbDeviceListHandle)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitAdbDeviceListCreate"/>.
    /// </remarks>
    public AdbDeviceListBuffer() : base(nint.Zero)
    {
        SetHandle(MaaToolkitAdbDeviceListCreate(), needReleased: true);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitAdbDeviceListDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaToolkitAdbDeviceListDestroy(Handle);

    /// <inheritdoc/>
    public override bool IsEmpty => MaaToolkitAdbDeviceListSize(Handle) != 0;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitAdbDeviceListSize"/>.
    /// </remarks>
    public override MaaSize MaaSizeCount => MaaToolkitAdbDeviceListSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitAdbDeviceListAt"/>.
    /// </remarks>
    public override AdbDeviceInfo this[MaaSize index]
    {
        get => new MaaToolkitAdbDevice(MaaToolkitAdbDeviceListAt(Handle, index).ThrowIfEquals(nint.Zero));
    }

    /// <inheritdoc/>
    public override bool Add(AdbDeviceInfo item)
        => throw new NotSupportedException($"{nameof(AdbDeviceListBuffer)} is read-only.");

    /// <inheritdoc/>
    public override bool RemoveAt(MaaSize index)
        => throw new NotSupportedException($"{nameof(AdbDeviceListBuffer)} is read-only.");

    /// <inheritdoc/>
    public override bool Clear()
        => throw new NotSupportedException($"{nameof(AdbDeviceListBuffer)} is read-only.");

    /// <inheritdoc/>
    public override bool IsReadOnly => true;

    /// <inheritdoc/>
    public override bool TryIndexOf(AdbDeviceInfo item, out ulong index)
    {
        if (item is not MaaToolkitAdbDevice info)
            throw new NotSupportedException($"{nameof(item)} must be the type: {typeof(MaaToolkitAdbDevice)}.");

        var count = MaaSizeCount;
        for (index = 0; index < count; index++)
            if (MaaToolkitAdbDeviceListAt(Handle, index).Equals(info.InfoHandle))
                return true;

        return false;
    }

    /// <summary>
    ///     A sealed record providing a reference implementation for <see cref="AdbDeviceInfo"/>.
    /// </summary>
    /// <param name="InfoHandle">The <see cref="MaaToolkitAdbDevice"/> handle in the <see cref="AdbDeviceListBuffer"/>.</param>
    protected internal sealed record MaaToolkitAdbDevice(MaaToolkitAdbDeviceHandle InfoHandle) : AdbDeviceInfo(
        Name: MaaToolkitAdbDeviceGetName(InfoHandle),
        AdbPath: MaaToolkitAdbDeviceGetAdbPath(InfoHandle),
        AdbSerial: MaaToolkitAdbDeviceGetAddress(InfoHandle),
        ScreencapMethods: (AdbScreencapMethods)MaaToolkitAdbDeviceGetScreencapMethods(InfoHandle),
        InputMethods: (AdbInputMethods)MaaToolkitAdbDeviceGetInputMethods(InfoHandle),
        Config: MaaToolkitAdbDeviceGetConfig(InfoHandle)
    );
}
