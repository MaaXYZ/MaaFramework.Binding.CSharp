namespace MaaFramework.Binding.Custom;

#pragma warning disable CA1040 // 避免使用空接口

/// <inheritdoc/>
public interface IMaaCustomExecutor : IMaaCustom
{
    /// <summary>
    ///     Gets or inits the execution path.
    /// </summary>
    public string Path { get; init; }

    /// <summary>
    ///     Gets or inits the parameter.
    /// </summary>
    public IEnumerable<string> Parameter { get; init; }
}
