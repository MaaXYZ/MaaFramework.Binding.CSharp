namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An interface defining methods that set options for <see cref="MaaFramework"/>.
/// </summary>
public interface IMaaOption<in T> where T : Enum
{
    /// <summary>
    ///     Sets value to a option.
    /// </summary>
    /// <param name="opt">The option.</param>
    /// <param name="value">The value.</param>
    /// <returns>true if the option was setted successfully; otherwise, false.</returns>
    bool SetOption(T opt, int value);

    /// <inheritdoc cref="SetOption(T, int)"/>
    bool SetOption(T opt, bool value);

    /// <inheritdoc cref="SetOption(T, int)"/>
    /// <exception cref="ArgumentException" />
    bool SetOption(T opt, string value);
}
