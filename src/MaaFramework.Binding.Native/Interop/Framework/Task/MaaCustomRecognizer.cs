global using MaaRecognizerApiTuple = (
    MaaFramework.Binding.Interop.Native.MaaCustomRecognizerApi Unmanaged,
    MaaFramework.Binding.Custom.IMaaCustomRecognizer Managed,
    MaaFramework.Binding.Interop.Native.IMaaCustomRecognizerExtension.Analyze Analyze
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
///     A class marshalled as a MaaCustomRecognizerApi into MaaFramework.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="IMaaCustomRecognizer"/> instead.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public class MaaCustomRecognizerApi
{
    public nint Analyze;
}

/// <summary>
///     A static class providing extension methods for the converter of <see cref="IMaaCustomRecognizer"/>.
/// </summary>
public static class IMaaCustomRecognizerExtension
{

    #region include/MaaFramework/Task/MaaCustomRecognizer.h, version: v1.6.4.

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MaaBool Analyze(MaaSyncContextHandle sync_context, MaaImageBufferHandle image, MaaStringView task_name, MaaStringView custom_recognition_param, MaaTransparentArg recognizer_arg, MaaRectHandle out_box, MaaStringBufferHandle out_detail);

    #endregion

    public static MaaCustomRecognizerApi Convert(this IMaaCustomRecognizer task, out MaaRecognizerApiTuple tuple)
    {
        MaaBool Analyze(MaaSyncContextHandle sync_context, MaaImageBufferHandle image, MaaStringView task_name, MaaStringView custom_recognition_param, MaaTransparentArg recognizer_arg, /*out*/ MaaRectHandle out_box, /*out*/ MaaStringBufferHandle out_detail)
            => task
               .Analyze(new Binding.MaaSyncContext(sync_context), new Buffers.MaaImageBuffer(image), task_name.ToStringUTF8(), custom_recognition_param.ToStringUTF8(), new Buffers.MaaRectBuffer(out_box), new Buffers.MaaStringBuffer(out_detail))
               .ToMaaBool();

        tuple = (new()
        {
            Analyze = Marshal.GetFunctionPointerForDelegate<Analyze>(Analyze),
        },
            task,
            Analyze
        );
        return tuple.Unmanaged;
    }
}
