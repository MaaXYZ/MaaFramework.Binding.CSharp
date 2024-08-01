using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;

namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaInstance"/> and <see cref="MaaInstance"/>.
/// </summary>
[TestClass]
public class Test_IMaaInstance
{
    public static Dictionary<MaaTypes, object> NewData => new()
    {
#if MAA_NATIVE
        {
            MaaTypes.Native, new MaaInstance()
            {
                Resource = new MaaResource(),
                Controller = new MaaAdbController(Common.AdbPath, Common.Address, AdbControllerTypes.InputPresetAdb | AdbControllerTypes.ScreencapEncode, Common.AdbConfig, Common.AgentPath, LinkOption.None),
                DisposeOptions = DisposeOptions.All,
            }
        },
#endif
    };
    public static Dictionary<MaaTypes, object> Data { get; private set; } = default!;

    public static Dictionary<MaaTypes, object> UninitializedData { get; private set; } = default!;

    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        UninitializedData = NewData;
        Data = NewData;
        foreach (var data in Data.Values.Cast<IMaaInstance>())
        {
            Assert.IsFalse(data.IsInvalid);
            data.Callback += Common.Callback;
            data.Resource
                .AppendPath(Common.ResourcePath)
                .Wait()
                .ThrowIfNot(MaaJobStatus.Success);
            data.Controller
                .LinkStart()
                .Wait()
                .ThrowIfNot(MaaJobStatus.Success);
            data.Controller
                .SetOption(ControllerOption.ScreenshotTargetShortSide, 720);
            Assert.IsTrue(data.Initialized);
        }
    }

    [ClassCleanup]
    public static void CleanUpClass()
    {
        Common.DisposeData(Data.Values.Cast<IMaaDisposable>());
        Common.DisposeData(UninitializedData.Values.Cast<IMaaDisposable>());
    }

#pragma warning disable S2699 // Tests should include assertions
    [TestMethod]
    public void CreateInstances()
    {
#if MAA_NATIVE
        using var nativeResource = new MaaResource();
        using var nativeController = new MaaAdbController(Common.AdbPath, Common.Address, AdbControllerTypes.InputPresetAdb | AdbControllerTypes.ScreencapEncode, Common.AdbConfig, Common.AgentPath);

        using var native1 = new MaaInstance(true)
        {
            Resource = nativeResource,
            Controller = nativeController,
            DisposeOptions = DisposeOptions.None,
        };
        using var native2 = new MaaInstance(nativeController, nativeResource, DisposeOptions.None);
#endif
    }
