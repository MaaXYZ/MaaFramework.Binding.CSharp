using MaaFramework.Binding.Abstractions;
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
    public void Interface_LinkStart_LinkStop_AgentServerProcess_IsConnected_IsAlive_SetTimeout(MaaTypes type, IMaaAgentClient maaAgentClient)
    {
        _ = Assert.ThrowsExactly<InvalidOperationException>(() =>
            maaAgentClient.AgentServerProcess);
        Assert.IsFalse(
            maaAgentClient.IsConnected);
        Assert.IsFalse(
            maaAgentClient.IsAlive);

        Assert.IsTrue(
            maaAgentClient.SetTimeout(TimeSpan.MaxValue));
        Assert.IsTrue(
            maaAgentClient.SetTimeout(TimeSpan.MinValue));
        Assert.IsTrue(
            maaAgentClient.SetTimeout(TimeSpan.FromMinutes(2)));

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
            maaAgentClient.IsAlive);

        Assert.IsTrue(
            maaAgentClient.LinkStop());
        Assert.IsTrue( // double stop
            maaAgentClient.LinkStop());
        Task.Delay(1000).Wait(); // wait for process exit
        Assert.IsTrue(
            maaAgentClient.AgentServerProcess.HasExited);
        Assert.IsFalse(
            maaAgentClient.IsConnected);
        Assert.IsFalse(
            maaAgentClient.IsAlive);
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_Cancel_CancelWith(MaaTypes type, IMaaAgentClient maaAgentClient)
    {
        using var res = new MaaResource();
        using var agent = MaaAgentClient.Create(res);
        var ct = new CancellationToken(true);
        var job = new MaaJob(0, res);
        Assert.AreEqual(MaaJobStatus.Invalid, job.Wait());

        Assert.IsFalse(
            agent.Cancel(waitFunc: agent.LinkStart));
        Assert.IsFalse(
            agent.Cancel(waitTask: Task.Run(agent.LinkStart)));
        Assert.IsFalse(
            agent.Cancel(waitJob: job));

        Assert.IsFalse(
            agent.CancelWith(ct, waitFunc: agent.LinkStart));
        Assert.IsFalse(
            agent.CancelWith(ct, waitTask: Task.Run(agent.LinkStart)));
        Assert.IsFalse(
            agent.CancelWith(ct, waitJob: job));
    }

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Case_RunTask(MaaTypes type, IMaaAgentClient maaAgentClient)
    {
        using var maa = type switch
        {
#if MAA_NATIVE
            MaaTypes.Native => new MaaTasker
            {
                Controller = new MaaAdbController(Common.AdbPath, Common.Address, AdbScreencapMethods.Encode, AdbInputMethods.AdbShell, Common.AdbConfig, Common.AgentPath),
                Resource = new MaaResource(),
                DisposeOptions = DisposeOptions.All,
            },
#endif
            _ => throw new NotImplementedException(),
        };
        Assert.IsTrue(maa.IsInitialized);

        using var agent = type switch
        {
#if MAA_NATIVE
            MaaTypes.Native => MaaAgentClient.Create("6CDC213A-085C-40C8-8665-635820D10425", maa.Resource),
#endif
            _ => throw new NotImplementedException(),
        };
        using (var cts = new CancellationTokenSource(10 * 1000))
        {
            Assert.IsTrue(
                // agent.LinkStart());
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

    [TestMethod]
    [MaaData(MaaTypes.All, nameof(Data))]
    public void Interface_AttachDisposeToResource_DetachDisposeToResource(MaaTypes type, IMaaAgentClient maaAgentClient)
    {
        var res = new MaaResource();
        var agent = MaaAgentClient
            .Create(res)
            .AttachDisposeToResource();
        Assert.IsFalse(
            res.IsInvalid);
        Assert.IsFalse(
            agent.IsInvalid);
        res.Dispose();
        Assert.IsTrue(
            res.IsInvalid);
        Assert.IsTrue(
            agent.IsInvalid);

        _ = agent.DetachDisposeToResource();
    }
}
