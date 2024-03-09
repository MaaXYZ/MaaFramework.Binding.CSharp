namespace MaaFramework.Binding;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
#pragma warning disable CA1008 // 枚举应具有零值
#pragma warning disable CA2217 // 不要使用 FlagsAttribute 标记枚举
#pragma warning disable S2346 // Flags enumerations zero-value members should be named "None"
#pragma warning disable S4070 // Non-flags enums should not be marked with "FlagsAttribute"

/// <summary>
///     Win32 controller types.
/// </summary>
/// <remarks>
///     <see cref="AdbControllerTypes"/> combines TouchType, KeyType and ScreencapType.
/// </remarks>
[Flags]
public enum Win32ControllerTypes
{
    Invalid = 0,
    TouchMask = 0xFF,
    KeyMask = 0xFF00,
    ScreencapMask = 0xFF0000,

    TouchSendMessage = 1,

    KeySendMessage = 1 << 8,

    ScreencapGDI = 1 << 16,
    ScreencapDXGIDesktopDup = 2 << 16,
    // ScreencapDXGIBackBuffer = 3 << 16,
    ScreencapDXGIFramePool = 4 << 16,
}
