using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Native.Interop;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1707 // 标识符不应包含下划线

/// <summary>
///     The base P/Invoke methods for MaaToolKit, use this class to call all the native methods.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="Binding.MaaToolKit"/> instead.
/// </summary>
public static partial class MaaToolKit
{

    #region include/MaaToolKit/Win32/MaaToolKitWin32Window.h, version: v1.4.0.

    [LibraryImport("MaaToolKit")]
    public static partial MaaSize MaaToolKitFindWindow([MarshalAs(UnmanagedType.LPUTF8Str)] string class_name, [MarshalAs(UnmanagedType.LPUTF8Str)] string window_name);

    [LibraryImport("MaaToolKit")]
    public static partial MaaSize MaaToolKitSearchWindow([MarshalAs(UnmanagedType.LPUTF8Str)] string class_name, [MarshalAs(UnmanagedType.LPUTF8Str)] string window_name);

    [LibraryImport("MaaToolKit")]
    public static partial MaaWin32Hwnd MaaToolKitGetWindow(MaaSize index);

    [LibraryImport("MaaToolKit")]
    public static partial MaaWin32Hwnd MaaToolKitGetCursorWindow();

    #endregion

}
