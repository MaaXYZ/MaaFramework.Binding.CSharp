namespace MaaFramework.Binding.Messages;

#pragma warning disable CA1034 // 嵌套类型应不可见
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
#pragma warning disable S2094 // Classes should not be empty
#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members

/// <summary>
///  A callback consists of a message and a payload.
///  The message is a string that indicates the type of the message.
///  The payload is a JSON object that contains the details of the message.
/// </summary>
public static class MaaMsg
{
    public const string Invalid = "Invalid";

    public static class Resource
    {
        public const string StartLoading = "Resource.StartLoading";
        public const string LoadingCompleted = "Resource.LoadingCompleted";
        public const string LoadingFailed = "Resource.LoadingFailed";
    }

    public static class Controller
    {
        public const string UUIDGot = "Controller.UUIDGot";
        public const string UUIDGetFailed = "Controller.UUIDGetFailed";
        public const string ResolutionGot = "Controller.ResolutionGot";
        public const string ResolutionGetFailed = "Controller.ResolutionGetFailed";
        public const string ScreencapInited = "Controller.ScreencapInited";
        public const string ScreencapInitFailed = "Controller.ScreencapInitFailed";
        public const string TouchInputInited = "Controller.TouchinputInited";
        public const string TouchInputInitFailed = "Controller.TouchinputInitFailed";
        public const string KeyInputInited = "Controller.KeyinputInited";
        public const string KeyInputInitFailed = "Controller.KeyinputInitFailed";
        public const string ConnectSuccess = "Controller.ConnectSuccess";
        public const string ConnectFailed = "Controller.ConnectFailed";

        // Controller action messages
        public static class Action
        {
            public const string Started = "Controller.Action.Started";
            public const string Completed = "Controller.Action.Completed";
            public const string Failed = "Controller.Action.Failed";
        }
    }

    public static class Task
    {
        public const string Started = "Task.Started";
        public const string Completed = "Task.Completed";
        public const string Failed = "Task.Failed";
        public const string Stopped = "Task.Stopped";

        public static class Focus
        {
            public const string Hit = "Task.Focus.Hit";
            public const string Runout = "Task.Focus.Runout";
            public const string Completed = "Task.Focus.Completed";
        }
    }
}

#pragma warning disable IDE1006 // 命名样式
#pragma warning disable CA1707 // 标识符不应包含下划线
#pragma warning disable CA1708 // 标识符应以大小写之外的差别进行区分
#pragma warning disable S101 // Types should be named in PascalCase
public static class Payload
{
    public class Invalid
    {
    }

    public class Failed
    {
        public required string why { get; init; }
    }

    public static class Resource
    {
        public class Loading
        {
            public required long id { get; init; }
            public required string path { get; init; }
        }
    }

    public static class Controller
    {
        public class Resolution
        {
            public required int width { get; init; }
            public required int height { get; init; }
        }

        public class UUIDGot
        {
            public required string uuid { get; init; }
        }

        public class ResolutionGot
        {
            public required Resolution resolution { get; init; }
        }

        public class Connected
        {
            public required string uuid { get; init; }
            public required Resolution height { get; init; }
        }

        public class Action
        {
            public required long id { get; init; }
        }
    }

    public class Task
    {
        public required long id { get; init; }
        public required string entry { get; init; }
        public required string name { get; init; }
        public required string uuid { get; init; }
        public required string hash { get; init; }

        public class Focus
        {
            public required long id { get; init; }
            public required string entry { get; init; }
            public required string name { get; init; }
            public required string uuid { get; init; }
            public required string hash { get; init; }
            public required string recognition { get; init; }
            public required uint run_times { get; init; }
            public required string last_time { get; init; }
            public required string status { get; init; }

        }
    }
}
