namespace MaaFramework.Binding.UnitTests;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

/// <summary>
///     The maa interop types
/// </summary>
[Flags]
public enum MaaTypes
{
    None = 0,
    All = Native | Placeholder,

    Native = 1 << 0,
    Placeholder = 1 << 30,
}
