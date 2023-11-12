namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaToolkit.
/// </summary>
public interface IMaaToolkit
{
    /// <summary>
    ///     Initializes Maa ToolKit.
    /// </summary>
    /// <returns>
    ///     true if the Maa ToolKit was initialized successfully; otherwise, false.
    /// </returns>
    bool Init();

    /// <summary>
    ///     Uninitializes Maa ToolKit.
    /// </summary>
    /// <returns>
    ///     true if the Maa ToolKit was uninitialized successfully; otherwise, false.
    /// </returns>
    bool Uninit();

    /// <summary>
    ///     Finds devices.
    /// </summary>
    /// <returns>
    ///     The number of devices.
    /// </returns>
    ulong FindDevice();

    /// <summary>
    ///     Finds devices that connected to a adb with specified path.
    /// </summary>
    /// <param name="adbPath">The adb path.</param>
    /// <returns>
    ///     The number of devices.
    /// </returns>
    ulong FindDevice(string adbPath);

    /// <summary>
    ///     Gets the name of a device.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The name.
    /// </returns>
    string GetDeviceName(ulong index);

    /// <summary>
    ///     Gets the path of a adb that a device connected to.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The path.
    /// </returns>
    string GetDeviceAdbPath(ulong index);

    /// <summary>
    ///     Get the adb serial of a device.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The adb serial.
    /// </returns>
    string GetDeviceAdbSerial(ulong index);

    /// <summary>
    ///     Get the <see cref="AdbControllerTypes"/> of a device.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The <see cref="AdbControllerTypes"/>.
    /// </returns>
    AdbControllerTypes GetDeviceAdbControllerType(ulong index);

    /// <summary>
    ///     Get the adb config of a device.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The adb config.
    /// </returns>
    string GetDeviceAdbConfig(ulong index);
}

