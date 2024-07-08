namespace MaaFramework.Binding.Custom;

/// <summary>
///     A class providing a reference implementation for recording a maa custom recognizer executor.
/// </summary>
public class MaaCustomRecognizerExecutor : IMaaCustomExecutor
{
    /// <inheritdoc/>
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc/>
    public required string Path { get; init; }

    /// <inheritdoc/>
    public required IEnumerable<string> Parameter { get; init; }
}
