using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Notification;

namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaResource"/> and <see cref="MaaResource"/>.
/// </summary>
[TestClass]
// ReSharper disable once InconsistentNaming
public class Test_IMaaResource
{
    public static Dictionary<MaaTypes, object> NewData => new()
    {
#if MAA_NATIVE
        { MaaTypes.Native, new MaaResource() },
#endif
    };
    public static Dictionary<MaaTypes, object> ImageData = new()
    {
#if MAA_NATIVE
        { MaaTypes.Native, MaaImage.Load<Buffers.MaaImageBuffer>(Common.ImagePath).Buffer },
#endif
    };
    public static Dictionary<MaaTypes, object> Data { get; private set; } = default!;
    public static Dictionary<MaaTypes, object> UnloadedData { get; private set; } = default!;

    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        UnloadedData = NewData;
        Data = NewData;
        foreach (var data in Data.Values.Cast<IMaaResource>())
        {
            Assert.IsFalse(data.IsInvalid);
            // 3 ways to subscribe to the Callback event
            data.Callback += Common.OnCallback;
            data.Callback += Common.NotificationHandlerRegistry.OnCallback;
            data.Callback += Common.OnResourceLoading.ToCallback();

            _ = data.SetInference_UseCpu();
        }
    }

    [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
    public static void CleanUpClass()
    {
        Common.DisposeData(Data.Values.Cast<IMaaDisposable>());
        Common.DisposeData(UnloadedData.Values.Cast<IMaaDisposable>());
        Common.DisposeData(ImageData.Values.Cast<IMaaDisposable>());
    }

#pragma warning disable S2699 // Tests should include assertions
    [TestMethod]
    public void CreateInstances()
    {
#if MAA_NATIVE
        using var native1 = new MaaResource();
        using var native2 = new MaaResource(Common.BundlePath, Common.BundlePath);
        using var native3 = new MaaResource(CheckStatusOption.None, Common.BundlePath);
        using var native4 = new MaaResource(new List<string> { Common.BundlePath, Common.BundlePath });
        using var native5 = new MaaResource(CheckStatusOption.None, new List<string>());
#endif
    }
#pragma warning restore S2699 // Tests should include assertions

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_AppendBundle_IsLoaded(MaaTypes type, IMaaResource maaResource)
    {
        Assert.IsNotNull(maaResource);

        Assert.IsTrue(
            maaResource.IsLoaded);
        var job =
            maaResource.AppendBundle(Common.BundlePath);
        Interface_IMaaPost_Success(job);
        Assert.IsTrue(
            maaResource.IsLoaded);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_OverridePipeline_OverrideNext_GetNodeData(MaaTypes type, IMaaResource maaResource)
    {
        Assert.IsNotNull(maaResource);

        var DiffParam = Custom.DiffParam;
        var DiffEntry = Custom.DiffEntry;
        Assert.IsTrue(
            maaResource.OverridePipeline(DiffParam));
        Assert.IsTrue(
            maaResource.OverrideNext(DiffEntry, [DiffEntry]));
        Assert.IsTrue(
            maaResource.GetNodeData(DiffEntry, out var data));

        Assert.IsNotNull(
            data);
        Assert.IsTrue(
            data.Contains($"\"next\":[\"{DiffEntry}\"]"));
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_OverrideImage(MaaTypes type, IMaaResource maaResource)
    {
        Assert.IsNotNull(maaResource);

        Assert.IsTrue(
            maaResource.OverrideImage("NewImageName", (IMaaImageBuffer)ImageData[type]));
    }

    private static void Interface_IMaaPost_Success(MaaJob job)
    {
        Assert.AreNotEqual(
            MaaJobStatus.Invalid, job.Status);
        Assert.AreEqual(
            MaaJobStatus.Succeeded, job.Wait());
        Assert.AreEqual(
            $"MaaJob {{ Status = Succeeded, Id = {job.Id} }}", job.ToString());
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(UnloadedData))]
    public void Interface_Hash_NodeList(MaaTypes type, IMaaResource maaResource)
    {
        Assert.IsNotNull(maaResource);

        Assert.IsTrue(
            string.IsNullOrWhiteSpace(maaResource.Hash));
        Assert.AreEqual(
            0, maaResource.NodeList.Count);

        Interface_IMaaPost_Success(
            maaResource.AppendBundle(Common.BundlePath));
        Assert.IsFalse(
            string.IsNullOrWhiteSpace(maaResource.Hash));
        Assert.AreNotEqual(
            0, maaResource.NodeList.Count);
        Assert.IsTrue(
            maaResource.NodeList.Any(static s => s == "EmptyNode"));
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), ResourceOption.InferenceDevice, (int)InferenceDevice.CPU)]
    [MaaData(MaaTypes.All, nameof(Data), ResourceOption.InferenceDevice, InferenceDevice.CPU)]
    [MaaData(MaaTypes.All, nameof(Data), ResourceOption.InferenceExecutionProvider, (int)InferenceExecutionProvider.CPU)]
    [MaaData(MaaTypes.All, nameof(Data), ResourceOption.InferenceExecutionProvider, InferenceExecutionProvider.CPU)]
    public void Interface_SetOption(MaaTypes type, IMaaResource maaResource, ResourceOption opt, object arg)
    {
        Assert.IsNotNull(maaResource);

        Assert.IsTrue(
            maaResource.SetOption(opt, arg));
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Clear(MaaTypes type, IMaaResource maaResource)
    {
        Assert.IsNotNull(maaResource);

        Assert.IsTrue(
            maaResource.Clear());
        Assert.IsTrue(
            maaResource.Clear(includeCustomResource: true));
    }

    #region Invalid data tests

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), ResourceOption.Invalid, "Anything")]
    [MaaData(MaaTypes.All, nameof(Data), ResourceOption.InferenceDevice, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), ResourceOption.InferenceExecutionProvider, 0.0)]
    public void Interface_SetOption_InvalidData(MaaTypes type, IMaaResource maaResource, ResourceOption opt, object arg)
    {
        Assert.IsNotNull(maaResource);

        _ = Assert.ThrowsExactly<NotSupportedException>(()
            => maaResource.SetOption(opt, arg));
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Register_Unregister_InvalidData(MaaTypes type, IMaaResource maaResource)
    {
        Assert.IsNotNull(maaResource);

        _ = Assert.ThrowsExactly<NotImplementedException>(()
            => maaResource.Register(Custom.InvalidResource));
        _ = Assert.ThrowsExactly<NotImplementedException>(()
            => maaResource.Register(nameof(Custom.InvalidResource), Custom.InvalidResource));

        _ = Assert.ThrowsExactly<NotImplementedException>(()
            => maaResource.Unregister(Custom.InvalidResource));
        _ = Assert.ThrowsExactly<NotImplementedException>(()
            => maaResource.Unregister<Custom.TestInvalidResource>(nameof(Custom.InvalidResource)));

        _ = Assert.ThrowsExactly<NotImplementedException>(()
            => maaResource.Clear<Custom.TestInvalidResource>());
    }

    #endregion
}
