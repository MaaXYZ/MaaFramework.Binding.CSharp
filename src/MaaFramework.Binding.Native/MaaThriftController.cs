using static MaaFramework.Binding.Native.Interop.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaThriftControllerCreate"/>.
/// </summary>
public class MaaThriftController : MaaController
{
    /// <inheritdoc cref="MaaThriftController(string, MaaCallbackTransparentArg)"/>
    public MaaThriftController(string param)
        : this(param, MaaCallbackTransparentArg.Zero)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaThriftController"/> instance.
    /// </summary>
    /// <param name="param"></param>
    /// <param name="maaCallbackTransparentArg">The MaaCallbackTransparentArg.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaThriftControllerCreate"/>.
    /// </remarks>
    public MaaThriftController(string param, MaaCallbackTransparentArg maaCallbackTransparentArg)
    {
        var handle = MaaThriftControllerCreate(param, maaApiCallback, maaCallbackTransparentArg);
        SetHandle(handle);
    }
}
