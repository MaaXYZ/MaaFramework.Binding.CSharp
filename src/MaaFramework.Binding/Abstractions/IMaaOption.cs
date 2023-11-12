namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An interface defining methods that set options for <see cref="MaaFramework"/>.
/// </summary>
public interface IMaaOption<in T> where T : Enum
{
    /// <summary>
    ///     Sets value to a option.
    /// </summary>
    /// <param name="option">The option.</param>
    /// <param name="value">The value.</param>
    /// <returns>true if the option was setted successfully; otherwise, false.</returns>
    bool SetOption(T option, int value);

    /// <inheritdoc cref="SetOption(T, int)"/>
    bool SetOption(T option, bool value);

    /// <inheritdoc cref="SetOption(T, int)"/>
    /// <exception cref="ArgumentException" />
    bool SetOption(T option, string value);
}
