namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaUtility"/> and <see cref="MaaUtility"/>.
/// </summary>
[TestClass]
// ReSharper disable InconsistentNaming
public class Test_IMaaMaaUtility
{
    public static Dictionary<MaaTypes, object> NewData => new()
    {
#if MAA_NATIVE
        { MaaTypes.Native, MaaUtility.Shared },
#endif
    };
    public static Dictionary<MaaTypes, object> Data { get; private set; } = default!;

    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        Data = NewData;
    }

    [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
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
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.LogDir, nameof(Common.DebugPath))]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.SaveDraw, false)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.Recording, false)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.StdoutLevel, LoggingLevel.Off)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.StdoutLevel, 0)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.ShowHitDraw, false)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.DebugMode, false)]
    public void Interface_SetOption(MaaTypes type, IMaaUtility maaUtility, GlobalOption opt, object arg)
    {
        Assert.IsNotNull(maaUtility);

        Assert.IsTrue(
            maaUtility.SetOption(opt, arg));
    }

    #region Invalid data tests

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.Invalid, "Anything")]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.LogDir, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.SaveDraw, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.Recording, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.StdoutLevel, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.ShowHitDraw, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.DebugMode, 0.0)]
    public void Interface_SetOption_InvalidData(MaaTypes type, IMaaUtility maaUtility, GlobalOption opt, object arg)
    {
        Assert.IsNotNull(maaUtility);

        _ = Assert.ThrowsExactly<NotSupportedException>(()
            => maaUtility.SetOption(opt, arg));
    }

    #endregion
}
