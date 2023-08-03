using MaaToolKit.Enums;
using MaaToolKit.Extensions.Exceptions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaToolKit.Interop.MaaApiWrapper;

namespace MaaToolKit.Extensions.ComponentModel;

/// <summary>
///     A class wrapping a <see cref="MaaInstance"/>, a <see cref="MaaResource"/> and a <see cref="MaaController"/>.
/// </summary>
public class MaaObject
{
    /// <summary>
    ///     Gets the MaaFramework version.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="GetMaaVersion"/>.
    /// </remarks>
    public static string FrameworkVersion => GetMaaVersion();

    /// <summary>
    ///     Gets the MaaToolkit version.
    /// </summary>
    public static string ToolkitVersion => "0.0.0.0";

    private static string s_frameworkLogDir = string.Empty;
    private static string s_toolkitLogDir = string.Empty;

    /// <summary>
    ///     Gets or sets the path to the MaaFramework log directory.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="SetGlobalOption(GlobalOption, string)"/>.
    /// </remarks>
    public static string FrameworkLogDir
    {
        get => s_frameworkLogDir;
        set
        {
            var path = Path.GetFullPath(value);
            var setted = SetGlobalOption(GlobalOption.Logging, path);
            if (setted)
            {
                s_frameworkLogDir = path;
            }
        }
    }
    /// <summary>
    ///     Gets or sets the path to the MaaToolkit log directory.
    /// </summary>
    public static string ToolkitLogDir
    {
        get => s_toolkitLogDir;
        set
        {
            var path = Path.GetFullPath(value);
            s_toolkitLogDir = path;
        }
    }

    private static bool debugMode;

    /// <summary>
    ///     Gets or sets whether turns on the debug mode.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="SetGlobalOption(GlobalOption, bool)"/>.
    /// </remarks>
    public static bool DebugMode
    {
        get => debugMode;
        set
        {
            var setted = SetGlobalOption(GlobalOption.DebugMode, value);
            if (setted)
            {
                debugMode = value;
            }
        }
    }

    /// <summary>
    ///     Gets or inits the wrapped <see cref="MaaInstance"/>.
    /// </summary>
    public required MaaInstance Instance { get; init; }

    /// <summary>
    ///     Gets or inits the wrapped <see cref="MaaResource"/>.
    /// </summary>
    public required MaaResource Resource { get; init; }

    /// <summary>
    ///     Gets or inits the wrapped <see cref="MaaController"/>.
    /// </summary>
    public required MaaController Controller { get; init; }

    /// <summary>
    ///     Binds the <see cref="Resource"/> and the <see cref="Controller"/> to the <see cref="MaaInstance"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException" />
    public MaaObject()
    {
        ArgumentNullException.ThrowIfNull(Instance);
        ArgumentNullException.ThrowIfNull(Resource);
        ArgumentNullException.ThrowIfNull(Controller);
        MaaBindException.ThrowIfFalse(
            Instance.BindResource(Resource));
        MaaBindException.ThrowIfFalse(
            Instance.BindController(Controller));
    }

    /// <summary>
    ///     Wraps a <see cref="MaaInstance"/>, a <see cref="MaaResource"/> and a <see cref="MaaController"/>.
    /// </summary>
    /// <param name="instance">The wrapped <see cref="MaaInstance"/>.</param>
    /// <param name="resource">The wrapped <see cref="MaaResource"/>.</param>
    /// <param name="controller">The wrapped <see cref="MaaController"/>.</param>
    /// <exception cref="ArgumentNullException" />
    [SetsRequiredMembers]
    public MaaObject(MaaInstance instance, MaaResource resource, MaaController controller)
    {
        Instance = instance;
        Resource = resource;
        Controller = controller;
        MaaBindException.ThrowIfFalse(
            Instance.BindResource(Resource));
        MaaBindException.ThrowIfFalse(
            Instance.BindController(Controller));
    }

    /// <summary>
    ///     Wraps a <see cref="MaaInstance"/>, a <see cref="MaaResource"/> and a <see cref="MaaController"/>.
    /// </summary>
    /// <param name="adbPath"></param>
    /// <param name="address"></param>
    /// <param name="type"></param>
    /// <param name="adbConfig"></param>
    /// <exception cref="ArgumentNullException" />
    [SetsRequiredMembers]
    public MaaObject(string adbPath, string address, AdbControllerType type, string adbConfig)
        : this(new(IntPtr.Zero), new(nameof(MaaObject)), new(adbPath, address, type, adbConfig, IntPtr.Zero))
    {
        Controller
            .LinkStart()
            .Wait()
            .ThrowIfNot(MaaJobStatus.Success);
    }

    /// <summary>
    ///     Wraps a <see cref="MaaInstance"/>, a <see cref="MaaResource"/> and a <see cref="MaaController"/>.
    /// </summary>
    /// <param name="adbPath"></param>
    /// <param name="address"></param>
    /// <param name="type"></param>
    /// <param name="adbConfig"></param>
    /// <param name="resourcePath"></param>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="ArgumentNullException" />
    [SetsRequiredMembers]
    public MaaObject(string adbPath, string address, AdbControllerType type, string adbConfig,
                     string resourcePath)
        : this(adbPath, address, type, adbConfig)
    {
        Resource
            .Append(Path.GetFullPath(resourcePath))
            .Wait()
            .ThrowIfNot(MaaJobStatus.Success);
    }
}
