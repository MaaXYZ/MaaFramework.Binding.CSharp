namespace MaaFramework.Binding;

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
    ///     value: <see cref="string"/>, log dir, eg: "C:\\Users\\Administrator\\Desktop\\log"; val_size: <see cref="string"/> length
    /// </remarks>
    LogDir = 1,

    /// <summary>
    ///     The debug mode option, value is <see cref="bool"/> indicating whether turns on the debug mode.
    /// </summary>
    /// <remarks>
    ///     value: <see cref="bool"/>, eg: true; val_size: sizeof(<see cref="bool"/>)
    /// </remarks>
    SaveDraw = 2,

    /// <summary>
    ///      Dump all screenshots and actions.
    ///      This option will || with <see cref="ControllerOption.Recording"/>.
    /// </summary>
    /// <remarks>
    ///      value: <see cref="bool"/>, eg: true; val_size: sizeof(<see cref="bool"/>)
    /// </remarks>
    Recording = 3,

    /// <summary>
    ///     The LogLevel.
    /// </summary>
    /// <remarks>
    ///     value: <see cref="LoggingLevel"/>, val_size: sizeof(<see cref="LoggingLevel"/>), default by <see cref="LoggingLevel.Error"/>
    /// </remarks>
    StdoutLevel = 4,

    /// <summary>
    ///     Use cv::imshow to show the hit draw.
    /// </summary>
    /// <remarks>
    ///     value: <see cref="bool"/>, eg: true; val_size: sizeof(<see cref="bool"/>)
    /// </remarks>
    ShowHitDraw = 5,
}
