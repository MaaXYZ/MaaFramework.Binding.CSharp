using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;

namespace MaaFramework.Binding.UnitTests;

internal static class Custom
{
    public static TestController Controller { get; } = new();
    public static TestAction Action { get; } = new();
    public static TestRecognition Recognition { get; } = new();
    public static TestResource Resource { get; } = new();
    public static MaaCustomActionExecutor ActionExecutor { get; } = new() { Name = Action.Name, Parameter = [], Path = "" };
    public static MaaCustomRecognitionExecutor RecognitionExecutor { get; } = new() { Name = Recognition.Name, Parameter = [], Path = "" };
    public static string TaskName => "中文字符测试";
    public static string Param => $$"""
    {
        "{{TaskName}}": {
            "recognition": "Custom",
            "custom_recognition": "{{Recognition.Name}}",
            "custom_recognition_param": {{RecognitionParam}},
            "action": "Custom",
            "custom_action": "{{Action.Name}}",
            "custom_action_param": {{ActionParam}}
        }
    }
    """;
    private static string RecognitionParam => $$"""{"{{nameof(RecognitionParam)}}":true}""";
    private static string ActionParam => $$"""{"{{nameof(ActionParam)}}":true}""";
    private static string Detail { get; set; } = string.Empty;
    private static string Box { get; set; } = string.Empty;

    internal sealed class TestRecognition : IMaaCustomRecognition
    {
        public string Name { get; set; } = nameof(TestRecognition);
        public bool Analyze(in IMaaContext context, in AnalyzeArgs args, in AnalyzeResults results)
        {
            Assert.AreEqual(TaskName, args.TaskName);
            Assert.AreEqual(RecognitionParam, args.RecognitionParam);

            var recognitionDetail = context.RunRecognition(DiffEntry, DiffParam, args.Image) as RecognitionDetail<MaaImageBuffer>;
            Assert.IsNotNull(recognitionDetail?.HitBox);

            recognitionDetail.HitBox.CopyTo(results.Box);
            results.Detail.SetValue(recognitionDetail.Detail);
            // return ret;

            // Using in assert
            Detail = recognitionDetail.Detail;
            Box = $"{results.Box.X}{results.Box.Y}{results.Box.Width}{results.Box.Height}";

            return true;
        }
    }

    private const string DiffEntry = "ColorMatch";
    private const string DiffParam = $$"""
    {
        "{{DiffEntry}}": {
            "recognition": "ColorMatch",
            "lower": [100, 100, 100],
            "upper": [255, 255, 255],
            "action": "Click"
        }
    }
    """;

    internal sealed class TestAction : IMaaCustomAction
    {
        public string Name { get; set; } = nameof(TestAction);

        public bool Run<T>(in IMaaContext context, in RunArgs<T> args) where T : IMaaImageBuffer
        {
            Assert.AreEqual(TaskName, args.TaskName);
            Assert.AreEqual(ActionParam, args.ActionParam);

            Assert.AreNotEqual(Detail, args.RecognitionDetail.Detail);
            Assert.AreEqual(Box, $"{args.RecognitionBox.X}{args.RecognitionBox.Y}{args.RecognitionBox.Width}{args.RecognitionBox.Height}");

            var nodeDetail = context.RunAction(DiffEntry, DiffParam, args.RecognitionBox, args.RecognitionDetail.Detail);
            Assert.IsNotNull(nodeDetail);
            return true;
        }
    }

    internal sealed class TestController : IMaaCustomController
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Click(int x, int y)
        {
            throw new NotImplementedException();
        }

        public bool Connect()
        {
            throw new NotImplementedException();
        }

        public bool InputText(string text)
        {
            throw new NotImplementedException();
        }

        public bool PressKey(int keycode)
        {
            throw new NotImplementedException();
        }

        public bool RequestResolution(out int width, out int height)
        {
            throw new NotImplementedException();
        }

        public bool RequestUuid(in IMaaStringBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public bool Screencap(in IMaaImageBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public bool StartApp(string intent)
        {
            throw new NotImplementedException();
        }

        public bool StopApp(string intent)
        {
            throw new NotImplementedException();
        }

        public bool Swipe(int x1, int y1, int x2, int y2, int duration)
        {
            throw new NotImplementedException();
        }

        public bool TouchDown(int contact, int x, int y, int pressure)
        {
            throw new NotImplementedException();
        }

        public bool TouchMove(int contact, int x, int y, int pressure)
        {
            throw new NotImplementedException();
        }

        public bool TouchUp(int contact)
        {
            throw new NotImplementedException();
        }
    }

    internal sealed class TestResource : IMaaCustomResource
    {
        public string Name { get; set; } = nameof(TestResource);
    }
}

