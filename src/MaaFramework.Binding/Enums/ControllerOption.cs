namespace MaaFramework.Binding;

/// <summary>
///     Controller option.
/// </summary>
public enum ControllerOption
{
    /// <summary>
    ///     Invalid option
    /// </summary>
    Invalid = 0,

    /// <summary>
    ///     Only one of long and short side can be set, and the other is automatically scaled according to the aspect ratio.
    /// </summary>
    /// <remarks>
    ///     value: <see cref="int"/>, eg: 1920; val_size: sizeof(<see cref="int"/>)
    /// </remarks>
    ScreenshotTargetLongSide = 1,

    /// <summary>
    ///     Only one of long and short side can be set, and the other is automatically scaled according to the aspect ratio.
    /// </summary>
    /// <remarks>
    ///     value: <see cref="int"/>, eg: 1080; val_size: sizeof(<see cref="int"/>)
    /// </remarks>
    ScreenshotTargetShortSide = 2,

    /// <summary>
    ///     For StartApp
    /// </summary>
    /// <remarks>
    ///     value: <see cref="string"/>, eg: "com.hypergryph.arknights/com.u8.sdk.U8UnityContext"; val_size: <see cref="string"/> length
    /// </remarks>
    DefaultAppPackageEntry = 3,

    /// <summary>
    ///     For StopApp
    /// </summary>
    /// <remarks>
    ///     value: <see cref="string"/>, eg: "com.hypergryph.arknights"; val_size: <see cref="string"/> length
    /// </remarks>
    DefaultAppPackage = 4,

    /// <summary>
    ///     Dump all screenshots and actions.
    ///     This option will || with <see cref="GlobalOption.Recording"/>.
    /// </summary>
    /// <remarks>
    ///     value: <see cref="bool"/>, eg: true; val_size: sizeof(<see cref="bool"/>)
    /// </remarks>
    Recording = 5,
}
