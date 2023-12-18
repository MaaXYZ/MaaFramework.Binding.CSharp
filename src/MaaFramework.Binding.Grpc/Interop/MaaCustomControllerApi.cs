using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Grpc.Interop;

#pragma warning disable CA1707 // 标识符不应包含下划线
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable S1104 // Fields should not have public accessibility

/// <summary>
///     A static class provides the delegates of <see cref="MaaCustomControllerApi" />.
/// </summary>
public static class MaaControllerApi
{
    public delegate bool SetOption(ControllerOption key, string value, nint handle_arg);

    public delegate bool Connect(nint handle_arg);
    public delegate bool Click(int x, int y, nint handle_arg);
    public delegate bool Swipe(int x1, int y1, int x2, int y2, int duration, nint handle_arg);
    public delegate bool PressKey(int keycode, nint handle_arg);

    public delegate bool TouchDown(int contact, int x, int y, int pressure, nint handle_arg);
    public delegate bool TouchMove(int contact, int x, int y, int pressure, nint handle_arg);
    public delegate bool TouchUp(int contact, nint handle_arg);

    public delegate bool StartApp(string package_name, nint handle_arg);
    public delegate bool StopApp(string package_name, nint handle_arg);

    public delegate bool GetResolution(nint handle_arg, /* out */ ref
        int width, /* out */ ref int height);

    public delegate bool GetImage(nint handle_arg, /* out */ IMaaImageBuffer buffer);
    public delegate bool GetUuid(nint handle_arg, /* out */ IMaaStringBuffer buffer);
}

/// <summary>
///     MaaCustomControllerApi
/// </summary>
public class MaaCustomControllerApi : IMaaDef
{
    public required MaaControllerApi.SetOption SetOption { get; init; }

    public required MaaControllerApi.Connect Connect { get; init; }
    public required MaaControllerApi.Click Click { get; init; }
    public required MaaControllerApi.Swipe Swipe { get; init; }
    public required MaaControllerApi.PressKey PressKey { get; init; }

    public required MaaControllerApi.TouchDown TouchDown { get; init; }
    public required MaaControllerApi.TouchMove TouchMove { get; init; }
    public required MaaControllerApi.TouchUp TouchUp { get; init; }

    public required MaaControllerApi.StartApp StartApp { get; init; }
    public required MaaControllerApi.StopApp StopApp { get; init; }

    public required MaaControllerApi.GetResolution GetResolution { get; init; }

    public required MaaControllerApi.GetImage GetImage { get; init; }
    public required MaaControllerApi.GetUuid GetUuid { get; init; }
}
