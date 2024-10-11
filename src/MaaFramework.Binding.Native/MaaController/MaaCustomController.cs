using MaaFramework.Binding.Custom;
using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaCustomControllerCreate"/>.
/// </summary>
public class MaaCustomController : MaaController
{
    private readonly IMaaCustomController _api;

    /// <summary>
    ///     Creates a <see cref="MaaCustomController"/> instance.
    /// </summary>
    /// <param name="api">The <see cref="IMaaCustomController"/>.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCustomControllerCreate"/>.
    /// </remarks>
    public MaaCustomController(IMaaCustomController api)
        : base()
    {
        _api = api;
        var handle = MaaCustomControllerCreate(_api, nint.Zero, MaaNotificationCallback, nint.Zero);
        SetHandle(handle, needReleased: true);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        MaaCustomControllerMarshaller.Free(_api);
        _api.Dispose();
    }
}
