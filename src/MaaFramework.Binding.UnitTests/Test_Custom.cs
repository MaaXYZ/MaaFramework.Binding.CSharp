using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;

namespace MaaFramework.Binding.UnitTests;

internal static class Custom
{
    public static TestAction Action { get; } = new();
    public static TestRecognition Recognition { get; } = new();
    public static TestInvalidResource InvalidResource { get; } = new();
    public static string NodeName => "中文字符测试";
    public static string Param => $$"""
    {
        "{{NodeName}}": {
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
            Assert.AreEqual(NodeName, args.NodeName);
            Assert.AreEqual(RecognitionParam, args.RecognitionParam);

            _ = Assert.ThrowsException<ArgumentException>(() =>
#if MAA_NATIVE
                new MaaContext(IntPtr.Zero));
#endif
            var cloneContext = (context as ICloneable).Clone() as IMaaContext;
            cloneContext = cloneContext?.Clone();
#if MAA_NATIVE
            cloneContext = (cloneContext as MaaContext)?.Clone();
#endif
            Assert.IsNotNull(cloneContext);
            Assert.IsNull(
                cloneContext.RunRecognition(DiffEntry, args.Image));
            if (!context.Tasker.IsStateless)
            {
                Assert.AreSame(
                    context.Tasker, cloneContext.Tasker);
            }

            Assert.AreEqual(
                context.TaskJob.Id, cloneContext.TaskJob.Id);

            var recognitionDetail =
                context.RunRecognition(DiffEntry, args.Image, DiffParam);
            Assert.IsNotNull(
                recognitionDetail?.HitBox);

            Assert.IsTrue(
                cloneContext.OverridePipeline(DiffParam));
            Assert.AreEqual(
                recognitionDetail.NodeName, cloneContext.RunTask(DiffEntry, "{}")?.QueryRecognitionDetail(cloneContext.Tasker)?.NodeName);
            Assert.IsTrue(
                cloneContext.OverrideNext(DiffEntry, [DiffEntry]));

            Assert.IsTrue(
                recognitionDetail.HitBox.TryCopyTo(results.Box));
            Assert.IsTrue(
                results.Detail.TrySetValue(recognitionDetail.Detail));
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

        public bool Run(in IMaaContext context, in RunArgs args, in RunResults results)
        {
            Assert.AreEqual(NodeName, args.NodeName);
            Assert.AreEqual(ActionParam, args.ActionParam);

            Assert.AreNotEqual(Detail, args.RecognitionDetail.Detail);
            Assert.AreEqual(Box, $"{args.RecognitionBox.X}{args.RecognitionBox.Y}{args.RecognitionBox.Width}{args.RecognitionBox.Height}");

            var nodeDetail = context.RunAction(DiffEntry, args.RecognitionBox, args.RecognitionDetail.Detail, DiffParam);
            Assert.IsNotNull(nodeDetail);
            return true;
        }
    }

    internal sealed class TestController(IMaaController c) : IMaaCustomController, IMaaDisposable
    {
        #region Test_IMaaDisposable

        public bool IsInvalid => c.IsInvalid;

        public bool ThrowOnInvalid
        {
            get => c.ThrowOnInvalid;
            set => c.ThrowOnInvalid = value;
        }

        public bool IsStateless => c.IsStateless;

        public void Dispose() => c.Dispose();

        #endregion

        public string Name { get; set; } = "TestController";

        public bool Click(int x, int y)
            => c.Click(x, y).Wait() == MaaJobStatus.Succeeded;

        public bool Connect()
            => c.LinkStart().Wait() == MaaJobStatus.Succeeded;

        public bool InputText(string text)
            => c.InputText(text).Wait() == MaaJobStatus.Succeeded;

        public bool PressKey(int keycode)
            => c.PressKey(keycode).Wait() == MaaJobStatus.Succeeded;

        public bool RequestResolution(out int width, out int height)
        {
#if MAA_NATIVE
            using var image = new MaaImageBuffer();
#endif
            if (Screencap(image))
            {
                width = image.Width;
                height = image.Height;
                return true;
            }

            width = height = -1;
            return false;
        }

        public bool RequestUuid(in IMaaStringBuffer buffer)
        {
            var uuid = c.Uuid;
            return uuid is not null && buffer.TrySetValue(uuid);
        }

        public bool Screencap(in IMaaImageBuffer buffer)
            => c.Screencap().Wait() == MaaJobStatus.Succeeded && c.GetCachedImage(buffer);

        public bool StartApp(string intent)
            => c.StartApp(intent).Wait() == MaaJobStatus.Succeeded;

        public bool StopApp(string intent)
            => c.StopApp(intent).Wait() == MaaJobStatus.Succeeded;

        public bool Swipe(int x1, int y1, int x2, int y2, int duration)
            => c.Swipe(x1, y1, x2, y2, duration).Wait() == MaaJobStatus.Succeeded;

        public bool TouchDown(int contact, int x, int y, int pressure)
            => c.TouchDown(contact, x, y, pressure).Wait() == MaaJobStatus.Succeeded;

        public bool TouchMove(int contact, int x, int y, int pressure)
            => c.TouchMove(contact, x, y, pressure).Wait() == MaaJobStatus.Succeeded;

        public bool TouchUp(int contact)
            => c.TouchUp(contact).Wait() == MaaJobStatus.Succeeded;
    }

    internal sealed class TestInvalidResource : IMaaCustomResource
    {
        public string Name { get; set; } = nameof(TestInvalidResource);
    }
}

