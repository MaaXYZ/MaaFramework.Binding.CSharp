#!/usr/bin/env dotnet-script
#r "nuget: Maa.Framework.Binding.Native, 4.0.0-preview.25163.6"

using MaaFramework.Binding;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;

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
agentServer.Register(new MyRec());
agentServer.Register(new MyAct());
agentServer.StartUp(socketId);
agentServer.Join();
agentServer.ShutDown();

Console.Write("Press any key to exit:");
Console.ReadKey();
return 0;

internal sealed class MyRec : IMaaCustomRecognition
{
    public string Name { get; set; } = nameof(MyRec);

    public bool Analyze(in IMaaContext context, in AnalyzeArgs args, in AnalyzeResults results)
    {
        Console.WriteLine("{0} Called", Name);
        
        results.Box.SetValues(0, 0, 100, 100);
        results.Detail.SetValue("Hello Client!");
        return true;
    }
}

internal sealed class MyAct : IMaaCustomAction
{
    public string Name { get; set; } = nameof(MyAct);

    public bool Run(in IMaaContext context, in RunArgs args)
    {
        Console.WriteLine("{0} Called", Name);
        Console.WriteLine("recognition detail: {0}", args.RecognitionDetail);
        Console.WriteLine("custom action param: {0}", args.ActionParam);
        
        return true;
    }
}
