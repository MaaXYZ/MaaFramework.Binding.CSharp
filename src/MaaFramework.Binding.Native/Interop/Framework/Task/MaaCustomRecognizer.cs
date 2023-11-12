using System.Runtime.InteropServices.Marshalling;
using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Native.Interop.Framework;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///     A static class provides the delegates of <see cref="MaaCustomRecognizerApi" />.
/// </summary>
public static class MaaRecognizerApi
{

    #region include/MaaFramework/Task/MaaCustomRecognizer.h, version: v1.1.1.

    #endregion

    public delegate MaaBool Analyze(MaaSyncContextHandle sync_context, MaaImageBufferHandle image, MaaStringView task_name,
                           MaaStringView custom_recognition_param, MaaTransparentArg recognizer_arg,
                           /*out*/ ref MaaRect out_box,
                           /*out*/ MaaStringBufferHandle detail_buff);
}

/// <summary>
///     MaaCustomRecognizerApi
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[NativeMarshalling(typeof(MaaCustomRecognizerApiMarshaller))]
public struct MaaCustomRecognizerApi : IMaaDefStruct
{
    public required MaaRecognizerApi.Analyze Analyze;
}

/// <summary>
///     A static class providing a reference implementation of marshaller for <see cref="MaaCustomRecognizerApi" />.
/// </summary>
[CustomMarshaller(typeof(MaaCustomRecognizerApi), MarshalMode.Default, typeof(MaaCustomRecognizerApiMarshaller))]
internal static class MaaCustomRecognizerApiMarshaller
{
    internal struct Unmanaged
    {
        public nint Analyze;
    }

    public static Unmanaged ConvertToUnmanaged(MaaCustomRecognizerApi managed)
        => new() { Analyze = Marshal.GetFunctionPointerForDelegate<MaaRecognizerApi.Analyze>(managed.Analyze) };

    public static MaaCustomRecognizerApi ConvertToManaged(Unmanaged unmanaged)
        => new() { Analyze = Marshal.GetDelegateForFunctionPointer<MaaRecognizerApi.Analyze>(unmanaged.Analyze) };

    public static void Free(Unmanaged unmanaged)
    {
        // Method intentionally left empty.
    }
}
