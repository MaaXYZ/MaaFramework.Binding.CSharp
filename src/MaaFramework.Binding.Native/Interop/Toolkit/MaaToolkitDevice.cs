using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Native;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1707 // 标识符不应包含下划线

/// <summary>
///     The base P/Invoke methods for MaaToolkit, use this class to call all the native methods.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="Binding.MaaToolkit"/> instead.
/// </summary>
public static partial class MaaToolkit
{

    #region include/MaaToolkit/Device/MaaToolkitDevice.h, version: v1.4.0.

    [LibraryImport("MaaToolkit")]
    public static partial MaaSize MaaToolkitFindDevice();

    [LibraryImport("MaaToolkit")]
    public static partial MaaSize MaaToolkitFindDeviceWithAdb([MarshalAs(UnmanagedType.LPUTF8Str)] string adb_path);

    [LibraryImport("MaaToolkit")]
    public static partial MaaStringView MaaToolkitGetDeviceName(MaaSize index);

    [LibraryImport("MaaToolkit")]
    public static partial MaaStringView MaaToolkitGetDeviceAdbPath(MaaSize index);

    [LibraryImport("MaaToolkit")]
    public static partial MaaStringView MaaToolkitGetDeviceAdbSerial(MaaSize index);

    [LibraryImport("MaaToolkit")]
    public static partial MaaAdbControllerType MaaToolkitGetDeviceAdbControllerType(MaaSize index);

    [LibraryImport("MaaToolkit")]
    public static partial MaaStringView MaaToolkitGetDeviceAdbConfig(MaaSize index);

    #endregion

}
