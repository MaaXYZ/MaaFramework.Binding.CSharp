using System.Runtime.InteropServices.Marshalling;
using System.Runtime.InteropServices;

namespace MaaToolKit.Extensions.Interop.Framework.Task;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///     A struct provides the delegates of <see cref="MaaCustomActionApi" />.
/// </summary>
public struct MaaActionApi
{

    #region include/MaaFramework/Task/MaaCustomAction.h, version: v1.1.0.

    #endregion

    public delegate MaaBool Run(MaaSyncContextHandle sync_context, MaaStringView task_name, MaaStringView custom_action_param, ref MaaRect cur_box, MaaStringView cur_rec_detail, MaaTransparentArg action_arg);

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

