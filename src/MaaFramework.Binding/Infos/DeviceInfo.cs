namespace MaaFramework.Binding;

/// <summary>
///     A class providing properties of device information.
/// </summary>
public sealed class DeviceInfo
{
    /// <summary>
    ///     Gets the name of a device.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Gets the path of a adb that a device connected to.
    /// </summary>
    public required string AdbPath { get; init; }

    /// <summary>
    ///     Gets the adb serial of a device.
    /// </summary>
    public required string AdbSerial { get; init; }

    /// <summary>
    ///     Gets the <see cref="AdbControllerTypes"/> of a device.
    /// </summary>
    public required AdbControllerTypes AdbTypes { get; init; }

    /// <summary>
    ///     Gets the adb config of a device.
    /// </summary>
    public required string AdbConfig { get; init; }
}
