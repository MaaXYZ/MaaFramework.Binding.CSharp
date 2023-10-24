namespace MaaFramework.Binding.Enums;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

public enum DebuggingControllerType
{
    Invalid = 0,

    Touch_Ignore = 1,
    Touch_Mask = 0xFF,

    Key_Ignore = 1 << 8,
    Key_Mask = 0xFF00,

    Input_Preset_Ignore = Touch_Ignore | Key_Ignore,

    Screencap_ReadIndex = 1 << 16,
    Screencap_Mask = 0xFF0000,
}
