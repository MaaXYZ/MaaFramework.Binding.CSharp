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
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaCustomControllerCreate"/>.
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="MaaJobStatusException"/>
    public MaaCustomController(IMaaCustomController api, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentNullException.ThrowIfNull(api);

        _api = api;
        var handle = MaaCustomControllerCreate(_api, nint.Zero, MaaNotificationCallback, nint.Zero);
        SetHandle(handle, needReleased: true);

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, "", api.Name);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        MaaCustomControllerMarshaller.Free(_api);
        _api.Dispose();
    }
}
