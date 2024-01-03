using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using System.Runtime.InteropServices;

namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaController"/> and <see cref="MaaController"/> and <see cref="MaaControllerGrpc"/>.
/// </summary>
[TestClass]
public class Test_IMaaController
{
    private static AdbControllerTypes s_inputPreset = AdbControllerTypes.InputPresetAdb;
    public static Dictionary<MaaTypes, object> NewData => new()
    {
        {
            MaaTypes.Native, new MaaAdbController(Common.AdbPath, Common.Address, s_inputPreset | AdbControllerTypes.ScreencapEncode, Common.AdbConfig, Common.AgentPath, LinkOption.None)
        },
        {
            MaaTypes.Grpc, new MaaAdbControllerGrpc(Common.GrpcChannel, Common.AdbPath, Common.Address, s_inputPreset | AdbControllerTypes.ScreencapEncode, Common.AdbConfig, Common.AgentPath, LinkOption.None)
        }
    };
    public static Dictionary<MaaTypes, object> Data { get; private set; } = default!;
    public static Dictionary<MaaTypes, object> MiniTouchData { get; private set; } = default!;
    public static Dictionary<MaaTypes, object> MaaTouchData { get; private set; } = default!;
    public static Dictionary<MaaTypes, object> TestLinkData { get; private set; } = default!;

    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        InitializeData(AdbControllerTypes.InputPresetMaatouch);
        MaaTouchData = Data;
        InitializeData(Common.InGithubActions
            ? AdbControllerTypes.InputPresetMaatouch
            : AdbControllerTypes.InputPresetAdb);
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

        using var grpc1 = new MaaAdbControllerGrpc(
            Common.GrpcChannel,
            Common.AdbPath,
            Common.Address,
            AdbControllerTypes.InputPresetAdb | AdbControllerTypes.ScreencapEncode,
            Common.AdbConfig,
            Common.AgentPath);
        using var grpc2 = new MaaAdbControllerGrpc(
            Common.GrpcChannel,
            Common.AdbPath,
            Common.Address,
            AdbControllerTypes.InputPresetAdb | AdbControllerTypes.ScreencapEncode,
            Common.AdbConfig,
            Common.AgentPath,
            LinkOption.None);
        using var grpc3 = new MaaAdbControllerGrpc(
            Common.GrpcChannel,
            Common.AdbPath,
            Common.Address,
            AdbControllerTypes.InputPresetAdb | AdbControllerTypes.ScreencapEncode,
            Common.AdbConfig,
            Common.AgentPath,
            LinkOption.Start,
            CheckStatusOption.None);
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

        if (opt is ControllerOption.Recording && type is MaaTypes.Grpc)
        {
            Assert.ThrowsException<NotImplementedException>(() => maaController.SetOption(opt, arg));
            return;
        }

        Assert.IsTrue(
            maaController.SetOption(opt, arg));
    }

    public static void Interface_IMaaPost_Success(IMaaJob job)
    {
        Assert.IsNotNull(job);

        Assert.ThrowsException<InvalidOperationException>(() =>
            job.SetParam("{}"));
        Assert.AreEqual(
            MaaJobStatus.Success, job.Wait());
        Assert.AreEqual(
            MaaJobStatus.Success, job.Status);
    }

    public static void Interface_IMaaPost_Failed(IMaaJob job)
    {
        Assert.IsNotNull(job);

        Assert.ThrowsException<InvalidOperationException>(() =>
            job.SetParam("{}"));
        Assert.AreEqual(
            MaaJobStatus.Failed, job.Wait());
        Assert.AreEqual(
            MaaJobStatus.Failed, job.Status);
    }

    public static void Interface_IMaaPost(bool assertSuccess, IMaaJob job)
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
    [MaaData(MaaTypes.All, nameof(Data), false, 0, 10, 10, 100, 100, "Adb")]
    [MaaData(MaaTypes.All, nameof(MiniTouchData), true, 0, 10, 10, 100, 100, "MiniTouch")]
    [MaaData(MaaTypes.All, nameof(MaaTouchData), true, 0, 10, 10, 100, 100, "Maatouch")]
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
        using IMaaImageBuffer buffer = type switch
        {
            MaaTypes.Native => new MaaImageBuffer(),
            MaaTypes.Grpc => new MaaImageBufferGrpc(Common.GrpcChannel),
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

        if (type is MaaTypes.Native)
        {
            var nativeBuffer = buffer as MaaImageBuffer;
            var info = buffer.Info;
            var length = info.Width * info.Height * GetChannel(info.Type);
            var rawDataHandle = nativeBuffer!.GetRawData();
            var cv2MatData = new byte[length];
            Marshal.Copy(rawDataHandle, cv2MatData, 0, length);
            CollectionAssert.AreNotEqual(new byte[length], cv2MatData);
        }

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

        if (opt is ControllerOption.Recording && type is MaaTypes.Grpc)
        {
            Assert.ThrowsException<NotImplementedException>(() => maaController.SetOption(opt, arg));
            return;
        }

        Assert.ThrowsException<InvalidOperationException>(() => maaController.SetOption(opt, arg));
    }

    #endregion
}
