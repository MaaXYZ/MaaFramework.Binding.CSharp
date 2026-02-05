using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Custom;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

/// <summary>
///     An interface representing implementation is maa custom controller.
/// </summary>
public interface IMaaCustomController : IMaaCustom, IDisposable
{
    bool Connect();
    bool Connected();

    /// <remarks>
    ///     Write result to buffer.
    /// </remarks>
    bool RequestUuid(IMaaStringBuffer buffer);
    ControllerFeatures GetFeatures();

    bool StartApp(string intent);
    bool StopApp(string intent);

    /// <remarks>
    ///     Write result to buffer.
    /// </remarks>
    bool Screencap(IMaaImageBuffer buffer);
    bool Click(int x, int y);
    bool Swipe(int x1, int y1, int x2, int y2, int duration);
    bool TouchDown(int contact, int x, int y, int pressure);
    bool TouchMove(int contact, int x, int y, int pressure);
    bool TouchUp(int contact);
    bool ClickKey(int keycode);
    bool InputText(string text);
    bool KeyDown(int keycode);
    bool KeyUp(int keycode);
    bool Scroll(int dx, int dy);
}
