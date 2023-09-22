using MaaToolkit.Extensions.UnitTests;
using MaaToolKit.Extensions.Enums;

namespace MaaToolKit.Extensions.UnitTests;

/// <summary>
///     Test <see cref="MaaTool"/>.
/// </summary>
[TestClass]
public class Test_MaaToolKit
{
    /// <summary>
    ///     Initializes the <see cref="Test_MaaToolKit"/>.
    /// </summary>
    /// <param name="context">The <see cref="TestContext"/></param>
    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        MaaObject.FrameworkLogDir = GlobalInfo.DebugPath;
        Assert.IsTrue(
            MaaTool.Init());
        MaaObject.FrameworkLogDir = GlobalInfo.DebugPath;
    }

    /// <summary>
    ///     Cleanup the <see cref="Test_MaaToolKit"/>.
    /// </summary>
    [ClassCleanup]
    public static void CleanUpClass()
    {
        Assert.IsTrue(
            MaaTool.Uninit());
    }

    /// <summary> Test a member of the <see cref="MaaTool"/>.</summary>
    [TestMethod]
    public void Method_FindDevice()
        => Assert.AreNotEqual<ulong>(0,
            MaaTool.FindDevice());

    /// <summary> Test a member of the <see cref="MaaTool"/>.</summary>
    [TestMethod]
    public void Method_FindDevice_param_string()
        => Assert.AreNotEqual<ulong>(0,
            MaaTool.FindDevice(GlobalInfo.AdbPath));

    /// <summary> Test a member of the <see cref="MaaTool"/>.</summary>
    [TestMethod]
    public void Method_GetDeviceName()
        => Assert.AreNotEqual(string.Empty,
            MaaTool.GetDeviceName(0));

    /// <summary> Test a member of the <see cref="MaaTool"/>.</summary>
    [TestMethod]
    public void Method_GetDeviceAdbPath()
        => Assert.AreNotEqual(string.Empty,
            MaaTool.GetDeviceAdbPath(0));

    /// <summary> Test a member of the <see cref="MaaTool"/>.</summary>
    [TestMethod]
    public void Method_GetDeviceAdbSerial()
        => Assert.AreNotEqual(string.Empty,
            MaaTool.GetDeviceAdbSerial(0));

    /// <summary> Test a member of the <see cref="MaaTool"/>.</summary>
    [TestMethod]
    public void Method_GetDeviceAdbControllerType()
        => Assert.AreNotEqual(AdbControllerType.Invalid,
            MaaTool.GetDeviceAdbControllerType(0));

    /// <summary> Test a member of the <see cref="MaaTool"/>.</summary>
    [TestMethod]
    public void Method_GetDeviceAdbConfig()
        => Assert.AreNotEqual(string.Empty,
            MaaTool.GetDeviceAdbConfig(0));
}
