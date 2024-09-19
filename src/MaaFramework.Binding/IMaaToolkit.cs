using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     An interface defining wrapped members for MaaToolkit.
/// </summary>
public interface IMaaToolkit
{
    /// <summary>
    ///     Gets the MaaToolkit Config.
    /// </summary>
    IMaaToolkitConfig Config { get; }

    /// <summary>
    ///     Gets the MaaToolkit AdbDevice.
    /// </summary>
    IMaaToolkitAdbDevice AdbDevice { get; }

    /// <summary>
    ///     Gets the MaaToolkit Desktop.
    /// </summary>
    IMaaToolkitDesktop Desktop { get; }
}

/// <summary>
///     An interface defining wrapped members for MaaToolkit Config.
/// </summary>
public interface IMaaToolkitConfig
{
    /// <summary>
    ///     Initializes MaaToolkit option config.
    /// </summary>
    /// <param name="userPath">The user path. Default is <see cref="Environment.CurrentDirectory"/>.</param>
    /// <param name="defaultJson">The default config. Default is an empty json.</param>
    /// <returns><see langword="true"/> if the option config was initialized successfully; otherwise, <see langword="false"/>. </returns>
    bool InitOption(string userPath = nameof(Environment.CurrentDirectory), string defaultJson = "{}");
}

/// <summary>
///     An interface defining wrapped members for MaaToolkit Adb Device.
/// </summary>
public interface IMaaToolkitAdbDevice
{
    /// <summary>
    ///     Finds devices.
    /// </summary>
    /// <param name="adbPath">The specified adb path that devices connected to.</param>
    /// <returns>
    ///     The List of device information.
    /// </returns>
    /// <exception cref="InvalidOperationException"/>
    IMaaListBuffer<AdbDeviceInfo> Find(string adbPath = "");

    /// <summary>
    ///     Finds devices in an asynchronous operation.
    /// </summary>
    /// <param name="adbPath">The specified adb path that devices connected to.</param>
    /// <returns>
    ///     The task object representing the asynchronous operation.
    /// </returns>
    /// <exception cref="InvalidOperationException"/>
    Task<IMaaListBuffer<AdbDeviceInfo>> FindAsync(string adbPath = "");
}

/// <summary>
///     An interface defining wrapped members for MaaToolkit Desktop.
/// </summary>
public interface IMaaToolkitDesktop
{
    /// <summary>
    ///     Gets the MaaToolkit Desktop Window.
    /// </summary>
    IMaaToolkitDesktopWindow Window { get; }
}

/// <summary>
///     An interface defining wrapped members for MaaToolkit Desktop Window.
/// </summary>
public interface IMaaToolkitDesktopWindow
{
    /// <summary>
    ///     Finds all desktop windows.
    /// </summary>
    /// <returns>
    ///     The list of window information.
    /// </returns>
    /// <exception cref="InvalidOperationException"/>
    IMaaListBuffer<DesktopWindowInfo> Find();
}
