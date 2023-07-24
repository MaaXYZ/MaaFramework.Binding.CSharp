using MaaToolKit.Enums;
using MaaToolKit.Extensions.ComponentModel;
using MaaToolKit.Extensions.Exceptions;

namespace MaaToolkit.Extensions.Test;

/// <summary>
///     Test <see cref="MaaToolKit.Extensions.ComponentModel"/>.
/// </summary>
[TestClass]
public class Test_ComponentModel
{
    private const string MaaPath = "./maa";

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    private static MaaResource Resource { get; set; }
    private static MaaController Controller { get; set; }
    private static MaaInstance Instance { get; set; }

    private static MaaJob ResourceJob { get; set; }
    private static MaaJob ControllerJob { get; set; }
    private static MaaJob InstanceJob { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

    private static void OnNotify(string msg, string detailsJson, nint identifier)
    {
        Assert.IsNotNull(msg);
        Assert.IsNotNull(detailsJson);
        Assert.IsNotNull(identifier);

        Console.WriteLine(msg);
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
        MaaResource_Method_Constructor();
        MaaController_Method_Constructor(context);
        MaaInstance_Method_Constructor();

        MaaResource_Method_Append();
        MaaController_Method_LinkStart();
        MaaInstance_Method_AppendTask();

        _ = ResourceJob.Wait();
        _ = ControllerJob.Wait();
        _ = InstanceJob.Wait();
    }

    /// <summary>
    ///     Cleanup the <see cref="Test_ComponentModel"/>.
    /// </summary>
    [ClassCleanup]
    public static void CleanUpClass()
    {
        MaaResource_Method_Dispose();
        MaaController_Method_Dispose();
        MaaInstance_Method_Dispose();
    }

    #region static MaaObject

    private static readonly string s_debug = Path.GetFullPath($"{MaaPath}/debug");

    /// <summary> Tests the static member of the <see cref="MaaObject"/>. </summary>
    public static void MaaObject_Property_GetSet_FrameworkLogDir()
    {
        MaaObject.FrameworkLogDir = s_debug;
        Assert.AreNotEqual(string.Empty, MaaObject.FrameworkLogDir);
    }

    /// <summary> Tests the static member of the <see cref="MaaObject"/>. </summary>
    public static void MaaObject_Property_GetSet_ToolkitLogDir()
    {
        MaaObject.ToolkitLogDir = s_debug;
        Assert.AreNotEqual(string.Empty, MaaObject.ToolkitLogDir);
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

    private static readonly string s_resource = Path.GetFullPath($"{MaaPath}/resource");


    /// <summary> Tests the constructor of the <see cref="MaaResource"/>. </summary>
    public static void MaaResource_Method_Constructor()
    {
        Resource = new MaaResource();
        Resource.Notify += OnNotify;
    }

    /// <summary> Test a member of the <see cref="MaaResource"/>. </summary>
    public static void MaaResource_Method_Dispose()
        => Resource.Dispose();

    /// <summary> Test a member of the <see cref="MaaResource"/>. </summary>
    public static void MaaResource_Method_Append()
    {
        ResourceJob = Resource.Append(s_resource);
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

    /// <summary> Test a member of the <see cref="MaaResource"/>. </summary>
    [TestMethod]
    public void C_MaaResource_Method_SetOption()
        => Assert.IsFalse(
            Resource.SetOption(ResourceOption.Invalid, "test"));

    /// <summary> Test a member of the <see cref="MaaResource"/>. </summary>
    [TestMethod]
    public void C_MaaResource_Property_Get_Hash()
        => Assert.IsNotNull(
            Resource.Hash);

    #endregion

    #region MaaController

    private static readonly string s_controllerConfig = Path.GetFullPath($"{s_resource}/controller_config.json");
    private static readonly string s_adbConfig = File.ReadAllText(s_controllerConfig);
    private static readonly AdbControllerType s_input = AdbControllerType.InputPresetAdb;
    private static readonly AdbControllerType s_screenCap = AdbControllerType.ScreenCapRawWithGzip;

    /// <summary> Tests the constructor of the <see cref="MaaController"/>. </summary>
    public static void MaaController_Method_Constructor(TestContext testContext)
    {
        var adbPath = testContext.Properties["adbPath"] as string;
        var address = testContext.Properties["address"] as string;
        ArgumentNullException.ThrowIfNull(adbPath);
        ArgumentNullException.ThrowIfNull(address);

        Controller = new MaaController(adbPath, address, s_input | s_screenCap, s_adbConfig);
        Controller.Notify += OnNotify;
    }

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    public static void MaaController_Method_Dispose()
        => Controller.Dispose();

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void H_MaaController_Property_GetSet_ScreenshotWidth()
    {
        Controller.ScreenshotHeight = 114514;
        Controller.ScreenshotWidth = 114514;
        Assert.AreEqual(0, Controller.ScreenshotHeight);
        Assert.AreNotEqual(0, Controller.ScreenshotWidth);
    }

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void H_MaaController_Property_GetSet_ScreenshotHeight()
    {
        Controller.ScreenshotWidth = 114514;
        Controller.ScreenshotHeight = 114514;
        Assert.AreEqual(0, Controller.ScreenshotWidth);
        Assert.AreNotEqual(0, Controller.ScreenshotHeight);
    }

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void H_MaaController_Property_GetSet_DefaultAppPackageEntry()
    {
        Controller.DefaultAppPackageEntry = 114514.ToString();
        Assert.AreNotEqual(string.Empty, Controller.DefaultAppPackageEntry);
    }

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void H_MaaController_Property_GetSet_DefaultAppPackage()
    {
        Controller.DefaultAppPackage = 114514.ToString();
        Assert.AreNotEqual(string.Empty, Controller.DefaultAppPackage);
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
    public void H_MaaController_Method_Screencap()
        => Assert.IsNotNull(
            Controller.Screencap());

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
    [TestMethod]
    public void P_MaaController_Method_LinkStop()
        => Assert.IsTrue(
            Controller.LinkStop());

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void I_MaaController_Method_GetImage()
        => Assert.IsNotNull(
            Controller.GetImage());

    /// <summary> Test a member of the <see cref="MaaController"/>. </summary>
    [TestMethod]
    public void H_MaaController_Property_Get_Uuid()
        => Assert.IsNotNull(
            Controller.Uuid);

    #endregion

    #region MaaInstance

    /// <summary> Tests the constructor of the <see cref="MaaInstance"/>. </summary>
    public static void MaaInstance_Method_Constructor()
    {
        Instance = new MaaInstance();
        Instance.Notify += OnNotify;
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
        => Assert.IsTrue(
            Instance.Initialized);

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    public static void MaaInstance_Method_AppendTask()
    {
        InstanceJob = Instance.AppendTask(114514.ToString(), string.Empty);
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
        => Assert.AreSame(Resource, Instance.GetBindedResource());

    /// <summary> Test a member of the <see cref="MaaInstance"/>. </summary>
    [TestMethod]
    public void O_MaaInstance_Method_GetBindedController()
        => Assert.AreSame(Controller, Instance.GetBindedController());

    #endregion
}
