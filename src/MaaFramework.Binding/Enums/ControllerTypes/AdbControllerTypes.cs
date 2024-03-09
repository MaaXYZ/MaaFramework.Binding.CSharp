namespace MaaFramework.Binding;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
#pragma warning disable CA1008 // 枚举应具有零值
#pragma warning disable CA2217 // 不要使用 FlagsAttribute 标记枚举
#pragma warning disable S2346 // Flags enumerations zero-value members should be named "None"
#pragma warning disable S4070 // Non-flags enums should not be marked with "FlagsAttribute"

/// <summary>
///     Adb controller types.
/// </summary>
/// <remarks>
///     <see cref="AdbControllerTypes"/> combines TouchType, KeyType and ScreencapType.
/// </remarks>
[Flags]
public enum AdbControllerTypes
{
    Invalid = 0,
    TouchMask = 0xFF,
    KeyMask = 0xFF00,
    ScreencapMask = 0xFF0000,

    TouchAdb = 1,
    TouchMiniTouch = 2,
    TouchMaaTouch = 3,
    TouchAutoDetect = TouchMask - 1,

    KeyAdb = 1 << 8,
    KeyMaaTouch = 2 << 8,
    KeyAutoDetect = KeyMask - (1 << 8),

    InputPresetAdb = TouchAdb | KeyAdb,
    InputPresetMinitouch = TouchMiniTouch | KeyAdb,
    InputPresetMaatouch = TouchMaaTouch | KeyMaaTouch,
    InputPresetAutoDetect = TouchAutoDetect | KeyAutoDetect,

    [Obsolete("This ScreencapFastestWayCompatible is about to be deprecated. Please use ScreencapFastestWay or ScreencapFastestLosslessWay instead.")]
    ScreencapFastestWayCompatible = 1 << 16,
    ScreencapRawByNetcat = 2 << 16,
    ScreencapRawWithGzip = 3 << 16,
    ScreencapEncode = 4 << 16,
    ScreencapEncodeToFile = 5 << 16,
    ScreencapMinicapDirect = 6 << 16,
    ScreencapMinicapStream = 7 << 16,
    ScreencapFastestLosslessWay = ScreencapMask - (2 << 16),
    ScreencapFastestWay = ScreencapMask - (1 << 16),
}
