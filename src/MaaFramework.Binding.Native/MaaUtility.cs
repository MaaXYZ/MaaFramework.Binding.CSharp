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
    public bool SetOption(GlobalOption opt, int value)
        => SetOption(opt, value.ToMaaOptionValues());

    /// <inheritdoc/>
    public bool SetOption(GlobalOption opt, bool value)
        => SetOption(opt, value.ToMaaOptionValues());

    /// <inheritdoc/>
    public bool SetOption(GlobalOption opt, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        return SetOption(opt, value.ToMaaOptionValues());
    }

    /// <inheritdoc cref="SetOption(GlobalOption, int)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetGlobalOption"/>.
    /// </remarks>
    protected static bool SetOption(GlobalOption opt, MaaOptionValue[] value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return MaaSetGlobalOption((MaaGlobalOption)opt, ref value[0], (MaaOptionValueSize)value.Length).ToBoolean();
    }
}
