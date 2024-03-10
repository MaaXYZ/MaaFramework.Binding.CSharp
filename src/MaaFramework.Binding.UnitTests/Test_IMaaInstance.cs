using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaInstance"/> and <see cref="MaaInstance"/> and <see cref="MaaInstanceGrpc"/>.
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
#if MAA_GRPC
        {
            MaaTypes.Grpc, new MaaInstanceGrpc(Common.GrpcChannel)
            {
                Resource = new MaaResourceGrpc(Common.GrpcChannel),
                Controller = new MaaAdbControllerGrpc(Common.GrpcChannel, Common.AdbPath, Common.Address, AdbControllerTypes.InputPresetAdb | AdbControllerTypes.ScreencapEncode, Common.AdbConfig, Common.AgentPath, LinkOption.None),
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
#if MAA_GRPC
        using var grpcResource = new MaaResourceGrpc(Common.GrpcChannel);
        using var grpcController = new MaaAdbControllerGrpc(Common.GrpcChannel, Common.AdbPath, Common.Address, AdbControllerTypes.InputPresetAdb | AdbControllerTypes.ScreencapEncode, Common.AdbConfig, Common.AgentPath);

        using var grpc1 = new MaaInstanceGrpc(Common.GrpcChannel, true)
        {
            Resource = grpcResource,
            Controller = grpcController,
            DisposeOptions = DisposeOptions.None,
        };
        using var grpc2 = new MaaInstanceGrpc(Common.GrpcChannel, grpcController, grpcResource, DisposeOptions.None);
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
    [MaaData(MaaTypes.All, nameof(Data), "EmptyTask")]
    public void Interface_AppendTask_AllTasksFinished(MaaTypes type, IMaaInstance maaInstance, string taskEntryName)
    {
        Assert.IsNotNull(maaInstance);

        // First job
        var job =
            maaInstance.AppendTask(taskEntryName);
        Assert.IsFalse(
            job.SetParam("{}"));
        Assert.AreEqual(
            MaaJobStatus.Running, job.Status);

        // Second job appended on running first job
        job =
            maaInstance.AppendTask(taskEntryName);
        Assert.IsFalse(
            maaInstance.AllTasksFinished);
        Interface_IMaaPost_Success(job);
        Assert.IsTrue(
            maaInstance.AllTasksFinished);
    }

    public static void Interface_IMaaPost_Success(IMaaJob job)
    {
        Assert.IsNotNull(job);

        Assert.IsTrue(
            job.SetParam("{}"));
        Assert.AreEqual(
            MaaJobStatus.Success, job.Wait());
        Assert.AreEqual(
            MaaJobStatus.Success, job.Status);
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
