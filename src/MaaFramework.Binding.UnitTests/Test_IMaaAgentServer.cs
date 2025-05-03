namespace MaaFramework.Binding.UnitTests;

internal static class Test_IMaaAgentServer
{
    public static int Main()
    {
        var commandLineArgs = Environment.GetCommandLineArgs();
        if (commandLineArgs.Length < 4)
        {
            Console.WriteLine("Call AgentMain.cs instead of this file.");
            return 1;
        }

        var socketId = commandLineArgs[^1];
        var userPath = commandLineArgs[^2];
        var dllPath = commandLineArgs[^3];

        NativeBindingInfo.Set(isAgentServer: true, dllPath); // First step
        _ = new MaaToolkit(true, userPath);
        var agentServer = new MaaAgentServer();
        _ = agentServer.Register(Custom.Recognition);
        _ = agentServer.Register(Custom.Action);
        _ = agentServer.StartUp(socketId);
        agentServer.Join();
        agentServer.ShutDown();
        return 0;
    }
}
