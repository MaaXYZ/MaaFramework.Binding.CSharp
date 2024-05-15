global using MaaControllerApiTuple = (
    MaaFramework.Binding.Interop.Native.MaaCustomControllerApi Unmanaged,
    MaaFramework.Binding.Custom.IMaaCustomController Managed,
    MaaFramework.Binding.Interop.Native.IMaaCustomControllerExtension.Connect Connect,
    MaaFramework.Binding.Interop.Native.IMaaCustomControllerExtension.RequestUuid RequestUuid,
    MaaFramework.Binding.Interop.Native.IMaaCustomControllerExtension.RequestResolution RequestResolution,
    MaaFramework.Binding.Interop.Native.IMaaCustomControllerExtension.StartApp StartApp,
    MaaFramework.Binding.Interop.Native.IMaaCustomControllerExtension.StopApp StopApp,
    MaaFramework.Binding.Interop.Native.IMaaCustomControllerExtension.Screencap Screencap,
    MaaFramework.Binding.Interop.Native.IMaaCustomControllerExtension.Click Click,
    MaaFramework.Binding.Interop.Native.IMaaCustomControllerExtension.Swipe Swipe,
    MaaFramework.Binding.Interop.Native.IMaaCustomControllerExtension.TouchDown TouchDown,
    MaaFramework.Binding.Interop.Native.IMaaCustomControllerExtension.TouchMove TouchMove,
    MaaFramework.Binding.Interop.Native.IMaaCustomControllerExtension.TouchUp TouchUp,
    MaaFramework.Binding.Interop.Native.IMaaCustomControllerExtension.PressKey PressKey,
    MaaFramework.Binding.Interop.Native.IMaaCustomControllerExtension.InputText InputText
);

using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;
using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Native;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1707 // 标识符不应包含下划线

/// <summary>
///     A class marshalled as a MaaCustomControllerApi into MaaFramework.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="IMaaCustomController"/> instead.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public class MaaCustomControllerApi
{
    public nint Connect;
    public nint RequestUuid;
    public nint RequestResolution;
    public nint StartApp;
    public nint StopApp;
    public nint Screencap;
    public nint Click;
    public nint Swipe;
    public nint TouchDown;
    public nint TouchMove;
    public nint TouchUp;
    public nint PressKey;
    public nint InputText;
}

/// <summary>
///     A static class providing extension methods for the converter of <see cref="IMaaCustomController"/>.
/// </summary>
public static class IMaaCustomControllerExtension
{

