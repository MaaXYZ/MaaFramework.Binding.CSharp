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

    #region include/MaaFramework/Task/MaaCustomAction.h, version: v1.6.4.

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool Run(MaaSyncContextHandle sync_context, MaaStringView task_name, MaaStringView custom_action_param, MaaRectHandle cur_box, MaaStringView cur_rec_detail, MaaTransparentArg action_arg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Abort(MaaTransparentArg action_arg);

    #endregion

}

/// <summary>
///     MaaCustomActionTask
/// </summary>
[NativeMarshalling(typeof(MaaCustomActionApiMarshaller))]
public class MaaCustomActionApi
{
    public static MaaCustomActionApi Convert(Custom.MaaCustomActionTask task) => new()
    {
        Run = (MaaSyncContextHandle sync_context, MaaStringView task_name, MaaStringView custom_action_param, MaaRectHandle cur_box, MaaStringView cur_rec_detail, MaaTransparentArg action_arg)
            => task.Run
                  .Invoke(new Binding.MaaSyncContext(sync_context), task_name.ToStringUTF8(), custom_action_param.ToStringUTF8(), new Buffers.MaaRectBuffer(cur_box), cur_rec_detail.ToStringUTF8())
                  .ToMaaBool(),
        Abort = (MaaTransparentArg action_arg)
            => task.Abort
                  .Invoke(),
    };

    public required MaaActionApi.Run Run { get; init; }
    public required MaaActionApi.Abort Abort { get; init; }
    internal MaaCustomActionApiMarshaller.Unmanaged Unmanaged { get; set; }
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
        => managed.Unmanaged = new()
        {
            Run = Marshal.GetFunctionPointerForDelegate<MaaActionApi.Run>(managed.Run),

            Stop = Marshal.GetFunctionPointerForDelegate<MaaActionApi.Abort>(managed.Abort)
        };

    public static void Free(Unmanaged unmanaged)
    {
        // Method intentionally left empty.
    }
}

