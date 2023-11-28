using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Grpc.Interop;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable S1104 // Fields should not have public accessibility

/// <summary>
///     A struct provides the delegates of <see cref="MaaCustomControllerApi" />.
/// </summary>
public struct MaaControllerApi
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
public struct MaaCustomControllerApi : IMaaDefStruct
{
    public required MaaControllerApi.SetOption SetOption;

    public required MaaControllerApi.Connect Connect;
    public required MaaControllerApi.Click Click;
    public required MaaControllerApi.Swipe Swipe;
    public required MaaControllerApi.PressKey PressKey;

    public required MaaControllerApi.TouchDown TouchDown;
    public required MaaControllerApi.TouchMove TouchMove;
    public required MaaControllerApi.TouchUp TouchUp;

    public required MaaControllerApi.StartApp StartApp;
    public required MaaControllerApi.StopApp StopApp;

    public required MaaControllerApi.GetResolution GetResolution;

    public required MaaControllerApi.GetImage GetImage;
    public required MaaControllerApi.GetUuid GetUuid;
}
