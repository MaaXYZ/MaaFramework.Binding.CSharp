using System.Diagnostics.CodeAnalysis;

namespace MaaCommon.Enums;

/// <summary>
/// 
/// </summary>
[SuppressMessage("Design", "CA1008:Enums should have zero value")]
public enum ControllerOptions
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
