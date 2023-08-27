using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MaaToolKit.Extensions.Interop;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

[StructLayout(LayoutKind.Sequential)]
public struct MaaRect
{
    public int32_t X;
    public int32_t Y;
    public int32_t Width;
    public int32_t Height;
}

[StructLayout(LayoutKind.Sequential)]
public struct MaaImage
{
    public int32_t Rows;
    public int32_t Cols;
    public int32_t Type;
    public nint Data;
};

[StructLayout(LayoutKind.Sequential)]
public struct MaaRecognitionResult
{
    public MaaRect Box;

    /// <remarks>
    ///      size = <see cref="MaaRecognizerApi.ResultDetailBuffSize"/>
    /// </remarks>
    public nint DetailBuff;
};

public static class MaaRecognizerApi
{
    public const MaaSize ResultDetailBuffSize = 16384;

    public delegate MaaBool Analyze(
        MaaSyncContextHandle syncContext,
        [MarshalAs(UnmanagedType.LPStruct)] MaaImage image,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string taskName,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string customRecognitionParam,
        out MaaRecognitionResult result);
}

[StructLayout(LayoutKind.Sequential)]
[NativeMarshalling(typeof(MaaCustomRecognizerApiMarshaller))]
public struct MaaCustomRecognizerApi : IMaaDefStruct
{
    public required MaaRecognizerApi.Analyze Analyze;
}

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
