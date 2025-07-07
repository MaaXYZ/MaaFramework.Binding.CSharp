using MaaFramework.Binding.Interop.Native;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaToolkit;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Toolkit Adb Device List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaToolkit"/>.
/// </summary>
public class AdbDeviceListBuffer : MaaListBuffer<MaaToolkitAdbDeviceListHandle, AdbDeviceInfo>
    , IAdbDeviceListBufferStatic<MaaToolkitAdbDeviceListHandle>
{
    /// <summary>
    ///     A sealed record providing a reference implementation for <see cref="AdbDeviceInfo"/>.
    /// </summary>
    /// <param name="InfoHandle">The MaaToolkitAdbDeviceHandle in the <see cref="AdbDeviceListBuffer"/>.</param>
    protected internal sealed record MaaToolkitAdbDeviceInfo(MaaToolkitAdbDeviceHandle InfoHandle) : AdbDeviceInfo(
        Name: MaaToolkitAdbDeviceGetName(InfoHandle),
        AdbPath: MaaToolkitAdbDeviceGetAdbPath(InfoHandle),
        AdbSerial: MaaToolkitAdbDeviceGetAddress(InfoHandle),
        ScreencapMethods: (AdbScreencapMethods)MaaToolkitAdbDeviceGetScreencapMethods(InfoHandle),
        InputMethods: (AdbInputMethods)MaaToolkitAdbDeviceGetInputMethods(InfoHandle),
        Config: MaaToolkitAdbDeviceGetConfig(InfoHandle)
    );

    /// <summary>
    ///     Creates a <see cref="AdbDeviceListBuffer"/> instance.
    /// </summary>
    /// <param name="handle">The MaaToolkitAdbDeviceListHandle.</param>
    public AdbDeviceListBuffer(MaaToolkitAdbDeviceListHandle handle) : base(MaaToolkitAdbDeviceListHandle.Zero)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <inheritdoc cref="AdbDeviceListBuffer(MaaToolkitAdbDeviceListHandle)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitAdbDeviceListCreate"/>.
    /// </remarks>
    public AdbDeviceListBuffer() : base(MaaToolkitAdbDeviceListHandle.Zero)
    {
        SetHandle(MaaToolkitAdbDeviceListCreate(), needReleased: true);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitAdbDeviceListDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle(MaaToolkitAdbDeviceListHandle handle)
        => MaaToolkitAdbDeviceListDestroy(handle);

    /// <inheritdoc/>
    public override bool IsEmpty => MaaToolkitAdbDeviceListSize(Handle) == 0;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitAdbDeviceListSize"/>.
    /// </remarks>
    public override MaaSize MaaSizeCount => MaaToolkitAdbDeviceListSize(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitAdbDeviceListAt"/>.
    /// </remarks>
    public override AdbDeviceInfo this[MaaSize index] => new MaaToolkitAdbDeviceInfo(
        MaaToolkitAdbDeviceListAt(Handle, index).ThrowIfEquals(MaaToolkitAdbDeviceHandle.Zero));

    /// <inheritdoc/>
    public override bool TryAdd(AdbDeviceInfo item)
        => throw new NotSupportedException($"{nameof(AdbDeviceListBuffer)} is read-only.");

    /// <inheritdoc/>
    public override bool TryRemoveAt(MaaSize index)
        => throw new NotSupportedException($"{nameof(AdbDeviceListBuffer)} is read-only.");

    /// <inheritdoc/>
    public override bool TryClear()
        => throw new NotSupportedException($"{nameof(AdbDeviceListBuffer)} is read-only.");

    /// <inheritdoc/>
    public override bool IsReadOnly => true;

    /// <inheritdoc/>
    public override bool TryIndexOf(AdbDeviceInfo item, out MaaSize index)
    {
        if (item is MaaToolkitAdbDeviceInfo info)
        {
            var count = MaaSizeCount;
            for (MaaSize tmpIndex = 0; tmpIndex < count; tmpIndex++)
            {
                if (MaaToolkitAdbDeviceListAt(Handle, tmpIndex).Equals(info.InfoHandle))
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
    public override bool TryCopyTo(MaaToolkitAdbDeviceHandle bufferHandle)
        => throw new NotSupportedException($"{nameof(AdbDeviceListBuffer)} is read-only.");

    /// <inheritdoc/>
    public static bool TryGetList(MaaToolkitAdbDeviceHandle handle, [MaybeNullWhen(false)] out IList<AdbDeviceInfo> deviceList)
    {
        if (handle == default)
        {
            deviceList = default;
            return false;
        }

        var size = (int)MaaToolkitAdbDeviceListSize(handle);
        if (size < 0 || size > Array.MaxLength)
        {
            deviceList = Array.Empty<AdbDeviceInfo>();
            return false;
        }

        var array = size == 0 ? [] : new AdbDeviceInfo[size];
        for (var i = 0; i < size; i++)
        {
            var device = MaaToolkitAdbDeviceListAt(handle, (MaaSize)i);
            if (device == default)
            {
                deviceList = Array.Empty<AdbDeviceInfo>();
                return false;
            }
            array[i] = new MaaToolkitAdbDeviceInfo(device);
        }

        deviceList = array;
        return true;
    }

    /// <inheritdoc/>
    public static bool TryGetList([MaybeNullWhen(false)] out IList<AdbDeviceInfo> deviceList, Func<MaaToolkitAdbDeviceHandle, bool> writeBuffer)
    {
        ArgumentNullException.ThrowIfNull(writeBuffer);
        var handle = MaaToolkitAdbDeviceListCreate();
        try
        {
            if (!writeBuffer.Invoke(handle))
            {
                deviceList = default;
                return false;
            }

            return TryGetList(handle, out deviceList);
        }
        finally
        {
            MaaToolkitAdbDeviceListDestroy(handle);
        }
    }
}
