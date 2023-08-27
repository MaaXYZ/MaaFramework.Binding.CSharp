using MaaToolKit.Extensions.Interop;
using static MaaToolKit.Extensions.Interop.MaaApi;

namespace MaaToolKit.Extensions.ComponentModel;

/// <summary>
///     A class providing a reference implementation for Maa Custom Controller section of <see cref="MaaApi"/>.
/// </summary>
public class MaaCustomController : MaaController
{
#pragma warning disable S4487 // Unread "private" fields should be removed
    private MaaCustomControllerApi _controller;
#pragma warning restore S4487 // Unread "private" fields should be removed

    /// <summary>
    ///     Creates a <see cref="MaaCustomController"/> instance.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="maaCallbackTransparentArg"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCustomControllerCreate"/>.
    /// </remarks>
    public MaaCustomController(MaaCustomControllerApi controller, MaaCallbackTransparentArg maaCallbackTransparentArg)
        : base()
    {
        _handle = MaaCustomControllerCreate(ref controller, _callback, maaCallbackTransparentArg);
        _controllers.Add(this);
        _controller = controller;
    }
}
