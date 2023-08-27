namespace MaaToolKit.Extensions.Enums;

/// <summary>
///     Controller options
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
    ///     value: int, eg: 1920; val_size: sizeof(int)
    /// </remarks>
    ScreenshotTargetLongSide = 1,

    /// <summary>
    ///     Only one of long and short side can be set, and the other is automatically scaled according to the aspect ratio.
    /// </summary>
    /// <remarks>
    ///     value: int, eg: 1080; val_size: sizeof(int)
    /// </remarks>
    ScreenshotTargetShortSide = 2,

    /// <summary>
    ///     For StartApp
    /// </summary>
    /// <remarks>
    ///     value: string, eg: "com.hypergryph.arknights/com.u8.sdk.U8UnityContext"; val_size: string length
    /// </remarks>
    DefaultAppPackageEntry = 3,

    /// <summary>
    ///     For StopApp
    /// </summary>
    /// <remarks>
    ///     value: string, eg: "com.hypergryph.arknights"; val_size: string length
    /// </remarks>
    DefaultAppPackage = 4,
}
