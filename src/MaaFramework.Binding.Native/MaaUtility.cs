using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaUtility;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaUtility"/>.
/// </summary>
public class MaaUtility : IMaaUtility
{
    /// <summary>
    ///    Gets the shared <see cref="MaaUtility"/> instance.
    /// </summary>
    public static MaaUtility Shared { get; } = new();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaVersion"/>.
    /// </remarks>
    public string Version => MaaVersion();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetGlobalOption"/>.
    /// </remarks>
    public bool SetOption<T>(GlobalOption opt, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var optValue = (value, opt) switch
        {
            (int vvvv, GlobalOption.StdoutLevel) => vvvv.ToMaaOptionValue(),
            (string v, GlobalOption.LogDir) => v.ToMaaOptionValue(),
#pragma warning disable CS0618 // 类型或成员已过时
            (bool vvv, GlobalOption.SaveDraw
                    or GlobalOption.Recording
                    or GlobalOption.ShowHitDraw
                    or GlobalOption.DebugMode) => vvv.ToMaaOptionValue(),
#pragma warning restore CS0618 // 类型或成员已过时

            (LoggingLevel v, GlobalOption.StdoutLevel) => ((int)v).ToMaaOptionValue(),

            _ => throw new NotSupportedException($"'{nameof(GlobalOption)}.{opt}' or type '{typeof(T)}' is not supported."),
        };

        return MaaSetGlobalOption((MaaGlobalOption)opt, optValue, (MaaOptionValueSize)optValue.Length);
    }
}
