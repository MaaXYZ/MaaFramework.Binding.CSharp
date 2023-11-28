namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An abstract class providing a common mechanism for releasing handles from <see cref="MaaFramework"/>.
/// </summary>
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
        if (!_handle.Equals(_invalidHandle))
        {
            ReleaseHandle();
            _handle = _invalidHandle;
        }
    }

    /// <inheritdoc/>
    public override bool IsInvalid => _handle.Equals(_invalidHandle);

    /// <summary>
    ///     Creats a <see cref="MaaDisposableHandle{T}"/> instance.
    /// </summary>
    /// <param name="invalidHandleValue">The invalid handle value.</param>
    protected MaaDisposableHandle(T invalidHandleValue)
    {
        _invalidHandle = invalidHandleValue;
        _handle = _invalidHandle;
    }

    private readonly T _invalidHandle;
    private T _handle;

    /// <summary>
    ///     Sets the handle to the specified pre-existing handle.
    /// </summary>
    /// <param name="handle">The pre-existing handle to use.</param>
    protected void SetHandle(T handle)
    {
        _handle = handle;
    }

    /// <summary>
    ///     When overridden in a derived class, executes the code required to free the handle.
    /// </summary>
    /// <returns>true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false.</returns>
    protected abstract void ReleaseHandle();
}
