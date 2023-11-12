namespace MaaFramework.Binding;

/// <summary>
///     <see cref="IMaaInstance"/> dispose options.
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
