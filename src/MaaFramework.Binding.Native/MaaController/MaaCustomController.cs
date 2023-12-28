using MaaFramework.Binding.Native.Interop;
using static MaaFramework.Binding.Native.Interop.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaCustomControllerCreate"/>.
/// </summary>
public class MaaCustomController : MaaController
{
#pragma warning disable S4487 // Unread "private" fields should be removed
#pragma warning disable IDE0052 // 删除未读的私有成员
    // Due to use customMarshaller, _api cannot be modified externally.
    private readonly MaaCustomControllerApi _nativeApi;
#pragma warning restore IDE0052 // 删除未读的私有成员
#pragma warning restore S4487 // Unread "private" fields should be removed

    /// <summary>
    ///     Creates a <see cref="MaaCustomController"/> instance.
    /// </summary>
    /// <param name="api">The MaaCustomControllerApi.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCustomControllerCreate"/>.
    /// </remarks>
    public MaaCustomController(Custom.MaaCustomControllerApi api)
        : base()
    {
        var nativeApi = MaaCustomControllerApi.Convert(api);
        var handle = MaaCustomControllerCreate(ref nativeApi, nint.Zero, MaaApiCallback, nint.Zero);
        SetHandle(handle, needReleased: true);
        _nativeApi = nativeApi;
    }
}
