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

//  设计思路：
//      这个是各种非托管资源的基础
//      更新迭代后可能不止 Handle 需要释放
//      所以拆开给 Buffer Instance 各种用到非托管资源的实现使用
