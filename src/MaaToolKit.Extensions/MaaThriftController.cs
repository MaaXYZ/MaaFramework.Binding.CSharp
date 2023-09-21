using MaaToolKit.Extensions.Interop;
using static MaaToolKit.Extensions.Interop.MaaApi;

namespace MaaToolKit.Extensions;

/// <summary>
///     A class providing a reference implementation for Maa Thrift Controller section of <see cref="MaaApi"/>.
/// </summary>
public class MaaThriftController : MaaController
{
    /// <summary>
    ///     Creates a <see cref="MaaThriftController"/> instance.
    /// </summary>
    /// <param name="param"></param>
    /// <param name="maaCallbackTransparentArg"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaThriftControllerCreate"/>.
    /// </remarks>
    public MaaThriftController(string param, MaaCallbackTransparentArg maaCallbackTransparentArg)
        : base()
    {
        _handle = MaaThriftControllerCreate(param, _callback, maaCallbackTransparentArg);
        _controllers.Add(this);
    }
}
