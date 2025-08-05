#pragma warning disable CS1591
#pragma warning disable CA1401
#pragma warning disable CA1711
#pragma warning disable S4200

using System.Runtime.InteropServices;
using System.Text;

namespace MaaFramework.Binding.Interop.Native;

public static partial class MaaBuffer
{
    public static unsafe bool MaaStringBufferSetEx(MaaStringBufferHandle handle, string str)
    {
        if (str is null)
        {
            return MaaStringBufferSetExFromNint(handle, nint.Zero, size: 0);
        }

        var allocated = false;
        void* unmanagedValuePtr = null;
        try
        {
            const long MaxUtf8BytesPerChar = 3;
            var charCount = str.Length;
            var bufferSize = 0x100;
            var buffer = stackalloc byte[bufferSize];

            // > for no null terminator
            // Use the cast to long to avoid the checked operation
            if (MaxUtf8BytesPerChar * charCount > bufferSize)
            {
                // Calculate accurate byte count when the provided stack-allocated buffer is not sufficient
                var exactByteCount = Encoding.UTF8.GetByteCount(str);
                if (exactByteCount > bufferSize)
                {
                    allocated = true;
                    unmanagedValuePtr = NativeMemory.Alloc((nuint)exactByteCount);
                    buffer = (byte*)unmanagedValuePtr;
                    bufferSize = exactByteCount;
                }
            }

            fixed (char* pChar = str)
            {
                var byteCount = Encoding.UTF8.GetBytes(pChar, charCount, buffer, bufferSize);
                return MaaStringBufferSetExFromNint(handle, (nint)buffer, (MaaSize)byteCount);
            }
        }
        finally
        {
            if (allocated)
            {
                NativeMemory.Free(unmanagedValuePtr);
            }
        }
    }

    [LibraryImport("MaaFramework", EntryPoint = "MaaStringBufferSetEx", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool MaaStringBufferSetExFromNint(MaaStringBufferHandle handle, nint str, MaaSize size);

    [LibraryImport("MaaFramework", EntryPoint = "MaaStringBufferGet", StringMarshalling = StringMarshalling.Utf8)]
    public static partial nint MaaStringBufferGetToNint(MaaStringBufferHandle handle);
}
