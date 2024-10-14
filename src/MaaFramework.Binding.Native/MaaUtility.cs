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
            (int vvvv, GlobalOption.StdoutLevel) => MaaMarshaller.ConvertToMaaOptionValue(vvvv),
            (string v, GlobalOption.LogDir) => MaaMarshaller.ConvertToMaaOptionValue(v),
            (bool vvv, GlobalOption.SaveDraw
                    or GlobalOption.Recording
                    or GlobalOption.ShowHitDraw
                    or GlobalOption.DebugMode) => MaaMarshaller.ConvertToMaaOptionValue(vvv),

            (LoggingLevel v, GlobalOption.StdoutLevel) => MaaMarshaller.ConvertToMaaOptionValue((int)v),

            _ => throw new InvalidOperationException(),
        };

        return MaaSetGlobalOption((MaaGlobalOption)opt, optValue, (MaaOptionValueSize)optValue.Length);
    }
}