    #region include/MaaFramework/Instance/MaaCustomController.h, version: v1.6.4.

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool Connect(MaaTransparentArg handle_arg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool RequestUuid(MaaTransparentArg handle_arg, MaaStringBufferHandle buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool RequestResolution(MaaTransparentArg handle_arg, out int32_t width, out int32_t height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool StartApp(MaaStringView intent, MaaTransparentArg handle_arg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool StopApp(MaaStringView intent, MaaTransparentArg handle_arg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool Screencap(MaaTransparentArg handle_arg, MaaImageBufferHandle buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool Click(int32_t x, int32_t y, MaaTransparentArg handle_arg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool Swipe(int32_t x1, int32_t y1, int32_t x2, int32_t y2, int32_t duration, MaaTransparentArg handle_arg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool TouchDown(int32_t contact, int32_t x, int32_t y, int32_t pressure, MaaTransparentArg handle_arg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool TouchMove(int32_t contact, int32_t x, int32_t y, int32_t pressure, MaaTransparentArg handle_arg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool TouchUp(int32_t contact, MaaTransparentArg handle_arg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool PressKey(int32_t keycode, MaaTransparentArg handle_arg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool InputText(MaaStringView text, MaaTransparentArg handle_arg);

    #endregion

    public static MaaCustomControllerApi Convert(this IMaaCustomController managed, out MaaControllerApiTuple tuple)
    {
        MaaBool Connect(MaaTransparentArg handle_arg)
            => managed
               .Connect()
               .ToMaaBool();

        MaaBool RequestUuid(MaaTransparentArg handle_arg, MaaStringBufferHandle buffer)
            => managed
               .RequestUuid(new MaaStringBuffer(buffer))
               .ToMaaBool();

        MaaBool RequestResolution(MaaTransparentArg handle_arg, out int32_t width, out int32_t height)
            => managed
               .RequestResolution(out width, out height)
               .ToMaaBool();

        MaaBool StartApp(MaaStringView intent, MaaTransparentArg handle_arg)
           => managed
               .StartApp(intent.ToStringUTF8())
               .ToMaaBool();

        MaaBool StopApp(MaaStringView intent, MaaTransparentArg handle_arg)
            => managed
               .StopApp(intent.ToStringUTF8())
               .ToMaaBool();

        MaaBool Screencap(MaaTransparentArg handle_arg, MaaImageBufferHandle buffer)
            => managed
               .Screencap(new MaaImageBuffer(buffer))
               .ToMaaBool();

        MaaBool Click(int32_t x, int32_t y, MaaTransparentArg handle_arg)
            => managed
               .Click(x, y)
               .ToMaaBool();

        MaaBool Swipe(int32_t x1, int32_t y1, int32_t x2, int32_t y2, int32_t duration, MaaTransparentArg handle_arg)
            => managed
               .Swipe(x1, y1, x2, y2, duration)
               .ToMaaBool();

        MaaBool TouchDown(int32_t contact, int32_t x, int32_t y, int32_t pressure, MaaTransparentArg handle_arg)
            => managed
               .TouchDown(contact, x, y, pressure)
               .ToMaaBool();

        MaaBool TouchMove(int32_t contact, int32_t x, int32_t y, int32_t pressure, MaaTransparentArg handle_arg)
            => managed
               .TouchMove(contact, x, y, pressure)
               .ToMaaBool();

        MaaBool TouchUp(int32_t contact, MaaTransparentArg handle_arg)
            => managed
               .TouchUp(contact)
               .ToMaaBool();

        MaaBool PressKey(int32_t keycode, MaaTransparentArg handle_arg)
            => managed
               .PressKey(keycode)
               .ToMaaBool();

        MaaBool InputText(MaaStringView text, MaaTransparentArg handle_arg)
            => managed
               .InputText(text.ToStringUTF8())
               .ToMaaBool();

        tuple = (new()
        {
            Connect = Marshal.GetFunctionPointerForDelegate<Connect>(Connect),
            RequestUuid = Marshal.GetFunctionPointerForDelegate<RequestUuid>(RequestUuid),
            RequestResolution = Marshal.GetFunctionPointerForDelegate<RequestResolution>(RequestResolution),
            StartApp = Marshal.GetFunctionPointerForDelegate<StartApp>(StartApp),
            StopApp = Marshal.GetFunctionPointerForDelegate<StopApp>(StopApp),
            Screencap = Marshal.GetFunctionPointerForDelegate<Screencap>(Screencap),
            Click = Marshal.GetFunctionPointerForDelegate<Click>(Click),
            Swipe = Marshal.GetFunctionPointerForDelegate<Swipe>(Swipe),
            TouchDown = Marshal.GetFunctionPointerForDelegate<TouchDown>(TouchDown),
            TouchMove = Marshal.GetFunctionPointerForDelegate<RequestUuid>(RequestUuid),
            TouchUp = Marshal.GetFunctionPointerForDelegate<TouchUp>(TouchUp),
            PressKey = Marshal.GetFunctionPointerForDelegate<PressKey>(PressKey),
            InputText = Marshal.GetFunctionPointerForDelegate<InputText>(InputText)
        },
            managed,
            Connect,
            RequestUuid,
            RequestResolution,
            StartApp,
            StopApp,
            Screencap,
            Click,
            Swipe,
            TouchDown,
            TouchMove,
            TouchUp,
            PressKey,
            InputText
        );
        return tuple.Unmanaged;
    }
}
