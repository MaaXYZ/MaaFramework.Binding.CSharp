using System.Diagnostics.CodeAnalysis;

namespace MaaCommon.Enums;

/// <summary>
/// 
/// </summary>
[SuppressMessage("Design", "CA1008:Enums should have zero value")]
public enum AdbControllerType
{
    /// <summary>
    ///     
    /// </summary>
    TouchAdb = 1,
    /// <summary>
    /// 
    /// </summary>
    TouchMiniTouch = 2,
    /// <summary>
    /// 
    /// </summary>
    TouchMaaTouch = 3,
    /// <summary>
    /// 
    /// </summary>
    TouchMask = 0xFF,

    /// <summary>
    /// 
    /// </summary>
    KeyAdb = 1 << 8,
    /// <summary>
    /// 
    /// </summary>
    KeyMaaTouch = 2 << 8,
    /// <summary>
    /// 
    /// </summary>
    KeyMask = 0xFF00,

    /// <summary>
    /// 
    /// </summary>
    ScreenCapFastestWay = 1 << 16,
    /// <summary>
    /// 
    /// </summary>
    ScreenCapRawByNetcat = 2 << 16,
    /// <summary>
    /// 
    /// </summary>
    ScreenCapRawWithGzip = 3 << 16,
    /// <summary>
    /// 
    /// </summary>
    ScreenCapEncode = 4 << 16,
    /// <summary>
    /// 
    /// </summary>
    ScreenCapEncodeToFile = 5 << 16,
    /// <summary>
    /// 
    /// </summary>
    ScreenCapMinicapDirect = 6 << 16,
    /// <summary>
    /// 
    /// </summary>
    ScreenCapMinicapStream = 7 << 16,
    /// <summary>
    /// 
    /// </summary>
    ScreenCapMask = 0xFF0000,

    /// <summary>
    /// 
    /// </summary>
    InputPresetAdb = TouchAdb | KeyAdb,
    
    /// <summary>
    /// 
    /// </summary>
    InputPresetMinitouch = TouchMiniTouch | KeyAdb,
    
    /// <summary>
    /// 
    /// </summary>
    InputPresetMaatouch = TouchMaaTouch | KeyMaaTouch,
}
