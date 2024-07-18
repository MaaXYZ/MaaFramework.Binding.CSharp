namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaToolkit"/> and <see cref="MaaToolkit"/>.
/// </summary>
[TestClass]
public class Test_IMaaToolkit
{
    public static Dictionary<MaaTypes, object> NewData => new()
    {
#if MAA_NATIVE
        { MaaTypes.Native, new MaaToolkit() },
#endif
    };
    public static Dictionary<MaaTypes, object> Data { get; private set; } = default!;

    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        Data = NewData;
    }

    [ClassCleanup]
    public static void CleanUpClass()
    {
        // CleanUp
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(NewData))]
    [Obsolete("Init_Uninit")]
    public void Interface_Config(MaaTypes type, IMaaToolkit maaToolkit)
    {
        Assert.IsNotNull(maaToolkit);

        Assert.IsTrue(
            maaToolkit.Config.Init());
        Assert.IsTrue(
            maaToolkit.Config.Uninit());
        Assert.IsTrue(
            maaToolkit.Config.InitOption());
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), "", true)]
    [MaaData(MaaTypes.All, nameof(Data), "", false)]
    [MaaData(MaaTypes.All, nameof(Data), nameof(Common.AdbPath), true)]
    [MaaData(MaaTypes.All, nameof(Data), nameof(Common.AdbPath), false)]
    public void Interface_Device(MaaTypes type, IMaaToolkit maaToolkit, string arg, bool useAsync)
    {
        Assert.IsNotNull(maaToolkit);

        var devices = useAsync ? maaToolkit.Device.FindAsync(arg).GetAwaiter().GetResult() : maaToolkit.Device.Find(arg);
        if (devices.Length == 0)
            return;

        CollectionAssert.AllItemsAreUnique(devices);
        foreach (var device in devices)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.AdbPath));
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.AdbSerial));
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.AdbConfig));
            device.AdbTypes.Check();
        }

        using var maaController = type switch
        {
#if MAA_NATIVE
            MaaTypes.Native => devices[0].ToAdbController(
                adbPath: Common.AdbPath,
                types: AdbControllerTypes.InputPresetAdb | AdbControllerTypes.ScreencapEncode,
                link: LinkOption.None),
#endif
            _ => throw new NotImplementedException(),
        };
        maaController
            .LinkStart()
            .Wait()
            .ThrowIfNot(MaaJobStatus.Success, MaaJobStatusException.MaaControllerMessage, devices[0].AdbPath, devices[0].AdbSerial);
    }

#if MAA_WIN32 && !GITHUB_ACTIONS
    [TestMethod]
    [MaaData(MaaTypes.All, nameof(NewData))]
    public void Interface_Win32_Window(MaaTypes type, IMaaToolkit maaToolkit)
    {
        Assert.IsNotNull(maaToolkit);

        Test_WindowInfos(type,
            maaToolkit.Win32.Window.Find(string.Empty, string.Empty));
        Test_WindowInfos(type,
            maaToolkit.Win32.Window.Search(string.Empty, string.Empty));
        Test_WindowInfos(type,
            maaToolkit.Win32.Window.ListWindows());

        Test_WindowInfos(type, [
            maaToolkit.Win32.Window.Cursor]);
        Test_WindowInfos(type, [
            maaToolkit.Win32.Window.Desktop]);
        Test_WindowInfos(type, [
            maaToolkit.Win32.Window.Foreground]);
    }

    private static void Test_WindowInfos(MaaTypes type, WindowInfo[] windows)
    {
        CollectionAssert.AllItemsAreUnique(windows);
        foreach (var window in windows)
        {
            Assert.AreNotEqual(nint.Zero, window.Hwnd);
        }

        using var maaController = type switch
        {
            MaaTypes.Native => windows[0].ToWin32Controller(
                Win32ControllerTypes.TouchSendMessage | Win32ControllerTypes.KeySendMessage | Win32ControllerTypes.ScreencapGDI,
                link: LinkOption.None),
            _ => throw new NotImplementedException(),
        };
        maaController
            .LinkStart()
            .Wait()
            .ThrowIfNot(MaaJobStatus.Success, MaaJobStatusException.MaaControllerMessage, windows[0].Hwnd);
    }
#endif
}
