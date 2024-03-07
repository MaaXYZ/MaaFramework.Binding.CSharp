using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Custom;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

/// <summary>
///     A static class provides the delegates of <see cref="MaaCustomControllerApi" />.
/// </summary>
public static class MaaControllerApi
{
    public delegate bool Connect();

    /// <param name="buffer">out</param>
    public delegate bool RequestUuid( /* out */ IMaaStringBuffer buffer);

    /// <param name="width">out</param>
    /// <param name="height">out</param>
    public delegate bool RequestResolution( /* out */ out int width, /* out */ out int height);

    public delegate bool StartApp(string intent);

    public delegate bool StopApp(string intent);

    /// <param name="buffer">out</param>
    public delegate bool Screencap( /* out */ IMaaImageBuffer buffer);

    public delegate bool Click(int x, int y);

    public delegate bool Swipe(int x1, int y1, int x2, int y2, int duration);

    public delegate bool TouchDown(int contact, int x, int y, int pressure);

    public delegate bool TouchMove(int contact, int x, int y, int pressure);

    public delegate bool TouchUp(int contact);

    public delegate bool PressKey(int keycode);

    public delegate bool InputText(string text);
}

public class MaaCustomControllerApi : IMaaCustom
{
    /// <inheritdoc/>
    public string Name { get; set; } = string.Empty;

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
