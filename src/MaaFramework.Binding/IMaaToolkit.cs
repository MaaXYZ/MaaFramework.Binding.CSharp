using MaaFramework.Binding.Buffers;
using System.Diagnostics.CodeAnalysis;

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

    /// <summary>
    ///     Gets the MaaToolkit macOS helper.
    /// </summary>
    IMaaToolkitMacOS MacOS { get; }
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
    bool InitOption(string userPath = nameof(Environment.CurrentDirectory), [StringSyntax("Json")] string defaultJson = "{}");
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
    /// <exception cref="MaaInteroperationException"/>
    IMaaListBuffer<AdbDeviceInfo> Find(string adbPath = "");

    /// <summary>
    ///     Finds devices in an asynchronous operation.
    /// </summary>
    /// <param name="adbPath">The specified adb path that devices connected to.</param>
    /// <returns>
    ///     The task object representing the asynchronous operation.
    /// </returns>
    /// <exception cref="MaaInteroperationException"/>
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
    /// <exception cref="MaaInteroperationException"/>
    IMaaListBuffer<DesktopWindowInfo> Find();
}

/// <summary>
///     An interface defining wrapped members for MaaToolkit macOS.
/// </summary>
public interface IMaaToolkitMacOS
{
    /// <summary>
    ///     Checks if the specified macOS permission is granted.
    /// </summary>
    /// <param name="permission">The permission to check.</param>
    /// <returns><see langword="true"/> if the permission is granted; otherwise, <see langword="false"/>.</returns>
    bool CheckPermission(MacOSPermission permission);

    /// <summary>
    ///     Requests the specified macOS permission.
    /// </summary>
    /// <param name="permission">The permission to request.</param>
    /// <returns><see langword="true"/> if the permission was granted; otherwise, <see langword="false"/>.</returns>
    bool RequestPermission(MacOSPermission permission);

    /// <summary>
    ///     Reveals the system permission settings for the specified macOS permission.
    /// </summary>
    /// <param name="permission">The permission to reveal settings for.</param>
    /// <returns><see langword="true"/> if the settings were revealed successfully; otherwise, <see langword="false"/>.</returns>
    bool RevealPermissionSettings(MacOSPermission permission);
}
