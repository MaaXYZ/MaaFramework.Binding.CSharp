using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MaaFramework.Binding.Interop.Native;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1707 // 标识符不应包含下划线

// TODOa: 缺失测试用例

/// <summary>
///     A static class provides the delegates of <see cref="MaaCustomControllerApi
///     " />.
/// </summary>
public static class MaaControllerApi
{

    #region include/MaaFramework/Instance/MaaCustomController.h, version: v1.6.3.

    public delegate MaaBool Connect();

    public delegate MaaBool RequestUuid( /* out */ MaaStringBufferHandle buffer);

    public delegate MaaBool RequestResolution( /* out */ out int32_t width, /* out */ out int32_t height);

    public delegate MaaBool StartApp(MaaStringView intent);

    public delegate MaaBool StopApp(MaaStringView intent);

    public delegate MaaBool Screencap( /* out */ MaaImageBufferHandle buffer);

    public delegate MaaBool Click(int32_t x, int32_t y);

    public delegate MaaBool Swipe(int32_t x1, int32_t y1, int32_t x2, int32_t y2, int32_t duration);

    public delegate MaaBool TouchDown(int32_t contact, int32_t x, int32_t y, int32_t pressure);

    public delegate MaaBool TouchMove(int32_t contact, int32_t x, int32_t y, int32_t pressure);

    public delegate MaaBool TouchUp(int32_t contact);

    public delegate MaaBool PressKey(int32_t keycode);

    public delegate MaaBool InputText(MaaStringView text);

    #endregion

}

/// <summary>
///     MaaCustomControllerApi
/// </summary>
[NativeMarshalling(typeof(MaaCustomControllerApiMarshaller))]
public class MaaCustomControllerApi
{
    public static MaaCustomControllerApi Convert(Custom.MaaCustomControllerApi api) => new()
    {
        Connect = ()
            => api.Connect
                  .Invoke()
                  .ToMaaBool(),
        RequestUuid = ( /* out */ MaaStringBufferHandle buffer)
            => api.RequestUuid
                  .Invoke(new Buffers.MaaStringBuffer(buffer))
                  .ToMaaBool(),
        RequestResolution = ( /* out */ out int32_t width, /* out */ out int32_t height)
            => api.RequestResolution
                  .Invoke(out width, out height)
                  .ToMaaBool(),
        StartApp = (MaaStringView intent)
            => api.StartApp
                  .Invoke(intent.ToStringUTF8())
                  .ToMaaBool(),
        StopApp = (MaaStringView intent)
            => api.StopApp
                  .Invoke(intent.ToStringUTF8())
                  .ToMaaBool(),
        Screencap = ( /* out */ MaaImageBufferHandle buffer)
            => api.Screencap
                  .Invoke(new Buffers.MaaImageBuffer(buffer))
                  .ToMaaBool(),
        Click = (int32_t x, int32_t y)
            => api.Click
                  .Invoke(x, y)
                  .ToMaaBool(),
        Swipe = (int32_t x1, int32_t y1, int32_t x2, int32_t y2, int32_t duration)
            => api.Swipe
                  .Invoke(x1, y1, x2, y2, duration)
                  .ToMaaBool(),
        TouchDown = (int32_t contact, int32_t x, int32_t y, int32_t pressure)
            => api.TouchDown
                  .Invoke(contact, x, y, pressure)
                  .ToMaaBool(),
        TouchMove = (int32_t contact, int32_t x, int32_t y, int32_t pressure)
            => api.TouchMove
                  .Invoke(contact, x, y, pressure)
                  .ToMaaBool(),
        TouchUp = (int32_t contact)
            => api.TouchUp
                  .Invoke(contact)
                  .ToMaaBool(),
        PressKey = (int32_t keycode)
            => api.PressKey
                  .Invoke(keycode)
                  .ToMaaBool(),
        InputText = (MaaStringView text)
            => api.InputText
                  .Invoke(text.ToStringUTF8())
                  .ToMaaBool(),
    };

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

/// <summary>
///     A static class providing a reference implementation of marshaller for <see cref="MaaCustomControllerApi" />.
/// </summary>
[CustomMarshaller(typeof(MaaCustomControllerApi), MarshalMode.Default, typeof(MaaCustomControllerApiMarshaller))]
internal static class MaaCustomControllerApiMarshaller
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Unmanaged
    {
        public required nint Connect;
        public required nint RequestUuid;
        public required nint RequestResolution;
        public required nint StartApp;
        public required nint StopApp;
        public required nint Screencap;
        public required nint Click;
        public required nint Swipe;
        public required nint TouchDown;
        public required nint TouchMove;
        public required nint TouchUp;
        public required nint PressKey;
        public required nint InputText;
    }

    public static Unmanaged ConvertToUnmanaged(MaaCustomControllerApi managed)
        => new()
        {
            Connect = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.Connect>(managed.Connect),
            RequestUuid = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.RequestUuid>(managed.RequestUuid),
            RequestResolution = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.RequestResolution>(managed.RequestResolution),
            StartApp = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.StartApp>(managed.StartApp),
            StopApp = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.StopApp>(managed.StopApp),
            Screencap = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.Screencap>(managed.Screencap),
            Click = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.Click>(managed.Click),
            Swipe = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.Swipe>(managed.Swipe),
            TouchDown = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.TouchDown>(managed.TouchDown),
            TouchMove = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.TouchMove>(managed.TouchMove),
            TouchUp = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.TouchUp>(managed.TouchUp),
            PressKey = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.PressKey>(managed.PressKey),
            InputText = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.InputText>(managed.InputText),
        };

    public static MaaCustomControllerApi ConvertToManaged(Unmanaged unmanaged)
        => new()
        {
            Connect = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.Connect>(unmanaged.Connect),
            RequestUuid = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.RequestUuid>(unmanaged.RequestUuid),
            RequestResolution = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.RequestResolution>(unmanaged.RequestResolution),
            StartApp = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.StartApp>(unmanaged.StartApp),
            StopApp = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.StopApp>(unmanaged.StopApp),
            Screencap = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.Screencap>(unmanaged.Screencap),
            Click = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.Click>(unmanaged.Click),
            Swipe = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.Swipe>(unmanaged.Swipe),
            TouchDown = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.TouchDown>(unmanaged.TouchDown),
            TouchMove = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.TouchMove>(unmanaged.TouchMove),
            TouchUp = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.TouchUp>(unmanaged.TouchUp),
            PressKey = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.PressKey>(unmanaged.PressKey),
            InputText = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.InputText>(unmanaged.InputText),
        };

    public static void Free(Unmanaged unmanaged)
    {
        // Method intentionally left empty.
    }
}
