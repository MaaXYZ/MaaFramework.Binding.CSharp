using Grpc.Net.Client;
using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     A static class of global info.
/// </summary>
[TestClass]
public static class Common
{
    static Common()
    {
        /*
        MaaRpc.Start(GrpcAddress);
        */
    }

    internal static bool InGithubActions;

    internal static string AdbPath { get; set; } = string.Empty;
    internal static string Address { get; set; } = string.Empty;

    internal static string DebugPath { get; set; } = Path.GetFullPath("./debug");
    internal static string ResourcePath { get; set; } = Path.GetFullPath("./SampleResource");
    internal static string AgentPath { get; set; } = Path.GetFullPath($"./MaaAgentBinary");
    internal static string AdbConfig { get; set; } = File.ReadAllText(Path.GetFullPath($"{ResourcePath}/controller_config.json"));

    internal static GrpcChannel GrpcChannel { get; set; } = GrpcChannel.ForAddress($"http://{GrpcAddress}/");
    private const string GrpcAddress = "127.0.0.1:8080";

    private static void InitializeInfo(TestContext testContext)
    {
        InGithubActions = bool.Parse(Environment.GetEnvironmentVariable("GITHUB_ACTIONS") ?? "false");
        var devices = new MaaToolkit().Device.Find();

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
        InitializeInfo(testContext);

        new MaaUtility().SetOption(GlobalOption.LogDir, DebugPath);
        new MaaUtility().SetOption(GlobalOption.StdoutLevel, LoggingLevel.Off);
        Task.Run(() =>
        {
            MaaRpc.Wait();
            Assert.Fail();
        });
    }

    /// <summary>
    ///     Assembly cleanup.
    /// </summary>
    [AssemblyCleanup]
    public static void CleanupAssembly()
    {
        /*
        GrpcChannel.Dispose();
        MaaRpc.Stop();
        */
    }

    internal static void DisposeData(IEnumerable<IMaaDisposable> data)
    {
        foreach (var d in data)
        {
            d.Dispose();
            Assert.IsTrue(d.IsInvalid);
        }
    }

    internal static void Callback(object? sender, MaaCallbackEventArgs e)
    {
        Assert.IsNotNull(sender);
        Assert.IsNotNull(e);
        Assert.IsFalse(string.IsNullOrWhiteSpace(e.Message));
    }
}
