namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaGlobal"/> and <see cref="MaaGlobal"/>.
/// </summary>
[TestClass]
// ReSharper disable InconsistentNaming
public class Test_IMaaGlobal
{
    public static Dictionary<MaaTypes, object> NewData => new()
    {
#if MAA_NATIVE
        { MaaTypes.Native, MaaGlobal.Shared },
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
    public void Interface_Version(MaaTypes type, IMaaGlobal maaGlobal)
    {
        Assert.IsNotNull(maaGlobal);

        Assert.IsFalse(string.IsNullOrWhiteSpace(
            NativeBindingContext.LibraryVersion));
        Assert.IsFalse(string.IsNullOrWhiteSpace(
            NativeBindingContext.BindingVersion));
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.LogDir, nameof(Common.DebugPath))]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.SaveDraw, false)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.StdoutLevel, LoggingLevel.Off)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.StdoutLevel, 0)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.DebugMode, false)]
    public void Interface_SetOption(MaaTypes type, IMaaGlobal maaGlobal, GlobalOption opt, object arg)
    {
        Assert.IsNotNull(maaGlobal);

        Assert.IsTrue(
            maaGlobal.SetOption(opt, arg));
    }

    #region Invalid data tests

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.Invalid, "Anything")]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.LogDir, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.SaveDraw, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.StdoutLevel, 0.0)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.DebugMode, 0.0)]
    public void Interface_SetOption_InvalidData(MaaTypes type, IMaaGlobal maaGlobal, GlobalOption opt, object arg)
    {
        Assert.IsNotNull(maaGlobal);

        _ = Assert.ThrowsExactly<NotSupportedException>(()
            => maaGlobal.SetOption(opt, arg));
    }

    #endregion
}
