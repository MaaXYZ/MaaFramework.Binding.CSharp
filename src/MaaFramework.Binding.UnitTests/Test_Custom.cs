using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;

namespace MaaFramework.Binding.UnitTests;

internal static class Custom
{
    public static TestController Controller { get; } = new();
    public static TestAction Action { get; } = new();
    public static TestRecognizer Recognizer { get; } = new();
    public static TestTask Task { get; } = new();
    public static MaaCustomActionExecutor ActionExecutor { get; } = new() { Name = Action.Name, Parameter = [], Path = "" };
    public static MaaCustomRecognizerExecutor RecognizerExecutor { get; } = new() { Name = Recognizer.Name, Parameter = [], Path = "" };
    public static string TaskName => "中文字符测试";
    public static string Param => $$"""
    {
        "{{TaskName}}": {
            "recognition": "Custom",
            "custom_recognition": "{{Recognizer.Name}}",
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

    internal static void Test_IMaaSyncContext_Interface(in IMaaSyncContext syncContext)
    {
        Assert.IsTrue(
            syncContext.RunTask("EmptyTask", "{}"));
        // syncContext.RunRecognizer
        // syncContext.RunAction
        Assert.IsTrue(
            syncContext.Click(0, 0));
        Assert.IsTrue(
            syncContext.Swipe(0, 0, 10, 10, 100));
        Assert.IsTrue(
            syncContext.PressKey(0x00000003));
        Assert.IsTrue(
            syncContext.InputText("0x00000003"));
        Assert.IsFalse( // AdbTapInput not supports
            syncContext.TouchDown(1, 100, 100, 0));
        Assert.IsFalse( // AdbTapInput not supports
            syncContext.TouchMove(1, 200, 200, 0));
        Assert.IsFalse( // AdbTapInput not supports
            syncContext.TouchUp(1));

        using var image1 = new MaaImageBuffer();
        Assert.IsTrue(
            syncContext.Screencap(image1));
        using var image2 = new MaaImageBuffer();
        Assert.IsTrue(
            syncContext.GetCachedImage(image2));

        Assert.AreEqual(
            image1.GetEncodedData(out var size1), image1.GetEncodedData(out var size2));
        Assert.AreEqual(
            size1, size2);
    }

    internal sealed class TestRecognizer : IMaaCustomRecognizer
    {
        public string Name { get; set; } = nameof(TestRecognizer);

        public bool Analyze(in IMaaSyncContext syncContext, IMaaImageBuffer image, string taskName, string customRecognitionParam, in IMaaRectBuffer outBox, in IMaaStringBuffer outDetail)
        {
            Assert.AreEqual(TaskName, taskName);
            Assert.AreEqual(RecognitionParam, customRecognitionParam);
            Test_IMaaSyncContext_Interface(syncContext);

            var ret = syncContext.RunRecognizer(image, DiffEntry, DiffParam, outBox, outDetail);

            outBox.SetValues(outBox.X, outBox.Y, outBox.Width, outBox.Height);
            outDetail.SetValue(outDetail.GetValue());
            // return ret;

            // Assert
            Detail = outDetail.GetValue();
            Box = $"{outBox.X}{outBox.Y}{outBox.Width}{outBox.Height}";

            return ret;
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

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public bool Run(in IMaaSyncContext syncContext, string taskName, string customActionParam, IMaaRectBuffer curBox, string curRecDetail)
        {
            Assert.AreEqual(TaskName, taskName);
            Assert.AreEqual(ActionParam, customActionParam);

            Assert.AreNotEqual(Detail, curRecDetail);
            Assert.AreEqual(Box, $"{curBox.X}{curBox.Y}{curBox.Width}{curBox.Height}");

            return syncContext.RunAction(DiffEntry, DiffParam, curBox, curRecDetail);
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

    internal sealed class TestTask : IMaaCustomTask
    {
        public string Name { get; set; } = nameof(TestTask);
    }
}

