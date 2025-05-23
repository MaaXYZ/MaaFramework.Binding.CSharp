﻿using MaaFramework.Binding.Abstractions;
using System.Diagnostics;

namespace MaaFramework.Binding.UnitTests;

/// <summary>
///     Test <see cref="IMaaAgentClient"/> and <see cref="IMaaAgentServer"/>.
/// </summary>
[TestClass]
// ReSharper disable InconsistentNaming
public class Test_IMaaAgentClient
{
    public static Dictionary<MaaTypes, object> NewData => new()
    {
#if MAA_NATIVE
        { MaaTypes.Native, MaaAgentClient.Create(new MaaResource()) },
#endif
    };
    public static Dictionary<MaaTypes, object> Data { get; private set; } = default!;

    [ClassInitialize]
    public static void InitializeClass(TestContext context)
    {
        Data = NewData;
        foreach (var data in Data.Values.Cast<IMaaAgentClient>())
        {
            Assert.IsFalse(data.IsInvalid);
        }
    }

    [ClassCleanup(ClassCleanupBehavior.EndOfClass)]
    public static void CleanUpClass()
    {
        Common.DisposeData(Data.Values.Cast<IMaaDisposable>());
    }

    [TestMethod]
    public void CreateInstances()
    {
#if MAA_NATIVE
        var newId = Guid.NewGuid().ToString();
        using var native1 = MaaAgentClient.Create(new MaaResource());
        using var native2 = MaaAgentClient.Create(new MaaResource());
        using var native3 = MaaAgentClient.Create(newId, new MaaResource());

        Assert.AreNotEqual(native1.Id, native2.Id);
        Assert.AreEqual(newId, native3.Id);
#endif
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Id_Resource(MaaTypes type, IMaaAgentClient maaAgentClient)
    {
        Assert.AreNotEqual(string.Empty, maaAgentClient.Id);
        Assert.IsNotNull(maaAgentClient.Resource);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_LinkStart_LinkStop_AgentServerProcess(MaaTypes type, IMaaAgentClient maaAgentClient)
    {
        _ = Assert.ThrowsExactly<InvalidOperationException>(() =>
            maaAgentClient.AgentServerProcess);
        Assert.IsTrue(
            maaAgentClient.IsConnected);    // IsConnected 的值有问题，先改了一下LinkStop()实现，记得改回来

        var ret = maaAgentClient.LinkStart(StartupAgentServer);
        Assert.IsTrue(
            ret);
        Assert.IsTrue( // double start
            maaAgentClient.LinkStart());
        Assert.IsFalse(
            maaAgentClient.AgentServerProcess.HasExited);
        Assert.IsTrue(
            maaAgentClient.IsConnected);

        Assert.IsTrue(
            maaAgentClient.LinkStop());
        Assert.IsTrue( // double stop
            maaAgentClient.LinkStop());
        Task.Delay(100).Wait(); // wait for process exit
        Assert.IsTrue(
            maaAgentClient.AgentServerProcess.HasExited);
        Assert.IsTrue(
            maaAgentClient.IsConnected);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void RunTask(MaaTypes type, IMaaAgentClient maaAgentClient)
    {
        using var maa = new MaaTasker
        {
            Controller = new MaaAdbController(Common.AdbPath, Common.Address, AdbScreencapMethods.Encode, AdbInputMethods.AdbShell, Common.AdbConfig, Common.AgentPath),
            Resource = new MaaResource(),
            DisposeOptions = DisposeOptions.All,
        };
        Assert.IsTrue(
            maa.IsInitialized);

        using var agent = MaaAgentClient.Create("6CDC213A-085C-40C8-8665-635820D10425", maa.Resource);
        using (var cts = new CancellationTokenSource(10000))
        {
            Assert.IsTrue(
            //  agent.LinkStart());
                agent.LinkStart(StartupAgentServer, cts.Token));
        }
        var status = maa
            .AppendTask(Custom.NodeName, Custom.Param)
            .Wait();

        Assert.AreEqual(MaaJobStatus.Succeeded,
            status);
        Assert.IsTrue(
            agent.LinkStop());
    }

    private static Process? StartupAgentServer(string identifier, string nativeLibraryDirectory)
    {
        return Process.Start(new ProcessStartInfo(
            "dotnet", $"{typeof(Test_IMaaAgentServer).Assembly.Location} {nativeLibraryDirectory} {Environment.CurrentDirectory} {identifier}")
        {
            UseShellExecute = false,
        });
    }
}
