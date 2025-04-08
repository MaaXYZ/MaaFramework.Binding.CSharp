#!/usr/bin/env dotnet-script

#r "nuget: Maa.Framework, 3.0.4"

using MaaFramework.Binding;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;

var toolkit = new MaaToolkit();
// Register custom action
toolkit.PI.Register(new MyAction());
// Run MaaPiCli
toolkit.PI.RunCli("sample/resource", "sample/cache", false);


internal sealed class MyAction : IMaaCustomAction
{
    public string Name { get; set; } = "MyAct";

    public bool Run(in IMaaContext context, in RunArgs args)
    {
        Console.WriteLine($"on MyAction.run, context: {context}, args: {args}");

        context.OverrideNext(args.NodeName, ["TaskA", "TaskB"]);

        using var image = new MaaImageBuffer();
        context.Tasker.Controller.GetCachedImage(image);
        context.Tasker.Controller.Click(100, 100).Wait();

        var RecognitionDetail = context.RunRecognition(
            "Cat", """{"Cat": {"recognition": "OCR", "expected": "喵喵喵"}}""", image
        );
        // if RecognitionDetail xxxx

        return true;
    }
}
