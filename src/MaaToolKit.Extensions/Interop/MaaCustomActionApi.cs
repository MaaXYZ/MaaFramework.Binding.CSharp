using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace MaaToolKit.Extensions.Interop;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

// chore: 移除struct API的导出宏 c063037

/// <summary>
///     A static class provides the delegates of <see cref="MaaCustomActionApi" />.
/// </summary>
public static class MaaActionApi
{
    public delegate MaaBool Run(
        MaaSyncContextHandle syncContext,
        MaaStringView taskName,
        MaaStringView customActionParam,
        ref MaaRectApi currentBox,
        MaaStringView curRecDetail);

    public delegate void Stop();
}

/// <summary>
///     MaaCustomActionApi
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[NativeMarshalling(typeof(MaaCustomActionApiMarshaller))]
public struct MaaCustomActionApi : IMaaDefStruct
{
    public required MaaActionApi.Run Run;
    public required MaaActionApi.Stop Stop;
}

/// <summary>
///     A static class providing a reference implementation of marshaller for <see cref="MaaCustomActionApi" />.
/// </summary>
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
