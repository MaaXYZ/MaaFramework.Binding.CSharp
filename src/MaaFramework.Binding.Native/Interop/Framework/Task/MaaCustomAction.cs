global using MaaActionApiTuple = (
    MaaFramework.Binding.Interop.Native.MaaCustomActionApi Unmanaged,
    MaaFramework.Binding.Custom.IMaaCustomAction Managed,
    MaaFramework.Binding.Interop.Native.IMaaCustomActionExtension.Run Run,
    MaaFramework.Binding.Interop.Native.IMaaCustomActionExtension.Abort Abort
);
using MaaFramework.Binding.Custom;
using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Native;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1707 // 标识符不应包含下划线

/// <summary>
///     A class marshalled as a MaaCustomActionApi into MaaFramework.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="IMaaCustomAction"/> instead.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public class MaaCustomActionApi
{
    public nint Run;
    public nint Stop;
}

/// <summary>
///     A static class providing extension methods for the converter of <see cref="IMaaCustomAction"/>.
/// </summary>
public static class IMaaCustomActionExtension
{

    #region include/MaaFramework/Task/MaaCustomAction.h, version: v1.6.4.

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool Run(MaaSyncContextHandle sync_context, MaaStringView task_name, MaaStringView custom_action_param, MaaRectHandle cur_box, MaaStringView cur_rec_detail, MaaTransparentArg action_arg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Abort(MaaTransparentArg action_arg);

    #endregion

    public static MaaCustomActionApi Convert(this IMaaCustomAction task, out MaaActionApiTuple tuple)
    {
        MaaBool Run(MaaSyncContextHandle sync_context, MaaStringView task_name, MaaStringView custom_action_param, MaaRectHandle cur_box, MaaStringView cur_rec_detail, MaaTransparentArg action_arg)
            => task
               .Run(new Binding.MaaSyncContext(sync_context), task_name.ToStringUTF8(), custom_action_param.ToStringUTF8(), new Buffers.MaaRectBuffer(cur_box), cur_rec_detail.ToStringUTF8())
               .ToMaaBool();

        void Abort(MaaTransparentArg action_arg)
            => task
            .Abort();

        tuple = (new()
        {
            Run = Marshal.GetFunctionPointerForDelegate<Run>(Run),
            Stop = Marshal.GetFunctionPointerForDelegate<Abort>(Abort),
        },
            task,
            Run,
            Abort
        );
        return tuple.Unmanaged;
    }
}

/// <summary>
///     A class providing implementation for managing marshaled parameters in <see cref="IMaaCustomAction"/>.
/// </summary>
public class MaaActionApi
{
    private readonly Dictionary<string, MaaActionApiTuple> _apis = [];

    public bool Set(MaaActionApiTuple tuple)
    {
        _apis[tuple.Managed.Name] = tuple;
        return true;
    }

    public bool Remove(string name)
    {
        return _apis.Remove(name);
    }

    public bool Clear()
    {
        _apis.Clear();
        return true;
    }
}
