using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaGlobal;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaGlobal"/>.
/// </summary>
public class MaaGlobal : IMaaGlobal
{
    /// <summary>
    ///    Gets the shared <see cref="MaaGlobal"/> instance.
    /// </summary>
    public static MaaGlobal Shared { get; } = new();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaUtility.MaaVersion"/>.
    /// </remarks>
    [Obsolete("Use NativeBindingContext.FrameworkVersion instead.")]
    public string Version => MaaUtility.MaaVersion();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGlobalSetOption"/>.
    /// </remarks>
    public bool SetOption<T>(GlobalOption opt, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var optValue = (value, opt) switch
        {
            (int vvvv, GlobalOption.StdoutLevel) => vvvv.ToMaaOptionValue(),
            (string v, GlobalOption.LogDir) => v.ToMaaOptionValue(),
            (bool vvv, GlobalOption.SaveDraw
                    or GlobalOption.DebugMode) => vvv.ToMaaOptionValue(),

            (LoggingLevel v, GlobalOption.StdoutLevel) => ((int)v).ToMaaOptionValue(),

            _ => throw new NotSupportedException($"'{nameof(GlobalOption)}.{opt}' or type '{typeof(T)}' is not supported."),
        };

        return MaaGlobalSetOption((MaaGlobalOption)opt, optValue, (MaaOptionValueSize)optValue.Length);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaGlobalLoadPlugin"/>.
    /// </remarks>
    public bool LoadPlugin(string libraryPath)
        => MaaGlobalLoadPlugin(libraryPath);
}
