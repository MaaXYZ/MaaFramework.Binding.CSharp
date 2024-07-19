using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;
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
    /// <param name="init">Whether invokes the <see cref="IMaaToolkitConfig.InitOption"/>.</param>
    /// <param name="userPath">The user path. Default is <see cref="Environment.CurrentDirectory"/>.</param>
    /// <param name="defaultJson">The default config. Default is an empty json.</param>
    public MaaToolkit(bool init = false, string userPath = nameof(Environment.CurrentDirectory), string defaultJson = "{}")
    {
        if (init)
        {
            if (userPath == nameof(Environment.CurrentDirectory))
                userPath = Environment.CurrentDirectory;
            Config.InitOption(userPath, defaultJson);
        }
    }

    /// <inheritdoc/>
    public IMaaToolkitConfig Config { get; } = new ConfigClass();

    /// <inheritdoc/>
    public IMaaToolkitDevice Device { get; } = new DeviceClass();

    /// <inheritdoc/>
    public IMaaToolkitExecAgent ExecAgent { get; } = new ExecAgentClass();

    /// <inheritdoc/>
    public IMaaToolkitWin32 Win32 { get; } = new Win32Class();

    /// <inheritdoc cref="MaaToolkit"/>
    protected class ConfigClass : IMaaToolkitConfig
    {
        /// <inheritdoc/>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitInitOptionConfig"/>.
        /// </remarks>
        public bool InitOption(string userPath = nameof(Environment.CurrentDirectory), string defaultJson = "{}")
            => MaaToolkitInitOptionConfig(userPath, defaultJson).ToBoolean();

        /// <inheritdoc/>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitInit"/>.
        /// </remarks>
        [Obsolete("Use InitOption() instead.")]
        public bool Init()
            => MaaToolkitInit().ToBoolean();

        /// <inheritdoc/>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitUninit"/>.
        /// </remarks>
        [Obsolete("Use InitOption() instead.")]
        public bool Uninit()
            => MaaToolkitUninit().ToBoolean();
    }

    /// <inheritdoc cref="MaaToolkit"/>
    protected class DeviceClass : IMaaToolkitDevice
    {
        /// <inheritdoc/>
        public DeviceInfo[] Find(string adbPath = "")
        {
            if (!FindDevice(adbPath))
                throw new InvalidOperationException();

            var size = WaitForFindDeviceToComplete();
            return GetDeviceInfo(size);
        }

        /// <inheritdoc/>
        public async Task<DeviceInfo[]> FindAsync(string adbPath = "")
        {
            if (!FindDevice(adbPath))
                throw new InvalidOperationException();

            var size = await Task.Run(WaitForFindDeviceToComplete);
            return GetDeviceInfo(size);
        }

        private static DeviceInfo[] GetDeviceInfo(ulong size)
        {
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
        ///     true if the find device operation posted successfully; otherwise, false.
        /// </returns>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitPostFindDevice"/> and <see cref="MaaToolkitPostFindDeviceWithAdb"/>.
        /// </remarks>
        protected static bool FindDevice(string adbPath = "")
            => string.IsNullOrEmpty(adbPath)
             ? MaaToolkitPostFindDevice().ToBoolean()
             : MaaToolkitPostFindDeviceWithAdb(adbPath).ToBoolean();

        /// <summary>
        ///     Get a value indicates whether the find device operation is completed.
        /// </summary>
        /// <returns>
        ///     true if the operation is completed; otherwise, false.
        /// </returns>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitIsFindDeviceCompleted"/>.
        /// </remarks>
        protected static bool IsFindDeviceCompleted()
            => MaaToolkitIsFindDeviceCompleted().ToBoolean();

        /// <summary>
        ///     Waits and gets the number of devices.
        /// </summary>
        /// <returns>
        ///     The number of devices.
        /// </returns>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitWaitForFindDeviceToComplete"/>.
        /// </remarks>
        protected static ulong WaitForFindDeviceToComplete()
            => MaaToolkitWaitForFindDeviceToComplete();

        /// <summary>
        ///     Gets the number of devices.
        /// </summary>
        /// <returns>
        ///     The number of devices.
        /// </returns>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitGetDeviceCount"/>.
        /// </remarks>
        protected static ulong GetDeviceCount()
            => MaaToolkitGetDeviceCount();

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
            => MaaToolkitGetDeviceName(index).ToStringUtf8();

        /// <summary>
        ///     Gets the path of an adb that a device connected to.
        /// </summary>
        /// <param name="index">The index of the device.</param>
        /// <returns>
        ///     The path.
        /// </returns>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitGetDeviceAdbPath"/>.
        /// </remarks>
        protected static string GetDeviceAdbPath(ulong index)
            => MaaToolkitGetDeviceAdbPath(index).ToStringUtf8();

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
            => MaaToolkitGetDeviceAdbSerial(index).ToStringUtf8();

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
            => MaaToolkitGetDeviceAdbConfig(index).ToStringUtf8();
    }

    /// <inheritdoc cref="MaaToolkit"/>
    protected class ExecAgentClass : IMaaToolkitExecAgent
    {
        /// <inheritdoc/>
        public bool Register<T>(IMaaInstance maaInstance, string name, T custom) where T : IMaaCustomExecutor
        {
            custom.Name = name;
            return Register(maaInstance, custom);
        }

        /// <inheritdoc/>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitRegisterCustomActionExecutor"/> and <see cref="MaaToolkitRegisterCustomRecognizerExecutor"/>.
        /// </remarks>
        public bool Register<T>(IMaaInstance maaInstance, T custom) where T : IMaaCustomExecutor => (maaInstance, custom) switch
        {
            (IMaaInstance<nint> maa, MaaCustomActionExecutor executor)
                => MaaToolkitRegisterCustomActionExecutor(maa.Handle, executor.Name, executor.Path, custom.Parameter.ToArray(), (MaaSize)custom.Parameter.LongCount()).ToBoolean(),
            (IMaaInstance<nint> maa, MaaCustomRecognizerExecutor executor)
                => MaaToolkitRegisterCustomRecognizerExecutor(maa.Handle, executor.Name, executor.Path, custom.Parameter.ToArray(), (MaaSize)custom.Parameter.LongCount()).ToBoolean(),
            (IMaaInstance<nint>, _)
                => throw new NotImplementedException(),
            _ => throw new ArgumentException("The parameter is not of IMaaInstance<nint>", nameof(maaInstance)),
        };

        /// <inheritdoc/>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitUnregisterCustomActionExecutor"/> and <see cref="MaaToolkitUnregisterCustomRecognizerExecutor"/>.
        /// </remarks>
        public bool Unregister<T>(IMaaInstance maaInstance, string name) where T : IMaaCustomExecutor => maaInstance switch
        {
            IMaaInstance<nint> maa when typeof(MaaCustomActionExecutor).IsAssignableFrom(typeof(T))
                => MaaToolkitUnregisterCustomActionExecutor(maa.Handle, name).ToBoolean(),
            IMaaInstance<nint> maa when typeof(MaaCustomRecognizerExecutor).IsAssignableFrom(typeof(T))
                => MaaToolkitUnregisterCustomRecognizerExecutor(maa.Handle, name).ToBoolean(),
            IMaaInstance<nint>
                => throw new NotImplementedException(),
            _ => throw new ArgumentException("The parameter is not of IMaaInstance<nint>", nameof(maaInstance)),
        };

        /// <inheritdoc/>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitUnregisterCustomActionExecutor"/> and <see cref="MaaToolkitUnregisterCustomRecognizerExecutor"/>.
        /// </remarks>
        public bool Unregister<T>(IMaaInstance maaInstance, T custom) where T : IMaaCustomExecutor => (maaInstance, custom) switch
        {
            (IMaaInstance<nint> maa, MaaCustomActionExecutor)
                => MaaToolkitUnregisterCustomActionExecutor(maa.Handle, custom.Name).ToBoolean(),
            (IMaaInstance<nint> maa, MaaCustomRecognizerExecutor)
                => MaaToolkitUnregisterCustomRecognizerExecutor(maa.Handle, custom.Name).ToBoolean(),
            (IMaaInstance<nint>, _)
                 => throw new NotImplementedException(),
            _ => throw new ArgumentException("The parameter is not of IMaaInstance<nint>", nameof(maaInstance)),
        };

        /// <inheritdoc/>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitClearCustomActionExecutor"/> and <see cref="MaaToolkitClearCustomRecognizerExecutor"/>.
        /// </remarks>
        public bool Clear<T>(IMaaInstance maaInstance) where T : IMaaCustomExecutor => (maaInstance, typeof(T).Name) switch
        {
            (IMaaInstance<nint> maa, nameof(MaaCustomActionExecutor))
                => MaaToolkitClearCustomActionExecutor(maa.Handle).ToBoolean(),
            (IMaaInstance<nint> maa, nameof(MaaCustomRecognizerExecutor))
                => MaaToolkitClearCustomRecognizerExecutor(maa.Handle).ToBoolean(),
            (IMaaInstance<nint>, _)
                => throw new NotImplementedException(),
            _ => throw new ArgumentException("The parameter is not of IMaaInstance<nint>", nameof(maaInstance))
        };
    }

    /// <inheritdoc cref="MaaToolkit"/>
    protected class Win32Class : IMaaToolkitWin32
    {
        /// <inheritdoc/>
        public IMaaToolkitWin32Window Window { get; } = new Win32WindowClass();

        /// <inheritdoc cref="MaaToolkit"/>
        protected class Win32WindowClass : IMaaToolkitWin32Window
        {
            /// <remarks>
            ///     Wrapper of <see cref="MaaToolkitFindWindow"/>.
            /// </remarks>
            /// <inheritdoc/>
            public WindowInfo[] Find(string className, string windowName)
                => GetWindowInfos(MaaToolkitFindWindow(className, windowName));

            /// <remarks>
            ///     Wrapper of <see cref="MaaToolkitSearchWindow"/>.
            /// </remarks>
            /// <inheritdoc/>
            public WindowInfo[] Search(string className, string windowName)
                => GetWindowInfos(MaaToolkitSearchWindow(className, windowName));

            /// <remarks>
            ///     Wrapper of <see cref="MaaToolkitListWindows"/>.
            /// </remarks>
            /// <inheritdoc/>
            public WindowInfo[] ListWindows()
                => GetWindowInfos(MaaToolkitListWindows());

            /// <remarks>
            ///     Wrapper of <see cref="MaaToolkitGetWindow"/>.
            /// </remarks>
            private static WindowInfo[] GetWindowInfos(ulong size)
            {
                var devices = new WindowInfo[size];
                for (ulong i = 0; i < size; i++)
                {
                    devices[i] = GetWindowInfo(MaaToolkitGetWindow(i));
                }
                return devices;
            }

            /// <remarks>
            ///     Wrapper of <see cref="MaaToolkitGetWindowWindowName"/> and <see cref="MaaToolkitGetWindowClassName"/>.
            /// </remarks>
            private static WindowInfo GetWindowInfo(nint handle) => new()
            {
                Handle = handle,
                Name = MaaStringBuffer.Get(buffer => MaaToolkitGetWindowWindowName(handle, buffer)),
                ClassName = MaaStringBuffer.Get(buffer => MaaToolkitGetWindowClassName(handle, buffer)),
            };

            /// <remarks>
            ///     Wrapper of <see cref="MaaToolkitGetCursorWindow"/>.
            /// </remarks>
            /// <inheritdoc/>
            public WindowInfo Cursor => GetWindowInfo(MaaToolkitGetCursorWindow());

            /// <remarks>
            ///     Wrapper of <see cref="MaaToolkitGetDesktopWindow"/>.
            /// </remarks>
            /// <inheritdoc/>
            public WindowInfo Desktop => GetWindowInfo(MaaToolkitGetDesktopWindow());

            /// <remarks>
            ///     Wrapper of <see cref="MaaToolkitGetForegroundWindow"/>.
            /// </remarks>
            /// <inheritdoc/>
            public WindowInfo Foreground => GetWindowInfo(MaaToolkitGetForegroundWindow());
        }
    }
}
