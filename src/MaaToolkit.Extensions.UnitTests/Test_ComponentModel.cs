using MaaToolKit.Extensions;
using MaaToolKit.Extensions.Enums;
using MaaToolKit.Extensions.Exceptions;
using MaaToolKit.Extensions.Interop;

namespace MaaToolkit.Extensions.UnitTests;

/// <summary>
///     Test <see cref="MaaToolKit.Extensions"/>.
/// </summary>
[TestClass]
public class Test_ComponentModel
{
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    private static MaaResource Resource { get; set; }
    private static MaaController Controller { get; set; }
    private static MaaInstance Instance { get; set; }

    private static MaaJob ResourceJob { get; set; }
    private static MaaJob ControllerJob { get; set; }
    private static MaaJob InstanceJob { get; set; }
    private static MaaJob ScreencapJob { get; set; }
    private static MaaImageBuffer Image { get; set; }

    private static MaaActionApi.Run Run { get; set; } = (
        /* MaaSyncContextHandle */  nint syncContext,
        /* MaaStringView */         nint taskName,
        /* MaaStringView */         nint customActionParam,
                                    ref MaaRectApi currentBox,
        /* MaaStringView */         nint curRecDetail) =>
    {
        currentBox.X = 0;
        currentBox.Y = 0;
        currentBox.Width = 1920;
        currentBox.Width = 1080;
        return 1;
    };

    private static MaaActionApi.Stop Stop { get; set; } =
        () => { };

    private static MaaRecognizerApi.Analyze Analyze { get; set; } = (
        /* MaaSyncContextHandle */  nint syncContext,
        /* MaaImageBufferHandle */  nint image,
        /* MaaStringView */         nint taskName,
        /* MaaStringView */         nint customRecognitionParam,
                                    ref MaaRectApi outBox,
        /* MaaStringBufferHandle */ nint detailBuff) =>
    {
        outBox.X = 0;
        outBox.Y = 0;
        outBox.Width = 1920;
        outBox.Width = 1080;
        return 1;
    };
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

    private static void OnCallback(string msg, string detailsJson, nint identifier)
    {
        Console.WriteLine($"msg: {msg}\ndetailsJson: {detailsJson}\nidentifier: {identifier}");
    }

    /// <summary>
    ///     Initializes the <see cref="Test_ComponentModel"/>.
    /// </summary>
    /// <param name="context">The <see cref="TestContext"/></param>
    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        MaaObject_Property_GetSet_FrameworkLogDir();
        MaaObject_Property_GetSet_ToolkitLogDir();
        MaaObject_Property_GetSet_DebugMode();
        MaaResource_Method_Constructor();
        MaaController_Method_Constructor();
        MaaInstance_Method_Constructor();
        MaaImage_Method_Constructor();

        MaaResource_Method_Append();
        MaaController_Method_LinkStart();
        MaaInstance_Method_AppendTask();
        MaaController_Method_Screencap();

