using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Native.Interop;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1707 // 标识符不应包含下划线

/// <summary>
///     The base P/Invoke methods for MaaRpc, use this class to call all the native methods.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="Binding"/> instead.
/// </summary>
public static partial class MaaRpc
{

    #region include/MaaRpc/MaaRpc.h, version: v1.1.1.

    [LibraryImport("MaaRpc")]
    public static partial void MaaRpcStart([MarshalAs(UnmanagedType.LPUTF8Str)] string address);

    [LibraryImport("MaaRpc")]
    public static partial void MaaRpcStop();

    [LibraryImport("MaaRpc")]
    public static partial void MaaRpcWait();

    #endregion


    #region include/MaaRpc/MaaFramework/MaaDef.h, version: v1.1.1.

    #endregion

}
