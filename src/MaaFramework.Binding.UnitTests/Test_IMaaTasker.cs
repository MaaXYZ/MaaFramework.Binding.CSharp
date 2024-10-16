using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;

namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaTasker"/> and <see cref="MaaTasker"/>.
/// </summary>
[TestClass]
// ReSharper disable InconsistentNaming
public class Test_IMaaTasker
{
    public static Dictionary<MaaTypes, object> NewData => new()
    {
#if MAA_NATIVE
        {
            MaaTypes.Native, new MaaTasker()
            {
                Resource = new MaaResource(),
                Controller = new MaaAdbController(Common.AdbPath, Common.Address, AdbScreencapMethods.Encode, AdbInputMethods.AdbShell, Common.AdbConfig, Common.AgentPath, LinkOption.None),
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
        foreach (var data in Data.Values.Cast<IMaaTasker>())
        {
            Assert.IsFalse(data.IsInvalid);
            data.Callback += Common.Callback;
            data.Resource
                .AppendPath(Common.ResourcePath)
                .Wait()
                .ThrowIfNot(MaaJobStatus.Succeeded);
            data.Resource
                .SetOption(ResourceOption.InferenceDevice, InferenceDevice.CPU);
            data.Controller
                .LinkStart()
                .Wait()
                .ThrowIfNot(MaaJobStatus.Succeeded);
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
        using var nativeController = new MaaAdbController(Common.AdbPath, Common.Address, AdbScreencapMethods.Encode, AdbInputMethods.AdbShell, Common.AdbConfig, Common.AgentPath);

        using var native1 = new MaaTasker(true)
        {
            Resource = nativeResource,
            Controller = nativeController,
            DisposeOptions = DisposeOptions.None,
        };
        using var native2 = new MaaTasker(nativeController, nativeResource, DisposeOptions.None);
#endif
    }
#pragma warning restore S2699 // Tests should include assertions

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Resource(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);

        Assert.IsNotNull(
            maaTasker.Resource);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Controller(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);

        Assert.IsNotNull(
            maaTasker.Controller);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Toolkit(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);

        Assert.IsNotNull(
            maaTasker.Toolkit);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Utility(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);

        Assert.IsNotNull(
            maaTasker.Utility);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(UninitializedData))]
    public void Interface_Initialized(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);

        Assert.IsFalse(maaTasker.Initialized);
        maaTasker
            .Resource
            .AppendPath(Common.ResourcePath)
            .Wait()
            .ThrowIfNot(MaaJobStatus.Succeeded);
        maaTasker.Controller
            .LinkStart()
            .Wait()
            .ThrowIfNot(MaaJobStatus.Succeeded);
        Assert.IsTrue(maaTasker.Initialized);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Register_Unregister_Clear(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);

        // Registers custom class
        Assert.IsTrue(
            maaTasker.Resource.Register(Custom.Action));
        Assert.IsTrue(
            maaTasker.Resource.Register(Custom.Recognition));

        // Updates if name is registered
        Assert.IsTrue(
            maaTasker.Resource.Register(Custom.Action.Name, Custom.Action));
        Assert.IsTrue(
            maaTasker.Resource.Register(Custom.Recognition.Name, Custom.Recognition));

        // Runs a custom task
        Assert.AreEqual(MaaJobStatus.Succeeded,
            maaTasker.AppendPipeline(Custom.TaskName, Custom.Param).Wait());

        // Unregisters custom class
        Assert.IsTrue(
            maaTasker.Resource.Unregister(Custom.Action));
        Assert.IsTrue(
            maaTasker.Resource.Unregister(Custom.Recognition));

        // Unregisters custom class by name
        Assert.IsFalse(
            maaTasker.Resource.Unregister<IMaaCustomAction>(Custom.Action.Name)
         // Actually unregisters IMaaCustomAction instead of TestAction
         || maaTasker.Resource.Unregister<Custom.TestAction>(Custom.Action.Name));
        Assert.IsFalse(
            maaTasker.Resource.Unregister<IMaaCustomRecognition>(Custom.Recognition.Name)
         // Actually unregisters IMaaCustomRecognition instead of TestRecognition
         || maaTasker.Resource.Unregister<Custom.TestRecognition>(Custom.Recognition.Name));

        // Clears custom class by interface
        Assert.IsTrue(
            maaTasker.Resource.Clear<IMaaCustomAction>());
        Assert.IsTrue(
            maaTasker.Resource.Clear<IMaaCustomRecognition>());

        // Cannot clear a specific implementation
        Assert.ThrowsException<NotImplementedException>(() =>
            maaTasker.Resource.Clear<Custom.TestAction>());
        Assert.ThrowsException<NotImplementedException>(() =>
            maaTasker.Resource.Clear<Custom.TestRecognition>());

        Assert.ThrowsException<NotImplementedException>(() =>
            maaTasker.Resource.Register(Custom.Resource));
        Assert.ThrowsException<NotImplementedException>(() =>
            maaTasker.Resource.Unregister(Custom.Resource));
    }


    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), "EmptyTask")]
    public void Interface_AppendPipeline(MaaTypes type, IMaaTasker maaTasker, string taskEntryName)
    {
        Assert.IsNotNull(maaTasker);

        // First job
        _ = maaTasker.AppendPipeline(taskEntryName);
        // Second job appended on running first job
        _ = maaTasker.AppendPipeline(taskEntryName);
        // Third job
        var job =
            maaTasker.AppendPipeline(taskEntryName);
        // Wait the third job
        Interface_IMaaPost_Success(job);
        Interface_Running(maaTasker);
    }

    private static void Interface_Running(IMaaTasker maaTasker)
    {
        Assert.IsFalse(
            maaTasker.Running);
    }

    private static void Interface_IMaaPost_Success(MaaTaskJob job)
    {
        Assert.AreEqual(
            MaaJobStatus.Succeeded, job.Wait());
        Assert.AreEqual(
            MaaJobStatus.Succeeded, job.Status);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), "AppendRecognition", """{"AppendRecognition":{"recognition":"OCR"}}""")]
    public void Interface_Query(MaaTypes type, IMaaTasker maaTasker, string taskEntryName, string diff)
    {
        Assert.IsNotNull(maaTasker);
        var job =
            maaTasker.AppendPipeline(taskEntryName, diff);
        Interface_IMaaPost_Success(job);

        Assert.IsTrue(
            maaTasker.GetTaskDetail(job.Id, out var enter, out var nodeIdList));
        Assert.AreEqual(
            taskEntryName, enter);
        Assert.IsTrue(
            nodeIdList.Length > 0);

        Assert.IsTrue(
            maaTasker.GetNodeDetail(nodeIdList[0], out _, out var recognitionId, out var actionCompleted));
        Assert.IsTrue(
            actionCompleted);

        using var hitBox = new MaaRectBuffer();
        // using var raw = new MaaImageBuffer();
        // using var draws = new MaaImageListBuffer();
        Assert.IsTrue(
            maaTasker.GetRecognitionDetail<MaaImageBuffer>(recognitionId, out _, out _, out _, hitBox, out var detailJson, null, null));
        Assert.AreNotEqual(string.Empty,
            detailJson);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), "AppendRecognition", """{"AppendRecognition":{"recognition":"OCR"}}""", true)]
    [MaaData(MaaTypes.All, nameof(Data), "AppendRecognition", """{"AppendRecognition":{"recognition":"OCR"}}""", false)]
    public void MaaTaskJob_Query(MaaTypes type, IMaaTasker maaTasker, string taskEntryName, string diff, bool debugMode)
    {
        Assert.IsNotNull(maaTasker);
        Assert.IsTrue(
            maaTasker.Utility.SetOption(GlobalOption.DebugMode, debugMode));

        var job =
            maaTasker.AppendPipeline(taskEntryName, diff);
        Interface_IMaaPost_Success(job);

        Assert.IsNotNull(
            job.QueryTaskDetail());
        Assert.IsNull(
            job.QueryNodeDetail(index: 1));

        // Tip: dispose the recognition detail to dispose HitBox, Raw and Draws after the query is completed.
        using var recognitionDetail = job.QueryRecognitionDetail();
        Assert.IsNotNull(recognitionDetail);
        if (debugMode)
        {
            Assert.IsNotNull(recognitionDetail.Raw);
            Assert.IsNotNull(recognitionDetail.Draws);
        }
        else
        {
            Assert.IsNull(recognitionDetail.Raw);
            Assert.IsNull(recognitionDetail.Draws);
        }

        Assert.IsTrue(
            maaTasker.Utility.SetOption(GlobalOption.DebugMode, false));
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Abort(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);
        Assert.IsTrue(
            maaTasker.Abort());
    }

    #region Invalid data tests

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), TaskerOption.Invalid, "Anything")]
    public void Interface_SetOption_InvalidData(MaaTypes type, IMaaTasker maaTasker, TaskerOption opt, object arg)
    {
        Assert.IsNotNull(maaTasker);

        Assert.ThrowsException<InvalidOperationException>(() => maaTasker.SetOption(opt, arg));
    }

    #endregion
}
