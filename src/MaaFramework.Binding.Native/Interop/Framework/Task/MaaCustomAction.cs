using System.Runtime.InteropServices;

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

    private static readonly Dictionary<string, MaaCustomActionApi> _apis = [];
    private static readonly Dictionary<string, Run> _runs = [];
    private static readonly Dictionary<string, Abort> _aborts = [];

    public static MaaCustomActionApi Convert(this Custom.MaaCustomActionTask task)
    {
        MaaBool run(MaaSyncContextHandle sync_context, MaaStringView task_name, MaaStringView custom_action_param, MaaRectHandle cur_box, MaaStringView cur_rec_detail, MaaTransparentArg action_arg)
                => task.Run
                      .Invoke(new Binding.MaaSyncContext(sync_context), task_name.ToStringUTF8(), custom_action_param.ToStringUTF8(), new Buffers.MaaRectBuffer(cur_box), cur_rec_detail.ToStringUTF8())
                      .ToMaaBool();
        void abort(MaaTransparentArg action_arg)
                => task.Abort
                      .Invoke();
        ArgumentException.ThrowIfNullOrEmpty(task?.Name);
        MaaCustomActionApi api = new()
        {
            Run = Marshal.GetFunctionPointerForDelegate<Run>(run),
            Stop = Marshal.GetFunctionPointerForDelegate<Abort>(abort),
        };
        _runs.Add(task.Name, run);
        _aborts.Add(task.Name, abort);
        _apis.Add(task.Name, api);
        return api;
    }
}

/// <summary>
///     MaaCustomActionTask
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public class MaaCustomActionApi
{
    public nint Run;
    public nint Stop;
}
