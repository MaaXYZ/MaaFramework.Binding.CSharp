using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Native;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1707 // 标识符不应包含下划线

/// <summary>
///     A static class provides the delegates of <see cref="MaaCustomRecognizerApi" />.
/// </summary>
public static class MaaRecognizerApi
{

    #region include/MaaFramework/Task/MaaCustomRecognizer.h, version: v1.6.4.

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool Analyze(MaaSyncContextHandle sync_context, MaaImageBufferHandle image, MaaStringView task_name, MaaStringView custom_recognition_param, MaaTransparentArg recognizer_arg, /*out*/ MaaRectHandle out_box, /*out*/ MaaStringBufferHandle out_detail);

    #endregion

    private static readonly Dictionary<string, MaaCustomRecognizerApi> _apis = [];
    private static readonly Dictionary<string, Analyze> _analyzes = [];

    public static MaaCustomRecognizerApi Convert(this Custom.MaaCustomRecognizerTask task)
    {
        MaaBool analyze(MaaSyncContextHandle sync_context, MaaImageBufferHandle image, MaaStringView task_name, MaaStringView custom_recognition_param, MaaTransparentArg recognizer_arg, /*out*/ MaaRectHandle out_box, /*out*/ MaaStringBufferHandle out_detail)
            => task.Analyze
                    .Invoke(new Binding.MaaSyncContext(sync_context), new Buffers.MaaImageBuffer(image), task_name.ToStringUTF8(), custom_recognition_param.ToStringUTF8(), new Buffers.MaaRectBuffer(out_box), new Buffers.MaaStringBuffer(out_detail))
                    .ToMaaBool();
        ArgumentException.ThrowIfNullOrEmpty(task?.Name);
        MaaCustomRecognizerApi api = new()
        {
            Analyze = Marshal.GetFunctionPointerForDelegate<Analyze>(analyze),
        };
        _analyzes.Add(task.Name, analyze);
        _apis.Add(task.Name, api);
        return api;
    }
}

/// <summary>
///     MaaCustomRecognizerTask
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public class MaaCustomRecognizerApi
{
    public nint Analyze;
}
