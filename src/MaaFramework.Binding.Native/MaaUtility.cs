using MaaFramework.Binding.Native.Interop;
using static MaaFramework.Binding.Native.Interop.MaaUtility;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Native.Interop.MaaUtility"/>.
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

        var bytes = opt switch
        {
            GlobalOption.Invalid => throw new InvalidOperationException(),
            GlobalOption.LogDir => value switch { string v => v.ToMaaOptionValues(), _ => throw new InvalidOperationException(), },
            GlobalOption.SaveDraw => value switch { bool v => v.ToMaaOptionValues(), _ => throw new InvalidOperationException(), },
            GlobalOption.Recording => value switch { bool v => v.ToMaaOptionValues(), _ => throw new InvalidOperationException(), },
            GlobalOption.ShowHitDraw => value switch { bool v => v.ToMaaOptionValues(), _ => throw new InvalidOperationException(), },
            GlobalOption.StdoutLevel => value switch
            {
                LoggingLevel v => ((int)v).ToMaaOptionValues(),
                int v => v.ToMaaOptionValues(),
                _ => throw new InvalidOperationException(),
            },
            _ => throw new NotImplementedException(),
        };

        return MaaSetGlobalOption((MaaGlobalOption)opt, ref bytes[0], (MaaOptionValueSize)bytes.Length).ToBoolean();
    }
}