#pragma warning restore S2699 // Tests should include assertions

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Resource(MaaTypes type, IMaaInstance maaInstance)
    {
        Assert.IsNotNull(maaInstance);

        Assert.IsNotNull(
            maaInstance.Resource);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Controller(MaaTypes type, IMaaInstance maaInstance)
    {
        Assert.IsNotNull(maaInstance);

        Assert.IsNotNull(
            maaInstance.Controller);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Toolkit(MaaTypes type, IMaaInstance maaInstance)
    {
        Assert.IsNotNull(maaInstance);

        Assert.IsNotNull(
            maaInstance.Toolkit);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Utility(MaaTypes type, IMaaInstance maaInstance)
    {
        Assert.IsNotNull(maaInstance);

        Assert.IsNotNull(
            maaInstance.Utility);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(UninitializedData))]
    public void Interface_Initialized(MaaTypes type, IMaaInstance maaInstance)
    {
        Assert.IsNotNull(maaInstance);

        Assert.IsFalse(maaInstance.Initialized);
        maaInstance
            .Resource
            .AppendPath(Common.ResourcePath)
            .Wait()
            .ThrowIfNot(MaaJobStatus.Success);
        maaInstance.Controller
            .LinkStart()
            .Wait()
            .ThrowIfNot(MaaJobStatus.Success);
        Assert.IsTrue(maaInstance.Initialized);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Register_Unregister_Clear(MaaTypes type, IMaaInstance maaInstance)
    {
        Assert.IsNotNull(maaInstance);

        // Registers custom class
        Assert.IsTrue(
            maaInstance.Register(Custom.Action));
        Assert.IsTrue(
            maaInstance.Register(Custom.Recognizer));

        // Updates if name is registered
        /*
            #289 bug, should be return true
            A callback was made on a garbage collected delegate of type 'MaaFramework.Binding.Native!MaaFramework.Binding.Interop.Native.IMaaCustomActionExtension+Run::Invoke'.)

        Assert.IsTrue(
            maaInstance.Register(Custom.Action.Name, Custom.Action));
        Assert.IsTrue(
            maaInstance.Register(Custom.Recognizer.Name, Custom.Recognizer));
        */

        // Runs a custom task
        Assert.AreEqual(MaaJobStatus.Success,
            maaInstance.AppendTask(Custom.TaskName, Custom.Param).Wait());

        // Unregisters custom class
        Assert.IsTrue(
            maaInstance.Unregister(Custom.Action));
        Assert.IsTrue(
            maaInstance.Unregister(Custom.Recognizer));

        // Unregisters custom class by name
        Assert.IsFalse(
            maaInstance.Unregister<IMaaCustomAction>(Custom.Action.Name)
         // Actually unregisters IMaaCustomAction instead of TestAction
         || maaInstance.Unregister<Custom.TestAction>(Custom.Action.Name));
        Assert.IsFalse(
            maaInstance.Unregister<IMaaCustomRecognizer>(Custom.Recognizer.Name)
         // Actually unregisters IMaaCustomRecognizer instead of TestRecognizer
         || maaInstance.Unregister<Custom.TestRecognizer>(Custom.Recognizer.Name));

        // Clears custom class by interface
        Assert.IsTrue(
            maaInstance.Clear<IMaaCustomAction>());
        Assert.IsTrue(
            maaInstance.Clear<IMaaCustomRecognizer>());

        // Cannot clear a specific implementation
        Assert.ThrowsException<NotImplementedException>(() =>
            maaInstance.Clear<Custom.TestAction>());
        Assert.ThrowsException<NotImplementedException>(() =>
            maaInstance.Clear<Custom.TestRecognizer>());

        Assert.ThrowsException<NotImplementedException>(() =>
            maaInstance.Register(Custom.Task));
        Assert.ThrowsException<NotImplementedException>(() =>
            maaInstance.Unregister(Custom.Task));

        // use the same tasks and name
        // maybe cause "A callback was made on a garbage collected delegate"
        IMaaToolkit_Interface_ExecAgent(type, maaInstance);
    }

    public static void IMaaToolkit_Interface_ExecAgent(MaaTypes type, IMaaInstance maaInstance)
    {
        Assert.IsNotNull(maaInstance);
        var maaToolkit = new MaaToolkit();

        // Registers custom class
        Assert.IsTrue(
            maaToolkit.ExecAgent.Register(maaInstance, Custom.ActionExecutor));
        Assert.IsTrue(
            maaToolkit.ExecAgent.Register(maaInstance, Custom.RecognizerExecutor));

        // Updates if name is registered
        // #289 bug, should be return true
        Assert.IsFalse(
            maaToolkit.ExecAgent.Register(maaInstance, Custom.ActionExecutor.Name, Custom.ActionExecutor));
        Assert.IsFalse(
            maaToolkit.ExecAgent.Register(maaInstance, Custom.RecognizerExecutor.Name, Custom.RecognizerExecutor));

        // Runs a custom task
        Assert.AreEqual(MaaJobStatus.Failed, // The test executor is invalid
            maaInstance.AppendTask(Custom.TaskName, Custom.Param).Wait());

        // Unregisters custom class
        Assert.IsTrue(
            maaToolkit.ExecAgent.Unregister(maaInstance, Custom.ActionExecutor));
        Assert.IsTrue(
            maaToolkit.ExecAgent.Unregister(maaInstance, Custom.RecognizerExecutor));

        // Unregisters custom class by name
        Assert.IsFalse(
            maaToolkit.ExecAgent.Unregister<MaaCustomActionExecutor>(maaInstance, Custom.ActionExecutor.Name));
        Assert.IsFalse(
            maaToolkit.ExecAgent.Unregister<MaaCustomRecognizerExecutor>(maaInstance, Custom.RecognizerExecutor.Name));

        // Clears custom class
        Assert.IsTrue(
            maaToolkit.ExecAgent.Clear<MaaCustomActionExecutor>(maaInstance));
        Assert.IsTrue(
            maaToolkit.ExecAgent.Clear<MaaCustomRecognizerExecutor>(maaInstance));
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), "EmptyTask")]
    [Obsolete("AllTasksFinished")]
    public void Interface_AppendTask_AppendRecognition_AppendAction(MaaTypes type, IMaaInstance maaInstance, string taskEntryName)
    {
        Assert.IsNotNull(maaInstance);

        // First job
        _ = maaInstance.AppendRecognition(taskEntryName);
        // Second job appended on running first job
        _ = maaInstance.AppendAction(taskEntryName);
        // Third job
        var job =
            maaInstance.AppendTask(taskEntryName);
        // Wait the third job
        Interface_IMaaPost_Success(job);
        Interface_Running_AllTasksFinished(maaInstance);
    }

    [Obsolete("AllTasksFinished")]
    private static void Interface_Running_AllTasksFinished(IMaaInstance maaInstance)
    {
        Assert.IsTrue(
            maaInstance.AllTasksFinished);
        Assert.IsFalse(
            maaInstance.Running);
    }

    private static void Interface_IMaaPost_Success(MaaTaskJob job)
    {
        Assert.IsTrue(
            job.SetParam("{}"));
        Assert.AreEqual(
            MaaJobStatus.Success, job.Wait());
        Assert.AreEqual(
            MaaJobStatus.Success, job.Status);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), "AppendRecognition", """{"AppendRecognition":{"recognition":"OCR"}}""")]
    public void IMaaMaaUtility_Query(MaaTypes type, IMaaInstance maaInstance, string taskEntryName, string diff)
    {
        Assert.IsNotNull(maaInstance);
        var job =
            maaInstance.AppendRecognition(taskEntryName, diff);
        Interface_IMaaPost_Success(job);

        Assert.IsTrue(
            maaInstance.Utility.QueryTaskDetail(job.Id, out var enter, out var nodeIdList));
        Assert.AreEqual(
            taskEntryName, enter);
        Assert.IsTrue(
            nodeIdList.Length > 0);

        Assert.IsTrue(
            maaInstance.Utility.QueryNodeDetail(nodeIdList[0], out var nodeName, out var recognitionId, out var runCompleted));
        Assert.IsFalse(
            runCompleted);

        using var hitBox = new MaaRectBuffer();
        // using var raw = new MaaImageBuffer();
        // using var draws = new MaaImageList();
        Assert.IsTrue(
            maaInstance.Utility.QueryRecognitionDetail<MaaImageBuffer>(recognitionId, out var recognitionName, out var hit, hitBox, out var detailJson, null, null));
        Assert.AreNotEqual(string.Empty,
            detailJson);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), "AppendRecognition", """{"AppendRecognition":{"recognition":"OCR"}}""")]
    public void MaaTaskJob_Query(MaaTypes type, IMaaInstance maaInstance, string taskEntryName, string diff)
    {
        Assert.IsNotNull(maaInstance);
        var job =
            maaInstance.AppendRecognition(taskEntryName, diff);
        Interface_IMaaPost_Success(job);

        Assert.IsNotNull(
            job.QueryTaskDetail());
        Assert.IsNull(
            job.QueryNodeDetail(index: 1));
        Assert.IsNotNull(
            job.QueryRecognitionDetail());
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Abort(MaaTypes type, IMaaInstance maaInstance)
    {
        Assert.IsNotNull(maaInstance);

        Assert.IsTrue(
            maaInstance.Abort());
    }

    #region Invalid data tests

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), InstanceOption.Invalid, "Anything")]
    public void Interface_SetOption_InvalidData(MaaTypes type, IMaaInstance maaInstance, InstanceOption opt, object arg)
    {
        Assert.IsNotNull(maaInstance);

        Assert.ThrowsException<InvalidOperationException>(() => maaInstance.SetOption(opt, arg));
    }

    #endregion
}
