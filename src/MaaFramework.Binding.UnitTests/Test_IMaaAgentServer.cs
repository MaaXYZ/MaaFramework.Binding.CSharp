﻿namespace MaaFramework.Binding.UnitTests;

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
        Test(socketId, userPath, dllPath);

        return 0;
    }

    public static void Test(string id, string userPath, string dllPath)
    {
        _ = MaaAgentServer.Current // test double call
            .WithIdentifier(id).WithIdentifier(id)
            .WithNativeLibrary(dllPath).WithNativeLibrary(dllPath)
            .WithToolkitConfig_InitOption(userPath).WithToolkitConfig_InitOption(userPath)
            .Register(Custom.Recognition) // cat not double call from here
            .Register(Custom.Action)
            .StartUp()
            .Join().Join() // test double call
            .ShutDown().ShutDown();
    }
}
