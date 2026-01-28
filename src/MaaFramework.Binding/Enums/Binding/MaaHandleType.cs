namespace MaaFramework.Binding;

/// <summary>
///     Represents the type of <see cref="MaaFramework"/> handle.
/// </summary>
[Flags]
public enum MaaHandleType
{
    /// <summary>
    ///     Invalid type.
    /// </summary>
    None = 0,
    /// <summary>
    ///     MaaTasker Handle.
    /// </summary>
    Tasker = 1,
    /// <summary>
    ///     MaaResource Handle.
    /// </summary>
    Resource = 2,
    /// <summary>
    ///     MaaController Handle.
    /// </summary>
    Controller = 4,
    /// <summary>
    ///     MaaContext Handle.
    /// </summary>
    Context = 8,
}
