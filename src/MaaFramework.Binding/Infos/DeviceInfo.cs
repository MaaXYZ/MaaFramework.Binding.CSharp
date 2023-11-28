namespace MaaFramework.Binding;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

/// <summary>
///     A class providing properties of device information. This class cannot be inherited.
/// </summary>
public sealed class DeviceInfo
{
    public required string Name { get; init; }
    public required string AdbPath { get; init; }
    public required string AdbSerial { get; init; }
    public required AdbControllerTypes AdbTypes { get; init; }
    public required string AdbConfig { get; init; }
}
