namespace MaaToolkit.Extensions.UnitTests;

/// <summary>
///     A static class of global info.
/// </summary>
[TestClass]
public static class GlobalInfo
{
    internal static string AdbPath { get; set; } = "adb";
    internal static string Address { get; set; } = "127.0.0.1:5555";

    internal static string DebugPath { get; set; } = Path.GetFullPath("./debug");
    internal static string ResourcePath { get; set; } = Path.GetFullPath("./SampleResource");

    /// <summary>
    ///     Assembly initialize
    /// </summary>
    /// <param name="testContext"></param>
    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext testContext)
    {
        // 请修改 TestParam.runsettings，并在测试资源管理器——设置——配置运行设置
        // 选择解决方案范围内的 runsettings 文件：src\Common\TestParam.runsettings
        var adbPath = testContext.Properties["adbPath"] as string
            ?? Environment.GetEnvironmentVariable("AdbPath");
        var address = testContext.Properties["address"] as string
            ?? Environment.GetEnvironmentVariable("Address");
        AdbPath = adbPath
            ?? AdbPath;
        Address = address
            ?? Address;
        DebugPath = testContext.TestDir
            ?? DebugPath;
        ResourcePath = Path.Combine(Environment.CurrentDirectory, "SampleResource");
    }
}
