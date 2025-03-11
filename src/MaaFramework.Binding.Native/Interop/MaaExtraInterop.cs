#pragma warning disable CS1591
#pragma warning disable CA1401
#pragma warning disable CA1711
#pragma warning disable S4200

using System.Runtime.InteropServices;
using System.Text;

namespace MaaFramework.Binding.Interop.Native;

public static partial class MaaBuffer
{
    public static bool MaaStringBufferSetEx(MaaStringBufferHandle handle, string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        return MaaStringBufferSetEx(handle, bytes, (MaaSize)bytes.LongLength);
    }

    [LibraryImport("MaaFramework", EntryPoint = "MaaStringBufferSetEx", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool MaaStringBufferSetExFromNint(MaaStringBufferHandle handle, nint str, MaaSize size);

    [LibraryImport("MaaFramework", EntryPoint = "MaaStringBufferGet", StringMarshalling = StringMarshalling.Utf8)]
    public static partial nint MaaStringBufferGetToNint(MaaStringBufferHandle handle);
}
