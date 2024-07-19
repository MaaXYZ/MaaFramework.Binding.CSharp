using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using System.Runtime.InteropServices;

namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaController"/> and <see cref="MaaController"/>.
/// </summary>
[TestClass]
public class Test_IMaaController
{
    private static AdbControllerTypes s_inputPreset = AdbControllerTypes.InputPresetAdb;
    public static Dictionary<MaaTypes, object> NewData => new()
    {
#if MAA_NATIVE
        {
            MaaTypes.Native, new MaaAdbController(Common.AdbPath, Common.Address, s_inputPreset | AdbControllerTypes.ScreencapEncode, Common.AdbConfig, Common.AgentPath, LinkOption.None)
        },
#endif
    };
    public static Dictionary<MaaTypes, object> Data { get; private set; } = default!;
    public static Dictionary<MaaTypes, object> MiniTouchData { get; private set; } = default!;
    public static Dictionary<MaaTypes, object> MaaTouchData { get; private set; } = default!;
    public static Dictionary<MaaTypes, object> TestLinkData { get; private set; } = default!;

    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        InitializeData(AdbControllerTypes.InputPresetMaaTouch);
        MaaTouchData = Data;
#if !GITHUB_ACTIONS
        InitializeData(AdbControllerTypes.InputPresetMiniTouch);
#endif
        MiniTouchData = Data;
        InitializeData(AdbControllerTypes.InputPresetAdb);
        TestLinkData = NewData;

        static void InitializeData(AdbControllerTypes inputPreset)
        {
            s_inputPreset = inputPreset;
            Data = NewData;
            foreach (var data in Data.Values.Cast<IMaaController>())
            {
                Assert.IsFalse(data.IsInvalid);
                data.Callback += Common.Callback;
                data.LinkStart()
                    .Wait()
                    .ThrowIfNot(MaaJobStatus.Success);
                Assert.IsTrue(
                    data.SetOption(ControllerOption.ScreenshotTargetShortSide, 720));
            }
        }
    }

    [ClassCleanup]
    public static void CleanUpClass()
    {
        Common.DisposeData(Data.Values.Cast<IMaaDisposable>());
        Common.DisposeData(MiniTouchData.Values.Cast<IMaaDisposable>());
        Common.DisposeData(MaaTouchData.Values.Cast<IMaaDisposable>());
    }

#pragma warning disable S2699 // Tests should include assertions
    [TestMethod]
    public void CreateInstances()
    {
#if MAA_NATIVE
        #region MaaAdbController

        using var native1 = new MaaAdbController(
            Common.AdbPath,
            Common.Address,
            AdbControllerTypes.InputPresetAdb | AdbControllerTypes.ScreencapEncode,
            Common.AdbConfig,
            Common.AgentPath);
        using var native2 = new MaaAdbController(
            Common.AdbPath,
            Common.Address,
            AdbControllerTypes.InputPresetAdb | AdbControllerTypes.ScreencapEncode,
            Common.AdbConfig,
            Common.AgentPath,
            LinkOption.None);
        using var native3 = new MaaAdbController(
            Common.AdbPath,
            Common.Address,
            AdbControllerTypes.InputPresetAdb | AdbControllerTypes.ScreencapEncode,
            Common.AdbConfig,
            Common.AgentPath,
            LinkOption.Start,
            CheckStatusOption.None);
        #endregion

        #region MaaWin32Controller
#if !GITHUB_ACTIONS
        var toolkit = new MaaToolkit();
        var windowInfo = toolkit.Win32.Window.Search(string.Empty, "Visual Studio").FirstOrDefault() ?? new() { Handle = nint.Zero, Name = string.Empty, ClassName = string.Empty };

        using var win32Native1 = new MaaWin32Controller(
            windowInfo.Handle,
            Win32ControllerTypes.TouchSendMessage | Win32ControllerTypes.KeySendMessage | Win32ControllerTypes.ScreencapGDI);
        using var win32Native2 = new MaaWin32Controller(
            windowInfo.Handle,
            Win32ControllerTypes.TouchSendMessage | Win32ControllerTypes.KeySendMessage | Win32ControllerTypes.ScreencapGDI,
            LinkOption.None);
        using var win32Native3 = new MaaWin32Controller(
            windowInfo.Handle,
            Win32ControllerTypes.TouchSendMessage | Win32ControllerTypes.KeySendMessage | Win32ControllerTypes.ScreencapGDI,
            LinkOption.Start,
            CheckStatusOption.None);
#endif
        #endregion
#endif
    }
