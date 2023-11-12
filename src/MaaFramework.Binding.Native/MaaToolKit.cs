using MaaFramework.Binding.Native.Interop;
using static MaaFramework.Binding.Native.Interop.MaaToolKit;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Native.Interop.MaaToolKit"/>.
/// </summary>
public class MaaToolKit : IMaaToolkit
{
    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitInit"/>.
    /// </remarks>
    public bool Init()
        => MaaToolKitInit().ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitUninit"/>.
    /// </remarks>
    public bool Uninit()
        => MaaToolKitUninit().ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitFindDevice"/>.
    /// </remarks>
    public ulong FindDevice()
        => MaaToolKitFindDevice();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitFindDeviceWithAdb"/>.
    /// </remarks>
    public ulong FindDevice(string adbPath)
        => MaaToolKitFindDeviceWithAdb(adbPath);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitGetDeviceName"/>.
    /// </remarks>
    public string GetDeviceName(ulong index)
        => MaaToolKitGetDeviceName(index).ToStringUTF8();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitGetDeviceAdbPath"/>.
    /// </remarks>
    public string GetDeviceAdbPath(ulong index)
        => MaaToolKitGetDeviceAdbPath(index).ToStringUTF8();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitGetDeviceAdbSerial"/>.
    /// </remarks>
    public string GetDeviceAdbSerial(ulong index)
        => MaaToolKitGetDeviceAdbSerial(index).ToStringUTF8();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitGetDeviceAdbControllerType"/>.
    /// </remarks>
    public AdbControllerTypes GetDeviceAdbControllerType(ulong index)
        => (AdbControllerTypes)MaaToolKitGetDeviceAdbControllerType(index);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitGetDeviceAdbConfig"/>.
    /// </remarks>
    public string GetDeviceAdbConfig(ulong index)
        => MaaToolKitGetDeviceAdbConfig(index).ToStringUTF8();
}
