namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An abstract class providing a common mechanism for releasing unmanaged resources from <see cref="MaaFramework"/>.
/// </summary>
public abstract class MaaDisposable : IDisposable
{
    /// <summary>
    ///     Releases all resources from <see cref="MaaFramework"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected abstract void Dispose(bool disposing);

    /// <summary>
    ///     When overridden in a derived class, gets a value indicating whether the unmanaged resources are invalid.
    /// </summary>
    public abstract bool IsInvalid { get; }
}
