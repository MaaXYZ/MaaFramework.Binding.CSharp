namespace MaaFramework.Binding.Enums;

/// <summary>
///     Global option.
/// </summary>
public enum GlobalOption
{
    /// <summary>
    ///     Invalid option
    /// </summary>
    Invalid = 0,

    /// <summary>
    ///     The logging option, value is the path to the log directory
    /// </summary>
    /// <remarks>
    ///     value: string, log dir, eg: "C:\\Users\\Administrator\\Desktop\\log"; val_size: string length
    /// </remarks>
    Logging = 1,

    /// <summary>
    ///     The debug mode option, value is bool indicating whether turns on the debug mode.
    /// </summary>
    /// <remarks>
    ///     value: bool, eg: true; val_size: sizeof(bool)
    /// </remarks>
    DebugMode = 2,
}
