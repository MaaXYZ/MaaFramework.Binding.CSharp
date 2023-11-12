using MaaFramework.Binding.Native.Interop.Framework;
using static MaaFramework.Binding.Native.Interop.Framework.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A class providing a reference implementation for <see cref="MaaFramework.Binding.Native.Interop.Framework.MaaCustomControllerApi"/>.
/// </summary>
public class MaaCustomController : MaaController
{
#pragma warning disable S4487 // Unread "private" fields should be removed
#pragma warning disable IDE0052 // 删除未读的私有成员
    private MaaCustomControllerApi _customController;
#pragma warning restore IDE0052 // 删除未读的私有成员
#pragma warning restore S4487 // Unread "private" fields should be removed

    /// <summary>
    ///     Creates a <see cref="MaaCustomController"/> instance.
    /// </summary>
    /// <param name="customController"></param>
    /// <param name="handleArg"></param>
    /// <param name="maaCallbackTransparentArg"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCustomControllerCreate"/>.
    /// </remarks>
    public MaaCustomController(MaaCustomControllerApi customController, MaaTransparentArg handleArg, MaaCallbackTransparentArg maaCallbackTransparentArg)
        : base()
    {
        _handle = MaaCustomControllerCreate(ref customController, handleArg, MaaApiCallback, maaCallbackTransparentArg);
        _customController = customController;
    }
}
