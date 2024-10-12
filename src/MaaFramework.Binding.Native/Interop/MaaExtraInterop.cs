#pragma warning disable CS1591
#pragma warning disable CA1401
#pragma warning disable CA1711
#pragma warning disable S4200

using System.Runtime.InteropServices;
namespace MaaFramework.Binding.Interop.Native;

public static partial class MaaBuffer
{
    [LibraryImport("MaaFramework", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool MaaStringBufferSetEx(MaaStringBufferHandle handle, nint str, MaaSize size);
}
