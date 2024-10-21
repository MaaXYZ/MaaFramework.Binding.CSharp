namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An interface defining methods that set options for <see cref="MaaFramework"/>.
/// </summary>
/// <typeparam name="TOption">The <see cref="ControllerOption"/>, <see cref="GlobalOption"/>, <see cref="TaskerOption"/>, <see cref="ResourceOption"/>.</typeparam>
public interface IMaaOption<in TOption> where TOption : Enum
{
    /// <summary>
    ///     Sets value to an option.
    /// </summary>
    /// <typeparam name="T">The type specified by the <paramref name="opt"/>(see remarks of enumerations).</typeparam>
    /// <param name="opt">The option.</param>
    /// <param name="value">The value.</param>
    /// <returns><see langword="true"/> if the option was set successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="NotSupportedException">The <paramref name="opt"/> or <paramref name="value"/> is not supported.</exception>
    bool SetOption<T>(TOption opt, T value);
}