#pragma warning restore S2699 // Tests should include assertions

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.ScreenshotTargetLongSide, 1280)]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.ScreenshotTargetShortSide, 720)]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.DefaultAppPackageEntry, "DefaultAppPackageEntry")]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.DefaultAppPackage, "DefaultAppPackage")]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.Recording, false)]
    public void Interface_SetOption(MaaTypes type, IMaaController maaController, ControllerOption opt, object arg)
    {
        Assert.IsNotNull(maaController);

        Assert.IsTrue(
            maaController.SetOption(opt, arg));
    }

    public static void Interface_IMaaPost_Success(MaaJob job)
    {
        Assert.IsNotNull(job);

        Assert.AreEqual(
            MaaJobStatus.Success, job.Wait());
        Assert.AreEqual(
            MaaJobStatus.Success, job.Status);
    }

    public static void Interface_IMaaPost_Failed(MaaJob job)
    {
        Assert.IsNotNull(job);

        Assert.AreEqual(
            MaaJobStatus.Failed, job.Wait());
        Assert.AreEqual(
            MaaJobStatus.Failed, job.Status);
    }

    public static void Interface_IMaaPost(bool assertSuccess, MaaJob job)
    {
        if (assertSuccess)
            Interface_IMaaPost_Success(job);
        else
            Interface_IMaaPost_Failed(job);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(TestLinkData))]
    public void Interface_LinkStart_LinkStop(MaaTypes type, IMaaController maaController)
    {
        Assert.IsNotNull(maaController);

        Assert.IsFalse(maaController.LinkStop());
        var job = maaController.LinkStart();
        Interface_IMaaPost_Success(job);
        Assert.IsTrue(maaController.LinkStop());
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), true, 0, 0)]
    public void Interface_Click(MaaTypes type, IMaaController maaController, bool assertSuccess, int x, int y)
    {
        Assert.IsNotNull(maaController);

        var job = maaController.Click(x, y);
        Interface_IMaaPost(assertSuccess, job);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), true, 10, 10, 100, 100, 500)]
    public void Interface_Swipe(MaaTypes type, IMaaController maaController, bool assertSuccess, int x1, int y1, int x2, int y2, int duration)
    {
        Assert.IsNotNull(maaController);

        var job = maaController.Swipe(x1, y1, x2, y2, duration);
        Interface_IMaaPost(assertSuccess, job);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), true, 4)]  // KEYCODE_BACK
    public void Interface_PressKey(MaaTypes type, IMaaController maaController, bool assertSuccess, int keyCode)
    {
        Assert.IsNotNull(maaController);

        var job = maaController.PressKey(keyCode);
        Interface_IMaaPost(assertSuccess, job);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), true, "")]
    [MaaData(MaaTypes.All, nameof(Data), true, "Anything")]
    public void Interface_InputText(MaaTypes type, IMaaController maaController, bool assertSuccess, string text)
    {
        Assert.IsNotNull(maaController);

        var job = maaController.InputText(text);
        Interface_IMaaPost(assertSuccess, job);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), false, "com.android.settings")]
    [MaaData(MaaTypes.All, nameof(Data), true, "com.android.settings/.Settings")]
    public void Interface_StartApp_StopApp(MaaTypes type, IMaaController maaController, bool assertSuccess, string intent)
    {
        Assert.IsNotNull(maaController);
        Assert.IsNotNull(intent);

        var job = maaController.StartApp(intent);
        Interface_IMaaPost(assertSuccess, job);

        job = maaController.StopApp(intent.Split('/')[0]);
        Interface_IMaaPost(true, job); // "adb shell am force-stop" always returns True
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), false, 0, 10, 10, 100, 100, "Adb")]
    [MaaData(MaaTypes.All, nameof(MiniTouchData), true, 0, 10, 10, 100, 100, "MiniTouch")]
    [MaaData(MaaTypes.All, nameof(MaaTouchData), true, 0, 10, 10, 100, 100, "MaaTouch")]
    public void Interface_TouchDown_TouchMove_TouchUp(MaaTypes type, IMaaController maaController, bool assertSuccess, int contact, int x1, int y1, int x2, int y2, string adbControllerTypes)
    {
        Assert.IsNotNull(maaController);

        var job = maaController.TouchDown(contact, x1, y1, 0);
        Interface_IMaaPost(assertSuccess, job);
        Task.Delay(100).Wait();

        job = maaController.TouchMove(contact, x2, y2, 0);
        Interface_IMaaPost(assertSuccess, job);
        Task.Delay(100).Wait();

        job = maaController.TouchUp(contact);
        Interface_IMaaPost(assertSuccess, job);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Screencap_GetImage(MaaTypes type, IMaaController maaController)
    {
        Assert.IsNotNull(maaController);

        var job = maaController.Screencap();
        Interface_IMaaPost_Success(job);
        using var buffer = type switch
        {
#if MAA_NATIVE
            MaaTypes.Native => new MaaImageBuffer(),
#endif
            _ => throw new NotImplementedException(),
        };
        Assert.IsTrue(
            maaController.GetImage(buffer));
        Assert.IsFalse(
            buffer.IsEmpty);

        var encodedDataHandle = buffer.GetEncodedData(out var size);
        var pngImageData = new byte[size];
        Marshal.Copy(encodedDataHandle, pngImageData, 0, (int)size);
        CollectionAssert.AreNotEqual(new byte[size], pngImageData);

        // if (type is MaaTypes.Native) { }
        var nativeBuffer = buffer;
        var info = buffer.Info;
        var length = info.Width * info.Height * GetChannel(info.Type);
        var rawDataHandle = nativeBuffer!.GetRawData();
        var cv2MatData = new byte[length];
        Marshal.Copy(rawDataHandle, cv2MatData, 0, length);
        CollectionAssert.AreNotEqual(new byte[length], cv2MatData);

        static int GetChannel(int type)
        {
            const int CV2_8UC3 = 16;

            return type switch
            {
                CV2_8UC3 => 3,
                _ => throw new NotImplementedException(),
            };
        }
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Uuid(MaaTypes type, IMaaController maaController)
    {
        Assert.IsNotNull(maaController);

        Assert.IsFalse(string.IsNullOrWhiteSpace(
            maaController.Uuid));
    }

    #region Invalid data tests

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.Invalid, "Anything")]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.ScreenshotTargetLongSide, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.ScreenshotTargetShortSide, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.DefaultAppPackageEntry, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.DefaultAppPackage, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.Recording, 0.0)]
    public void Interface_SetOption_InvalidData(MaaTypes type, IMaaController maaController, ControllerOption opt, object arg)
    {
        Assert.IsNotNull(maaController);

        Assert.ThrowsException<InvalidOperationException>(() => maaController.SetOption(opt, arg));
    }

    #endregion
}
