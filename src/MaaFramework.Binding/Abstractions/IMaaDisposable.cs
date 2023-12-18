namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An interface defining member about handles from <see cref="MaaFramework"/>.
/// </summary>
public interface IMaaDisposable : IDisposable
{
    /// <summary>
    ///     When overridden in a derived class, gets a value indicating whether the unmanaged resources from <see cref="MaaFramework"/> are invalid.
    /// </summary>
    bool IsInvalid { get; }
}
