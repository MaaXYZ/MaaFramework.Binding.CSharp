namespace MaaFramework.Binding;

/// <summary>
///     An abstract record providing properties of adb device information.
/// </summary>
/// <param name="Name">Gets the name of a device.</param>
/// <param name="AdbPath">Gets the path of an adb that a device connected to.</param>
/// <param name="AdbSerial">Gets the adb serial of a device.</param>
/// <param name="ScreencapMethods">Gets the <see cref="AdbScreencapMethods"/> of a device.</param>
/// <param name="InputMethods">Gets the <see cref="AdbInputMethods"/> of a device.</param>
/// <param name="Config">Gets the config of a device.</param>
public abstract record AdbDeviceInfo(
    string Name,
    string AdbPath,
    string AdbSerial,
    AdbScreencapMethods ScreencapMethods,
    AdbInputMethods InputMethods,
    string Config
);
