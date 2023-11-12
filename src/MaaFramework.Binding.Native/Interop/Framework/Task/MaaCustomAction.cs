using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MaaFramework.Binding.Native.Interop;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///     A struct provides the delegates of <see cref="MaaCustomActionApi" />.
/// </summary>
public struct MaaActionApi
{

    #region include/MaaFramework/Task/MaaCustomAction.h, version: v1.1.1.

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sync_context">The MaaSyncContextHandle.</param>
    /// <param name="task_name">The MaaStringView.</param>
    /// <param name="custom_action_param">The MaaStringView.</param>
    /// <param name="cur_box">The MaaRectHandle.</param>
    /// <param name="cur_rec_detail">The MaaStringView.</param>
    /// <param name="action_arg">The MaaTransparentArg.</param>
    /// <returns></returns>
    public delegate MaaBool Run(MaaSyncContextHandle sync_context, MaaStringView task_name, MaaStringView custom_action_param, MaaRectHandle cur_box, MaaStringView cur_rec_detail, MaaTransparentArg action_arg);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="action_arg">The MaaTransparentArg.</param>
    public delegate void Stop(MaaTransparentArg action_arg);
}

/// <summary>
///     MaaCustomActionApi
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[NativeMarshalling(typeof(MaaCustomActionApiMarshaller))]
public struct MaaCustomActionApi : IMaaDefStruct
{
    public required MaaActionApi.Run Run;

    public required MaaActionApi.Stop Stop;
}

/// <summary>
///     A static class providing a reference implementation of marshaller for <see cref="MaaCustomActionApi" />.
/// </summary>
[CustomMarshaller(typeof(MaaCustomActionApi), MarshalMode.Default, typeof(MaaCustomActionApiMarshaller))]
internal static class MaaCustomActionApiMarshaller
{
    internal struct Unmanaged
    {
        public nint Run;
        public nint Stop;
    }

    public static Unmanaged ConvertToUnmanaged(MaaCustomActionApi managed)
        => new()
        {
            Run = Marshal.GetFunctionPointerForDelegate<MaaActionApi.Run>(managed.Run),

            Stop = Marshal.GetFunctionPointerForDelegate<MaaActionApi.Stop>(managed.Stop)
        };

    public static MaaCustomActionApi ConvertToManaged(Unmanaged unmanaged)
        => new()
        {
            Run = Marshal.GetDelegateForFunctionPointer<MaaActionApi.Run>(unmanaged.Run),

            Stop = Marshal.GetDelegateForFunctionPointer<MaaActionApi.Stop>(unmanaged.Stop)
        };

    public static void Free(Unmanaged unmanaged)
    {
        // Method intentionally left empty.
    }
}

