#!/usr/bin/env dotnet-script
#r "nuget: Maa.Framework.Native, 4.0.0-preview.25163.6"
#r "nuget: Maa.Framework.Runtime.win-x64, 4.0.0"

using System.Diagnostics;
using MaaFramework.Binding;

var toolkit = new MaaToolkit(true);
var resource = new MaaResource();
var maa = new MaaTasker
{
    Controller = toolkit.AdbDevice.Find().First().ToAdbController(),
    Resource = resource,
    DisposeOptions = DisposeOptions.All,
    Toolkit = toolkit,
};

if (!maa.Initialized)
    throw new InvalidOperationException("Failed to init tasker.");
    
var agent = new MaaAgentClient
{
    Resource = resource,
    DisposeOptions = DisposeOptions.All,
};

var socketId = agent.CreateSocket(string.Empty)
    ?? throw new InvalidOperationException("Failed to create socket.");

var p = Process.Start(new ProcessStartInfo(
    "dotnet",["script",
        "AgentChild.cs",
        NativeBindingInfo.NativeAssemblyDirectory
            ?? throw new ArgumentNullException("Native.BindingInfo.NativeAssemblyDirectory"),
        Environment.CurrentDirectory,
        socketId]) { UseShellExecute = true });

if (!agent.LinkStart())
    throw new InvalidOperationException("Failed to connect.");

var ppover = """
{
    "Entry": {"next": "Rec"},
    "Rec": {
        "recognition": "Custom",
        "custom_recognition": "MyRec",
        "action": "Custom",
        "custom_action": "MyAct",
        "custom_action_param": {
            "param": "Hello Server!"
        }
    }
}
""";
Console.WriteLine(ppover);

var detail = maa
    .AppendTask("Entry", ppover)
    .WaitFor(MaaJobStatus.Succeeded)
    .QueryTaskDetail()
    ?? throw new InvalidOperationException("Failed to pipeline.");
Console.WriteLine($"pipeline detail: {detail}");
Console.WriteLine($"MyRec detail: {detail.QueryRecognitionDetail(maa, 1)?.Detail}");

agent.LinkStop();

Console.Write("Press any key to exit:");
Console.ReadKey();
