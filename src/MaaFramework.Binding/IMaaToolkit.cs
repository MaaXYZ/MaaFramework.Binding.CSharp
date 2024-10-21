using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;

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
    ///     Gets the MaaToolkit Project Interface.
    /// </summary>
    IMaaToolkitProjectInterface PI { get; }
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
///     An interface defining wrapped members for MaaToolkit Project Interface.
/// </summary>
public interface IMaaToolkitProjectInterface : IMaaCommon
{
    /// <summary>
    ///     Registers a <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/> in the <see cref="IMaaToolkitProjectInterface"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</typeparam>
    /// <param name="name">The new name that will be used to reference it.</param>
    /// <param name="custom">The <see cref="IMaaCustomAction"/> or <see cref="IMaaCustomRecognition"/>.</param>
    /// <returns><see langword="true"/> if the custom action or recognition was registered successfully; otherwise, <see langword="false"/>.</returns>
    bool Register<T>(string name, T custom) where T : IMaaCustomResource;

    /// <inheritdoc cref="Register{T}(string, T)"/>
    bool Register<T>(T custom) where T : IMaaCustomResource;

    /// <summary>
    ///     Runs a cli.
    /// </summary>
    /// <param name="resourcePath">The resource path.</param>
    /// <param name="userPath">The user path.</param>
    /// <param name="directly">A value indicating whether directly runs.</param>
    /// <returns>true if the option was set successfully; otherwise, false.</returns>
    bool RunCli(string resourcePath, string userPath, bool directly = false);

    /// <summary>
    ///     Gets a pi with the specific instance <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The instance id.</param>
    /// <returns>The pi.</returns>
    IMaaToolkitProjectInterface this[ulong id] { get; }
}
