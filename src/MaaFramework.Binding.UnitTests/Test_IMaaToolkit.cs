namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaToolkit"/> and <see cref="MaaToolkit"/>.
/// </summary>
[TestClass]
// ReSharper disable InconsistentNaming
public class Test_IMaaToolkit
{
    public static Dictionary<MaaTypes, object> NewData => new()
    {
#if MAA_NATIVE
        { MaaTypes.Native, MaaToolkit.Shared },
#endif
    };
    public static Dictionary<MaaTypes, object> Data { get; private set; } = default!;

    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        Data = NewData;
    }

    [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
    public static void CleanUpClass()
    {
        // CleanUp
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Config(MaaTypes type, IMaaToolkit maaToolkit)
    {
        Assert.IsNotNull(maaToolkit);

        Assert.IsTrue(
            maaToolkit.Config.InitOption());
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), "", true)]
    [MaaData(MaaTypes.All, nameof(Data), "", false)]
    [MaaData(MaaTypes.All, nameof(Data), nameof(Common.AdbPath), true)]
    [MaaData(MaaTypes.All, nameof(Data), nameof(Common.AdbPath), false)]
    public void Interface_AdbDevice(MaaTypes type, IMaaToolkit maaToolkit, string arg, bool useAsync)
    {
        Assert.IsNotNull(maaToolkit);

        var devices = useAsync ? maaToolkit.AdbDevice.FindAsync(arg).GetAwaiter().GetResult() : maaToolkit.AdbDevice.Find(arg);
        if (devices.MaaSizeCount == 0)
            return;

        foreach (var device in devices)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.AdbPath));
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.AdbSerial));
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.Config));
        }

        using var maaController = type switch
        {
            MaaTypes.Native => devices[0].ToAdbController(
                adbPath: Common.AdbPath,
                screencapMethods: AdbScreencapMethods.Encode,
                inputMethods: AdbInputMethods.AdbShell,
                link: LinkOption.None),

            _ => throw new NotImplementedException(),
        };
        _ = maaController
            .LinkStart()
            .Wait()
            .ThrowIfNot(MaaJobStatus.Succeeded, MaaJobStatusException.MaaControllerMessage, devices[0].AdbPath, devices[0].AdbSerial);

#if MAA_WIN32
        using var optionalArgumentDefaultValuesTest = type switch
        {
            MaaTypes.Native => devices[0].ToAdbController(),
            _ => throw new NotImplementedException(),
        };
#endif
    }

#if MAA_WIN32 && !GITHUB_ACTIONS
    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Desktop_Window(MaaTypes type, IMaaToolkit maaToolkit)
    {
        Assert.IsNotNull(maaToolkit);

        Test_Win32_WindowInfos(type,
            maaToolkit.Desktop.Window.Find());
    }

    private static void Test_Win32_WindowInfos(MaaTypes type, IList<DesktopWindowInfo> windows)
    {
        foreach (var window in windows)
        {
            Assert.AreNotEqual(nint.Zero, window.Handle);
        }

        using var maaController = type switch
        {
            MaaTypes.Native => windows[0].ToWin32Controller(
                screencapMethod: Win32ScreencapMethod.GDI,
                mouseMethod: Win32InputMethod.SendMessage,
                keyboardMethod: Win32InputMethod.SendMessage,
                link: LinkOption.None),
            _ => throw new NotImplementedException(),
        };
        _ = maaController
            .LinkStart()
            .Wait()
            .ThrowIfNot(MaaJobStatus.Succeeded, MaaJobStatusException.MaaControllerMessage, windows[0].Handle);

#if MAA_WIN32
        using var optionalArgumentDefaultValuesTest = type switch
        {
            MaaTypes.Native => windows[0].ToWin32Controller(
                screencapMethod: Win32ScreencapMethod.GDI,
                mouseMethod: Win32InputMethod.SendMessage,
                keyboardMethod: Win32InputMethod.SendMessage),
            _ => throw new NotImplementedException(),
        };
#endif
    }
#endif

    #region Invalid data tests

    #endregion
}
