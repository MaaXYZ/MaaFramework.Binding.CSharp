using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MaaFramework.Binding.Interop.Native;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1707 // 标识符不应包含下划线

/// <summary>
///     A static class provides the delegates of <see cref="MaaCustomActionApi" />.
/// </summary>
public static class MaaActionApi
{

    #region include/MaaFramework/Task/MaaCustomAction.h, version: v1.4.0.

    public delegate MaaBool Run(MaaSyncContextHandle sync_context, MaaStringView task_name, MaaStringView custom_action_param, MaaRectHandle cur_box, MaaStringView cur_rec_detail);

    public delegate void Abort();

    #endregion

}

/// <summary>
///     MaaCustomActionApi
/// </summary>
[NativeMarshalling(typeof(MaaCustomActionApiMarshaller))]
public class MaaCustomActionApi
{
    public static MaaCustomActionApi Convert(Custom.MaaCustomActionApi api) => new()
    {
        Run = (MaaSyncContextHandle sync_context, MaaStringView task_name, MaaStringView custom_action_param, MaaRectHandle cur_box, MaaStringView cur_rec_detail)
            => api.Run
                  .Invoke(new Binding.MaaSyncContext(sync_context), task_name.ToStringUTF8(), custom_action_param.ToStringUTF8(), new Buffers.MaaRectBuffer(cur_box), cur_rec_detail.ToStringUTF8())
                  .ToMaaBool(),
        Abort = ()
            => api.Abort
                  .Invoke(),
    };

    public required MaaActionApi.Run Run { get; init; }
    public required MaaActionApi.Abort Abort { get; init; }
}

/// <summary>
///     A static class providing a reference implementation of marshaller for <see cref="MaaCustomActionApi" />.
/// </summary>
[CustomMarshaller(typeof(MaaCustomActionApi), MarshalMode.Default, typeof(MaaCustomActionApiMarshaller))]
internal static class MaaCustomActionApiMarshaller
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Unmanaged
    {
        public nint Run;
        public nint Stop;
    }

    public static Unmanaged ConvertToUnmanaged(MaaCustomActionApi managed)
        => new()
        {
            Run = Marshal.GetFunctionPointerForDelegate<MaaActionApi.Run>(managed.Run),

            Stop = Marshal.GetFunctionPointerForDelegate<MaaActionApi.Abort>(managed.Abort)
        };

    public static MaaCustomActionApi ConvertToManaged(Unmanaged unmanaged)
        => new()
        {
            Run = Marshal.GetDelegateForFunctionPointer<MaaActionApi.Run>(unmanaged.Run),

            Abort = Marshal.GetDelegateForFunctionPointer<MaaActionApi.Abort>(unmanaged.Stop)
        };

    public static void Free(Unmanaged unmanaged)
    {
        // Method intentionally left empty.
    }
}

