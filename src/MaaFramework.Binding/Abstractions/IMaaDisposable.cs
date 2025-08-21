namespace MaaFramework.Binding.Abstractions;

/// <summary>
///     An interface defining member about handles from <see cref="MaaFramework"/>.
/// </summary>
public interface IMaaDisposable : IDisposable
{
    /// <summary>
    ///     Gets a value indicating whether the unmanaged resources from <see cref="MaaFramework"/> are invalid, when overridden in a derived class.
    /// </summary>
    bool IsInvalid { get; }

    /// <summary>
    ///     Gets a value indicating whether an <see cref="ObjectDisposedException"/> is thrown when the current instance is invalid but still called.
    /// </summary>
    bool ThrowOnInvalid { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the current instance is stateless.
    /// </summary>
    /// <remarks>
    ///     The lifetime of unmanaged resources is controlled by <see cref="MaaFramework"/> if <see langword="this"/> is stateless.
    /// </remarks>
    bool IsStateless { get; }

    /// <summary>
    ///     Occurs directly after <see cref="IDisposable.Dispose"/> is called, and can be handled to cancel the unmanaged resources release.
    /// </summary>
    event System.ComponentModel.CancelEventHandler? Releasing;

    /// <summary>
    ///     Occurs when the unmanaged resources is released.
    /// </summary>
    event EventHandler? Released;
}

//  设计思路：
//      这个是各种非托管资源的基础
//      更新迭代后可能不止 Handle 需要释放
//      所以拆开给 Buffer Instance 各种用到非托管资源的实现使用
