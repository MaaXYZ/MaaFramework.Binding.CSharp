using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using System.Runtime.InteropServices;
using MaaFramework.Binding.Notification;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaController"/> and <see cref="MaaController"/>.
/// </summary>
[TestClass]
// ReSharper disable InconsistentNaming
public class Test_IMaaController
{
    private static AdbInputMethods s_inputPreset = AdbInputMethods.AdbShell;
    public static Dictionary<MaaTypes, object> NewData => new()
    {
#if MAA_NATIVE
        {
            MaaTypes.Native, new MaaAdbController(Common.AdbPath, Common.Address, AdbScreencapMethods.Encode, s_inputPreset, Common.AdbConfig, Common.AgentPath, LinkOption.None)
        },
#pragma warning disable CA2000
        {
            MaaTypes.Custom, new MaaCustomController(new Custom.TestController(new MaaAdbController(Common.AdbPath, Common.Address, AdbScreencapMethods.Encode, s_inputPreset, Common.AdbConfig, Common.AgentPath, LinkOption.None)), LinkOption.None)
        },
#pragma warning restore CA2000
#endif
    };
    public static Dictionary<MaaTypes, object> Data { get; private set; } = default!;
    public static Dictionary<MaaTypes, object> MiniTouchData { get; private set; } = default!;
    public static Dictionary<MaaTypes, object> MaaTouchData { get; private set; } = default!;
    public static Dictionary<MaaTypes, object> TestLinkData { get; private set; } = default!;

    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        // The minimum Android API level for MaaTouch is 23.
        InitializeData(AdbInputMethods.Maatouch);
        MaaTouchData = Data;
#if !GITHUB_ACTIONS // MiniTouch crashes.
        InitializeData(AdbInputMethods.MinitouchAndAdbKey);
#endif
        MiniTouchData = Data;
        InitializeData(AdbInputMethods.AdbShell);
        TestLinkData = NewData;

        static void InitializeData(AdbInputMethods inputPreset)
        {
            s_inputPreset = inputPreset;
            Data = NewData;
            foreach (var data in Data.Values.Cast<IMaaController>())
            {
                Assert.IsFalse(data.IsInvalid);
                // 3 ways to subscribe to the Callback event
                data.Callback += Common.OnCallback;
                data.Callback += Common.NotificationHandlerRegistry.OnCallback;
                data.Callback += Common.OnControllerAction.ToCallback();

                _ = data
                    .LinkStart()
                    .Wait()
                    .ThrowIfNot(MaaJobStatus.Succeeded);
                Assert.IsTrue(
                    data.SetOption_ScreenshotTargetShortSide(720));
            }
        }
    }

    [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
    public static void CleanUpClass()
    {
        Common.DisposeData(Data.Values.Cast<IMaaDisposable>());
        Common.DisposeData(MiniTouchData.Values.Cast<IMaaDisposable>());
        Common.DisposeData(MaaTouchData.Values.Cast<IMaaDisposable>());
        Common.DisposeData(TestLinkData.Values.Cast<IMaaDisposable>());
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
            AdbScreencapMethods.Encode,
            AdbInputMethods.AdbShell,
            Common.AdbConfig,
            Common.AgentPath);
        using var native2 = new MaaAdbController(
            Common.AdbPath,
            Common.Address,
            AdbScreencapMethods.Encode,
            AdbInputMethods.AdbShell,
            Common.AdbConfig,
            Common.AgentPath,
            LinkOption.None);
        using var native3 = new MaaAdbController(
            Common.AdbPath,
            Common.Address,
            AdbScreencapMethods.Encode,
            AdbInputMethods.AdbShell,
            Common.AdbConfig,
            Common.AgentPath,
            LinkOption.Start,
            CheckStatusOption.None);
        #endregion

#if !GITHUB_ACTIONS
        #region MaaWin32Controller
        var windowInfo = MaaToolkit.Shared.Desktop.Window.Find().First(static x
            => x.Name.Contains("Visual Studio", StringComparison.OrdinalIgnoreCase)
               || x.Name.Contains("Maa", StringComparison.OrdinalIgnoreCase));

        using var win32Native1 = new MaaWin32Controller(
            windowInfo.Handle,
            Win32ScreencapMethod.GDI,
            Win32InputMethod.SendMessage);
        using var win32Native2 = new MaaWin32Controller(
            windowInfo.Handle,
            Win32ScreencapMethod.GDI,
            Win32InputMethod.SendMessage,
            LinkOption.None);
        using var win32Native3 = new MaaWin32Controller(
            windowInfo.Handle,
            Win32ScreencapMethod.GDI,
            Win32InputMethod.SendMessage,
            LinkOption.Start,
            CheckStatusOption.None);
        #endregion
#endif
#endif
    }
