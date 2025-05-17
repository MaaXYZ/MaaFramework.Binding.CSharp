using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;
using MaaFramework.Binding.Notification;

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
            // 3 ways to subscribe to the Callback event
            data.Callback += Common.OnCallback;
            data.Callback += Common.NotificationHandlerRegistry.OnCallback;
            {
                data.Callback += Common.OnTaskerTask.ToCallback();
                data.Callback += Common.OnNodeNextList.ToCallback();
                data.Callback += Common.OnNodeRecognition.ToCallback();
                data.Callback += Common.OnNodeAction.ToCallback();
            }

            _ = data.Resource
                .AppendBundle(Common.BundlePath)
                .Wait()
                .ThrowIfNot(MaaJobStatus.Succeeded);
            _ = data.Resource
                .SetOption_InferenceDevice(InferenceDevice.CPU);
            _ = data.Resource
                .SetOption_InferenceExecutionProvider(InferenceExecutionProvider.CPU);
            _ = data.Controller
                .LinkStart()
                .Wait()
                .ThrowIfNot(MaaJobStatus.Succeeded);
            _ = data.Controller
                .SetOption_ScreenshotTargetShortSide(720);
            Assert.IsTrue(data.IsInitialized);
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

        Assert.IsNotNull(maaTasker.Resource = maaTasker.Resource);
        switch (type)
        {
            case MaaTypes.Native:
                var native = maaTasker as MaaTasker;
                Assert.IsNotNull(native);
                Assert.IsNotNull(native.Resource = native.Resource);
                break;
            case MaaTypes.None:
            case MaaTypes.All:
            case MaaTypes.Custom:
            default:
                throw new NotImplementedException();
        }
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Controller(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);

        Assert.IsNotNull(maaTasker.Controller = maaTasker.Controller);
        switch (type)
        {
            case MaaTypes.Native:
                var native = maaTasker as MaaTasker;
                Assert.IsNotNull(native);
                Assert.IsNotNull(native.Controller = native.Controller);
                break;
            case MaaTypes.None:
            case MaaTypes.All:
            case MaaTypes.Custom:
            default:
                throw new NotImplementedException();
        }
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Toolkit(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);

        Assert.IsNotNull(maaTasker.Toolkit = maaTasker.Toolkit);
        switch (type)
        {
            case MaaTypes.Native:
                var native = maaTasker as MaaTasker;
                Assert.IsNotNull(native);
                Assert.IsNotNull(native.Toolkit = native.Toolkit);
                break;
            case MaaTypes.None:
            case MaaTypes.All:
            case MaaTypes.Custom:
            default:
                throw new NotImplementedException();
        }
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Utility(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);

        Assert.IsNotNull(
            maaTasker.Utility);
        Assert.IsNotNull(maaTasker.Utility = maaTasker.Utility);
        switch (type)
        {
            case MaaTypes.Native:
                var native = maaTasker as MaaTasker;
                Assert.IsNotNull(native);
                Assert.IsNotNull(native.Utility = native.Utility);
                break;
            case MaaTypes.None:
            case MaaTypes.All:
            case MaaTypes.Custom:
            default:
                throw new NotImplementedException();
        }
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(UninitializedData))]
    public void Interface_IsInitialized(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);

        Assert.IsFalse(maaTasker.IsInitialized);
        _ = maaTasker.Resource
            .AppendBundle(Common.BundlePath)
            .Wait()
            .ThrowIfNot(MaaJobStatus.Succeeded);
        _ = maaTasker.Controller
            .LinkStart()
            .Wait()
            .ThrowIfNot(MaaJobStatus.Succeeded);
        Assert.IsTrue(maaTasker.IsInitialized);
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
            maaTasker.AppendTask(Custom.NodeName, Custom.Param).Wait());

        // Unregisters custom class
        Assert.IsTrue(
            // Actually unregisters IMaaCustomAction instead of TestAction
            maaTasker.Resource.Unregister(Custom.Action)
            && maaTasker.Resource.Unregister<IMaaCustomAction>(Custom.Action.Name)
            && maaTasker.Resource.Unregister<Custom.TestAction>(Custom.Action.Name));
        Assert.IsTrue(
            // Actually unregisters IMaaCustomRecognition instead of TestRecognition
            maaTasker.Resource.Unregister(Custom.Recognition)
            && maaTasker.Resource.Unregister<IMaaCustomRecognition>(Custom.Recognition.Name)
            && maaTasker.Resource.Unregister<Custom.TestRecognition>(Custom.Recognition.Name));

        // Clears custom class by interface
        Assert.IsTrue(
            maaTasker.Resource.Clear<IMaaCustomAction>());
        Assert.IsTrue(
            maaTasker.Resource.Clear<IMaaCustomRecognition>());

        // Cannot clear a specific implementation
        _ = Assert.ThrowsException<NotImplementedException>(()
            => maaTasker.Resource.Clear<Custom.TestAction>());
        _ = Assert.ThrowsException<NotImplementedException>(()
            => maaTasker.Resource.Clear<Custom.TestRecognition>());

        _ = Assert.ThrowsException<NotImplementedException>(()
            => maaTasker.Resource.Register(Custom.InvalidResource));
        _ = Assert.ThrowsException<NotImplementedException>(()
            => maaTasker.Resource.Unregister(Custom.InvalidResource));
    }


    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), "EmptyNode")]
    public void Interface_AppendTask(MaaTypes type, IMaaTasker maaTasker, string taskEntryName)
    {
        Assert.IsNotNull(maaTasker);

        // First job
        _ = maaTasker.AppendTask(taskEntryName);
        // Second job appended on running first job
        _ = maaTasker.AppendTask(taskEntryName);
        // Third job
        var job =
            maaTasker.AppendTask(taskEntryName);
        // Wait the third job
        Interface_IMaaPost_Success(job);
        Interface_IsRunning(maaTasker);
    }

    private static void Interface_IsRunning(IMaaTasker maaTasker)
    {
        Assert.IsFalse(
            maaTasker.IsRunning);
    }

    private static void Interface_IMaaPost_Success(MaaTaskJob job)
    {
        Assert.AreNotEqual(
            MaaJobStatus.Invalid, job.Status);
        Assert.AreEqual(
            MaaJobStatus.Succeeded, job.Wait());
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), "AppendRecognition", """{"AppendRecognition":{"recognition":"OCR"}}""")]
    public void Interface_Query(MaaTypes type, IMaaTasker maaTasker, string taskEntryName, string diff)
    {
        Assert.IsNotNull(maaTasker);
        var job =
            maaTasker.AppendTask(taskEntryName, diff);
        Interface_IMaaPost_Success(job);

        Assert.IsTrue(
            maaTasker.GetTaskDetail(job.Id, out var enter, out var nodeIdList, out var status));
        Assert.AreEqual(
            taskEntryName, enter);
        Assert.IsTrue(
            nodeIdList.Length > 0);
        Assert.AreEqual(
            MaaJobStatus.Succeeded, status);

        Assert.IsTrue(
            maaTasker.GetNodeDetail(nodeIdList[0], out var nodeName, out var recognitionId, out var actionCompleted));
        Assert.IsTrue(
            actionCompleted);

        Assert.IsTrue(
            maaTasker.GetLatestNode(nodeName, out var nodeLatestId));
        Assert.AreEqual(
            nodeIdList[0], nodeLatestId);

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
            maaTasker.Utility.SetOption_DebugMode(debugMode));

        var job =
            maaTasker.AppendTask(taskEntryName, diff);
        Interface_IMaaPost_Success(job);

        Assert.IsNotNull(
            job.QueryTaskDetail());
        Assert.IsNull(
            job.QueryNodeDetail(index: 1));

        var node = job.QueryNodeDetail();
        Assert.AreSame(
            node, node?.QueryLatest(job.Tasker));

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
            maaTasker.Utility.SetOption_DebugMode(false));
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Abort(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);
        var job =
            maaTasker.Abort();
        Interface_IMaaPost_Success(job);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_ClearCache(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);
        Assert.IsTrue(
            maaTasker.ClearCache());
    }

    #region Invalid data tests

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), TaskerOption.Invalid, "Anything")]
    public void Interface_SetOption_InvalidData(MaaTypes type, IMaaTasker maaTasker, TaskerOption opt, object arg)
    {
        Assert.IsNotNull(maaTasker);

        _ = Assert.ThrowsException<NotSupportedException>(()
            => maaTasker.SetOption(opt, arg));
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_GetTaskDetail_InvalidData(MaaTypes type, IMaaTasker maaTasker)
    {
        Assert.IsNotNull(maaTasker);

        Assert.IsFalse(
            maaTasker.GetTaskDetail(0, out var entry, out var nodeIdList, out var status));
        Assert.AreEqual(
            string.Empty, entry);
        Assert.AreEqual(
            0, nodeIdList.Length);
        Assert.AreEqual(
            MaaJobStatus.Invalid, status);
    }

    #endregion
}
