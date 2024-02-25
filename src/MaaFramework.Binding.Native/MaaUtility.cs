using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaUtility;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaUtility"/>.
/// </summary>
public class MaaUtility : IMaaUtility
{
    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaVersion"/>.
    /// </remarks>
    public string Version => MaaVersion().ToStringUTF8();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetGlobalOption"/>.
    /// </remarks>
    public bool SetOption<T>(GlobalOption opt, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var bytes = (value, opt) switch
        {
            (int vvvv, GlobalOption.StdoutLevel) => vvvv.ToMaaOptionValues(),
            (string v, GlobalOption.LogDir) => v.ToMaaOptionValues(),
            (bool vvv, GlobalOption.SaveDraw
                    or GlobalOption.Recording
                    or GlobalOption.ShowHitDraw) => vvv.ToMaaOptionValues(),

            (LoggingLevel v, GlobalOption.StdoutLevel) => ((int)v).ToMaaOptionValues(),

            _ => throw new InvalidOperationException(),
        };

        return MaaSetGlobalOption((MaaGlobalOption)opt, ref bytes[0], (MaaOptionValueSize)bytes.Length).ToBoolean();
    }
}
