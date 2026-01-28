using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaPlayCoverControllerCreate"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaPlayCoverController : MaaController
{
    private readonly string _debugAddress;
    private readonly string _debugBundleId;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ Address = {_debugAddress}, BundleId = {_debugBundleId} }}";

    /// <summary>
    ///     Creates a <see cref="MaaPlayCoverController"/> instance.
    /// </summary>
    /// <param name="address">The PlayTools service address in "host:port" format.</param>
    /// <param name="uuid">The application bundle identifier (e.g., "com.hypergryph.arknights").</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaPlayCoverControllerCreate"/>.
    ///     <para>This controller is designed for PlayCover on macOS.</para>
    ///     <para>Some features are not supported: start_app, input_text, click_key, key_down, key_up, scroll.</para>
    ///     <para>Only single touch is supported (contact must be 0).</para>
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="MaaJobStatusException"/>
    public MaaPlayCoverController(string address, string uuid, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentException.ThrowIfNullOrEmpty(address);
        ArgumentException.ThrowIfNullOrEmpty(uuid);

        var handle = MaaPlayCoverControllerCreate(address, uuid);
        _ = MaaControllerAddSink(handle, MaaEventCallback, (nint)MaaHandleType.Controller);
        SetHandle(handle, needReleased: true);

        _debugAddress = address;
        _debugBundleId = uuid;

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, address, uuid);
    }
}
