using Grpc.Core;
using MaaFramework.Binding.Interop.Grpc;

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
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.Invalid, "Anything")]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.LogDir, nameof(Common.DebugPath))]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.SaveDraw, false)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.Recording, false)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.StdoutLevel, LoggingLevel.All)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.StdoutLevel, 7)]
    [MaaData(MaaTypes.All, nameof(Data), GlobalOption.ShowHitDraw, false)]
    public void Interface_SetOption(MaaTypes type, IMaaUtility maaUtility, GlobalOption opt, object arg)
    {
        Assert.IsNotNull(maaUtility);

        if (opt is GlobalOption.Invalid)
        {
            Assert.ThrowsException<InvalidOperationException>(() => maaUtility.SetOption(opt, arg));
            return;
        }

        Assert.IsTrue(
            maaUtility.SetOption(opt, arg));
    }

    [TestMethod]
    [MaaData(MaaTypes.Grpc, nameof(Data))]
    public void Grpc_RegisterCallback_UnregisterCallback(MaaTypes type, MaaUtilityGrpc maaUtility)
    {
        Assert.IsNotNull(maaUtility);
        var registered = true;

#pragma warning disable CA2000 // Dispose the streamingCall
        Assert.IsTrue(
            maaUtility.RegisterCallback(out var callbackId, out var streamingCall));

        // read response
        var readResponse = Task.Run(async () =>
        {
            await foreach (var response in streamingCall.ResponseStream.ReadAllAsync())
            {
                Common.Callback(this, new MaaCallbackEventArgs(response.Msg, response.Detail));
            }

            streamingCall.Dispose();
            if (registered)
                throw new TaskCanceledException();
        });
#pragma warning restore CA2000 // Dispose the streamingCall

        // register callback id
        streamingCall.RequestStream.WriteAsync(new CallbackRequest
        {
            Ok = true,
            Init = new IdRequest { Id = callbackId, },
        }).Wait();
        streamingCall.RequestStream.CompleteAsync().Wait();

        // Unregister callback id
        registered = false;
        Assert.IsTrue(
            maaUtility.UnregisterCallback(callbackId));
        readResponse.Wait();
    }

    [TestMethod]
    public void Grpc_Static_RegisterCallback_UnregisterCallback()
    {
        var registered = true;

#pragma warning disable CA2000 // Dispose the streamingCall
        Assert.IsTrue(
            MaaUtilityGrpc.RegisterCallback(Common.GrpcChannel, out var callbackId, out var streamingCall));

        // read response
        var readResponse = Task.Run(async () =>
        {
            await foreach (var response in streamingCall.ResponseStream.ReadAllAsync())
            {
                Common.Callback(this, new MaaCallbackEventArgs(response.Msg, response.Detail));
            }

            streamingCall.Dispose();
            if (registered)
                throw new TaskCanceledException();
        });
#pragma warning restore CA2000 // Dispose the streamingCall

        // register callback id
        streamingCall.RequestStream.WriteAsync(new CallbackRequest
        {
            Ok = true,
            Init = new IdRequest { Id = callbackId, },
        }).Wait();
        streamingCall.RequestStream.CompleteAsync().Wait();

        // Unregister callback id
        registered = false;
        Assert.IsTrue(
            MaaUtilityGrpc.UnregisterCallback(Common.GrpcChannel, callbackId));
        readResponse.Wait();
    }
}