        ResourceJob.Wait().ThrowIfNot(MaaJobStatus.Success);
        ControllerJob.Wait().ThrowIfNot(MaaJobStatus.Success);
        InstanceJob.Wait();
        ScreencapJob.Wait().ThrowIfNot(MaaJobStatus.Success);
    }

    /// <summary>
    ///     Cleanup the <see cref="Test_ComponentModel"/>.
    /// </summary>
    [ClassCleanup]
    public static void CleanUpClass()
    {
        MaaController_Method_LinkStop();
        MaaResource_Method_Dispose();
        MaaController_Method_Dispose();
        MaaInstance_Method_Dispose();
        MaaImage_Method_Dispose();
    }

    #region static MaaObject

    /// <summary> Tests the static member of the <see cref="MaaObject"/>. </summary>
    public static void MaaObject_Property_GetSet_FrameworkLogDir()
    {
        MaaObject.FrameworkLogDir = GlobalInfo.DebugPath;
        Assert.AreNotEqual(string.Empty, MaaObject.FrameworkLogDir);
    }

    /// <summary> Tests the static member of the <see cref="MaaObject"/>. </summary>
    public static void MaaObject_Property_GetSet_ToolkitLogDir()
    {
        MaaObject.ToolkitLogDir = GlobalInfo.DebugPath;
        Assert.AreNotEqual(string.Empty, MaaObject.ToolkitLogDir);
    }

    /// <summary> Tests the static member of the <see cref="MaaObject"/>. </summary>
    public static void MaaObject_Property_GetSet_DebugMode()
    {
        MaaObject.DebugMode = true;
        Assert.IsTrue(MaaObject.DebugMode);
    }

    /// <summary> Tests the static member of the <see cref="MaaObject"/>. </summary>
    [TestMethod]
    public void Z_MaaObject_Property_Get_FrameworkVersion()
        => Assert.IsNotNull(
            MaaObject.FrameworkVersion);

    /// <summary> Tests the static member of the <see cref="MaaObject"/>. </summary>
    [TestMethod]
    public void Z_MaaObject_Property_Get_ToolkitVersion()
        => Assert.IsNotNull(
            MaaObject.ToolkitVersion);

    #endregion

    #region MaaResource

    /// <summary> Tests the constructor of the <see cref="MaaResource"/>. </summary>
    public static void MaaResource_Method_Constructor()
    {
        Resource = new MaaResource();
        Resource.Callback += OnCallback;
    }

    /// <summary> Test a member of the <see cref="MaaResource"/>. </summary>
    public static void MaaResource_Method_Dispose()
        => Resource.Dispose();

    /// <summary> Test a member of the <see cref="MaaResource"/>. </summary>
    public static void MaaResource_Method_Append()
    {
        ResourceJob = Resource.AppendPath(GlobalInfo.ResourcePath);
        Assert.IsNotNull(ResourceJob);
    }

    /// <summary> Test a member of the <see cref="MaaResource"/>. </summary>
    [TestMethod]
    public void D_MaaResource_MaaJob_Method_SetParam()
        => Assert.IsFalse(
            ResourceJob.SetParam(string.Empty));

    /// <summary> Test a member of the <see cref="MaaResource"/>. </summary>
    [TestMethod]
    [ExpectedException(typeof(MaaJobStatusException))]
    public void D_MaaResource_MaaJob_Method_Wait_MaaJobStatus_Method_ThrowIf()
        => ResourceJob.Wait().ThrowIf(MaaJobStatus.Success);

    /// <summary> Test a member of the <see cref="MaaResource"/>. </summary>
    [TestMethod]
    [ExpectedException(typeof(MaaJobStatusException))]
    public void D_MaaResource_MaaJob_Property_Get_Status_MaaJobStatus_Method_ThrowIfNot()
        => ResourceJob.Status.ThrowIfNot(MaaJobStatus.Failed);

    /// <summary> Test a member of the <see cref="MaaResource"/>. </summary>
    [TestMethod]
    public void C_MaaResource_Property_Get_Loaded()
        => Assert.IsTrue(
            Resource.Loaded);
    /*
    /// <summary> Test a member of the <see cref="MaaResource"/>. </summary>
    [TestMethod]
    public void C_MaaResource_Method_SetOption()
        => Assert.IsFalse(
            Resource.SetOption(ResourceOption.Invalid, "test"));
    */

    /// <summary> Test a member of the <see cref="MaaResource"/>. </summary>
    // [TestMethod]
    public void C_MaaResource_Property_Get_Hash()
        => Assert.IsNotNull(
            Resource.Hash);

    #endregion

    #region MaaController

    private static readonly string s_controllerConfig = Path.GetFullPath($"{GlobalInfo.ResourcePath}/controller_config.json");
    private static readonly string s_adbConfig = File.ReadAllText(s_controllerConfig);
    private static readonly AdbControllerType s_input = AdbControllerType.InputPresetAdb;
    private static readonly AdbControllerType s_screenCap = AdbControllerType.ScreenCapEncode;

    /// <summary> Tests the constructor of the <see cref="MaaController"/>. </summary>
    public static void MaaController_Method_Constructor()
    {
        Controller = new MaaController(GlobalInfo.AdbPath, GlobalInfo.Address, s_input | s_screenCap, s_adbConfig);
        Controller.Callback += OnCallback;
    }

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    public static void MaaController_Method_Dispose()
        => Controller.Dispose();

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void H_MaaController_Method_SetOption()
    {
        static void action() => Controller.SetOption(ControllerOption.Invalid, string.Empty);
        Assert.ThrowsException<ArgumentException>(action);
    }

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void H_MaaController_Method_SetOption_ScreenshotTargetLongSide()
    {
        var ret = Controller.SetOption(ControllerOption.ScreenshotTargetLongSide, 1920);
        Assert.IsTrue(ret);
    }

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void H_MaaController_Method_SetOption_ScreenshotTargetShortSide()
    {
        var ret = Controller.SetOption(ControllerOption.ScreenshotTargetShortSide, 1080);
        Assert.IsTrue(ret);
    }

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void H_MaaController_Method_SetOption_DefaultAppPackage()
    {
        var ret = Controller.SetOption(ControllerOption.DefaultAppPackage, "114514");
        Assert.IsTrue(ret);
    }

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void H_MaaController_Method_SetOption_DefaultAppPackageEntry()
    {
        var ret = Controller.SetOption(ControllerOption.DefaultAppPackageEntry, "114514");
        Assert.IsTrue(ret);
    }

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    public static void MaaController_Method_LinkStart()
    {
        ControllerJob = Controller.LinkStart();
        Assert.IsNotNull(ControllerJob);
    }

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void H_MaaController_Method_Click()
        => Assert.IsNotNull(
            Controller.Click(0, 0));

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void H_MaaController_Method_Swipe()
        => Assert.IsNotNull(
            Controller.Swipe(0, 0, 1, 1, 1));

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    public static void MaaController_Method_Screencap()
    {
        ScreencapJob = Controller.Screencap();
        Assert.IsNotNull(
                ScreencapJob);
    }

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void I_MaaController_MaaJob_Method_SetParam()
        => Assert.IsFalse(
            ControllerJob.SetParam(string.Empty));

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    [ExpectedException(typeof(MaaJobStatusException))]
    public void I_MaaController_MaaJob_Method_Wait_MaaJobStatus_Method_ThrowIf()
        => ControllerJob.Wait().ThrowIf(MaaJobStatus.Success);

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    [ExpectedException(typeof(MaaJobStatusException))]
    public void I_MaaController_MaaJob_Property_Get_Status_MaaJobStatus_Method_ThrowIfNot()
        => ControllerJob.Status.ThrowIfNot(MaaJobStatus.Failed);

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    public static void MaaController_Method_LinkStop()
        => Assert.IsTrue(
            Controller.LinkStop());

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void I_MaaController_Method_GetImage()
    {
        Assert.IsTrue(
                Controller.GetImage(Image));
    }

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void H_MaaController_Property_Get_Uuid()
    {
        Controller.LinkStart().Wait().ThrowIfNot(MaaJobStatus.Success);
        Assert.IsNotNull(
                Controller.Uuid);
    }

    #endregion

    #region MaaInstance

    /// <summary> Tests the constructor of the <see cref="MaaInstance"/>. </summary>
    public static void MaaInstance_Method_Constructor()
    {
        Instance = new MaaInstance();
        Instance.Callback += OnCallback;
    }

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    public static void MaaInstance_Method_Dispose()
        => Instance.Dispose();

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void N_MaaInstance_Method_BindResource()
        => Assert.IsTrue(
            Instance.BindResource(Resource));

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void N_MaaInstance_Method_BindController()
        => Assert.IsTrue(
            Instance.BindController(Controller));

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void O_MaaInstance_Property_Get_Initialized()
    {
        var instance = new MaaInstance();
        Assert.IsTrue(
            instance.BindResource(Resource));
        Assert.IsTrue(
            instance.BindController(Controller));
        Assert.IsTrue(
            instance.Initialized);
    }

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void N_MaaInstance_Method_Register_MaaCustomRecognizer()
        => Assert.IsTrue(
            Instance.Register("114514", new MaaCustomRecognizerApi() { Analyze = Analyze }));

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void N_MaaInstance_Method_Unregister_MaaCustomRecognizer()
    {
        Assert.IsTrue(
            Instance.Register("1919810", new MaaCustomRecognizerApi() { Analyze = Analyze }));
        Assert.IsTrue(
                Instance.Unregister<MaaCustomRecognizerApi>("1919810"));
    }

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void N_MaaInstance_Method_Clear_MaaCustomRecognizer()
        => Assert.IsTrue(
            Instance.Clear<MaaCustomRecognizerApi>());

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void N_MaaInstance_Method_Register_MaaCustomAction()
        => Assert.IsTrue(
            Instance.Register("114514", new MaaCustomActionApi() { Run = Run, Stop = Stop }));

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void N_MaaInstance_Method_Unregister_MaaCustomAction()
    {
        Assert.IsTrue(
            Instance.Register("1919810", new MaaCustomActionApi() { Run = Run, Stop = Stop }));
        Assert.IsTrue(
                Instance.Unregister<MaaCustomActionApi>("1919810"));
    }

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void N_MaaInstance_Method_Clear_MaaCustomAction()
        => Assert.IsTrue(
            Instance.Clear<MaaCustomActionApi>());

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    public static void MaaInstance_Method_AppendTask()
    {
        InstanceJob = Instance.AppendTask("114514", string.Empty);
        Assert.IsNotNull(InstanceJob);
    }

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void O_MaaInstance_MaaJob_Method_SetParam()
        => Assert.IsFalse(
            InstanceJob.SetParam(string.Empty));

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    [ExpectedException(typeof(MaaJobStatusException))]
    public void O_MaaInstance_MaaJob_Method_Wait_MaaJobStatus_Method_ThrowIf()
        => InstanceJob.Wait().ThrowIf(MaaJobStatus.Invalid);

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    [ExpectedException(typeof(MaaJobStatusException))]
    public void O_MaaInstance_MaaJob_Property_Get_Status_MaaJobStatus_Method_ThrowIfNot()
        => InstanceJob.Status.ThrowIfNot(MaaJobStatus.Failed);

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void N_MaaInstance_Property_Get_AllTasksFinished()
        => Assert.IsTrue(
            Instance.AllTasksFinished);

