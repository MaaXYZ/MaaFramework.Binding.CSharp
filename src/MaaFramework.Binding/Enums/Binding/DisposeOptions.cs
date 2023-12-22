namespace MaaFramework.Binding;

/// <summary>
///     <see cref="IMaaInstance"/> dispose options.
/// </summary>
///
[Flags]
public enum DisposeOptions
{
    /// <summary>
    ///     No additional disposal required.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Disposes <see cref="IMaaResource"/> and <see cref="IMaaController"/>.
    /// </summary>
    All = Resource | Controller,

    /// <summary>
    ///     Disposes <see cref="IMaaResource"/>.
    /// </summary>
    Resource = 1 << 0,

    /// <summary>
    ///     Disposes <see cref="IMaaController"/>.
    /// </summary>
    Controller = 1 << 1,
}
