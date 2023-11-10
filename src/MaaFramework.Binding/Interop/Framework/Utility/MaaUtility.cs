using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Framework;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///     The base P/Invoke methods for MaaUtility, use this class to call all the native methods.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="Binding"/> instead.
/// </summary>
public static partial class MaaUtility
{

    #region include/MaaFramework/Utility/MaaUtility.h, version: v1.1.1.

    [LibraryImport("MaaFramework")]
    public static partial MaaStringView MaaVersion();

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetGlobalOption(MaaGlobalOption key, ref MaaOptionValue value, MaaOptionValueSize val_size);

    #endregion

}