#pragma warning restore S2699 // Tests should include assertions

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.ScreenshotTargetLongSide, 1280)]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.ScreenshotTargetShortSide, 720)]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.ScreenshotUseRawSize, false)]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.Recording, false)]
    public void Interface_SetOption(MaaTypes type, IMaaController maaController, ControllerOption opt, object arg)
    {
        Assert.IsNotNull(maaController);

        Assert.IsTrue(
            maaController.SetOption(opt, arg));
    }

    private static void Interface_IMaaPost_Success(MaaJob job)
    {
        Assert.AreNotEqual(
            MaaJobStatus.Invalid, job.Status);
        Assert.AreEqual(
            MaaJobStatus.Succeeded, job.Wait());
    }

    private static void Interface_IMaaPost_Failed(MaaJob job)
    {
        Assert.AreNotEqual(
            MaaJobStatus.Invalid, job.Status);
        Assert.AreEqual(
            MaaJobStatus.Failed, job.Wait());
    }

    private static void Interface_IMaaPost(bool assertSuccess, MaaJob job)
    {
        if (assertSuccess)
            Interface_IMaaPost_Success(job);
        else
            Interface_IMaaPost_Failed(job);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(TestLinkData))]
    public void Interface_LinkStart_IsConnected(MaaTypes type, IMaaController maaController)
    {
        Assert.IsNotNull(maaController);

        Assert.IsFalse(maaController.IsConnected);
        var job = maaController.LinkStart();
        Interface_IMaaPost_Success(job);
        Assert.IsTrue(maaController.IsConnected);
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
#if GITHUB_ACTIONS // true if Android API level is less than 26
    [MaaData(MaaTypes.All, nameof(Data), true, "com.android.settings")]
#else
    [MaaData(MaaTypes.All, nameof(Data), false, "com.android.settings")]
#endif
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

    private static MaaImageBuffer GetImage(MaaTypes type, IMaaController maaController)
    {
        var job = maaController.Screencap();
        Interface_IMaaPost_Success(job);
        var buffer = type switch
        {
            MaaTypes.Native => new MaaImageBuffer(),
            MaaTypes.Custom => new MaaImageBuffer(),
            _ => throw new NotImplementedException(),
        };
        Assert.IsTrue(
            maaController.GetCachedImage(buffer));
        Assert.IsFalse(
            buffer.IsEmpty);

        return buffer;
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Screencap_GetImage_Array(MaaTypes type, IMaaController maaController)
    {
        Assert.IsNotNull(maaController);
        using var buffer = GetImage(type, maaController);

        Assert.IsTrue(
            buffer.TryGetEncodedData(out byte[]? pngImageData));
        CollectionAssert.AreNotEqual(new byte[pngImageData.Length], pngImageData);

        // if (type is MaaTypes.Native) { }
        var length = buffer.Width * buffer.Height * buffer.Channels;
        Assert.IsTrue(
            buffer.TryGetRawData(out var rawDataHandle));
        var cv2MatData = new byte[length];
        Marshal.Copy(rawDataHandle, cv2MatData, 0, length);
        CollectionAssert.AreNotEqual(new byte[length], cv2MatData);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Screencap_GetImage_Stream(MaaTypes type, IMaaController maaController)
    {
        Assert.IsNotNull(maaController);
        using var buffer = GetImage(type, maaController);
        Assert.IsTrue(
            buffer.TryGetEncodedData(out Stream? data));
        using var image = Image.Load(data);
        Assert.AreEqual(
            image.Height, buffer.Height);
        Assert.AreEqual(
            image.Width, buffer.Width);

        using var stream = new MemoryStream();
        image.Mutate(static c => c.Resize(30, 30));
        image.Save(stream, image.Metadata.DecodedImageFormat!);
        stream.Position = 0;
        Assert.IsTrue(
            buffer.TrySetEncodedData(stream));

        Assert.IsTrue(
            buffer.TryGetEncodedData(out data));
        using var resizeImage = Image.Load(data);
        Assert.AreEqual(
            30, image.Height);
        Assert.AreEqual(
            30, image.Width);
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
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.ScreenshotUseRawSize, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), ControllerOption.Recording, 0.0)]
    public void Interface_SetOption_InvalidData(MaaTypes type, IMaaController maaController, ControllerOption opt, object arg)
    {
        Assert.IsNotNull(maaController);

        _ = Assert.ThrowsExactly<NotSupportedException>(()
            => maaController.SetOption(opt, arg));
    }

    [TestMethod]
    public void CreateInvalidInstances()
    {

#if MAA_NATIVE
        #region MaaAdbController
        Assert.ThrowsExactly<ArgumentException>(static () => new MaaAdbController("test", "test", AdbScreencapMethods.None, AdbInputMethods.All, "{}", "test"));
        Assert.ThrowsExactly<ArgumentException>(static () => new MaaAdbController("test", "test", AdbScreencapMethods.All, AdbInputMethods.None, "{}", "test"));
        #endregion

#if !GITHUB_ACTIONS
        #region MaaWin32Controller
        Assert.ThrowsExactly<ArgumentException>(static () => new MaaWin32Controller(1, Win32ScreencapMethod.None, Win32InputMethod.Seize));
        Assert.ThrowsExactly<ArgumentException>(static () => new MaaWin32Controller(1, Win32ScreencapMethod.GDI, Win32InputMethod.None));
        #endregion
#endif
#endif
    }

    #endregion
}
