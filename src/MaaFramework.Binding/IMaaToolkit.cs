using MaaFramework.Binding.Custom;

namespace MaaFramework.Binding;

#pragma warning disable S1133 // Deprecated code should be removed

/// <summary>
///     An interface defining wrapped members for MaaToolkit.
/// </summary>
public interface IMaaToolkit
{
    /// <summary>
    ///     Gets or sets the MaaToolkit Config.
    /// </summary>
    IMaaToolkitConfig Config { get; set; }

    /// <summary>
    ///     Gets or sets the MaaToolkit Device.
    /// </summary>
    IMaaToolkitDevice Device { get; set; }

    /// <summary>
    ///     Gets or sets the MaaToolkit ExecAgent.
    /// </summary>
    IMaaToolkitExecAgent ExecAgent { get; set; }

    /// <summary>
    ///     Gets or sets the MaaToolkit Win32.
    /// </summary>
    IMaaToolkitWin32 Win32 { get; set; }
}

/// <summary>
///     An interface defining wrapped members for MaaToolkit Config.
/// </summary>
public interface IMaaToolkitConfig
{
    /// <summary>
    ///     Initializes Maa Toolkit option config.
    /// </summary>
    /// <param name="userPath">The user path. Default is <see cref="Environment.CurrentDirectory"/>.</param>
    /// <param name="defaultJson">The default config. Default is a empty json.</param>
    /// <returns>
    ///     true if the Maa Toolkit option config was initialized successfully; otherwise, false.
    /// </returns>
    bool InitOption(string userPath = nameof(Environment.CurrentDirectory), string defaultJson = "{}");

    /// <summary>
    ///     Initializes Maa Toolkit.
    /// </summary>
    /// <returns>
    ///     true if the Maa Toolkit was initialized successfully; otherwise, false.
    /// </returns>
    [Obsolete("Use InitOption() instead.")]
    bool Init();

    /// <summary>
    ///     Uninitializes Maa Toolkit.
    /// </summary>
    /// <returns>
    ///     true if the Maa Toolkit was uninitialized successfully; otherwise, false.
    /// </returns>
    [Obsolete("Use InitOption() instead.")]
    bool Uninit();
}

/// <summary>
///     An interface defining wrapped members for MaaToolkit Device.
/// </summary>
public interface IMaaToolkitDevice
{
    /// <summary>
    ///     Finds devices.
    /// </summary>
    /// <param name="adbPath">The adb path that devices connected to.</param>
    /// <returns>
    ///     The arrays of device information.
    /// </returns>
    DeviceInfo[] Find(string adbPath = "");

    /// <summary>
    ///     Finds devices in an asynchronous operation.
    /// </summary>
    /// <param name="adbPath">The adb path that devices connected to.</param>
    /// <returns>
    ///     The task object representing the asynchronous operation.
    /// </returns>
    Task<DeviceInfo[]> FindAsync(string adbPath = "");
}

/// <summary>
///     An interface defining wrapped members for MaaToolkit ExecAgent.
/// </summary>
public interface IMaaToolkitExecAgent
{
    /// <summary>
    ///     Registers a <see cref="MaaCustomActionExecutor"/> or <see cref="MaaCustomRecognizerExecutor"/> in the <see cref="IMaaInstance"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="MaaCustomActionExecutor"/> or <see cref="MaaCustomRecognizerExecutor"/>.</typeparam>
    /// <param name="maaInstance">The maa instance.</param>
    /// <param name="name">The new name.</param>
    /// <param name="custom">The <see cref="MaaCustomActionExecutor"/> or <see cref="MaaCustomRecognizerExecutor"/>.</param>
    /// <returns>
    ///     true if the custom action or recognizer was registered successfully; otherwise, false.
    /// </returns>
    /// <exception cref="ArgumentException"/>
    bool Register<T>(IMaaInstance maaInstance, string name, T custom) where T : IMaaCustomExecutor;  // TODOa 缺少测试用例

    /// <inheritdoc cref="Register{T}(IMaaInstance, string, T)"/>
    bool Register<T>(IMaaInstance maaInstance, T custom) where T : IMaaCustomExecutor;

    /// <summary>
    ///     Unregisters a <see cref="MaaCustomActionExecutor"/> or <see cref="MaaCustomRecognizerExecutor"/> in the <see cref="IMaaInstance"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="MaaCustomActionExecutor"/> or <see cref="MaaCustomRecognizerExecutor"/>.</typeparam>
    /// <param name="maaInstance">The maa instance.</param>
    /// <param name="name">The name of <see cref="MaaCustomActionExecutor"/> or <see cref="MaaCustomRecognizerExecutor"/>.</param>
    /// <returns>
    ///     true if the custom action or recognizer was unregistered successfully; otherwise, false.
    /// </returns>
    /// <exception cref="ArgumentException"/>
    bool Unregister<T>(IMaaInstance maaInstance, string name) where T : IMaaCustomExecutor;

    /// <inheritdoc cref="Unregister{T}(IMaaInstance, string)"/>
    /// <param name="maaInstance">The maa instance.</param>
    /// <param name="custom">The <see cref="MaaCustomActionExecutor"/> or <see cref="MaaCustomRecognizerExecutor"/>.</param>
    bool Unregister<T>(IMaaInstance maaInstance, T custom) where T : IMaaCustomExecutor;
}

/// <summary>
///     An interface defining wrapped members for MaaToolkit Win32.
/// </summary>
public interface IMaaToolkitWin32
{
    /// <summary>
    ///     Gets or sets the MaaToolkit Win32Window.
    /// </summary>
    IMaaToolkitWin32Window Window { get; set; }
}

/// <summary>
///     An interface defining wrapped members for MaaToolkit Win32 Window.
/// </summary>
public interface IMaaToolkitWin32Window
{
    /// <summary>
    ///     Finds a win32 window by class name and window name.
    /// </summary>
    /// <param name="className">
    ///     The class name of the window.
    ///     If passed an empty string, class name will not be filtered.
    /// </param>
    /// <param name="windowName">
    ///     The window name of the window.
    ///     If passed an empty string, window name will not be filtered.
    /// </param>
    /// <remarks>
    ///     Finds by exact match. See also <see cref="Search"/>().
    /// </remarks>
    /// <returns>
    ///     The arrays of window information.
    /// </returns>
    WindowInfo[] Find(string className, string windowName);

    /// <summary>
    ///     Searches a win32 window by class name and window name.
    /// </summary>
    /// <param name="className">
    ///     The class name of the window.
    ///     If passed an empty string, class name will not be filtered.
    /// </param>
    /// <param name="windowName">
    ///     The window name of the window.
    ///     If passed an empty string, window name will not be filtered.
    /// </param>
    /// <remarks>
    ///     Searches by substring match. See also <see cref="Find"/>().
    /// </remarks>
    /// <returns>
    ///     The arrays of window information.
    /// </returns>
    WindowInfo[] Search(string className, string windowName);

    /// <summary>
    ///     Gets the window under the cursor.
    /// </summary>
    /// <remarks>
    ///     Uses the WindowFromPoint() system API.
    /// </remarks>
    WindowInfo Cursor { get; }

    /// <summary>
    ///     Get the desktop window.
    /// </summary>
    /// <remarks>
    ///     Uses the GetDesktopWindow() system API.
    /// </remarks>
    WindowInfo Desktop { get; }

    /// <summary>
    ///     Get the foreground window.
    /// </summary>
    /// <remarks>
    ///     Uses the GetForegroundWindow() system API.
    /// </remarks>
    WindowInfo Foreground { get; }
}
