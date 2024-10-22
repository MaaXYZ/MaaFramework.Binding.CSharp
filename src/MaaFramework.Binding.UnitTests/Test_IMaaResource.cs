using MaaFramework.Binding.Abstractions;

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
            data.Callback += Common.Callback;
            data.SetOption(ResourceOption.InferenceDevice, InferenceDevice.CPU);
        }
    }

    [ClassCleanup]
    public static void CleanUpClass()
    {
        Common.DisposeData(Data.Values.Cast<IMaaDisposable>());
        Common.DisposeData(UnloadedData.Values.Cast<IMaaDisposable>());
    }

#pragma warning disable S2699 // Tests should include assertions
    [TestMethod]
    public void CreateInstances()
    {
#if MAA_NATIVE
        using var native1 = new MaaResource();
        using var native2 = new MaaResource(Common.ResourcePath, Common.ResourcePath);
        using var native3 = new MaaResource(CheckStatusOption.None, Common.ResourcePath);
        using var native4 = new MaaResource(new List<string> { Common.ResourcePath, Common.ResourcePath });
        using var native5 = new MaaResource(CheckStatusOption.None, new List<string>());
#endif
    }
#pragma warning restore S2699 // Tests should include assertions

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_AppendPath_Loaded(MaaTypes type, IMaaResource maaResource)
    {
        Assert.IsNotNull(maaResource);

        Assert.IsTrue(
            maaResource.Loaded);
        var job =
            maaResource.AppendPath(Common.ResourcePath);
        Interface_IMaaPost_Success(job);
        Assert.IsTrue(
            maaResource.Loaded);
    }

    private static void Interface_IMaaPost_Success(MaaJob job)
    {
        Assert.AreNotEqual(
            MaaJobStatus.Invalid, job.Status);
        Assert.AreEqual(
            MaaJobStatus.Succeeded, job.Wait());
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(UnloadedData))]
    public void Interface_Hash_TaskList(MaaTypes type, IMaaResource maaResource)
    {
        Assert.IsNotNull(maaResource);

        Assert.IsFalse(
            string.IsNullOrWhiteSpace(maaResource.Hash));
        Assert.AreEqual(
            "0", maaResource.Hash);
        Assert.AreEqual(
            0, maaResource.TaskList.Count);

        Interface_IMaaPost_Success(
            maaResource.AppendPath(Common.ResourcePath));
        Assert.AreNotEqual(
            "0", maaResource.Hash);
        Assert.AreNotEqual(
            0, maaResource.TaskList.Count);
        Assert.IsTrue(
            maaResource.TaskList.Any(s => s == "EmptyTask"));
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), ResourceOption.InferenceDevice, InferenceDevice.CPU)]
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
            maaResource.Clear(true));
    }

    #region Invalid data tests

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), ResourceOption.Invalid, "Anything")]
    public void Interface_SetOption_InvalidData(MaaTypes type, IMaaResource maaResource, ResourceOption opt, object arg)
    {
        Assert.IsNotNull(maaResource);

        Assert.ThrowsException<NotSupportedException>(() => maaResource.SetOption(opt, arg));
    }

    #endregion
}
