using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaToolKit;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaToolKit"/>.
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

    #region MaaToolKitConfig

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

    #endregion

    #region MaaToolKitDevice

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
    ///     Gets the adb serial of a device.
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
    ///     Gets the <see cref="AdbControllerTypes"/> of a device.
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
    ///     Gets the adb config of a device.
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

    #endregion

    #region MaaToolKitWin32Window

    /// <returns>
    ///     The number of windows.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitFindWindow"/>.
    /// </remarks>
    public static ulong FindWindow(string className, string windowName)
        => MaaToolKitFindWindow(className, windowName);

    /// <returns>
    ///     The number of windows.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitSearchWindow"/>.
    /// </remarks>
    public static ulong SearchWindow(string className, string windowName)
        => MaaToolKitSearchWindow(className, windowName);

    /// <returns>
    ///     The MaaWin32Hwnd.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitGetWindow"/>.
    /// </remarks>
    public static nint GetWindow(ulong index)
        => MaaToolKitGetWindow(index);

    /// <returns>
    ///     The MaaWin32Hwnd.
    /// </returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitGetCursorWindow"/>.
    /// </remarks>
    public static nint GetCursorWindow()
        => MaaToolKitGetCursorWindow();

    #endregion

    #region MaaToolKitExecAgent

    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitRegisterCustomRecognizerExecutor"/>.
    /// </remarks>
    public static nint RegisterCustomRecognizerExecutor(MaaInstanceHandle handle, string recognizerName, string recognizerExecPath, string recognizerExecParamJson)
        => MaaToolKitRegisterCustomRecognizerExecutor(handle, recognizerName, recognizerExecPath, recognizerExecParamJson);

    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitUnregisterCustomRecognizerExecutor"/>.
    /// </remarks>
    public static nint UnregisterCustomRecognizerExecutor(MaaInstanceHandle handle, string recognizerName)
        => MaaToolKitUnregisterCustomRecognizerExecutor(handle, recognizerName);

    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitRegisterCustomActionExecutor"/>.
    /// </remarks>
    public static nint RegisterCustomActionExecutor(MaaInstanceHandle handle, string actionName, string actionExecPath, string actionExecParamJson)
        => MaaToolKitRegisterCustomActionExecutor(handle, actionName, actionExecPath, actionExecParamJson);

    /// <remarks>
    ///     Wrapper of <see cref="MaaToolKitUnregisterCustomActionExecutor"/>.
    /// </remarks>
    public static nint UnregisterCustomActionExecutor(MaaInstanceHandle handle, string actionName)
        => MaaToolKitUnregisterCustomActionExecutor(handle, actionName);
    #endregion

    // Todoa: 没搞懂，等一个文档
}
