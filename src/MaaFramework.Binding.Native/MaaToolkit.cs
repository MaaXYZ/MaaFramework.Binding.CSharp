using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaToolkit;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaToolkit"/>.
/// </summary>
public class MaaToolkit : IMaaToolkit
{
    /// <summary>
    ///     Creates a <see cref="MaaToolkit"/> instance.
    /// </summary>
    /// <param name="init">Whether invokes the <see cref="IMaaToolkit.Init"/>.</param>
    public MaaToolkit(bool init = false)
    {
        if (init)
        {
            Init();
        }
    }

    #region MaaToolkitConfig

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitInit"/>.
    /// </remarks>
    public bool Init()
        => MaaToolkitInit().ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitUninit"/>.
    /// </remarks>
    public bool Uninit()
        => MaaToolkitUninit().ToBoolean();

    #endregion

    #region MaaToolkitDevice

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
    ///     Wrapper of <see cref="MaaToolkitFindDevice"/> and <see cref="MaaToolkitFindDeviceWithAdb"/>.
    /// </remarks>
    protected static ulong FindDevice(string adbPath = "")
        => string.IsNullOrEmpty(adbPath)
         ? MaaToolkitFindDevice()
         : MaaToolkitFindDeviceWithAdb(adbPath);

    /// <summary>
    ///     Gets the name of a device.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The name.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitGetDeviceName"/>.
    /// </remarks>
    protected static string GetDeviceName(ulong index)
        => MaaToolkitGetDeviceName(index).ToStringUTF8();

    /// <summary>
    ///     Gets the path of a adb that a device connected to.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The path.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitGetDeviceAdbPath"/>.
    /// </remarks>
    protected static string GetDeviceAdbPath(ulong index)
        => MaaToolkitGetDeviceAdbPath(index).ToStringUTF8();

    /// <summary>
    ///     Gets the adb serial of a device.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The adb serial.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitGetDeviceAdbSerial"/>.
    /// </remarks>
    protected static string GetDeviceAdbSerial(ulong index)
        => MaaToolkitGetDeviceAdbSerial(index).ToStringUTF8();

    /// <summary>
    ///     Gets the <see cref="AdbControllerTypes"/> of a device.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The <see cref="AdbControllerTypes"/>.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitGetDeviceAdbControllerType"/>.
    /// </remarks>
    protected static AdbControllerTypes GetDeviceAdbControllerTypes(ulong index)
        => (AdbControllerTypes)MaaToolkitGetDeviceAdbControllerType(index);

    /// <summary>
    ///     Gets the adb config of a device.
    /// </summary>
    /// <param name="index">The index of the device.</param>
    /// <returns>
    ///     The adb config.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitGetDeviceAdbConfig"/>.
    /// </remarks>
    protected static string GetDeviceAdbConfig(ulong index)
        => MaaToolkitGetDeviceAdbConfig(index).ToStringUTF8();

    #endregion

    #region MaaToolkitWin32Window

    /// <returns>
    ///     The number of windows.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitFindWindow"/>.
    /// </remarks>
    public static ulong FindWindow(string className, string windowName)
        => MaaToolkitFindWindow(className, windowName);

    /// <returns>
    ///     The number of windows.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitSearchWindow"/>.
    /// </remarks>
    public static ulong SearchWindow(string className, string windowName)
        => MaaToolkitSearchWindow(className, windowName);

    /// <returns>
    ///     The MaaWin32Hwnd.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitGetWindow"/>.
    /// </remarks>
    public static nint GetWindow(ulong index)
        => MaaToolkitGetWindow(index);

    /// <returns>
    ///     The MaaWin32Hwnd.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitGetCursorWindow"/>.
    /// </remarks>
    public static nint GetCursorWindow()
        => MaaToolkitGetCursorWindow();

    #endregion

    #region MaaToolkitExecAgent

    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitRegisterCustomRecognizerExecutor"/>.
    /// </remarks>
    public static nint RegisterCustomRecognizerExecutor(MaaInstanceHandle handle, string recognizerName, string recognizerExecPath, string recognizerExecParamJson)
        => MaaToolkitRegisterCustomRecognizerExecutor(handle, recognizerName, recognizerExecPath, recognizerExecParamJson);

    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitUnregisterCustomRecognizerExecutor"/>.
    /// </remarks>
    public static nint UnregisterCustomRecognizerExecutor(MaaInstanceHandle handle, string recognizerName)
        => MaaToolkitUnregisterCustomRecognizerExecutor(handle, recognizerName);

    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitRegisterCustomActionExecutor"/>.
    /// </remarks>
    public static nint RegisterCustomActionExecutor(MaaInstanceHandle handle, string actionName, string actionExecPath, string actionExecParamJson)
        => MaaToolkitRegisterCustomActionExecutor(handle, actionName, actionExecPath, actionExecParamJson);

    /// <remarks>
    ///     Wrapper of <see cref="MaaToolkitUnregisterCustomActionExecutor"/>.
    /// </remarks>
    public static nint UnregisterCustomActionExecutor(MaaInstanceHandle handle, string actionName)
        => MaaToolkitUnregisterCustomActionExecutor(handle, actionName);
    #endregion

    // Todoa: 没搞懂，等一个文档
}
