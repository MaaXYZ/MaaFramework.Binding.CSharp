using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;
using System.Text.Json;

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
    public static string DirectCustomAction = $$"""
    {
        "{{nameof(DirectCustomAction)}}": {
            "action": "Custom",
            "custom_action": "{{nameof(EmptyAction)}}"
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
        public bool Analyze<T>(T context, in AnalyzeArgs args, in AnalyzeResults results) where T : IMaaContext
        {
            Assert.AreEqual(NodeName, args.NodeName);
            Assert.AreEqual(RecognitionParam, args.RecognitionParam);

            var cloneContext = (context as ICloneable).Clone() as IMaaContext;
            cloneContext = cloneContext!.Clone();
#if MAA_NATIVE
            cloneContext = (cloneContext as MaaContext)!.Clone();
#endif
            Assert.IsNotNull(cloneContext);

            Assert.IsFalse(
                context.IsCancellationRequested);
            Assert.IsTrue(
                context.WaitFreezes(TimeSpan.FromSeconds(1)));

            Assert.IsFalse(
                context.GetAnchor(DefaultAnchorName, out var nodeName));
            Assert.IsNull(
                nodeName);
            Assert.IsTrue(
                context.SetAnchor(DefaultAnchorName, DefaultAnchorNodeName));
            Assert.IsTrue(
                context.GetAnchor(DefaultAnchorName, out nodeName));
            Assert.AreEqual(
                DefaultAnchorNodeName, nodeName);

            Assert.IsNotNull(
                context.RunTask(DefaultAnchorNodeName));
            Assert.IsTrue(
                context.GetHitCount(DefaultAnchorNodeName, out var hitCount));
            Assert.AreNotEqual<ulong>(
                0, hitCount);
            Assert.IsTrue(
                context.ClearHitCount(DefaultAnchorNodeName));
            Assert.IsTrue(
                context.GetHitCount(DefaultAnchorNodeName, out hitCount));
            Assert.AreEqual<ulong>(
                0, hitCount);

            Assert.IsNull(
                cloneContext.RunRecognition(DiffEntry, args.Image));
            if (!context.Tasker.IsStateless)
            {
                Assert.AreSame(
                    context.Tasker, cloneContext.Tasker);
            }

            Assert.AreEqual(
                context.TaskJob.Id, cloneContext.TaskJob.Id);

            Assert.IsFalse(
                context.GetNodeData(DiffEntry, out var data));
            Assert.IsNull(data);

            using var recognitionDetail =
                context.RunRecognition(DiffEntry, args.Image, DiffParam);
            Assert.IsFalse(
                context.GetNodeData(DiffEntry, out data));
            Assert.IsNotNull(
                recognitionDetail?.HitBox);


            using var recoDirectDetail =
                context.RunRecognitionDirect(RecoDirectType, RecoDirectParam, args.Image);
            Assert.IsNotNull(
                recoDirectDetail?.HitBox);

            Assert.IsTrue(
                cloneContext.OverridePipeline(DiffParam));
            Assert.AreEqual(
                recognitionDetail.NodeName, cloneContext.RunTask(DiffEntry, "{}")?.QueryRecognitionDetail(cloneContext.Tasker)?.NodeName);
            Assert.IsTrue(
                cloneContext.OverrideNext(DiffEntry, [DiffEntry]));
#if MAA_NATIVE
            using var image = MaaImage.Load<MaaImageBuffer>(Common.ImagePath);
            Assert.IsTrue(
                cloneContext.OverrideImage("NewImageName", image.Buffer));
#endif

            Assert.IsFalse(
                context.GetNodeData(DiffEntry, out data));
            Assert.IsTrue(
                cloneContext.GetNodeData(DiffEntry, out data));
            Assert.IsNotNull(data);

            using var document = JsonDocument.Parse(data);
            var root = document.RootElement;
            Assert.IsTrue(root.TryGetProperty("next", out var nextElement),
                "Expected JSON to contain a 'next' property.");
            Assert.AreEqual(JsonValueKind.Array, nextElement.ValueKind,
                "Expected 'next' to be a JSON array.");
            var containsDiffEntry = nextElement
                .EnumerateArray()
                .Any(element =>
                    element.ValueKind == JsonValueKind.Object &&
                    element.TryGetProperty("name", out var nameProperty) &&
                    nameProperty.GetString() == DiffEntry);
            Assert.IsTrue(containsDiffEntry,
                $"Expected 'next' array to contain an element with name '{DiffEntry}'.");


            Assert.IsTrue(
                recognitionDetail.HitBox.TryCopyTo(results.Box));
            Assert.IsTrue(
                results.Detail.TrySetValue(recognitionDetail.Detail));
            // return ret;

            // Assert in other testings
            Detail = recognitionDetail.Detail;
            Box = $"{results.Box.X}{results.Box.Y}{results.Box.Width}{results.Box.Height}";

            return true;
        }
    }

    internal const string DiffEntry = "ColorMatch";
    internal const string DiffParam = $$"""
    {
        "{{DiffEntry}}": {
            "recognition": "ColorMatch",
            "lower": [100, 100, 100],
            "upper": [255, 255, 255],
            "action": "Click"
        }
    }
    """;

    internal const string RecoDirectType = "ColorMatch";
    internal const string RecoDirectParam = """
    {
        "recognition": "ColorMatch",
        "lower": [100, 100, 100],
        "upper": [255, 255, 255]
    }
    """;

    internal const string ActionDirectType = "Click";
    internal const string ActionDirectParam = "{}";

    internal const string DefaultAnchorName = "DefaultAnchorName";
    internal const string DefaultAnchorNodeName = "EmptyNode";

    internal sealed class TestAction : IMaaCustomAction
    {
        public string Name { get; set; } = nameof(TestAction);

        public bool Run<T>(T context, in RunArgs args, in RunResults results) where T : IMaaContext
        {
            Assert.AreEqual(NodeName, args.NodeName);
            Assert.AreEqual(ActionParam, args.ActionParam);

            Assert.AreNotEqual(Detail, args.RecognitionDetail.Detail);
            Assert.AreEqual(Box, $"{args.RecognitionBox.X}{args.RecognitionBox.Y}{args.RecognitionBox.Width}{args.RecognitionBox.Height}");

            using var actionDetail =
                context.RunAction(DiffEntry, args.RecognitionBox, args.RecognitionDetail.Detail, DiffParam);
            Assert.IsNotNull(
                actionDetail);

            using var actionDirectDetail =
                context.RunActionDirect(ActionDirectType, ActionDirectParam, args.RecognitionBox, args.RecognitionDetail.Detail);
            Assert.IsNotNull(
                actionDirectDetail);
            return true;
        }
    }

    internal sealed class EmptyAction : IMaaCustomAction
    {
        public string Name { get; set; } = nameof(EmptyAction);

        public bool Run<T>(T context, in RunArgs args, in RunResults results) where T : IMaaContext
        {
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

        event EventHandler? IMaaDisposable.Releasing
        {
            add => c.Releasing += value;
            remove => c.Releasing -= value;
        }

        event EventHandler? IMaaDisposable.Released
        {
            add => c.Released += value;
            remove => c.Released -= value;
        }

        public void Dispose() => c.Dispose();

        #endregion

        public string Name { get; set; } = "TestController";

        public bool Click(int x, int y)
            => c.Click(x, y).Wait().IsSucceeded();

        public bool Connect()
            => c.LinkStart().Wait().IsSucceeded();

        public bool Connected()
            => c.IsConnected;

        public bool InputText(string text)
            => c.InputText(text).Wait().IsSucceeded();

        public bool ClickKey(int keycode)
            => c.ClickKey(keycode).Wait().IsSucceeded();

        public bool RequestUuid(IMaaStringBuffer buffer)
        {
            var uuid = c.Uuid;
            return uuid is not null && buffer.TrySetValue(uuid);
        }

        public ControllerFeatures GetFeatures() => ControllerFeatures.None;

        public bool Screencap(IMaaImageBuffer buffer)
            => c.Screencap().Wait().IsSucceeded() && c.GetCachedImage(buffer);

        public bool StartApp(string intent)
            => c.StartApp(intent).Wait().IsSucceeded();

        public bool StopApp(string intent)
            => c.StopApp(intent).Wait().IsSucceeded();

        public bool Swipe(int x1, int y1, int x2, int y2, int duration)
            => c.Swipe(x1, y1, x2, y2, duration).Wait().IsSucceeded();

        public bool TouchDown(int contact, int x, int y, int pressure)
            => c.TouchDown(contact, x, y, pressure).Wait().IsSucceeded();

        public bool TouchMove(int contact, int x, int y, int pressure)
            => c.TouchMove(contact, x, y, pressure).Wait().IsSucceeded();

        public bool TouchUp(int contact)
            => c.TouchUp(contact).Wait().IsSucceeded();

        public bool KeyDown(int keycode)
            => c.KeyDown(keycode).Wait().IsSucceeded();
        public bool KeyUp(int keycode)
            => c.KeyUp(keycode).Wait().IsSucceeded();
        public bool Scroll(int dx, int dy)
            => c.Scroll(dx, dy).Wait().IsSucceeded();

        public bool RelativeMove(int dx, int dy)
            => c.RelativeMove(dx, dy).Wait().IsSucceeded();

        public bool Shell(string cmd, long millisecondsTimeout, IMaaStringBuffer buffer)
            => c.Shell(cmd, millisecondsTimeout).Wait().IsSucceeded() && c.GetShellOutput(out _);

        public bool Inactive()
                => c.Inactive().Wait().IsSucceeded();
        public bool GetInfo(IMaaStringBuffer buffer)
        {
            var info = c.Info;
            return info is not null && buffer.TrySetValue(info);
        }
    }

    internal sealed class TestInvalidResource : IMaaCustom
    {
        public string Name { get; set; } = nameof(TestInvalidResource);
    }
}

