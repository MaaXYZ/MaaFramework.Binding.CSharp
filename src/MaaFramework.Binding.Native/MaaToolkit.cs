using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;
using MaaFramework.Binding.Interop.Native;
using System.Collections.Concurrent;
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

    /// <inheritdoc/>
    public IMaaToolkitProjectInterface PI { get; } = ProjectInterfaceClass.Get(0);

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

    /// <inheritdoc cref="MaaToolkit"/>
    /// <remarks>Exists risk of memory leak.</remarks>
    protected internal class ProjectInterfaceClass : IMaaToolkitProjectInterface
    {
        /// <inheritdoc/>
        public event EventHandler<MaaCallbackEventArgs>? Callback;

        /// <summary>
        ///     Raises the Callback event.
        /// </summary>
        /// <param name="message">The MaaStringView.</param>
        /// <param name="detailsJson">The MaaStringView.</param>
        /// <param name="callbackArg">The MaaCallbackTransparentArg.</param>
        /// <remarks>
        ///     Usually invoked by MaaFramework.
        /// </remarks>
        protected virtual void OnCallback(string message, string detailsJson, nint callbackArg)
        {
            Callback?.Invoke(this, new MaaCallbackEventArgs(message, detailsJson));
        }

        /// <summary>
        ///     Gets the delegate to avoid garbage collection before MaaFramework calls <see cref="OnCallback"/>.
        /// </summary>
        protected MaaNotificationCallback MaaNotificationCallback { get; }

        /// <summary>
        ///     Creates a <see cref="ProjectInterfaceClass"/> instance.
        /// </summary>
        /// <param name="instanceId">The instance id.</param>
        protected ProjectInterfaceClass(ulong instanceId)
        {
            _instanceId = instanceId;
            MaaNotificationCallback = OnCallback;
        }

        private readonly ulong _instanceId;
        private static readonly ConcurrentDictionary<ulong, ProjectInterfaceClass> s_instances = [];
        private readonly MaaMarshaledApis<MaaCustomActionCallback> _actions = new();
        private readonly MaaMarshaledApis<MaaCustomRecognitionCallback> _recognitions = new();

        private bool RegisterCustomAction(IMaaCustomAction res)
        {
            MaaToolkitProjectInterfaceRegisterCustomAction(_instanceId, res.Name, res.Convert(out var callback), nint.Zero);
            return _actions.Set(res.Name, callback);
        }
        private bool RegisterCustomRecognition(IMaaCustomRecognition res)
        {
            MaaToolkitProjectInterfaceRegisterCustomRecognition(_instanceId, res.Name, res.Convert(out var callback), nint.Zero);
            return _recognitions.Set(res.Name, callback);
        }

        /// <inheritdoc/>
        public bool Register<T>(string name, T custom) where T : IMaaCustomResource
        {
            custom.Name = name;
            return Register(custom);
        }

        /// <inheritdoc/>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitProjectInterfaceRegisterCustomAction"/> and <see cref="MaaToolkitProjectInterfaceRegisterCustomRecognition"/>.
        /// </remarks>
        public bool Register<T>(T custom) where T : IMaaCustomResource => custom switch
        {
            IMaaCustomAction res => RegisterCustomAction(res),
            IMaaCustomRecognition res => RegisterCustomRecognition(res),
            _ => throw new NotImplementedException(),
        };

        /// <inheritdoc/>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitProjectInterfaceRunCli"/>.
        /// </remarks>
        public bool RunCli(string resourcePath, string userPath, bool directly = false)
            => MaaToolkitProjectInterfaceRunCli(_instanceId, resourcePath, userPath, directly.ToMaaBool(), MaaNotificationCallback, nint.Zero).ToBoolean();

        /// <inheritdoc/>
        public IMaaToolkitProjectInterface this[ulong id] => Get(id);

        /// <inheritdoc cref="this"/>
        public static IMaaToolkitProjectInterface Get(ulong id)
            => s_instances.GetOrAdd(id, x => new ProjectInterfaceClass(x));
    }
}
