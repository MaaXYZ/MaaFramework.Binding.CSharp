namespace MaaFramework.Binding.Enums;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

/// <summary>
///     Debugging controller types.
/// </summary>
/// <remarks>
///     <see cref="DebuggingControllerTypes"/> combines TouchType, KeyType and ScreencapType.
/// </remarks>
public enum DebuggingControllerTypes
{
    Invalid = 0,

    TouchIgnore = 1,
    TouchMask = 0xFF,

    KeyIgnore = 1 << 8,
    KeyMask = 0xFF00,

    InputPresetIgnore = TouchIgnore | KeyIgnore,

    ScreencapReadIndex = 1 << 16,
    ScreencapMask = 0xFF0000,
}
