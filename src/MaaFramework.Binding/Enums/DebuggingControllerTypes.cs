namespace MaaFramework.Binding;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
#pragma warning disable CA1008 // 枚举应具有零值
#pragma warning disable CA2217 // 不要使用 FlagsAttribute 标记枚举
#pragma warning disable S2346 // Flags enumerations zero-value members should be named "None"
#pragma warning disable S4070 // Non-flags enums should not be marked with "FlagsAttribute"

/// <summary>
///     Debugging controller types.
/// </summary>
/// <remarks>
///     <see cref="DebuggingControllerTypes"/> combines TouchType, KeyType and ScreencapType.
/// </remarks>
[Flags]
public enum DebuggingControllerTypes
{
    Invalid = 0,

    TouchIgnore = 1,
    TouchMask = 0xFF,

    KeyIgnore = 1 << 8,
    KeyMask = 0xFF00,

    InputPresetIgnore = TouchIgnore | KeyIgnore,

    ScreenCapReadIndex = 1 << 16,
    ScreenCapMask = 0xFF0000,
}
