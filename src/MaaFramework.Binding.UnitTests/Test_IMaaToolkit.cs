namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaToolkit"/> and <see cref="MaaToolkit"/> and <see cref="MaaToolkitGrpc"/>.
/// </summary>
[TestClass]
public class Test_IMaaToolkit
{
    public static Dictionary<MaaTypes, object> NewData => new()
    {
#if MAA_NATIVE
        { MaaTypes.Native, new MaaToolkit() },
#endif
#if MAA_GRPC
        { MaaTypes.Grpc,   new MaaToolkitGrpc(Common.GrpcChannel) },
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
    public void Interface_Config_Init_Uninit(MaaTypes type, IMaaToolkit maaToolkit)
    {
        Assert.IsNotNull(maaToolkit);
        // In Maa.Framework.Runtimes v1.4.0.
        // Notes: maaToolkit.Config.Init() will change the log path to ./debug/maa.log.
        Assert.IsTrue(
            maaToolkit.Config.Init());
        Assert.IsTrue(
            maaToolkit.Config.Uninit());
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), "", true)]
    [MaaData(MaaTypes.All, nameof(Data), "", false)]
    [MaaData(MaaTypes.All, nameof(Data), nameof(Common.AdbPath), true)]
    [MaaData(MaaTypes.All, nameof(Data), nameof(Common.AdbPath), false)]
    public void Interface_Device_Find_FindAsync(MaaTypes type, IMaaToolkit maaToolkit, string arg, bool useAsync)
    {
        Assert.IsNotNull(maaToolkit);

        var devices = useAsync ? maaToolkit.Device.FindAsync(arg).GetAwaiter().GetResult() : maaToolkit.Device.Find(arg);
#if GITHUB_ACTIONS
        if (devices.Length == 0)
            return;
#endif
        Assert.AreNotEqual(0, devices.Length);

        CollectionAssert.AllItemsAreUnique(devices);
        foreach (var device in devices)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.AdbPath));
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.AdbSerial));
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.AdbConfig));
            device.AdbTypes.Check();
        }

        using IMaaController maaController = type switch
        {
            MaaTypes.Native => devices[0].ToAdbController(
                Path.GetFullPath("./MaaAgentBinary"),
                types: AdbControllerTypes.InputPresetAdb | AdbControllerTypes.ScreencapEncode,
                link: LinkOption.None),
            MaaTypes.Grpc => devices[0].ToAdbControllerGrpc(
                Common.GrpcChannel,
                Path.GetFullPath("./MaaAgentBinary"),
                types: AdbControllerTypes.InputPresetAdb | AdbControllerTypes.ScreencapEncode,
                link: LinkOption.None),
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
    public void Interface_Win32Window_Find(MaaTypes type, IMaaToolkit maaToolkit)
    {
        Assert.IsNotNull(maaToolkit);

        Test_WindowInfos(type,
            maaToolkit.Win32.Window.Find(string.Empty, string.Empty));
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(NewData))]
    public void Interface_Win32Window_Search(MaaTypes type, IMaaToolkit maaToolkit)
    {
        Assert.IsNotNull(maaToolkit);

        Test_WindowInfos(type,
            maaToolkit.Win32.Window.Search(string.Empty, string.Empty));
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

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(NewData))]
    public void Interface_Win32Window_Cursor(MaaTypes type, IMaaToolkit maaToolkit)
    {
        Assert.IsNotNull(maaToolkit);

        Test_WindowInfos(type,
            [maaToolkit.Win32.Window.Cursor]);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(NewData))]
    public void Interface_Win32Window_Desktop(MaaTypes type, IMaaToolkit maaToolkit)
    {
        Assert.IsNotNull(maaToolkit);

        Test_WindowInfos(type,
            [maaToolkit.Win32.Window.Desktop]);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(NewData))]
    public void Interface_Win32Window_Foreground(MaaTypes type, IMaaToolkit maaToolkit)
    {
        Assert.IsNotNull(maaToolkit);

        Test_WindowInfos(type,
            [maaToolkit.Win32.Window.Foreground]);
    }
#endif
}
