using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Custom;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

/// <summary>
///     An interface representing implementation is maa custom controller.
/// </summary>
public interface IMaaCustomController : IMaaCustomResource, IDisposable
{
    bool Connect();

    /// <remarks>
    ///     Write result to buffer.
    /// </remarks>
    bool RequestUuid(in IMaaStringBuffer buffer);
    ControllerFeatures GetFeatures();

    /// <remarks>
    ///     Write result to width and height.
    /// </remarks>
    bool RequestResolution(out int width, out int height);
    bool StartApp(string intent);
    bool StopApp(string intent);

    /// <remarks>
    ///     Write result to buffer.
    /// </remarks>
    bool Screencap(in IMaaImageBuffer buffer);
    bool Click(int x, int y);
    bool Swipe(int x1, int y1, int x2, int y2, int duration);
    bool TouchDown(int contact, int x, int y, int pressure);
    bool TouchMove(int contact, int x, int y, int pressure);
    bool TouchUp(int contact);
    bool ClickKey(int keycode);
    bool InputText(string text);
    bool KeyDown(int keycode);
    bool KeyUp(int keycode);
}
