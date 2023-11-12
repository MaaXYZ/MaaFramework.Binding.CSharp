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
    public bool SetOption(GlobalOption option, int value)
        => SetOption(option, value.ToMaaOptionValues());

    /// <inheritdoc/>
    public bool SetOption(GlobalOption option, bool value)
        => SetOption(option, value.ToMaaOptionValues());

    /// <inheritdoc/>
    public bool SetOption(GlobalOption option, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        return SetOption(option, value.ToMaaOptionValues());
    }

    /// <inheritdoc cref="SetOption(GlobalOption, int)"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetGlobalOption"/>.
    /// </remarks>
    protected static bool SetOption(GlobalOption option, MaaOptionValue[] value)
        => MaaSetGlobalOption((MaaGlobalOption)option, ref value[0], (MaaOptionValueSize)value.Length).ToBoolean();
}
