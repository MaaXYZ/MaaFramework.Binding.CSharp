using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Native.Interop.ToolKit;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///     The base P/Invoke methods for MaaToolKitDevice, use this class to call all the native methods.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="Binding.MaaToolKit"/> instead.
/// </summary>
public static partial class MaaToolKitConfig
{

    #region include/MaaToolKit/Config/MaaToolKitConfig.h, version: v1.1.1.

    [LibraryImport("MaaToolKit")]
    public static partial MaaBool MaaToolKitInit();

    [LibraryImport("MaaToolKit")]
    public static partial MaaBool MaaToolKitUninit();

    #endregion

}
