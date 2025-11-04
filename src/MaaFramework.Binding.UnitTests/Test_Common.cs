using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Notification;

namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     A static class of global info.
/// </summary>
[TestClass]
public static class Common
{
    static Common()
    {
    }

    internal static string AdbPath { get; set; } = string.Empty;
    internal static string Address { get; set; } = string.Empty;

    internal static string DebugPath { get; set; } = Path.GetFullPath("./debug");
    internal static string BundlePath { get; set; } = Path.GetFullPath("./SampleResource");
    internal static string AgentPath { get; set; } = Path.GetFullPath($"./MaaAgentBinary");
    internal static string AdbConfig { get; set; } = File.ReadAllText(Path.GetFullPath($"{BundlePath}/controller_config.json"));
    internal static string ImagePath { get; set; } = Path.Join(BundlePath, "empty_1920x1080.png");

    private static void InitializeInfo(TestContext testContext)
    {
#if GITHUB_ACTIONS // use environment "AdbPath"
        AdbDeviceInfo[] devices = [];
#else
        var devices = MaaToolkit.Shared.AdbDevice.Find();
#endif

        // 请修改 TestParam.runsettings，并在测试资源管理器——设置——配置运行设置
        // 选择解决方案范围内的 runsettings 文件：src\Common\TestParam.runsettings
        AdbPath = testContext.Properties["AdbPath"] as string
            ?? Environment.GetEnvironmentVariable("AdbPath")
            ?? devices.FirstOrDefault()?.AdbPath
            ?? throw new InvalidOperationException("Failed to get AdbPath.");

        Address = testContext.Properties["Address"] as string
            ?? Environment.GetEnvironmentVariable("Address")
            ?? devices.FirstOrDefault()?.AdbSerial
            ?? throw new InvalidOperationException("Failed to get Address.");
    }

    /// <summary>
    ///     Assembly initialize.
    /// </summary>
    /// <param name="testContext">The TestContext.</param>
    [AssemblyInitialize]
    public static void InitializeAssembly(TestContext testContext)
    {
        ArgumentNullException.ThrowIfNull(testContext);

        _ = MaaGlobal.Shared.SetOption_LogDir(DebugPath);
        _ = MaaGlobal.Shared.SetOption_StdoutLevel(LoggingLevel.Off);

        InitializeInfo(testContext);
    }

    /// <summary>
    ///     Assembly cleanup.
    /// </summary>
    [AssemblyCleanup]
    public static void CleanupAssembly()
    {
        // Assembly cleanup.
    }

    internal static void DisposeData(IEnumerable<IMaaDisposable> data)
    {
        foreach (var d in data)
        {
            d.Dispose();
            if (d is not MaaImage)
            {
                Assert.IsTrue(d.IsInvalid);
            }
        }
    }

    internal static void OnCallback(object? sender, MaaCallbackEventArgs e)
    {
        Assert.IsNotNull(sender);
        Assert.IsNotNull(e);
        Assert.IsFalse(string.IsNullOrWhiteSpace(e.Message));
    }

    internal static NotificationHandlerRegistry NotificationHandlerRegistry { get; set; } = new();
    internal static NotificationHandler<ResourceLoadingDetail> OnResourceLoading = (type, detail) => Assert.IsNotNull(detail);
    internal static NotificationHandler<ControllerActionDetail> OnControllerAction = (type, detail) => Assert.IsNotNull(detail);
    internal static NotificationHandler<TaskerTaskDetail> OnTaskerTask = (type, detail) => Assert.IsNotNull(detail);
    internal static NotificationHandler<NodeNextListDetail> OnNodeNextList = (type, detail) => Assert.IsNotNull(detail);
    internal static NotificationHandler<NodeRecognitionDetail> OnNodeRecognition = (type, detail) => Assert.IsNotNull(detail);
    internal static NotificationHandler<NodeActionDetail> OnNodeAction = (type, detail) => Assert.IsNotNull(detail);
}
