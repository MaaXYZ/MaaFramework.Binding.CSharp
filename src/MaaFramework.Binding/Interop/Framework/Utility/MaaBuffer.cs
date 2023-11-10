global using MaaImageRawData = nint; // void*
global using MaaImageEncodedData = nint; // uint8_t*

using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Interop.Framework;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///     The base P/Invoke methods for MaaBuffer, use this class to call all the native methods.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="Binding"/> instead.
/// </summary>
public static partial class MaaBuffer
{

    #region include/MaaFramework/Utility/MaaBuffer.h, version: v1.1.1.

    [LibraryImport("MaaFramework")]
    public static partial MaaStringBufferHandle MaaCreateStringBuffer();

    [LibraryImport("MaaFramework")]
    public static partial void MaaDestroyStringBuffer(MaaStringBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaIsStringEmpty(MaaStringBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaClearString(MaaStringBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaStringView MaaGetString(MaaStringBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaSize MaaGetStringSize(MaaStringBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetString(MaaStringBufferHandle handle, [MarshalAs(UnmanagedType.LPUTF8Str)] string str);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetStringEx(MaaStringBufferHandle handle, ref byte str, MaaSize size);

    [LibraryImport("MaaFramework")]
    public static partial MaaImageBufferHandle MaaCreateImageBuffer();

    [LibraryImport("MaaFramework")]
    public static partial void MaaDestroyImageBuffer(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaIsImageEmpty(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaClearImage(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaImageRawData MaaGetImageRawData(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial int32_t MaaGetImageWidth(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial int32_t MaaGetImageHeight(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial int32_t MaaGetImageType(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetImageRawData(MaaImageBufferHandle handle, MaaImageRawData data, int32_t width, int32_t height, int32_t type);

    [LibraryImport("MaaFramework")]
    public static partial MaaImageEncodedData MaaGetImageEncoded(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaSize MaaGetImageEncodedSize(MaaImageBufferHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetImageEncoded(MaaImageBufferHandle handle, MaaImageEncodedData data, MaaSize size);

    [LibraryImport("MaaFramework")]
    public static partial MaaRectHandle MaaCreateRectBuffer();

    [LibraryImport("MaaFramework")]
    public static partial void MaaDestroyRectBuffer(MaaRectHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial int32_t MaaGetRectX(MaaRectHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial int32_t MaaGetRectY(MaaRectHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial int32_t MaaGetRectW(MaaRectHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial int32_t MaaGetRectH(MaaRectHandle handle);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetRectX(MaaRectHandle handle, int32_t value);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetRectY(MaaRectHandle handle, int32_t value);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetRectW(MaaRectHandle handle, int32_t value);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaSetRectH(MaaRectHandle handle, int32_t value);

    #endregion

}
