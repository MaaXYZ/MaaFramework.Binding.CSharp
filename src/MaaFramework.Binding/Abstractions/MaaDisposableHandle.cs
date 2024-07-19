namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An abstract class providing a common mechanism for releasing handles from <see cref="MaaFramework"/>.
/// </summary>
/// <typeparam name="T">The type of handle.</typeparam>
public abstract class MaaDisposableHandle<T> : MaaDisposable, IMaaDisposableHandle<T> where T : IEquatable<T>
{
    /// <inheritdoc/>
    public T Handle => _handle;

#pragma warning disable CA1816 // Dispose 方法应调用 SuppressFinalize
#pragma warning disable S3971 // "GC.SuppressFinalize" should not be called
    /// <inheritdoc/>
    public void SetHandleAsInvalid()
    {
        _handle = _invalidHandle;
        GC.SuppressFinalize(this);
    }
#pragma warning restore S3971 // "GC.SuppressFinalize" should not be called
#pragma warning restore CA1816 // Dispose 方法应调用 SuppressFinalize

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (_handle.Equals(_invalidHandle)) return;
        if (_needReleased) ReleaseHandle();
        _handle = _invalidHandle;
    }

    /// <inheritdoc/>
    public override bool IsInvalid => _handle.Equals(_invalidHandle);

    /// <summary>
    ///     Creates a <see cref="MaaDisposableHandle{T}"/> instance.
    /// </summary>
    /// <param name="invalidHandleValue">The invalid handle value.</param>
    protected MaaDisposableHandle(T invalidHandleValue)
    {
        _invalidHandle = invalidHandleValue;
        _handle = _invalidHandle;
    }

    private readonly T _invalidHandle;
    private T _handle;
    private bool _needReleased;

    /// <summary>
    ///     Sets the handle to the specified pre-existing handle.
    /// </summary>
    /// <param name="handle">The pre-existing handle to use.</param>
    /// <param name="needReleased">The value indicates whether the <paramref name="handle"/> needs to be released on <see cref="Dispose"/></param>
    protected void SetHandle(T handle, bool needReleased)
    {
        _handle = handle;
        _needReleased = needReleased;
    }

    /// <summary>
    ///     When overridden in a derived class, executes the code required to free the handle.
    /// </summary>
    /// <returns>true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false.</returns>
    protected abstract void ReleaseHandle();
}
