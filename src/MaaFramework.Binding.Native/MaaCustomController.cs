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
    private readonly MaaCustomControllerApi _customController;
#pragma warning restore IDE0052 // 删除未读的私有成员
#pragma warning restore S4487 // Unread "private" fields should be removed

    /// <summary>
    ///     Creates a <see cref="MaaCustomController"/> instance.
    /// </summary>
    /// <param name="customController">The MaaCustomControllerApi.</param>
    /// <param name="handleArg">The MaaTransparentArg.</param>
    /// <param name="maaCallbackTransparentArg">The MaaCallbackTransparentArg.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCustomControllerCreate"/>.
    /// </remarks>
    public MaaCustomController(MaaCustomControllerApi customController, MaaTransparentArg handleArg, MaaCallbackTransparentArg maaCallbackTransparentArg)
        : base()
    {
        var handle = MaaCustomControllerCreate(ref customController, handleArg, MaaApiCallback, maaCallbackTransparentArg);
        SetHandle(handle, needReleased: true);
        _customController = customController;
    }
}
