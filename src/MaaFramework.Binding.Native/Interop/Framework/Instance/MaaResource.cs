using System.Runtime.InteropServices;

namespace MaaFramework.Binding.Native.Interop.Framework;

#pragma warning disable S1133 // Deprecated code should be removed
#pragma warning disable S4200 // Native methods should be wrapped
#pragma warning disable CA1401 // P/Invoke method should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///     The base P/Invoke methods for MaaResource, use this class to call all the native methods.
///     If you do not known what you are doing, do not use this class. In most situations, you
///     should use <see cref="Binding.MaaResource"/> instead.
/// </summary>
public static partial class MaaResource
{

    #region include/MaaFramework/Instance/MaaResource.h, version: v1.1.1.

    [LibraryImport("MaaFramework")]
    public static partial MaaResourceHandle MaaResourceCreate(MaaResourceCallback callback, MaaCallbackTransparentArg callback_arg);

    [LibraryImport("MaaFramework")]
    public static partial void MaaResourceDestroy(MaaResourceHandle res);

    [LibraryImport("MaaFramework")]
    public static partial MaaResId MaaResourcePostPath(MaaResourceHandle res, [MarshalAs(UnmanagedType.LPUTF8Str)] string path);

    [LibraryImport("MaaFramework")]
    public static partial MaaStatus MaaResourceStatus(MaaResourceHandle res, MaaResId id);

    [LibraryImport("MaaFramework")]
    public static partial MaaStatus MaaResourceWait(MaaResourceHandle res, MaaResId id);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaResourceLoaded(MaaResourceHandle res);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaResourceSetOption(MaaResourceHandle res, MaaResOption key, ref MaaOptionValue value, MaaOptionValueSize val_size);

    [LibraryImport("MaaFramework")]
    public static partial MaaBool MaaResourceGetHash(MaaResourceHandle res, /* out */ MaaStringBufferHandle buffer);

    #endregion

}
