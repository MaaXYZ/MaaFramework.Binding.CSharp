using MaaFramework.Binding.Enums;
using MaaFramework.Binding.Interop;
using static MaaFramework.Binding.Interop.MaaApi;

namespace MaaFramework.Binding;

/// <summary>
///     A static class providing a reference implementation for Maa ToolKit section of <see cref="MaaApi"/>.
/// </summary>
public static class MaaTool
{
    /// <summary>
    ///     Initializes Maa ToolKit.
    /// </summary>
    /// <returns>
    ///     true if the Maa ToolKit was initialized successfully; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitInit"/>.
    /// </remarks>
    public static bool Init()
        => MaaToolKitInit().ToBoolean();

    /// <summary>
    ///     Uninitializes Maa ToolKit.
    /// </summary>
    /// <returns>
    ///     true if the Maa ToolKit was uninitialized successfully; otherwise, false.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitUninit"/>.
    /// </remarks>
    public static bool Uninit()
        => MaaToolKitUninit().ToBoolean();

    /// <summary>
    ///     Finds devices.
    /// </summary>
    /// <returns>
    ///     The number of devices.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitFindDevice"/>.
    /// </remarks>
    public static ulong FindDevice()
        => MaaToolKitFindDevice();

    /// <summary>
    ///     Finds devices that connected to a adb with specified path.
    /// </summary>
    /// <param name="adbPath">The adb path.</param>
    /// <returns>
    ///     The number of devices.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitFindDeviceWithAdb"/>.
    /// </remarks>
    public static ulong FindDevice(string adbPath)
        => MaaToolKitFindDeviceWithAdb(adbPath);

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
    public static string GetDeviceName(ulong index)
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
    public static string GetDeviceAdbPath(ulong index)
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
    public static string GetDeviceAdbSerial(ulong index)
        => MaaToolKitGetDeviceAdbSerial(index).ToStringUTF8();

    /// <summary>
    ///     Get the <see cref="AdbControllerType"/> of a device.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The <see cref="AdbControllerType"/>.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitGetDeviceAdbControllerType"/>.
    /// </remarks>
    public static AdbControllerType GetDeviceAdbControllerType(ulong index)
        => (AdbControllerType)MaaToolKitGetDeviceAdbControllerType(index);

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
    public static string GetDeviceAdbConfig(ulong index)
        => MaaToolKitGetDeviceAdbConfig(index).ToStringUTF8();
}
