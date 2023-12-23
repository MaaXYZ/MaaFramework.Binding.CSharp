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
    #region include/MaaFramework/Instance/MaaCustomController.h, version: v1.4.0.

    public delegate bool Connect(nint handle_arg);

    public delegate bool RequestUuid(nint handle_arg, /* out */ IMaaStringBuffer buffer);

    public delegate bool RequestResolution(nint handle_arg, /* out */ ref int width, /* out */ ref int height);

    public delegate bool StartApp(string intent, nint handle_arg);

    public delegate bool StopApp(string intent, nint handle_arg);

    public delegate bool Screencap(nint handle_arg, /* out */ IMaaImageBuffer buffer);

    public delegate bool Click(int x, int y, nint handle_arg);

    public delegate bool Swipe(int x1, int y1, int x2, int y2, int duration, nint handle_arg);

    public delegate bool TouchDown(int contact, int x, int y, int pressure, nint handle_arg);

    public delegate bool TouchMove(int contact, int x, int y, int pressure, nint handle_arg);

    public delegate bool TouchUp(int contact, nint handle_arg);

    public delegate bool PressKey(int keycode, nint handle_arg);

    public delegate bool InputText(string text, nint handle_arg);

    #endregion

}

/// <summary>
///     MaaCustomControllerApi
/// </summary>
public class MaaCustomControllerApi : IMaaDef
{
    public required MaaControllerApi.Connect Connect { get; init; }
    public required MaaControllerApi.RequestUuid RequestUuid { get; init; }
    public required MaaControllerApi.RequestResolution RequestResolution { get; init; }
    public required MaaControllerApi.StartApp StartApp { get; init; }
    public required MaaControllerApi.StopApp StopApp { get; init; }
    public required MaaControllerApi.Screencap Screencap { get; init; }
    public required MaaControllerApi.Click Click { get; init; }
    public required MaaControllerApi.Swipe Swipe { get; init; }
    public required MaaControllerApi.TouchDown TouchDown { get; init; }
    public required MaaControllerApi.TouchMove TouchMove { get; init; }
    public required MaaControllerApi.TouchUp TouchUp { get; init; }
    public required MaaControllerApi.PressKey PressKey { get; init; }
    public required MaaControllerApi.InputText InputText { get; init; }
}
