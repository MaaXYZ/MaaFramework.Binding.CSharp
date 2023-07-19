using System.Diagnostics.CodeAnalysis;

namespace MaaToolKit.Enums;

/// <summary>
///
/// </summary>
[SuppressMessage("Design", "CA1008:Enums should have zero value")]
public enum ControllerOption
{
    /// <summary>
    ///
    /// </summary>
    ScreenshotTargetWidth = 1,

    /// <summary>
    ///
    /// </summary>
    ScreenshotTargetHeight = 2,

    /// <summary>
    ///
    /// </summary>
    DefaultAppPackageEntry = 3,

    /// <summary>
    ///
    /// </summary>
    DefaultAppPackage = 4,
}
