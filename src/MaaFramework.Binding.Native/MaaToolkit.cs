using MaaFramework.Binding.Buffers;
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
            Config.InitOption(userPath, defaultJson);
        }
    }

    /// <inheritdoc/>
    public IMaaToolkitConfig Config { get; } = new ConfigClass();

    /// <inheritdoc/>
    public IMaaToolkitAdbDevice AdbDevice { get; } = new AdbDeviceClass();

    /// <inheritdoc/>
    public IMaaToolkitDesktop Desktop { get; } = new DesktopClass();

    /// <inheritdoc cref="MaaToolkit"/>
    protected internal class ConfigClass : IMaaToolkitConfig
    {
        /// <inheritdoc/>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitConfigInitOption"/>.
        /// </remarks>
        public bool InitOption(string userPath = nameof(Environment.CurrentDirectory), string defaultJson = "{}")
        {
            if (userPath == nameof(Environment.CurrentDirectory))
                userPath = Environment.CurrentDirectory;
            return MaaToolkitConfigInitOption(userPath, defaultJson).ToBoolean();
        }
    }

    /// <inheritdoc cref="MaaToolkit"/>
    protected internal class AdbDeviceClass : IMaaToolkitAdbDevice
    {
        /// <inheritdoc/>
        public IMaaListBuffer<AdbDeviceInfo> Find(string adbPath = "")
        {
            var list = new AdbDeviceListBuffer();
            var ret = string.IsNullOrWhiteSpace(adbPath)
                ? MaaToolkitAdbDeviceFind(list.Handle)
                : MaaToolkitAdbDeviceFindSpecified(adbPath, list.Handle);
            if (!ret.ToBoolean())
                throw new InvalidOperationException();
            return list;
        }

        /// <inheritdoc/>
        public async Task<IMaaListBuffer<AdbDeviceInfo>> FindAsync(string adbPath = "")
        {
            var list = new AdbDeviceListBuffer();
            var ret = await Task.Run(() =>
                string.IsNullOrWhiteSpace(adbPath)
                    ? MaaToolkitAdbDeviceFind(list.Handle)
                    : MaaToolkitAdbDeviceFindSpecified(adbPath, list.Handle)
            );
            if (!ret.ToBoolean())
                throw new InvalidOperationException();
            return list;
        }
    }

    /// <inheritdoc cref="MaaToolkit"/>
    protected internal class DesktopClass : IMaaToolkitDesktop
    {
        /// <inheritdoc/>
        public IMaaToolkitDesktopWindow Window { get; } = new DesktopWindowClass();
    }

    /// <inheritdoc cref="MaaToolkit"/>
    protected internal class DesktopWindowClass : IMaaToolkitDesktopWindow
    {
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitDesktopWindowFindAll"/>.
        /// </remarks>
        /// <inheritdoc/>
        public IMaaListBuffer<DesktopWindowInfo> Find()
        {
            var list = new DesktopWindowListBuffer();
            var ret = MaaToolkitDesktopWindowFindAll(list.Handle);
            if (!ret.ToBoolean())
                throw new InvalidOperationException();
            return list;
        }
    }
}
