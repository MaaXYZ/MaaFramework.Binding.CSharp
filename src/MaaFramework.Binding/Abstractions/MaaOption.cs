using MaaFramework.Binding.Interop;

namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     A abstract class providing common overloading members of SetOption method.
/// </summary>
public abstract class MaaOption<T> where T : Enum
{
    /// <summary>
    ///     Sets value to a option.
    /// </summary>
    /// <param name="option">The option.</param>
    /// <param name="value">The value.</param>
    /// <returns>true if the option was setted successfully; otherwise, false.</returns>
    internal abstract bool SetOption(T option, MaaOptionValue[] value);

    /// <inheritdoc cref="SetOption(T, MaaOptionValue[])"/>
    public bool SetOption(T option, int value)
        => SetOption(option, value.ToMaaOptionValues());

    /// <inheritdoc cref="SetOption(T, MaaOptionValue[])"/>
    public bool SetOption(T option, bool value)
        => SetOption(option, value.ToMaaOptionValues());

    /// <inheritdoc cref="SetOption(T, MaaOptionValue[])"/>
    /// <exception cref="ArgumentException" />
    public bool SetOption(T option, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        return SetOption(option, value.ToMaaOptionValues());
    }
}
