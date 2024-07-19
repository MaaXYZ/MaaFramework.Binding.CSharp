namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An interface defining methods that set options for <see cref="MaaFramework"/>.
/// </summary>
/// <typeparam name="TOption">The <see cref="ControllerOption"/>, <see cref="GlobalOption"/>, <see cref="InstanceOption"/>, <see cref="ResourceOption"/>.</typeparam>
public interface IMaaOption<in TOption> where TOption : Enum
{
    /// <summary>
    ///     Sets value to an option.
    /// </summary>
    /// <typeparam name="T">The type specified by the <paramref name="opt"/>(see remarks of enumerations).</typeparam>
    /// <param name="opt">The option.</param>
    /// <param name="value">The value.</param>
    /// <returns>true if the option was set successfully; otherwise, false.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    bool SetOption<T>(TOption opt, T value);
}
