using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaToolkit;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     A class providing a reference implementation for Maa Toolkit Adb Device List Buffer section of <see cref="MaaFramework.Binding.Interop.Native.MaaToolkit"/>.
/// </summary>
public class AdbDeviceListBuffer : MaaListBuffer<MaaToolkitAdbDeviceListHandle, AdbDeviceInfo>
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
    protected override void ReleaseHandle()
        => MaaToolkitAdbDeviceListDestroy(Handle);

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
    public override AdbDeviceInfo this[MaaSize index]
    {
        get => new MaaToolkitAdbDeviceInfo(MaaToolkitAdbDeviceListAt(Handle, index).ThrowIfEquals(MaaToolkitAdbDeviceHandle.Zero));
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
        if (item is not MaaToolkitAdbDeviceInfo info)
            throw new NotSupportedException($"{nameof(item)} must be the type: {typeof(MaaToolkitAdbDeviceInfo)}.");

        var count = MaaSizeCount;
        for (index = 0; index < count; index++)
            if (MaaToolkitAdbDeviceListAt(Handle, index).Equals(info.InfoHandle))
                return true;

        return false;
    }

    /// <inheritdoc/>
    public override bool TryCopyTo(MaaImageListBufferHandle bufferHandle)
        => throw new NotSupportedException($"{nameof(AdbDeviceListBuffer)} is read-only.");

    /// <summary>
    ///     Gets a <see cref="AdbDeviceInfo"/> list from a MaaToolkitAdbDeviceListHandle.
    /// </summary>
    /// <param name="handle">The MaaToolkitAdbDeviceListHandle.</param>
    /// <returns>The <see cref="AdbDeviceInfo"/> list.</returns>
    public static IList<AdbDeviceInfo> Get(MaaToolkitAdbDeviceListHandle handle)
    {
        var count = MaaToolkitAdbDeviceListSize(handle);
        if (count <= int.MaxValue)
        {
            return Enumerable.Range(0, (int)count)
                .Select(index => new MaaToolkitAdbDeviceInfo(MaaToolkitAdbDeviceListAt(handle, (MaaSize)index)) as AdbDeviceInfo)
                .ToList();
        }

        var list = new List<AdbDeviceInfo>();
        for (MaaSize index = 0; index < count; index++)
            list.Add(new MaaToolkitAdbDeviceInfo(MaaToolkitAdbDeviceListAt(handle, index)));
        return list;
    }

    /// <summary>
    ///     Gets a <see cref="AdbDeviceInfo"/> list from a MaaToolkitAdbDeviceListHandle.
    /// </summary>
    /// <param name="list">The <see cref="AdbDeviceInfo"/> list.</param>
    /// <param name="func">A function that takes a MaaToolkitAdbDeviceListHandle and returns a boolean indicating success or failure.</param>
    /// <returns><see langword="true"/> if the operation was executed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool Get(out IList<AdbDeviceInfo> list, Func<MaaToolkitAdbDeviceListHandle, bool> func)
    {
        var h = MaaToolkitAdbDeviceListCreate();
        var ret = func?.Invoke(h) ?? false;
        list = Get(h);
        MaaToolkitAdbDeviceListDestroy(h);
        return ret;
    }
}