#pragma warning disable S2699 // Tests should include assertions
    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void P_MaaInstance_Method_Stop()
        => Instance.Stop();
#pragma warning restore S2699 // Tests should include assertions

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void O_MaaInstance_Method_GetBindedResource()
    {
        var instance = new MaaInstance();
        Assert.IsTrue(
            instance.BindResource(Resource));
        Assert.AreSame(Resource,
            instance.GetBindedResource());
    }

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void O_MaaInstance_Method_GetBindedController()
    {
        var instance = new MaaInstance();
        Assert.IsTrue(
            instance.BindController(Controller));
        Assert.AreSame(Controller,
            instance.GetBindedController());
    }

    #endregion

    #region MaaImage

    /// <summary> Tests the constructor of the <see cref="MaaImageBuffer"/>. </summary>
    public static void MaaImage_Method_Constructor()
    {
        Image = new MaaImageBuffer();
    }

    /// <summary> Test a member of the <see cref="MaaImageBuffer"/>. </summary>
    public static void MaaImage_Method_Dispose()
        => Image.Dispose();

    /// <summary> Test a member of the <see cref="MaaImageBuffer"/>. </summary>
    [TestMethod]
    public void K_MaaImage_Method_GetRawData()
    {
        Assert.IsNotNull(
                Image.GetRawData());
    }

    /// <summary> Test a member of the <see cref="MaaImageBuffer"/>. </summary>
    [TestMethod]
    public void K_MaaImage_Property_Get_Width()
    {
        Assert.IsNotNull(
                Image.Width);
    }

    /// <summary> Test a member of the <see cref="MaaImageBuffer"/>. </summary>
    [TestMethod]
    public void K_MaaImage_Property_Get_Height()
    {
        Assert.IsNotNull(
                Image.Height);
    }

    /// <summary> Test a member of the <see cref="MaaImageBuffer"/>. </summary>
    [TestMethod]
    public void K_MaaImage_Property_Get_Type()
    {
        Assert.IsNotNull(
                Image.Type);
    }

    /// <summary> Test a member of the <see cref="MaaImageBuffer"/>. </summary>
    [TestMethod]
    public void L_MaaImage_Method_SetRawData()
    {
        Assert.IsNotNull(
                Image.SetRawData(nint.Zero, 0, 0, 0));
    }

    /// <summary> Test a member of the <see cref="MaaImageBuffer"/>. </summary>
    [TestMethod]
    public void J_MaaImage_Method_GetEncodedData()
    {
        using var image = new MaaImageBuffer();
        Assert.IsTrue(
                Controller.GetImage(image));
        Assert.IsNotNull(
                image.GetEncodedData());
    }

    /// <summary> Test a member of the <see cref="MaaImageBuffer"/>. </summary>
    [TestMethod]
    public void J_MaaImage_Property_Get_Size()
    {
        using var image = new MaaImageBuffer();
        Assert.IsTrue(
                Controller.GetImage(image));
        Assert.IsNotNull(
                image.Size);
    }

    /// <summary> Test a member of the <see cref="MaaImageBuffer"/>. </summary>
    [TestMethod]
    public void L_MaaImage_Method_SetEncodedData()
    {
        Assert.IsNotNull(
                Image.SetEncodedData(nint.Zero, 0));
    }

    #endregion 
}
