using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MaaFramework.Binding.Native.Interop;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// TODOa: 缺失测试用例

/// <summary>
///     A struct provides the delegates of <see cref="MaaCustomControllerApi
///     " />.
/// </summary>
public struct MaaControllerApi
{

    #region include/MaaFramework/Instance/MaaCustomController.h, version: v1.1.1.

    #endregion

    public delegate MaaBool SetOption(MaaCtrlOption key, MaaStringView value, MaaTransparentArg handle_arg);

    public delegate MaaBool Connect(MaaTransparentArg handle_arg);
    public delegate MaaBool Click(int32_t x, int32_t y, MaaTransparentArg handle_arg);
    public delegate MaaBool Swipe(int32_t x1, int32_t y1, int32_t x2, int32_t y2, int32_t duration, MaaTransparentArg handle_arg);
    public delegate MaaBool PressKey(int32_t keycode, MaaTransparentArg handle_arg);

    public delegate MaaBool TouchDown(int32_t contact, int32_t x, int32_t y, int32_t pressure, MaaTransparentArg handle_arg);
    public delegate MaaBool TouchMove(int32_t contact, int32_t x, int32_t y, int32_t pressure, MaaTransparentArg handle_arg);
    public delegate MaaBool TouchUp(int32_t contact, MaaTransparentArg handle_arg);

    public delegate MaaBool StartApp(MaaStringView package_name, MaaTransparentArg handle_arg);
    public delegate MaaBool StopApp(MaaStringView package_name, MaaTransparentArg handle_arg);

    public delegate MaaBool GetResolution(MaaTransparentArg handle_arg, /* out */ ref
        int32_t width, /* out */ ref int32_t height);

    public delegate MaaBool GetImage(MaaTransparentArg handle_arg, /* out */ MaaImageBufferHandle buffer);
    public delegate MaaBool GetUuid(MaaTransparentArg handle_arg, /* out */ MaaStringBufferHandle buffer);
}

/// <summary>
///     MaaCustomControllerApi
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[NativeMarshalling(typeof(MaaCustomControllerApiMarshaller))]
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

/// <summary>
///     A static class providing a reference implementation of marshaller for <see cref="MaaCustomControllerApi" />.
/// </summary>
[CustomMarshaller(typeof(MaaCustomControllerApi), MarshalMode.Default, typeof(MaaCustomControllerApiMarshaller))]
internal static class MaaCustomControllerApiMarshaller
{
    internal struct Unmanaged
    {
        public required nint SetOption;
        public required nint Connect;
        public required nint Click;
        public required nint Swipe;
        public required nint TouchDown;
        public required nint TouchMove;
        public required nint TouchUp;
        public required nint PressKey;
        public required nint StartApp;
        public required nint StopApp;
        public required nint GetResolution;
        public required nint GetImage;
        public required nint GetUuid;
    }

    public static Unmanaged ConvertToUnmanaged(MaaCustomControllerApi managed)
        => new()
        {
            SetOption = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.SetOption>(managed.SetOption),

            Connect = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.Connect>(managed.Connect),
            Click = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.Click>(managed.Click),
            Swipe = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.Swipe>(managed.Swipe),
            PressKey = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.PressKey>(managed.PressKey),

            TouchDown = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.TouchDown>(managed.TouchDown),
            TouchMove = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.TouchMove>(managed.TouchMove),
            TouchUp = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.TouchUp>(managed.TouchUp),

            StartApp = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.StartApp>(managed.StartApp),
            StopApp = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.StopApp>(managed.StopApp),

            GetResolution = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.GetResolution>(managed.GetResolution),

            GetImage = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.GetImage>(managed.GetImage),
            GetUuid = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.GetUuid>(managed.GetUuid),
        };

    public static MaaCustomControllerApi ConvertToManaged(Unmanaged unmanaged)
        => new()
        {
            SetOption = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.SetOption>(unmanaged.SetOption),

            Connect = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.Connect>(unmanaged.Connect),
            Click = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.Click>(unmanaged.Click),
            Swipe = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.Swipe>(unmanaged.Swipe),
            PressKey = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.PressKey>(unmanaged.PressKey),

            TouchDown = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.TouchDown>(unmanaged.TouchDown),
            TouchMove = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.TouchMove>(unmanaged.TouchMove),
            TouchUp = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.TouchUp>(unmanaged.TouchUp),

            StartApp = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.StartApp>(unmanaged.StartApp),
            StopApp = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.StopApp>(unmanaged.StopApp),

            GetResolution = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.GetResolution>(unmanaged.GetResolution),

            GetImage = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.GetImage>(unmanaged.GetImage),
            GetUuid = Marshal.GetDelegateForFunctionPointer<MaaControllerApi.GetUuid>(unmanaged.GetUuid)
        };

    public static void Free(Unmanaged unmanaged)
    {
        // Method intentionally left empty.
    }
}
