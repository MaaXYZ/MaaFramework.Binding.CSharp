namespace MaaFramework.Binding.Custom;

/// <inheritdoc/>
public interface IMaaCustomExecutor : IMaaCustom
{
    /// <summary>
    ///     Gets or initializes the execution path.
    /// </summary>
    public string Path { get; init; }

    /// <summary>
    ///     Gets or initializes the parameter.
    /// </summary>
    public IEnumerable<string> Parameter { get; init; }
}
