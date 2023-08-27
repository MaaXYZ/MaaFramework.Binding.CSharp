using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MaaToolKit.Extensions.Interop;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public static class MaaActionApi
{
    public delegate MaaBool Run(
        MaaSyncContextHandle syncContext,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string taskName,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string customActionParam,
        out MaaRect currentBox);

    public delegate void Stop();
}

[StructLayout(LayoutKind.Sequential)]
[NativeMarshalling(typeof(MaaCustomActionApiMarshaller))]
public struct MaaCustomActionApi : IMaaDefStruct
{
    public required MaaActionApi.Run Run;
    public required MaaActionApi.Stop Stop;
}


[CustomMarshaller(typeof(MaaCustomActionApi), MarshalMode.Default, typeof(MaaCustomActionApiMarshaller))]
internal static class MaaCustomActionApiMarshaller
{
    public static Unmanaged ConvertToUnmanaged(MaaCustomActionApi managed)
        => new()
        {
            Run = Marshal.GetFunctionPointerForDelegate<MaaActionApi.Run>(managed.Run),
            Stop = Marshal.GetFunctionPointerForDelegate<MaaActionApi.Stop>(managed.Stop)
        };

    public static MaaCustomActionApi ConvertToManaged(Unmanaged unmanaged)
        => new()
        {
            Run = Marshal.GetDelegateForFunctionPointer<MaaActionApi.Run>(unmanaged.Run),
            Stop = Marshal.GetDelegateForFunctionPointer<MaaActionApi.Stop>(unmanaged.Stop)
        };

    public static void Free(Unmanaged unmanaged)
    {
        // Method intentionally left empty.
    }

    internal struct Unmanaged
    {
        public nint Run;
        public nint Stop;
    }
}
