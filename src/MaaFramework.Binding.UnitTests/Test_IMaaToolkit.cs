namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaToolkit"/> and <see cref="MaaToolKit"/> and <see cref="MaaToolKitGrpc"/>.
/// </summary>
[TestClass]
public class Test_IMaaToolkit
{
    public static Dictionary<MaaTypes, IMaaUtility> MaaUtilityData { get; } = new()
    {
        { MaaTypes.Native, new MaaUtility() },
        { MaaTypes.Grpc,   new MaaUtilityGrpc(Common.GrpcChannel) },
    };

    public static Dictionary<MaaTypes, object> NewData => new()
    {
        { MaaTypes.Native, new MaaToolKit() },
        { MaaTypes.Grpc,   new MaaToolKitGrpc(Common.GrpcChannel) },
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
    [MaaData(MaaTypes.All, nameof(NewData))]
    public void Interface_Init_Uninit(MaaTypes type, IMaaToolkit maaToolkit)
    {
        Assert.IsNotNull(maaToolkit);
        // In Maa.Framework.Runtimes v1.4.0.
        // Notes: maaToolkit.Init() will change the log path to ./debug/maa.log.
        Assert.IsTrue(
            maaToolkit.Init());
        Assert.IsTrue(
            maaToolkit.Uninit());
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), "")]
    [MaaData(MaaTypes.All, nameof(Data), nameof(Common.AdbPath))]
    public void Interface_Find(MaaTypes type, IMaaToolkit maaToolkit, string arg)
    {
        Assert.IsNotNull(maaToolkit);

        var devices = maaToolkit.Find(arg);
        Assert.AreNotEqual(0, devices.Length);
        CollectionAssert.AllItemsAreUnique(devices);
        foreach (var device in devices)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.AdbPath));
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.AdbSerial));
            Assert.IsFalse(string.IsNullOrWhiteSpace(device.AdbConfig));
            device.AdbTypes.Check();
        }

        using IMaaController maaController = type switch
        {
            MaaTypes.Native => devices[0].ToAdbController(Path.GetFullPath("./MaaAgentBinary"),
                                                          link: LinkOption.None),
            MaaTypes.Grpc => devices[0].ToAdbControllerGrpc(Common.GrpcChannel,
                                                            Path.GetFullPath("./MaaAgentBinary"),
                                                            link: LinkOption.None),
            _ => throw new NotImplementedException(),
        };
        maaController
            .LinkStart()
            .Wait()
            .ThrowIfNot(MaaJobStatus.Success, MaaJobStatusException.MaaControllerMessage);
    }
}
