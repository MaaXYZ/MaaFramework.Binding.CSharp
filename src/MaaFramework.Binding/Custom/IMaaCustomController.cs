using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Custom;

/// <summary>
///     An interface representing implementation is maa custom controller.
/// </summary>
public interface IMaaCustomController : IMaaCustomTask
{
    /// <summary/>
    bool Connect();

    /// <summary>
    /// Write result to buffer.
    /// </summary>
    bool RequestUuid(in IMaaStringBuffer buffer);

    /// <summary>
    /// Write result to width and height.
    /// </summary>
    bool RequestResolution(out int width, out int height);

    /// <summary/>
    bool StartApp(string intent);

    /// <summary/>
    bool StopApp(string intent);
    /// <summary>
    /// Write result to buffer.
    /// </summary>
    bool Screencap(in IMaaImageBuffer buffer);

    /// <summary/>
    bool Click(int x, int y);

    /// <summary/>
    bool Swipe(int x1, int y1, int x2, int y2, int duration);

    /// <summary/>
    bool TouchDown(int contact, int x, int y, int pressure);

    /// <summary/>
    bool TouchMove(int contact, int x, int y, int pressure);

    /// <summary/>
    bool TouchUp(int contact);

    /// <summary/>
    bool PressKey(int keycode);

    /// <summary/>
    bool InputText(string text);
}
