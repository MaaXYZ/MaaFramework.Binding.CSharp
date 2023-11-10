using MaaFramework.Binding.Enums;
using MaaFramework.Binding.Interop;
using static MaaFramework.Binding.Interop.Framework.MaaUtility;

namespace MaaFramework.Binding;

/// <summary>
///     A static class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Framework.MaaUtility"/>.
/// </summary>
public static class MaaUtility
{
    /// <summary>
    ///     Gets version of MaaFramework.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="MaaVersion"/>.
    /// </remarks>
    public static string Version => MaaVersion().ToStringUTF8();

    /// <summary>
    ///     Sets the path to the log directory of MaaFramework.
    /// </summary>
    /// <param name="logDirectory">The log directory.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetGlobalOption"/>.
    /// </remarks>
    /// <returns>true if the log directory was setted successfully; otherwise, false.</returns>
    /// <exception cref="ArgumentException" />
    public static bool SetLogging(string logDirectory)
        => SetOption(GlobalOption.Logging, logDirectory);

    /// <summary>
    ///     Sets a value indicating whether turns on the debug mode of MaaFramework.
    /// </summary>
    /// <param name="enable">The value.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetGlobalOption"/>.
    /// </remarks>
    /// <returns>true if the debug mode was setted successfully; otherwise, false.</returns>
    public static bool SetDebugMode(bool enable)
        => SetOption(GlobalOption.Logging, enable);

    #region MaaOption

    /// <summary>
    ///     Sets value to a option.
    /// </summary>
    /// <param name="option">The option.</param>
    /// <param name="value">The value.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaSetGlobalOption"/>.
    /// </remarks>
    /// <returns>true if the option was setted successfully; otherwise, false.</returns>
    internal static bool SetOption(GlobalOption option, MaaOptionValue[] value)
        => MaaSetGlobalOption((MaaGlobalOption)option, ref value[0], (MaaOptionValueSize)value.Length).ToBoolean();

    /// <inheritdoc cref="SetOption(GlobalOption, MaaOptionValue[])"/>
    public static bool SetOption(GlobalOption option, int value)
        => SetOption(option, value.ToMaaOptionValues());

    /// <inheritdoc cref="SetOption(GlobalOption, MaaOptionValue[])"/>
    public static bool SetOption(GlobalOption option, bool value)
        => SetOption(option, value.ToMaaOptionValues());

    /// <inheritdoc cref="SetOption(GlobalOption, MaaOptionValue[])"/>
    /// <exception cref="ArgumentException" />
    public static bool SetOption(GlobalOption option, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        return SetOption(option, value.ToMaaOptionValues());
    }

    #endregion
}
