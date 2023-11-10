namespace MaaFramework.Binding.Enums;

/// <summary>
///     <see cref="MaaInstance"/> dispose options.
/// </summary>
///
[Flags]
public enum DisposeOptions
{
    /// <summary>
    ///     None dispose.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Resource dispose.
    /// </summary>
    Resource = 1 << 0,

    /// <summary>
    ///     Controller dispose.
    /// </summary>
    Controller = 1 << 1,
}
