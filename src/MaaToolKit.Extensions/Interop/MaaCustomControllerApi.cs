using System.Runtime.InteropServices.Marshalling;
using System.Runtime.InteropServices;

namespace MaaToolKit.Extensions.Interop;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// TODOa: 缺失测试用例

public static class MaaControllerApi
{
    public delegate MaaBool SetOption(MaaCtrlOption key, MaaString value);
    public delegate MaaBool Connect();
    public delegate MaaBool Click(int32_t x, int32_t y);
    public delegate MaaBool Swipe(ref int32_t x_steps_buff, ref int32_t y_steps_buff, ref int32_t step_delay_buff, MaaSize buff_size);
    public delegate MaaBool PressKey(int32_t keycode);
    public delegate MaaBool StartApp(MaaString package_name);
    public delegate MaaBool StopApp(MaaString package_name);
    public delegate MaaBool GetResolution(ref int32_t width, ref int32_t height);
    public delegate MaaBool GetImage(nint buff, MaaSize buff_size);
    public delegate MaaBool GetUuid(nint buff, MaaSize buff_size);
}

[StructLayout(LayoutKind.Sequential)]
[NativeMarshalling(typeof(MaaCustomControllerApiMarshaller))]
public struct MaaCustomControllerApi : IMaaDefStruct
{
    public required MaaControllerApi.SetOption SetOption;
    public required MaaControllerApi.Connect Connect;
    public required MaaControllerApi.Click Click;
    public required MaaControllerApi.Swipe Swipe;
    public required MaaControllerApi.PressKey PressKey;
    public required MaaControllerApi.StartApp StartApp;
    public required MaaControllerApi.StopApp StopApp;
    public required MaaControllerApi.GetResolution GetResolution;
    public required MaaControllerApi.GetImage GetImage;
    public required MaaControllerApi.GetUuid GetUuid;
}


[CustomMarshaller(typeof(MaaCustomControllerApi), MarshalMode.Default, typeof(MaaCustomControllerApiMarshaller))]
internal static class MaaCustomControllerApiMarshaller
{
    public static Unmanaged ConvertToUnmanaged(MaaCustomControllerApi managed)
        => new()
        {
            SetOption = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.SetOption>(managed.SetOption),
            Connect = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.Connect>(managed.Connect),
            Click = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.Click>(managed.Click),
            Swipe = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.Swipe>(managed.Swipe),
            PressKey = Marshal.GetFunctionPointerForDelegate<MaaControllerApi.PressKey>(managed.PressKey),
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

    internal struct Unmanaged
    {
        public nint SetOption;
        public nint Connect;
        public nint Click;
        public nint Swipe;
        public nint PressKey;
        public nint StartApp;
        public nint StopApp;
        public nint GetResolution;
        public nint GetImage;
        public nint GetUuid;
    }
}
