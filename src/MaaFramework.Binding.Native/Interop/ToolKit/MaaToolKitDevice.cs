using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Native;

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

    #region include/MaaToolKit/Device/MaaToolKitDevice.h, version: v1.4.0.

    [LibraryImport("MaaToolKit")]
    public static partial MaaSize MaaToolKitFindDevice();

    [LibraryImport("MaaToolKit")]
    public static partial MaaSize MaaToolKitFindDeviceWithAdb([MarshalAs(UnmanagedType.LPUTF8Str)] string adb_path);

    [LibraryImport("MaaToolKit")]
    public static partial MaaStringView MaaToolKitGetDeviceName(MaaSize index);

    [LibraryImport("MaaToolKit")]
    public static partial MaaStringView MaaToolKitGetDeviceAdbPath(MaaSize index);

    [LibraryImport("MaaToolKit")]
    public static partial MaaStringView MaaToolKitGetDeviceAdbSerial(MaaSize index);

    [LibraryImport("MaaToolKit")]
    public static partial MaaAdbControllerType MaaToolKitGetDeviceAdbControllerType(MaaSize index);

    [LibraryImport("MaaToolKit")]
    public static partial MaaStringView MaaToolKitGetDeviceAdbConfig(MaaSize index);

    #endregion

}
