namespace MaaToolKit.Extensions.Enums;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

public enum AdbControllerType
{
    Invalid = 0,

    TouchAdb = 1,
    TouchMiniTouch = 2,
    TouchMaaTouch = 3,
    TouchMask = 0xFF,

    KeyAdb = 1 << 8,
    KeyMaaTouch = 2 << 8,
    KeyMask = 0xFF00,

    InputPresetAdb = TouchAdb | KeyAdb,
    InputPresetMinitouch = TouchMiniTouch | KeyAdb,
    InputPresetMaatouch = TouchMaaTouch | KeyMaaTouch,

    ScreenCapFastestWay = 1 << 16,
    ScreenCapRawByNetcat = 2 << 16,
    ScreenCapRawWithGzip = 3 << 16,
    ScreenCapEncode = 4 << 16,
    ScreenCapEncodeToFile = 5 << 16,
    ScreenCapMinicapDirect = 6 << 16,
    ScreenCapMinicapStream = 7 << 16,
    ScreenCapMask = 0xFF0000,
}
