namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An interface defining member about handles from <see cref="MaaFramework"/>.
/// </summary>
public interface IMaaDisposableHandle<out T> : IMaaDisposable
{
    /// <summary>
    ///     Gets the handle to be wrapped.
    /// </summary>
    /// <remarks>
    ///     Throws if handle is invalid and "ThrowOnInvalid" is <see langword="true"/>.
    /// </remarks>
    /// <exception cref="InvalidOperationException">The handle is invalid.</exception>
    T Handle { get; }

    /// <summary>
    ///     Marks a handle as no longer used.
    /// </summary>
    void SetHandleAsInvalid();
}
