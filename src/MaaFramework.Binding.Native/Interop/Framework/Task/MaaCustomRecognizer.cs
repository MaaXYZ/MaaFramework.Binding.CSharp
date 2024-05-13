global using MaaRecognizerApiTuple = (
    MaaFramework.Binding.Interop.Native.MaaCustomRecognizerApi Api,
    MaaFramework.Binding.Custom.MaaCustomRecognizerTask Task,
    MaaFramework.Binding.Interop.Native.MaaRecognizerApi.Analyze Analyze
);

using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Native;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA1707 // 标识符不应包含下划线

/// <summary>
///     MaaCustomRecognizerTask
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public class MaaCustomRecognizerApi
{
    public nint Analyze;
}

public static class MaaCustomRecognizerTaskExtension
{
    #region include/MaaFramework/Task/MaaCustomRecognizer.h, version: v1.6.4.

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool Analyze(MaaSyncContextHandle sync_context, MaaImageBufferHandle image, MaaStringView task_name, MaaStringView custom_recognition_param, MaaTransparentArg recognizer_arg, /*out*/ MaaRectHandle out_box, /*out*/ MaaStringBufferHandle out_detail);

    #endregion

    public static MaaCustomRecognizerApi Convert(this Custom.MaaCustomRecognizerTask task, out MaaRecognizerApiTuple tuple)
    {
        MaaBool analyze(MaaSyncContextHandle sync_context, MaaImageBufferHandle image, MaaStringView task_name, MaaStringView custom_recognition_param, MaaTransparentArg recognizer_arg, /*out*/ MaaRectHandle out_box, /*out*/ MaaStringBufferHandle out_detail)
            => task.Analyze
                .Invoke(new Binding.MaaSyncContext(sync_context), new Buffers.MaaImageBuffer(image), task_name.ToStringUTF8(), custom_recognition_param.ToStringUTF8(), new Buffers.MaaRectBuffer(out_box), new Buffers.MaaStringBuffer(out_detail))
                .ToMaaBool();

        tuple = (new()
        {
            Analyze = Marshal.GetFunctionPointerForDelegate<Analyze>(analyze),
        }, task, analyze);
        return tuple.Api;
    }
}

/// <summary>
///     A static class provides the delegates of <see cref="MaaCustomRecognizerApi" />.
/// </summary>
public class MaaRecognizerApi
{
    private readonly Dictionary<string, MaaRecognizerApiTuple> _apis = [];

    public bool Set(MaaRecognizerApiTuple tuple)
    {
        _apis[tuple.Task.Name] = tuple;
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
