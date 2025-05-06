using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;
using MaaFramework.Binding.Interop.Native;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaToolkit;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaToolkit"/>.
/// </summary>
public class MaaToolkit : IMaaToolkit
{
    /// <summary>
    ///    Gets the shared <see cref="MaaToolkit"/> instance.
    /// </summary>
    public static MaaToolkit Shared { get; } = new();

    /// <summary>
    ///     Creates a <see cref="MaaToolkit"/> instance.
    /// </summary>
    /// <param name="init">Whether invokes the <see cref="IMaaToolkitConfig.InitOption"/>.</param>
    /// <param name="userPath">The user path. Default is <see cref="Environment.CurrentDirectory"/>.</param>
    /// <param name="defaultJson">The default config. Default is an empty json.</param>
    public MaaToolkit(bool init = false, string userPath = nameof(Environment.CurrentDirectory), [StringSyntax("Json")] string defaultJson = "{}")
    {
        if (init)
        {
            _ = Config.InitOption(userPath, defaultJson);
        }
    }

    /// <inheritdoc/>
    public IMaaToolkitConfig Config { get; } = new ConfigClass();

    /// <inheritdoc/>
    public IMaaToolkitAdbDevice AdbDevice { get; } = new AdbDeviceClass();

    /// <inheritdoc/>
    public IMaaToolkitDesktop Desktop { get; } = new DesktopClass();

    /// <inheritdoc/>
    public IMaaToolkitProjectInterface PI { get; set; } = ProjectInterfaceClass.Get(0);

    /// <inheritdoc cref="MaaToolkit"/>
    protected internal class ConfigClass : IMaaToolkitConfig
    {
        /// <inheritdoc/>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitConfigInitOption"/>.
        /// </remarks>
        public bool InitOption(string userPath = nameof(Environment.CurrentDirectory), [StringSyntax("Json")] string defaultJson = "{}")
        {
            if (userPath == nameof(Environment.CurrentDirectory))
                userPath = Environment.CurrentDirectory;
            return MaaToolkitConfigInitOption(userPath, defaultJson);
        }
    }

    /// <inheritdoc cref="MaaToolkit"/>
    protected internal class AdbDeviceClass : IMaaToolkitAdbDevice
    {
        /// <inheritdoc/>
        public IMaaListBuffer<AdbDeviceInfo> Find(string adbPath = "")
        {
            var list = new AdbDeviceListBuffer();
            _ = string.IsNullOrWhiteSpace(adbPath)
                ? MaaToolkitAdbDeviceFind(list.Handle).ThrowIfFalse()
                : MaaToolkitAdbDeviceFindSpecified(adbPath, list.Handle).ThrowIfFalse();
            return list;
        }

        /// <inheritdoc/>
        public async Task<IMaaListBuffer<AdbDeviceInfo>> FindAsync(string adbPath = "")
        {
            var list = new AdbDeviceListBuffer();
            await Task.Run(() =>
            {
                _ = string.IsNullOrWhiteSpace(adbPath)
                    ? MaaToolkitAdbDeviceFind(list.Handle).ThrowIfFalse()
                    : MaaToolkitAdbDeviceFindSpecified(adbPath, list.Handle).ThrowIfFalse();
            });
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
            _ = MaaToolkitDesktopWindowFindAll(list.Handle).ThrowIfFalse();
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
        [ExcludeFromCodeCoverage(Justification = "Can not test RunCli.")]
        protected virtual void OnCallback(string message, [StringSyntax("Json")] string detailsJson, nint callbackArg)
            => Callback?.Invoke(this, new MaaCallbackEventArgs(message, detailsJson));

        /// <summary>
        ///     Gets the delegate to avoid garbage collection before MaaFramework calls <see cref="OnCallback"/>.
        /// </summary>
        [ExcludeFromCodeCoverage(Justification = "Can not test RunCli.")]
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
        private readonly MaaMarshaledApiRegistry<MaaCustomActionCallback> _actions = new();
        private readonly MaaMarshaledApiRegistry<MaaCustomRecognitionCallback> _recognitions = new();

        private bool RegisterCustomAction(IMaaCustomAction res)
        {
            MaaToolkitProjectInterfaceRegisterCustomAction(_instanceId, res.Name, res.Convert(out var callback), nint.Zero);
            return _actions.Register(res.Name, callback);
        }
        private bool RegisterCustomRecognition(IMaaCustomRecognition res)
        {
            MaaToolkitProjectInterfaceRegisterCustomRecognition(_instanceId, res.Name, res.Convert(out var callback), nint.Zero);
            return _recognitions.Register(res.Name, callback);
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
            _ => throw new NotImplementedException($"Type '{typeof(T)}' is not implemented."),
        };

        /// <inheritdoc/>
        /// <remarks>
        ///     Wrapper of <see cref="MaaToolkitProjectInterfaceRunCli"/>.
        /// </remarks>
        [ExcludeFromCodeCoverage(Justification = "Need standard input. " + nameof(MaaNotificationCallback) + nameof(OnCallback))]
        public bool RunCli(string resourcePath, string userPath, bool directly = false)
            => MaaToolkitProjectInterfaceRunCli(_instanceId, resourcePath, userPath, directly, MaaNotificationCallback, nint.Zero);

        /// <inheritdoc/>
        public IMaaToolkitProjectInterface this[ulong id] => Get(id);

        /// <inheritdoc cref="this"/>
        public static IMaaToolkitProjectInterface Get(ulong id)
            => s_instances.GetOrAdd(id, static x => new ProjectInterfaceClass(x));
    }
}
