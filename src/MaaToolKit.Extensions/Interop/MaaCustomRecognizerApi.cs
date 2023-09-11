using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MaaToolKit.Extensions.Interop;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///     A static class provides the delegates of <see cref="MaaCustomRecognizerApi" />.
/// </summary>
public static class MaaRecognizerApi
{
    public delegate MaaBool Analyze(
        MaaSyncContextHandle syncContext,
        MaaImageBufferHandle image,
        MaaStringView taskName,
        MaaStringView customRecognitionParam,
        ref MaaRectApi outBox,
        MaaStringBufferHandle detailBuff);
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
    public static Unmanaged ConvertToUnmanaged(MaaCustomRecognizerApi managed)
        => new() { Analyze = Marshal.GetFunctionPointerForDelegate<MaaRecognizerApi.Analyze>(managed.Analyze) };

    public static MaaCustomRecognizerApi ConvertToManaged(Unmanaged unmanaged)
        => new() { Analyze = Marshal.GetDelegateForFunctionPointer<MaaRecognizerApi.Analyze>(unmanaged.Analyze) };

    public static void Free(Unmanaged unmanaged)
    {
        // Method intentionally left empty.
    }

    internal struct Unmanaged
    {
        public nint Analyze;
    }
}
