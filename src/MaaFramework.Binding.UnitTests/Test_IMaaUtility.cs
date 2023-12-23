namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaUtility"/> and <see cref="MaaUtility"/> and <see cref="MaaUtilityGrpc"/>.
/// </summary>
[TestClass]
public class Test_IMaaMaaUtility
{
    public static Dictionary<MaaTypes, object> NewData => new()
    {
        { MaaTypes.Native, new MaaUtility() },
        { MaaTypes.Grpc,   new MaaUtilityGrpc(Common.GrpcChannel) },
    };
    public static Dictionary<MaaTypes, object> Data { get; private set; } = default!;

    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        Data = NewData;
    }

    [ClassCleanup]
    public static void CleanUpClass()
    {
        // CleanUp
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Version(MaaTypes type, IMaaUtility maaUtility)
    {
        Assert.IsNotNull(maaUtility);

        Assert.IsFalse(string.IsNullOrWhiteSpace(
            maaUtility.Version));
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.Logging, nameof(Common.DebugPath))]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.DebugMode, false)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.Invalid, "Anything")]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.Invalid, false)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.Invalid, 0)]
    public void Interface_SetOption(MaaTypes type, IMaaUtility maaUtility, GlobalOption opt, object arg)
    {
        Assert.IsNotNull(maaUtility);

        var ret = Common.SetOption(maaUtility, opt, arg);
        if (opt is GlobalOption.Invalid)
            Assert.IsFalse(ret);
        else
            Assert.IsTrue(ret);
    }

    [TestMethod]
    [MaaData(MaaTypes.Grpc, nameof(Data))]
    public void Grpc_RegisterCallback_UnregisterCallback(MaaTypes type, MaaUtilityGrpc maaUtility)
    {
        Assert.IsNotNull(maaUtility);

        Assert.IsTrue(
            maaUtility.RegisterCallback(out var callbackId, out _));
        Assert.IsTrue(
            maaUtility.UnregisterCallback(callbackId));
    }

    [TestMethod]
    public void Grpc_Static_RegisterCallback_UnregisterCallback()
    {
        Assert.IsTrue(
            MaaUtilityGrpc.RegisterCallback(Common.GrpcChannel, out var callbackId, out _));
        Assert.IsTrue(
            MaaUtilityGrpc.UnregisterCallback(Common.GrpcChannel, callbackId));
    }
}
