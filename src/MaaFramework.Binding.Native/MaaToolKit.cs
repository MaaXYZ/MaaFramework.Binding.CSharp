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
    public DeviceInfo[] Find(string adbPath = "")
    {
        var size = FindDevice(adbPath);
        var devices = new DeviceInfo[size];
        for (ulong i = 0; i < size; i++)
        {
            devices[i] = new DeviceInfo
            {
                Name = GetDeviceName(i),
                AdbConfig = GetDeviceAdbConfig(i),
                AdbPath = GetDeviceAdbPath(i),
                AdbSerial = GetDeviceAdbSerial(i),
                AdbTypes = GetDeviceAdbControllerTypes(i),
            };
        }

        return devices;
    }

    /// <summary>
    ///     Finds devices.
    /// </summary>
    /// <param name="adbPath">The adb path that devices connected to.</param>
    /// <returns>
    ///     The number of devices.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitFindDevice"/> and <see cref="MaaToolKitFindDeviceWithAdb"/>.
    /// </remarks>
    protected static ulong FindDevice(string adbPath = "")
        => string.IsNullOrEmpty(adbPath)
         ? MaaToolKitFindDevice()
         : MaaToolKitFindDeviceWithAdb(adbPath);

    /// <summary>
    ///     Gets the name of a device.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The name.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitGetDeviceName"/>.
    /// </remarks>
    protected static string GetDeviceName(ulong index)
        => MaaToolKitGetDeviceName(index).ToStringUTF8();

    /// <summary>
    ///     Gets the path of a adb that a device connected to.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The path.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitGetDeviceAdbPath"/>.
    /// </remarks>
    protected static string GetDeviceAdbPath(ulong index)
        => MaaToolKitGetDeviceAdbPath(index).ToStringUTF8();

    /// <summary>
    ///     Get the adb serial of a device.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The adb serial.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitGetDeviceAdbSerial"/>.
    /// </remarks>
    protected static string GetDeviceAdbSerial(ulong index)
        => MaaToolKitGetDeviceAdbSerial(index).ToStringUTF8();

    /// <summary>
    ///     Get the <see cref="AdbControllerTypes"/> of a device.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The <see cref="AdbControllerTypes"/>.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitGetDeviceAdbControllerType"/>.
    /// </remarks>
    protected static AdbControllerTypes GetDeviceAdbControllerTypes(ulong index)
        => (AdbControllerTypes)MaaToolKitGetDeviceAdbControllerType(index);

    /// <summary>
    ///     Get the adb config of a device.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The adb config.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitGetDeviceAdbConfig"/>.
    /// </remarks>
    protected static string GetDeviceAdbConfig(ulong index)
        => MaaToolKitGetDeviceAdbConfig(index).ToStringUTF8();
}
