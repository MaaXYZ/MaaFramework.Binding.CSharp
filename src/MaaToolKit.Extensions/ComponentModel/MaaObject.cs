using MaaToolKit.Extensions.Enums;
using MaaToolKit.Extensions.Exceptions;
using MaaToolKit.Extensions.Interop;
using System.Diagnostics.CodeAnalysis;
using static MaaToolKit.Extensions.Interop.MaaApi;

namespace MaaToolKit.Extensions.ComponentModel;

/// <summary>
///     A class wrapping a <see cref="MaaInstance"/>, a <see cref="MaaResource"/> and a <see cref="MaaController"/>.
/// </summary>
public class MaaObject : IDisposable
{
    private bool disposed;

    /// <summary>
    ///     Sets <paramref name="value"/> to a option of the <see cref="MaaObject"/>.
    /// </summary>
    /// <param name="option">The option.</param>
    /// <param name="value">The value.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetGlobalOption"/>.
    /// </remarks>
    /// <returns>true if the option was setted successfully; otherwise, false.</returns>
    private static bool SetOption(GlobalOption option, MaaOptionValue[] value)
        => MaaSetGlobalOption((MaaGlobalOption)option, ref value[0], (MaaOptionValueSize)value.Length).ToBoolean();

    /// <summary>
    ///     Gets the MaaFramework version.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaVersion"/>.
    /// </remarks>
    public static string FrameworkVersion => MaaVersion().ToStringUTF8();

    /// <summary>
    ///     Gets the MaaToolkit version.
    /// </summary>
    public static string ToolkitVersion => "0.0.0.0";

    private static string s_frameworkLogDir = string.Empty;

    /// <summary>
    ///     Gets or sets the path to the MaaFramework log directory.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetGlobalOption"/>.
    /// </remarks>
    public static string FrameworkLogDir
    {
        get => s_frameworkLogDir;
        set
        {
            var setted = SetOption(GlobalOption.Logging, value.ToMaaOptionValues());
            if (setted)
            {
                s_frameworkLogDir = value;
            }
        }
    }

    /// <summary>
    ///     Gets or sets the path to the MaaToolkit log directory.
    /// </summary>
    public static string ToolkitLogDir { get; set; } = string.Empty;

    private static bool debugMode;

    /// <summary>
    ///     Gets or sets whether turns on the debug mode.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetGlobalOption"/>.
    /// </remarks>
    public static bool DebugMode
    {
        get => debugMode;
        set
        {
            var setted = SetOption(GlobalOption.DebugMode, value.ToMaaOptionValues());
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
    ///     Wraps a <see cref="MaaInstance"/>, a <see cref="MaaResource"/> and a <see cref="MaaController"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="MaaBindException" />
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

    /// <inheritdoc cref="MaaObject"/>
    /// <param name="instance">The wrapped <see cref="MaaInstance"/>.</param>
    /// <param name="resource">The wrapped <see cref="MaaResource"/>.</param>
    /// <param name="controller">The wrapped <see cref="MaaController"/>.</param>
    /// <exception cref="MaaBindException" />
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

    /// <inheritdoc cref="MaaObject(MaaInstance, MaaResource, MaaController)"/>
    /// <param name="adbPath"></param>
    /// <param name="address"></param>
    /// <param name="type"></param>
    /// <param name="adbConfig"></param>
    /// <param name="resourcePaths"></param>
    /// <exception cref="MaaJobStatusException" />
    [SetsRequiredMembers]
    public MaaObject(string adbPath, string address, AdbControllerType type, string adbConfig, params string[] resourcePaths)
        : this(new MaaInstance(),
               new MaaResource(resourcePaths),
               new MaaController(adbPath, address, type, adbConfig, linkStart: true))
    {
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Disposes the <see cref="MaaObject"/> instance.
    /// </summary>
    /// <param name="disposing"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaController.Dispose()"/>, <see cref="MaaInstance.Dispose()"/>, <see cref="MaaResource.Dispose()"/>.
    /// </remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Controller.Dispose();
                Instance.Dispose();
                Resource.Dispose();
            }

            disposed = true;
        }
    }
}
