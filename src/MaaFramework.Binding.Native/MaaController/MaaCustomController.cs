using MaaFramework.Binding.Custom;
using MaaFramework.Binding.Interop.Native;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaCustomControllerCreate"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaCustomController : MaaController
{
    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ {nameof(_api.Name)} = {_api.Name} }}";

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
        var handle = MaaCustomControllerCreate(_api, nint.Zero);
        _ = MaaControllerAddSink(handle, MaaEventCallback, (nint)MaaHandleType.Controller);
        SetHandle(handle, needReleased: true);

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, "", api.Name);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        MaaCustomControllerMarshaller.Free(_api); // disposing 无关
        _api.Dispose();
    }
}
