namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An abstract class providing a common mechanism for releasing handles from <see cref="MaaFramework"/>.
/// </summary>
public abstract class MaaDisposableHandle : MaaDisposable, IMaaDisposableHandle
{
    /// <inheritdoc/>
    public nint Handle => _handle;

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
        if (_handle != _invalidHandle)
        {
            ReleaseHandle();
            _handle = _invalidHandle;
        }
    }

    /// <inheritdoc/>
    public override bool IsInvalid => _handle == _invalidHandle;

    /// <summary>
    ///     Creats a <see cref="MaaDisposableHandle"/> instance.
    /// </summary>
    /// <param name="invalidHandleValue">The invalid handle value.</param>
    protected MaaDisposableHandle(nint invalidHandleValue)
    {
        _invalidHandle = invalidHandleValue;
        _handle = _invalidHandle;
    }

    private readonly nint _invalidHandle;
    private nint _handle;

    /// <summary>
    ///     Sets the handle to the specified pre-existing handle.
    /// </summary>
    /// <param name="handle">The pre-existing handle to use.</param>
    protected void SetHandle(nint handle)
    {
        _handle = handle;
    }

    /// <summary>
    ///     When overridden in a derived class, executes the code required to free the handle.
    /// </summary>
    /// <returns>true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false.</returns>
    protected abstract void ReleaseHandle();
}
